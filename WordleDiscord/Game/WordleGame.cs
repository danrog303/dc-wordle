
namespace WordleDiscord.Game;

class WordleGame 
{
    public ulong DiscordGuildId {get; set;}
    public ulong DiscordChannelId {get; set;}
    public DateTime CreationDate {get; set;}
    public DateTime LastActionDate {get; set;}
    public string CorrectPhrase {get; set;}
    public List<string> Guesses {get; set;}
    public bool Aborted {get; set;}
    public bool Won {get; set;}

    public WordleGame(ulong discordGuildId, ulong discordChannelId, string correctPhrase) {
        DiscordGuildId = discordGuildId;
        DiscordChannelId = discordChannelId;
        CreationDate = new DateTime();
        LastActionDate = new DateTime();
        CorrectPhrase = correctPhrase;
        Guesses = [];
        Aborted = false;
        Won = false;
    }
}
