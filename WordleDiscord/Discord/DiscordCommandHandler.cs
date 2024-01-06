using Discord;
using Discord.WebSocket;
using WordleDiscord.Discord;
using WordleDiscord.Game;
using WordleDiscord.Game.Exception;

namespace DiscordWordle.Discord;

class DiscordCommandHandler 
{
    private WordleGameController _wordleGameController = new WordleGameController();
    private WordleGameDiscordRenderer _renderer = new WordleGameDiscordRenderer();

    public async Task WordleCreateGame(SocketSlashCommand command)
    {
        try 
        {
            var guildId = command.GuildId!.Value;
            var channelId = command.ChannelId!.Value;
            var game = _wordleGameController.StartNewGame(guildId, channelId);
            await _renderer.RenderGameStatus(game, command);
        }
        catch (GameAlreadyStartedException) 
        {
            await HandleGameAlreadyStartedException(command);
        }
    }

    public async Task WordleEndGame(SocketSlashCommand command) 
    {
        try
        {
            var guildId = command.GuildId!.Value;
            var channelId = command.ChannelId!.Value;
            var game = _wordleGameController.EndGame(guildId, channelId);
            await _renderer.RenderGameStatus(game, command);
        }
        catch (GameNotStartedException)
        {
            await HandleGameNotStartedException(command);
        }

    }

    public async Task WordleGuessPhrase(SocketSlashCommand command) 
    {
        try
        {
            var guildId = command.GuildId!.Value;
            var channelId = command.ChannelId!.Value;
            var phrase = command.Data.Options.First().Options.First().Value.ToString()?.ToLower();
            var game = _wordleGameController.GuessPhrase(channelId, guildId, phrase!);
            await _renderer.RenderGameStatus(game, command);
        }
        catch (GameNotStartedException)
        {
            await HandleGameNotStartedException(command);
        }

    }

    public async Task WordleRestartGame(SocketSlashCommand command) 
    {
        try
        {
            var guildId = command.GuildId!.Value;
            var channelId = command.ChannelId!.Value;
            var game = _wordleGameController.RestartGame(channelId, guildId);
            await _renderer.RenderGameStatus(game, command);
        }
        catch (GameNotStartedException)
        {
            await HandleGameNotStartedException(command);
        }
        

    }

    private static async Task HandleGameAlreadyStartedException(SocketSlashCommand command) {
        const string errorMsg = "Gra jest już wystartowana!\nWpisz **/wordle guess _słowo_** aby odgadnąć słowo.";
        var errorEmbed = DiscordEmbedMaker.MakeErrorEmbed(errorMsg).Build();
        await command.RespondAsync(embed: errorEmbed, ephemeral: true);
    }

    private static async Task HandleGameNotStartedException(SocketSlashCommand command) {
        const string errorMsg = "Gra nie jest wystartowana!\nWpisz **/wordle start** żeby wystartować grę.";
        var errorEmbed = DiscordEmbedMaker.MakeErrorEmbed(errorMsg).Build();
        await command.RespondAsync(embed: errorEmbed, ephemeral: true);
    }
}
