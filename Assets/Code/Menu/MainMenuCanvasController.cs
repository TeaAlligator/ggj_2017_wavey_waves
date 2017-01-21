using Assets.Code.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu
{
    class MainMenuSession {}

    class MainMenuCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private HostOptionsCanvasController _hostCanvas;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _findButton;
        [SerializeField] private Button _exitButton;
        
        private MainMenuSession _session;

        protected void Awake()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _findButton.onClick.AddListener(OnFindClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
        }

        public void StartSession(MainMenuSession session)
        {
            if (_session != null) return;

            ShowCanvas();

            _session = session;
        }

        private void OnHostClicked()
        {
            HideCanvas();

            _hostCanvas.StartSession(new HostOptionsSession
            {
                OnConfirmed = result =>
                {
                    // TODO: START SERVER
                },
                OnCancelled = () =>
                {
                    ShowCanvas();
                }
            });
        }

        private void OnFindClicked()
        {

        }

        private void OnExitClicked()
        {
            Application.Quit();
        }

        private void ShowCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }

        private void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
