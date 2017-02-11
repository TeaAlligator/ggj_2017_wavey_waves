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
        private bool _firingLock;
        private readonly Dictionary<Guid, Action> _fireList;

        private readonly HashSet<Guid> _removeQueue;

        public SubscribedEvent()
        {
            _firingLock = false;

            _fireList = new Dictionary<Guid, Action>();
            _removeQueue = new HashSet<Guid>();
        }

        public void Invoke()
        {
            _firingLock = true;
            
            foreach(var item in _fireList)
                if (!_removeQueue.Contains(item.Key))
                    item.Value.Invoke();

            _firingLock = false;

            if (_removeQueue.Count > 0)
            {
                foreach (var id in _removeQueue)
                    _fireList.Remove(id);

                _removeQueue.Clear();
            }
        }

        public SubscribedEventToken Subscribe(Action call)
        {
            var id = Guid.NewGuid();
            _fireList.Add(id, call);

            return new SubscribedEventToken { Cancel = () => RemoveSubscription(id) };
        }

        private void RemoveSubscription(Guid id)
        {
            if (_firingLock) _removeQueue.Add(id);
            else _fireList.Remove(id);
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
