using UnityEngine;

namespace Assets.Code.Input
{
    class GroundRaycaster : MonoBehaviour, IResolveable
    {
        [SerializeField] private Camera _camera;

        private static Plane _xzPlane = new Plane(Vector3.up, Vector3.zero);

        public Vector3 GetMouseGroundPosition(Vector2 screenspace)
        {
            float distance;
            var ray = _camera.ScreenPointToRay(screenspace);

            if (_xzPlane.Raycast(ray, out distance))
            {
                var hit = ray.GetPoint(distance);
                hit.y = 0f;

                return hit;
            }

            // really id like to return a bool or something
            // to signify if the trace was successful
            // but with the isometric camera i cant think of a way
            // to click on something other than the xz plane

            // so i dont really care
            return Vector3.zero;
        }
    }
}
