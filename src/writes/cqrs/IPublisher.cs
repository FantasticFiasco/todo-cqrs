namespace Cqrs
{
    /// <summary>
    /// Interface responsible for publishing events of a particular type.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type to publish.
    /// </typeparam>
    public interface IPublisher<in TEvent>
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        void Publish(TEvent e);
    }
}
