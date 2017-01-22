using UnityEngine;

namespace Assets.Code.Extensions
{
    public static class AngleMath
    {
        // http://stackoverflow.com/a/19684901
        public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
        {
            // angle in [0,180]
            float angle = Vector3.Angle(a, b);
            float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

            // angle in [-179,180]
            float signedAngle = angle * sign;

            // angle in [0,360] (not used but included here for completeness)
            //float angle360 =  (signed_angle + 180) % 360;

            return signedAngle;
        }
    }
}
