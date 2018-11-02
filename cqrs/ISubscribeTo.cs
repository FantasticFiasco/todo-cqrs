namespace Cqrs
{
    /// <summary>
    /// Implemented by anything that wishes to subscribe to an event emitted by
    /// an aggregate and successfully stored.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface ISubscribeTo<in TEvent>
    {
        void Handle(TEvent e);
    }
}
