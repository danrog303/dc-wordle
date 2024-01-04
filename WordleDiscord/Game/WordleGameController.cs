using WordleDiscord.Game.Exception;
using WordleDiscord.Phrases;

namespace WordleDiscord.Game;

class WordleGameController 
{
    public const int GuessesLimit = 7;
    public const int WordLength = 5;

    private readonly List<WordleGame> _activeGames = [];
    private readonly IWordlePhraseProvider _phraseProvider = new PolishWordlePhraseProvider();

    public WordleGame StartNewGame(ulong discordGuildId, ulong discordChannelId)
    {
        var existingGame = _activeGames.FirstOrDefault(game => game.DiscordChannelId == discordChannelId && game.DiscordGuildId == discordGuildId);
        if (existingGame != null) 
        {
            throw new GameAlreadyStartedException();
        }

        var phrase = _phraseProvider.GetRandomWordlePhrase();
        var game = new WordleGame(discordGuildId, discordChannelId, phrase);
        _activeGames.Add(game);
        return game;
    }

    public WordleGame EndGame(ulong discordGuildId, ulong discordChannelId)
    {
        var game = FindByDiscordIds(discordChannelId, discordGuildId);
        game.Aborted = true;
        game.LastActionDate = new DateTime();
        _activeGames.Remove(game);
        return game;
    }

    public WordleGame GuessPhrase(ulong discordChannelId, ulong discordGuildId, string phrase)
    {
        var game = FindByDiscordIds(discordChannelId, discordGuildId);
        game.LastActionDate = new DateTime();
        game.Guesses.Add(phrase);
        if(phrase == game.CorrectPhrase)
        {
            game.Won = true;
            _activeGames.Remove(game);
        }
        else if (game.Guesses.Count == GuessesLimit) 
        {
            game.Aborted = true;
            _activeGames.Remove(game);
        }
        return game;
    }

    public WordleGame RestartGame(ulong discordChannelId, ulong discordGuildId)
    {
        EndGame(discordGuildId, discordChannelId);
        return StartNewGame(discordGuildId, discordChannelId);
    }

    private WordleGame FindByDiscordIds(ulong discordChannelId, ulong discordGuildId)
    {
        try
        {
            return _activeGames.First(game => game.DiscordChannelId == discordChannelId && game.DiscordGuildId == discordGuildId);
        }
        catch (InvalidOperationException e)
        {
            throw new GameNotStartedException("Game is not started", e);
        }
    }
}
