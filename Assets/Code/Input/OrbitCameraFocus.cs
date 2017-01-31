using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraFocus : MonoBehaviour
    {
        [SerializeField] public Transform _facingTransform;
        [SerializeField] private float _panSensitivity = 0.5f;

        private bool _isLerpingPosition;
        private Vector3 _positionLerpOld;
        [SerializeField] private Vector3 _positionLerpTarget;
        private float _positionLerpProgress;
        private float _positionLerpTime = 1.0f;

        private bool _isPanningPosition;

        protected void Awake()
        {
            _positionLerpTarget = transform.localPosition;
        }

        protected void LateUpdate()
        {
            if (_isLerpingPosition) HandleLerpZoom();
            else if (_isPanningPosition) HandlePan();
        }

        public void StartPan()
        {
            EndLerpZoom();
            _isPanningPosition = true;
        }

        private void HandlePan()
        {
            var delta =
                _facingTransform.TransformDirection(-new Vector3(UnityEngine.Input.GetAxis("Mouse X"), 0,
                    UnityEngine.Input.GetAxis("Mouse Y")));
            delta.y = 0f;
            delta.Normalize();
            delta = new Vector3(delta.x * _panSensitivity, 0, delta.z * _panSensitivity * (Screen.width / (float) Screen.height));

            transform.localPosition += delta;
        }

        public void EndPan()
        {
            _isPanningPosition = false;
        }

        public void SetPosition(Vector3 position)
        {
            _positionLerpTarget = position;
            transform.localPosition = position;
        }

        public void StartLerpPosition(Vector3 position, float time)
        {
            if (_isPanningPosition) return;

            _isLerpingPosition = true;

            _positionLerpTarget = position;
            _positionLerpOld = transform.localPosition;

            _positionLerpProgress = 0f;
            _positionLerpTime = time;
        }

        private void HandleLerpZoom()
        {
            _positionLerpProgress += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(_positionLerpOld, _positionLerpTarget,
                _positionLerpProgress / _positionLerpTime);

            if (_positionLerpProgress > _positionLerpTime) EndLerpZoom();
        }

        private void EndLerpZoom()
        {
            _isLerpingPosition = false;
        }
    }
}
