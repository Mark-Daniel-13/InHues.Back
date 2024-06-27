namespace InHues.Domain.Persistence
{
    public interface IErrorLogger
    {
        public void LogError(string message, string description);
    }
}
