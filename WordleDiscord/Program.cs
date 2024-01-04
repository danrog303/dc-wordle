using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordWordle.Discord;
using Newtonsoft.Json;

namespace WordleDiscord;

class Program {
    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig() {
         GatewayIntents = GatewayIntents.None 
    });

    private readonly DiscordCommandDispatcher _discordCommandDispatcher = new DiscordCommandDispatcher();

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        _client.Log += Log;
        _client.Ready += ClientReady;
        _client.SlashCommandExecuted += _discordCommandDispatcher.ProcessDiscordCommand;

        var token = Environment.GetEnvironmentVariable("WORDLE_DISCORD_TOKEN");
        if (token == null) 
        {
            Console.WriteLine("Could not start bot - token missing");
            Console.WriteLine("WORDLE_DISCORD_TOKEN env variable not set");
            Environment.Exit(1);
        }

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private async Task ClientReady() {
        var wordleCommand = new SlashCommandBuilder()
            .WithName("wordle")
            .WithDescription("Wordle - gra w zgadywanie słów")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("start")
                .WithDescription("Rozpocznij grę na bieżącym kanale tekstowym")
                .WithType(ApplicationCommandOptionType.SubCommand)
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("end")
                .WithDescription("Zatrzymaj grę na bieżącym kanale tekstowym")
                .WithType(ApplicationCommandOptionType.SubCommand)
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("restart")
                .WithDescription("Restartuj grę")
                .WithType(ApplicationCommandOptionType.SubCommand)
            )
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("guess")
                .WithDescription("Odgadnij słowo")
                .WithType(ApplicationCommandOptionType.SubCommand)
                .AddOption("phrase", ApplicationCommandOptionType.String, "Słowo które chcesz odgadnąć", isRequired: true, minLength: 5, maxLength: 5)
            );

        try
        {
            await _client.CreateGlobalApplicationCommandAsync(wordleCommand.Build());
        }
        catch(HttpException exception)
        {
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
