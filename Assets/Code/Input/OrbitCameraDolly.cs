using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraDolly : MonoBehaviour
    {
        public float MechanicalZoom { get { return _zoomLerpTarget; } }

        private bool _isLerpingZoom = false;
        private float _zoomLerpOld = 0f;
        [SerializeField] private float _zoomLerpTarget = 0f;
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

            _zoomLerpTarget = zoom;
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
}
