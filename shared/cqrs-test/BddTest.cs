using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Shouldly;

namespace Cqrs
{
    /// <summary>
    /// Provides infrastructure for a set of tests on a given aggregate.
    /// </summary>
    public class BddTest<TAggregate>
        where TAggregate : Aggregate, new()
    {
        private TAggregate sut;

        public void BddTestSetup()
        {
            sut = new TAggregate();
        }

        protected void Test(IEnumerable given, Func<TAggregate, object> when, Action<object> then)
        {
            then(when(ApplyEvents(sut, given)));
        }

        protected IEnumerable Given(params object[] events)
        {
            return events;
        }

        protected Func<TAggregate, object> When<TCommand>(TCommand command)
        {
            return agg =>
            {
                try
                {
                    return DispatchCommand(command).Cast<object>().ToArray();
                }
                catch (Exception e)
                {
                    return e;
                }
            };
        }

        protected Action<object> Then(params object[] expectedEvents)
        {
            return got =>
            {
                if (got is object[] gotEvents)
                {
                    if (gotEvents.Length == expectedEvents.Length)
                        for (var i = 0; i < gotEvents.Length; i++)
                            if (gotEvents[i].GetType() == expectedEvents[i].GetType())
                                Serialize(gotEvents[i]).ShouldBe(Serialize(expectedEvents[i]));
                            else
                                throw new Exception($"Incorrect event in results; expected a {expectedEvents[i].GetType().Name} but got a {gotEvents[i].GetType().Name}");
                    else if (gotEvents.Length < expectedEvents.Length)
                        throw new Exception($"Expected event(s) missing: {string.Join(", ", EventDiff(expectedEvents, gotEvents))}");
                    else
                        throw new Exception($"Unexpected event(s) emitted: {string.Join(", ", EventDiff(gotEvents, expectedEvents))}");
                }
                else if (got is CommandHandlerNotDefinedException exception)
                    throw new Exception(exception.Message);
                else
                    throw new Exception($"Expected events, but got exception {got.GetType().Name}");
            };
        }

        private string[] EventDiff(object[] a, object[] b)
        {
            var diff = a.Select(e => e.GetType().Name).ToList();

            foreach (var remove in b.Select(e => e.GetType().Name))
                diff.Remove(remove);

            return diff.ToArray();
        }

        protected Action<object> ThenFailWith<TException>()
        {
            return got =>
            {
                switch (got)
                {
                    case TException _:
                        break;
                    case Exception _:
                        throw new Exception($"Expected exception {typeof(TException).Name}, but got exception {got.GetType().Name}");
                    default:
                        throw new Exception($"Expected exception {typeof(TException).Name}, but got event result");
                }
            };
        }

        private IEnumerable DispatchCommand<TCommand>(TCommand command)
        {
            if (!(sut is IHandleCommand<TCommand> handler))
                throw new CommandHandlerNotDefinedException($"Aggregate {sut.GetType().Name} does not yet handle command {command.GetType().Name}");

            return handler.Handle(command);
        }

        private static TAggregate ApplyEvents(TAggregate agg, IEnumerable events)
        {
            agg.ApplyEvents(events);

            return agg;
        }

        private static string Serialize(object obj)
        {
            var ser = new XmlSerializer(obj.GetType());
            var ms = new MemoryStream();
            ser.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);

            return new StreamReader(ms).ReadToEnd();
        }

        private class CommandHandlerNotDefinedException : Exception
        {
            public CommandHandlerNotDefinedException(string msg) : base(msg) { }
        }
    }
}
