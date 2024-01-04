namespace WordleDiscord.Phrases;

class PolishWordlePhraseProvider : IWordlePhraseProvider
{
    private readonly Random random = new Random();

    private readonly string[] Phrases = [
        "kolor",
        "trawa",
        "nóżka",
        "czapa",
        "fotel",
        "miska",
        "karta",
        "rurka",
        "obcas",
        "blask",
        "runda",
        "wiatr",
        "mosty",
        "długi",
        "grunt",
        "cegła",
        "zimno",
        "wyrok",
        "piłka",
        "głowa",
        "ptaki",
        "motyl",
        "lampa",
        "błoto",
        "ziele",
        "mydło",
        "słowo",
        "szafa",
        "worek",
        "dzban",
        "dzwon",
        "siano",
        "miska",
        "balon",
        "pszen",
        "pomoc",
        "chata",
        "wafel",
        "łóżko",
        "zimny",
        "twarz",
        "wieża",
        "cisza",
        "laska",
        "ranny",
        "basen",
        "krawa",
        "dachy",
        "słoma",
        "piana",
        "złoto",
        "słuch",
        "kłoda",
        "plama",
        "kawał",
        "brama",
        "syfon",
        "wilki",
        "pierś",
        "wiatr",
        "wózki",
        "błoto",
        "świat",
    ];

    public string GetRandomWordlePhrase()
    {
        int randomIndex = random.Next(0, Phrases.Length);
        return Phrases[randomIndex];
    }
}
