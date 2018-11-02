using System.Collections;

namespace Cqrs
{
    public interface IHandleCommand<in TCommand>
    {
        IEnumerable Handle(TCommand c);
    }
}
