using Discord;
using Discord.WebSocket;
using WordleDiscord.Discord;

namespace DiscordWordle.Discord;

class DiscordCommandDispatcher 
{
    private DiscordCommandHandler _discordCommandHandler = new DiscordCommandHandler();

    public async Task ProcessDiscordCommand(SocketSlashCommand command) {
        if (!command.GuildId.HasValue || !command.ChannelId.HasValue) {
            await HandleGuildIdUnavailable(command);
            return;
        }

        var subcommandName = command.Data.Options.First().Name;
        switch (subcommandName)
        {
            case "start":
                await _discordCommandHandler.WordleCreateGame(command);
                break;
            
            case "end":
                await _discordCommandHandler.WordleEndGame(command);
                break;
            
            case "guess":
                await _discordCommandHandler.WordleGuessPhrase(command);
                break;

            case "restart":
                await _discordCommandHandler.WordleRestartGame(command);
                break;
        }
    }

    private static async Task HandleGuildIdUnavailable(SocketSlashCommand command) {
        const string errorMsg = "Ten bot nie obsługuje wiadomości prywatnych.";
        var errorEmbed = DiscordEmbedMaker.MakeErrorEmbed(errorMsg).Build();
        await command.RespondAsync(embed: errorEmbed, ephemeral: true);
    }
}
