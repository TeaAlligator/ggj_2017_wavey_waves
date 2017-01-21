using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraFocus : MonoBehaviour
    {
        private bool _isLerpingPosition;
        private Vector3 _positionLerpOld;
        [SerializeField] private Vector3 _positionLerpTarget;
        private float _positionLerpProgress;
        private float _positionLerpTime = 1.0f;

        protected void Awake()
        {
            _positionLerpTarget = transform.localPosition;
        }

        protected void Update()
        {
            if (_isLerpingPosition) HandleLerpZoom();
        }

        public void SetPosition(Vector3 position)
        {
            _positionLerpTarget = position;
            transform.localPosition = position;
        }

        public void StartLerpPosition(Vector3 position, float time)
        {
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
            transform.localPosition = _positionLerpTarget;
        }
    }
}
