using Discord;

namespace WordleDiscord.Discord;

public static class DiscordEmbedMaker
{
    public static EmbedBuilder MakeErrorEmbed(string msg)
    {
        return GetCommonEmbedBuilder()
            .AddField("Błąd!", msg)
            .WithColor(Color.Red);
    }

    public static EmbedBuilder MakeSuccessEmbed()
    {
        return GetCommonEmbedBuilder()
            .WithColor(Color.Green);
    }

    private static EmbedBuilder GetCommonEmbedBuilder()
    {
        const string footerImage = "https://github.com/danrog303/danrog303/assets/32397526/5420bddc-b324-4e10-b9ff-a8d242364538";
        return new EmbedBuilder()
            .WithTitle("Wordle")
            .WithDescription("Wordle - gra w zgadywanie słów")
            .WithFooter("discord-wordle by Daniel Rogowski & Anna Czarnecka", footerImage);
    }
}
