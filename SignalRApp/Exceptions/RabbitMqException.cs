namespace SignalRApp.Exceptions
{
    public class RabbitMqException : Exception
    {
        public RabbitMqException(string message) : base(message) { }
    }
}
