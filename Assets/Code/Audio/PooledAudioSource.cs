using UnityEngine;

namespace Assets.Code.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;

        /* PROPERTIES */
        private PooledAudioRequest _currentRequest;
        public bool IsActive { get { return _currentRequest != null; } }

        // lerpy loo
        private bool _isTrailingOff;
        private float _trailOffTime;
        private float _trailOffProgress;
        
        public AudioToken PlaySound(PooledAudioRequest request)
        {
            _isTrailingOff = false;
            if (_currentRequest != null && _currentRequest.OnFinished != null)
                _currentRequest.OnFinished();
            _currentRequest = request;

            // bail gracefully if request is null
            if (request == null)
                return new AudioToken
                {
                    IsCurrentlyActive = false,

                    Replace = replacementRequest => PlaySound(replacementRequest),
                    End = () => { },
                    TrailOff = time => { },
                    AdjustVolume = volume => { }
                };

            _audio.loop = request.Next == null && request.IsLoop;
            _audio.transform.position = request.Target;

            _audio.clip = request.Sound;
            _audio.volume = request.Volume;
            _audio.Play();

            var token = new AudioToken
            {
                IsCurrentlyActive = true,

                Replace = replacementRequest => PlaySound(replacementRequest),
                TrailOff = time => TrailOff(time),
                AdjustVolume = volume =>
                {
                    if (_currentRequest != null)
                    {
                        _currentRequest.Volume = volume;
                        _audio.volume = _currentRequest.Volume;
                    }
                }
            };
            token.End = () =>
            {
                Stop();
                token.IsCurrentlyActive = false;
            };
            return token;
        }

        public void Stop()
        {
            _isTrailingOff = false;
            _audio.Stop();
            _currentRequest = null;
        }

        public void TrailOff(float time)
        {
            if (_isTrailingOff) return;

            _isTrailingOff = true;
            _trailOffProgress = 0f;
            _trailOffTime = time;
        }
        
        protected void Update()
        {
            if (_currentRequest == null) return;

            if (_isTrailingOff)
            {
                _audio.volume = _currentRequest.Volume * (1 - (_trailOffProgress / _trailOffTime));
                _trailOffProgress += Time.deltaTime;

                if (_trailOffProgress >= _trailOffTime)
                {
                    if (_currentRequest.OnFinished != null)
                        _currentRequest.OnFinished();
                    Stop();
                }
            }

            // once finished, we move on to the next sound
            // or it will be null (both are cool)
            else if (!_audio.isPlaying)
            {
                if (_currentRequest.Next != null)
                    PlaySound(_currentRequest.Next);
                else if (_currentRequest.OnFinished != null)
                    _currentRequest.OnFinished();
                else
                    Stop();
            }
        }
    }
}
