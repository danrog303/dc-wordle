namespace WordleDiscord.Game.Exception;

class GameNotStartedException : System.Exception
{
    public GameNotStartedException()
    {
    }

    public GameNotStartedException(string message): base(message)
    {
    }

    public GameNotStartedException(string message, System.Exception inner): base(message, inner)
    {
    }
}
