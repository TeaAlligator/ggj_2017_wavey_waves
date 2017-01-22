using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraController : MonoBehaviour
    {
        [SerializeField] private OrbitCameraFocus _focus;
        [SerializeField] private OrbitCameraDolly _dolly;
        [SerializeField] private OrbitCameraPivot _pivot;

        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 50f;
        [SerializeField] private float _zoomStrength = 1f;
        [SerializeField] private float _zoomLerpSpeed = 1f;
        [SerializeField] private float _positionLerpSpeed = 1f;

        [AutoResolve] private GroundRaycaster _groundRay;

        protected void Awake()
        {
            Resolver.AutoResolve(this);
        }

        protected void Update()
        {
            if (UnityEngine.Input.GetButtonDown("move_camera"))
            {
                _focus.StartLerpPosition(_groundRay.GetMouseGroundPosition(UnityEngine.Input.mousePosition),
                    _positionLerpSpeed);
            }
            if (UnityEngine.Input.GetButtonDown("rotate_camera"))
            {
                _pivot.EnableRotation();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (UnityEngine.Input.GetButtonUp("rotate_camera"))
            {
                _pivot.DisableRotation();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            if (UnityEngine.Input.mousePosition.x >= 0 && UnityEngine.Input.mousePosition.x <= Screen.width &&
                UnityEngine.Input.mousePosition.y >= 0 && UnityEngine.Input.mousePosition.y <= Screen.height)
                HandleZoomCamera();
        }

        private void HandleZoomCamera()
        {
            if (UnityEngine.Input.mouseScrollDelta.y > 0 || UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                var target = _dolly.MechanicalZoom - _zoomStrength;
                target = Mathf.Clamp(target, _minZoom, _maxZoom);

                _dolly.StartLerpZoom(target, _zoomLerpSpeed);
            }
            if (UnityEngine.Input.mouseScrollDelta.y < 0 || UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                var target = _dolly.MechanicalZoom + _zoomStrength;
                target = Mathf.Clamp(target, _minZoom, _maxZoom);

                _dolly.StartLerpZoom(target, _zoomLerpSpeed);
            }
        }
    }
}
