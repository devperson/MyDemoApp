using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Common.Events
{
    public interface IEventAggregator
    {
        TEvent GetEvent<TEvent>() where TEvent : class, new();
    }

    public interface IEvent
    {
        void Subscribe(Action handler);
        void Unsubscribe(Action handler);
        void Publish();
    }

    public interface IEvent<T>
    {
        void Subscribe(Action<T> handler);
        void Unsubscribe(Action<T> handler);
        void Publish(T args);
    }

}
