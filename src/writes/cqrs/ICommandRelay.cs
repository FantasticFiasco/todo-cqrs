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
        /// Registers an aggregate as being the handler for all its implementations of
        /// <see cref="IHandleCommand{T}"/>.
        /// </summary>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling commands.
        /// </typeparam>
        void RegisterHandlersFor<TAggregate>()
            where TAggregate : Aggregate, new();

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Adds an object that subscribes to the specified event, by virtue of implementing the
        /// <see cref="IPublisher{TEvent}"/> interface.
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
        /// <exception cref="Exception">
        /// No registered handler for command is found.
        /// </exception>
        void SendCommand<TCommand>(TCommand command);
    }
}
