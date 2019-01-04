namespace Cqrs
{
    /// <summary>
    /// TODO: Rewrite
    /// </summary>
    /// <typeparam name="TEvent">
    /// TODO: Rewrite
    /// </typeparam>
    public interface IPublisher<in TEvent>
    {
        /// <summary>
        /// TODO: Rewrite
        /// </summary>
        /// <param name="e">TODO: Rewrite</param>
        void Publish(TEvent e);
    }
}
