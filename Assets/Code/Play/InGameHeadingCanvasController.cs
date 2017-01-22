using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Play
{
    class InGameHeadingSession
    {
        public Action OnExit;
    }

    class InGameHeadingCanvasController : CanvasController
    {
        [SerializeField] private Button _exitButton;

        private InGameHeadingSession _session;

        protected override void Awake()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);

            base.Awake();
        }

        public void StartSession(InGameHeadingSession session)
        {
            if (_session != null)
            {
                session.OnExit();
                return;
            }

            _session = session;

            ShowCanvas();
        }

        private void OnExitButtonClicked()
        {
            var exitCall = _session.OnExit;
            CloseSession();

            exitCall();
        }

        public override void CloseSession()
        {
            base.CloseSession();
            _session = null;
        }
    }
}
