using UnityEngine;

namespace Assets.Code
{
    abstract class CanvasController : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;

        protected virtual void Awake()
        {
            Resolver.AutoResolve(this);
        }

        protected virtual void ShowCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }

        protected virtual void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }

        protected virtual void CloseSession()
        {
            HideCanvas();
        }
    }
}
