using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraController : MonoBehaviour
    {
        [SerializeField] private OrbitCameraDolly _dolly;
        [SerializeField] private OrbitCameraFocus _focus;

        [SerializeField] private float _minZoom = 3f;
        [SerializeField] private float _maxZoom = 20f;
        [SerializeField] private float _zoomStrength = 1f;
        [SerializeField] private float _zoomLerpSpeed = 3f;

        protected void Update()
        {
            if (UnityEngine.Input.GetButton("rotate_camera")) HandleRotateCamera();
            if (UnityEngine.Input.mousePosition.x >= 0 && UnityEngine.Input.mousePosition.x <= Screen.width &&
                UnityEngine.Input.mousePosition.y >= 0 && UnityEngine.Input.mousePosition.y <= Screen.height)
                HandleZoomCamera();
        }

        private void HandleRotateCamera()
        {
            _focus.DoRotate();
        }

        private void HandleZoomCamera()
        {
            if (UnityEngine.Input.mouseScrollDelta.y > 0)
            {
                var target = _dolly.MechanicalZoom + _zoomStrength;
                target = Mathf.Clamp(target, _minZoom, _maxZoom);

                _dolly.StartLerpZoom(target, _zoomLerpSpeed);
            }
        }
    }

    class OrbitCameraDolly : MonoBehaviour
    {
        public float MechanicalZoom { get { return _zoomLerpTarget; } }

        private bool _isLerpingZoom = false;
        private float _zoomLerpOld = 0f;
        private float _zoomLerpTarget = 0f;
        private float _zoomLerpProgress = 0f;
        private float _zoomLerpTime = 1.0f;

        protected void Update()
        {
            if (_isLerpingZoom) HandleLerpZoom();
        }

        public void SetZoom(float zoom)
        {
            _zoomLerpTarget = zoom;
            transform.localPosition = new Vector3(0, 0, zoom);
        }

        public void StartLerpZoom(float zoom, float time)
        {
            _isLerpingZoom = true;

            _zoomLerpTarget = _zoomLerpTarget + zoom;
            _zoomLerpOld = transform.localPosition.z;

            _zoomLerpProgress = 0f;
            _zoomLerpTime = time;
        }

        private void HandleLerpZoom()
        {
            _zoomLerpProgress += Time.deltaTime;

            transform.localPosition = new Vector3(0, 0,
                Mathf.Lerp(_zoomLerpOld, _zoomLerpTarget, _zoomLerpProgress / _zoomLerpTime));

            if (_zoomLerpProgress > _zoomLerpTime) EndLerpZoom();
        }

        private void EndLerpZoom()
        {
            _isLerpingZoom = false;
            transform.localPosition = new Vector3(0, 0, _zoomLerpTarget);
        }
    }

    class OrbitCameraFocus : MonoBehaviour
    {
        private Vector2 _oldMousePosition = Vector2.zero;

        public void DoRotate()
        {
            var mousePosition = UnityEngine.Input.mousePosition;

            transform.Rotate(mousePosition.y - _oldMousePosition.y, mousePosition.x - _oldMousePosition.x, 0f);

            _oldMousePosition = UnityEngine.Input.mousePosition;
        }
    }
}
