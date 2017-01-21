using UnityEngine;

namespace Assets.Code.Input
{
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
