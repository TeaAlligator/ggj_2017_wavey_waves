using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraController : MonoBehaviour
    {
        [SerializeField] private OrbitCameraDolly _dolly;
        [SerializeField] private OrbitCameraPivot _pivot;

        [SerializeField] private float _minZoom = 3f;
        [SerializeField] private float _maxZoom = 20f;
        [SerializeField] private float _zoomStrength = 1f;
        [SerializeField] private float _zoomLerpSpeed = 3f;

        protected void Update()
        {
            if (UnityEngine.Input.GetButtonDown("rotate_camera"))
            {
                _pivot.EnableRotation();
                Cursor.visible = false;
            }
            if (UnityEngine.Input.GetButtonUp("rotate_camera"))
            {
                _pivot.DisableRotation();
                Cursor.visible = true;
            }
            if (UnityEngine.Input.mousePosition.x >= 0 && UnityEngine.Input.mousePosition.x <= Screen.width &&
                UnityEngine.Input.mousePosition.y >= 0 && UnityEngine.Input.mousePosition.y <= Screen.height)
                HandleZoomCamera();
        }

        private void HandleZoomCamera()
        {
            if (UnityEngine.Input.mouseScrollDelta.y > 0)
            {
                var target = _dolly.MechanicalZoom - _zoomStrength;
                target = Mathf.Clamp(target, _minZoom, _maxZoom);

                _dolly.StartLerpZoom(target, _zoomLerpSpeed);
            }
            if (UnityEngine.Input.mouseScrollDelta.y < 0)
            {
                var target = _dolly.MechanicalZoom + _zoomStrength;
                target = Mathf.Clamp(target, _minZoom, _maxZoom);

                _dolly.StartLerpZoom(target, _zoomLerpSpeed);
            }
        }
    }
}
