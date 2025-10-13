using System.Collections.Concurrent;

namespace Base.Abstractions.Messaging
{
    public class SimpleMessageCenter : IMessagesCenter
    {
        private readonly ConcurrentDictionary<Type, object> _events = new();

        public TEvent GetEvent<TEvent>() where TEvent : class, new()
        {
            var eventType = typeof(TEvent);

            // Try to get an existing instance
            if (_events.TryGetValue(eventType, out var existingEvent))
                return (TEvent)existingEvent;

            // Otherwise, create and store a new instance
            var newEvent = new TEvent();
            _events[eventType] = newEvent;

            return newEvent;
        }
    }

    public class SubMessage<T> : IMessageEvent<T>
    {
        private readonly List<Action<T>> _handlers = new();
        private readonly object _lock = new();

        public void Subscribe(Action<T> handler)
        {
            lock (_lock)
            {
                if (!_handlers.Contains(handler))
                    _handlers.Add(handler);
            }
        }       

        public void Unsubscribe(Action<T> handler)
        {
            lock (_lock)
            {
                _handlers.Remove(handler);
            }
        }

        public void Publish(T args)
        {
            List<Action<T>> handlersCopy;
            lock (_lock)
            {
                // Make a shallow copy of the handlers list.
                // This prevents InvalidOperationException if a handler subscribes/unsubscribes
                // during event publication, since modifying a collection while iterating
                // over it is not allowed in .NET (throws exception Collection modified in foreach).
                handlersCopy = _handlers.ToList();
            }

            // Invoke handlers outside the lock to avoid blocking new subscriptions
            // while event delivery is happening.
            foreach (var handler in handlersCopy)
                handler(args);
        }
    }

    
}
