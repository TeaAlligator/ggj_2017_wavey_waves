using System;
using UnityEngine;

namespace Assets.Code.Audio
{
    public class PooledAudioRequest
    {
        public AudioClip Sound;
        public float Volume;
        public Vector3 Target;
        public bool IsLoop;
        public PooledAudioRequest Next;

        public Action OnFinished;

        public PooledAudioRequest()
        {
            Volume = 1f;
            IsLoop = false;
        }
    }

    public class AudioToken
    {
        public bool IsCurrentlyActive;

        public Action<float> AdjustVolume;
        public Func<PooledAudioRequest, AudioToken> Replace;
        public Action End;
        public Action<float> TrailOff;
    }

    public class AudioPooler : MonoBehaviour, IResolveable
    {
        [SerializeField] private GameObject _audioSourcePrefab;
        [SerializeField] private Transform _sourceParent;
        private readonly PooledAudioSource[] _sources;

        private const int NumberOfSources = 32;

        protected void Awake()
        {
            for (var i = 0; i < NumberOfSources; i++)
            {
                _sources[i] = Instantiate(_audioSourcePrefab).GetComponent<PooledAudioSource>();
                _sources[i].transform.SetParent(_sourceParent.transform);
            }
        }

        public AudioToken PlaySound(PooledAudioRequest request)
        {
            for (var i = 0; i < NumberOfSources; i++)
                if (!_sources[i].IsActive)
                {
                    var source = _sources[i];
                    return source.PlaySound(request);
                }
            
            return new AudioToken
            {
                IsCurrentlyActive = false,

                Replace = newRequest => PlaySound(newRequest),
                End = () => { },
                TrailOff = time => { },
                AdjustVolume = volume => { }
            };
        }

        public void KillAllSounds()
        {
            foreach (var source in _sources)
                source.Stop();
        }
    }
}
