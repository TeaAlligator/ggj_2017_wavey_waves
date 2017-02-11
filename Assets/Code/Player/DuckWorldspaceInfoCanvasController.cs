using Assets.Code.Play;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Player
{
    class DuckWorldspaceInfoCanvasController : CanvasController
    {
        [SerializeField] private Image _healthImageMechanical;
        [SerializeField] private Image _healthImageLerp;

        private DuckInfoSession _session;

        private SubscribedEventToken _onHealthChanged;

        private const float HealthLerpTime = 1.0f;
        private bool _isLerpingHealth;
        private float _healthLerpOld;
        private float _healthLerpTarget;
        private float _healthLerpProgress;

        protected override void Awake()
        {
            if (_session == null) // thanks, unity script execution order (Y)
                HideCanvas();

            base.Awake();
        }

        protected void Update()
        {
            if (_isLerpingHealth) HandleHealthLerp();
        }

        public void StartSession(DuckInfoSession session)
        {
            if (_session != null) return;
            
            _session = session;
            ShowCanvas();

            _onHealthChanged = _session.Subject.Stats.OnHealthChanged.Subscribe(OnSubjectHealthChanged);
        }

        private void OnSubjectHealthChanged(HealthChangedData data)
        {
            _healthImageMechanical.fillAmount = data.Percent;

            _isLerpingHealth = true;
            _healthLerpOld = _healthImageLerp.fillAmount;
            _healthLerpProgress = 0f;
            _healthLerpTarget = data.Percent;
        }

        private void HandleHealthLerp()
        {
            _healthLerpProgress += Time.deltaTime;

            _healthImageLerp.fillAmount = Mathf.Lerp(_healthLerpOld, _healthLerpTarget, _healthLerpProgress / HealthLerpTime);
        }

        public override void CloseSession()
        {
            if (_onHealthChanged != null) _onHealthChanged.Cancel();

            base.CloseSession();
            _session = null;
        }
    }
}
