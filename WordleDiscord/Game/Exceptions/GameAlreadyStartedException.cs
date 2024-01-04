namespace WordleDiscord.Game.Exception;

class GameAlreadyStartedException : System.Exception
{
    public GameAlreadyStartedException()
    {
    }

    public GameAlreadyStartedException(string message): base(message)
    {
    }

    public GameAlreadyStartedException(string message, System.Exception inner): base(message, inner)
    {
    }
}
