using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Timing
{
    public class DelayedAction
    {
        public float RemainingTime { get; set; }
        public Action Payload { get; set; }
        public bool IgnorePaused { get; set; }
    }

    class Delayer : MonoBehaviour, IResolveable
    {
        [SerializeField] public bool IsPaused;
        private List<DelayedAction> _timeDelayedActions;

        protected void Awake()
        {
            _timeDelayedActions = new List<DelayedAction>();
        }

        protected void Update()
        {
            for (var i = 0; i < _timeDelayedActions.Count; i++)
            {
                if (IsPaused && !_timeDelayedActions[i].IgnorePaused) return;
                _timeDelayedActions[i].RemainingTime -= Time.deltaTime;

                if (_timeDelayedActions[i].RemainingTime <= 0)
                {
                    _timeDelayedActions[i].Payload();
                    _timeDelayedActions.RemoveAt(i);

                    i--;
                }
            }
        }

        public void Delay(Action action, float delayTime = 0f, bool ignorePaused = false)
        {
            _timeDelayedActions.Add(new DelayedAction
            {
                Payload = action,
                RemainingTime = delayTime,
                IgnorePaused = ignorePaused
            });
        }
    }
}
