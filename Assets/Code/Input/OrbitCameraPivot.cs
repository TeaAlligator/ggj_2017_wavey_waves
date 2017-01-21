using System;
using UnityEngine;

namespace Assets.Code.Input
{
    class OrbitCameraPivot : MonoBehaviour
    {
        [SerializeField] private float _minXAxis = 0f;
        [SerializeField] private float _maxXAxis = 90f;
        [SerializeField] private float _xAxis;
        [SerializeField] private float _yAxis;
        [SerializeField] private float _xRotationSensitivity = 0.5f;
        [SerializeField] private float _yRotationSensitivity = 0.5f;

        private const int SmoothingStrength = 8;
        private Vector2[] _smoothDeltas;

        private bool _isRotationEnabled;

        protected void Awake()
        {
            _smoothDeltas = new Vector2[SmoothingStrength];
        }

        public void DisableRotation() {_isRotationEnabled = false; }
        public void EnableRotation()
        {
            _isRotationEnabled = true;

            for (var i = 0; i < SmoothingStrength; i++)
            {
                _smoothDeltas[i] = Vector2.zero;
            }
        }

        protected void Update()
        {
            if (_isRotationEnabled) DoRotate();
        }
        
        private void DoRotate()
        {
            // add new delta
            var newDeltas = new Vector2[SmoothingStrength];
            Array.Copy(_smoothDeltas, 0, newDeltas, 1, SmoothingStrength - 1);
            newDeltas[0] = new Vector2 (
                UnityEngine.Input.GetAxis("Mouse X"),
                UnityEngine.Input.GetAxis("Mouse Y"));

            // get our smoothed delta for proper value
            var smoothedDelta = Vector2.zero;
            for (var i = 0; i < newDeltas.Length; i++)
                smoothedDelta += newDeltas[i];
            smoothedDelta /= newDeltas.Length;

            _xAxis += -smoothedDelta.y * _xRotationSensitivity;
            _yAxis += -smoothedDelta.x * _yRotationSensitivity;

            _xAxis = Mathf.Clamp(_xAxis, _minXAxis, _maxXAxis);

            transform.localRotation = Quaternion.Euler(_xAxis, _yAxis, 0f);
        }
    }
}
