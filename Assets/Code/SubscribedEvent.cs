using System;
using System.Collections.Generic;

namespace Assets.Code
{
    class SubscribedEventToken
    {
        public Action Cancel;
    }

    class SubscribedEvent
    {
        private readonly Dictionary<Guid, Action> _fireList;

        public SubscribedEvent()
        {
            _fireList = new Dictionary<Guid, Action>();
        }

        public void Fire()
        {
            foreach (var fire in _fireList)
                fire.Value();
        }

        public SubscribedEventToken Subscribe(Action call)
        {
            var id = Guid.NewGuid();
            _fireList.Add(id, call);

            return new SubscribedEventToken { Cancel = () => RemoveSubscription(id) };
        }

        private void RemoveSubscription(Guid id)
        {
            if (_fireList.ContainsKey(id))
                _fireList.Remove(id);
        }
    }

    class SubscribedEvent<T>
    {
        private readonly Dictionary<Guid, Action<T>> _fireList;

        public SubscribedEvent()
        {
            _fireList = new Dictionary<Guid, Action<T>>();
        }

        public void Fire(T data)
        {
            foreach (var fire in _fireList)
                fire.Value(data);
        }

        public SubscribedEventToken Subscribe(Action<T> call)
        {
            var id = Guid.NewGuid();
            _fireList.Add(id, call);

            return new SubscribedEventToken {Cancel = () => RemoveSubscription(id)};
        }

        private void RemoveSubscription(Guid id)
        {
            if (_fireList.ContainsKey(id))
                _fireList.Remove(id);
        }
    }
}
