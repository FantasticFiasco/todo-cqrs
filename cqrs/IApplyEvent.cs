namespace Cqrs
{
    /// <summary>
    /// Implemented by an aggregate once for each event type it can apply.
    /// </summary>
    public interface IApplyEvent<in TEvent>
    {
        void Apply(TEvent e);
    }
}
