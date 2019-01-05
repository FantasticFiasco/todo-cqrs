namespace Cqrs
{
    /// <summary>
    /// TODO: Write
    /// </summary>
    public interface ICommandRelay
    {
        /// <summary>
        /// Registers an aggregate as being the handler for a particular command.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling the particular command.
        /// </typeparam>
        void RegisterHandlerFor<TCommand, TAggregate>()
            where TAggregate : Aggregate, new();

        /// <summary>
        /// Registers an aggregate as being the handler for commands defined by all its
        /// implementations of <see cref="IHandleCommand{T}"/>.
        /// </summary>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling commands.
        /// </typeparam>
        void RegisterHandlersFor<TAggregate>()
            where TAggregate : Aggregate, new();

        /// <summary>
        /// Registers a publisher for a particular event produced by a command invocation.
        /// <para />
        /// Please note that the preferred architecture is to use an event store that has the
        /// capability of publishing events, or at least provide a stream of events a publisher can
        /// read from. The negative aspect of registering a publisher using this method is that
        /// persisting and publishing an event are two separate operations, not protected by a
        /// transaction, and thus not being atomic in nature. The application might terminate after
        /// the event has been persisted but before the event has been published, leading to a
        /// forever inconsistent read model. Only use this method if that scenario is acceptable
        /// according to your requirements.
        /// </summary>
        void RegisterPublisherFor<TEvent>(IPublisher<TEvent> publisher);

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Looks at the specified object instance, examples what commands it handles
        /// or events it subscribes to, and registers it as a receiver/subscriber.
        /// </summary>
        void RegisterPublishersFor(object instance);

        /// <summary>
        /// Invokes the intent of a command by sending it to its registered handler.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command.
        /// </typeparam>
        void SendCommand<TCommand>(TCommand command);
    }
}
