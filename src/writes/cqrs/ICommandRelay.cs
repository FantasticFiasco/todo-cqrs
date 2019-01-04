namespace Cqrs
{
    public interface ICommandRelay
    {
        void RegisterHandlerFor<TCommand, TAggregate>()
            where TAggregate : Aggregate, new();

        void RegisterHandlersFor<TAggregate>()
            where TAggregate : Aggregate, new();

        void RegisterPublisherFor<TEvent>(IPublisher<TEvent> publisher);

        void RegisterPublishersFor(object instance);

        void SendCommand<TCommand>(TCommand command);
    }
}
