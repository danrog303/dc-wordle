using System.Linq;
using Discord;
using Discord.WebSocket;
using SkiaSharp;
using WordleDiscord.Game;

namespace WordleDiscord.Discord;

class WordleGameDiscordRenderer
{
    public async Task RenderGameStatus(WordleGame game, SocketSlashCommand command) 
    {
        await command.DeferAsync();
        var embedBuilder = DiscordEmbedMaker.MakeSuccessEmbed();
        var remainingTries = WordleGameController.GuessesLimit - game.Guesses.Count;

        if (game.Guesses.Count == 0 && !game.Aborted)
        {
            embedBuilder.AddField("Gra rozpoczęta!", "Rozpoczęto grę w Wordle!");
            embedBuilder.AddField("Podpowiedź", "Aby spróbować odgadnąć słowo, wpisz\n**/wordle guess słowo**");
        }
        else if (game.Aborted && game.Guesses.Count < WordleGameController.GuessesLimit) 
        {
            embedBuilder.AddField("Gra zakończona!", "Zakończyliśmy grę na twoje życzenie.");
            embedBuilder.AddField("Poprawne hasło", $"Hasłem do odgadnięcia było **{game.CorrectPhrase}**.");
            embedBuilder.WithColor(Color.Red);
            game.Guesses.Add(game.CorrectPhrase);
        }
        else if (game.Aborted && game.Guesses.Count >= WordleGameController.GuessesLimit) 
        {
            embedBuilder.WithColor(Color.Red);
            embedBuilder.AddField("Gra zakończona!", "Wykorzystałeś wszystkie swoje próby...");
            embedBuilder.AddField("Poprawne hasło", $"Hasłem do odgadnięcia było **{game.CorrectPhrase}**.");
            game.Guesses.Add(game.CorrectPhrase);
        }
        else if (game.Won)
        {
            embedBuilder.AddField("Gra zakończona!", "Wygrałeś!");
            embedBuilder.AddField("Liczba prób", $"{game.Guesses.Count}");
        }
        else
        {
            embedBuilder.AddField("Pozostała ilość prób", remainingTries);
            embedBuilder.AddField("Podpowiedź", "Zielony - właściwa literka na niewłaściwej pozycji\nŻółty - właściwa literka na niewłaściwej pozycji\nSzary - literki nie ma w słowie");
        }
        
        var statusImage = GenerateImage(game.CorrectPhrase, game.Guesses);
        embedBuilder.WithImageUrl("attachment://wordle.png");
        var statusImageStream = new MemoryStream(statusImage);
        
        await command.FollowupWithFileAsync(statusImageStream, "wordle.png", embed: embedBuilder.Build());
    }

    private static byte[] GenerateImage(String correctWord, List<string> wordList)
    {
        var wordCount = wordList.Count;
        if (wordCount == 0)
        {
            wordList = ["     "];
            wordCount = 1;
        }
        
        var maxWordLength = WordleGameController.WordLength;
        var cellWidth = 50;
        var cellHeight = 50;

        var imageWidth = maxWordLength * cellWidth;
        var imageHeight = wordCount * cellHeight;

        using var surface = SKSurface.Create(new SKImageInfo(imageWidth, imageHeight));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        var random = new Random();

        for (var i = 0; i < wordCount; i++)
        {
            var word = wordList[i];

            for (var j = 0; j < word.Length; j++)
            {
                var letter = word[j];
                var bgColor = GetRandomColor(correctWord, word, j);

                using (var paint = new SKPaint())
                {
                    var left = j * cellWidth;
                    var top = i * cellHeight;
                    var right = (j + 1) * cellWidth;
                    var bottom = (i + 1) * cellHeight;
                    paint.Color = bgColor;
                    canvas.DrawRect(new SKRect(left, top, right, bottom), paint);
                }

                using (var paint = new SKPaint())
                {
                    var x = j * cellWidth + cellWidth / 2;
                    var y = i * cellHeight + cellHeight / 2 + 10;
                    paint.Color = SKColors.Black;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial");
                    paint.TextAlign = SKTextAlign.Center;
                    paint.TextSize = cellHeight * 0.7f;
                    paint.IsAntialias = true;
                    canvas.DrawText(letter.ToString(), x, y, paint);
                }
                
                using (var framePaint = new SKPaint())
                {
                    var left = j * cellWidth;
                    var top = i * cellHeight;
                    var right = (j + 1) * cellWidth;
                    var bottom = (i + 1) * cellHeight;
                    framePaint.Color = SKColors.Black;
                    framePaint.Style = SKPaintStyle.Stroke;
                    canvas.DrawRegion(new SKRegion(new SKRectI(left, top, right, bottom)), framePaint);
                }
            }
        }

        using var stream = new MemoryStream();
        surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).SaveTo(stream);
        return stream.ToArray();
    }

    private static SKColor GetRandomColor(string correctWord, string currentWord, int characterIndex)
    {
        if (correctWord[characterIndex] == currentWord[characterIndex])
        {
            return SKColors.Green;
        }
        else if(correctWord.Any(character => character == currentWord[characterIndex]))
        {
            return SKColors.Yellow;
        }
        else
        {
            return SKColors.Gray;
        }
    }    
}
