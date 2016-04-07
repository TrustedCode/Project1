using System;
using OpenTK;

namespace Animation_Engine.Engine.Core.MathUtil
{
    public class MathHelperExt
    {
        public static readonly Random RANDOM = new Random();
        public static readonly float TO_RADIANS = (float)(1f/180f*System.Math.PI);

        public static Vector3 QuaternionToVector3(Quaternion q)
        {
            return new Vector3( (float)System.Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z),
                                (float)System.Math.Asin(2f * (q.X * q.Z + q.W * q.Y)),
                                (float)System.Math.Atan2(-2f * (q.X * q.Y - q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));
        }

        public static Quaternion Vector3ToQuaternion(Vector3 vec)
        {
            float c1 = (float)System.Math.Cos(vec.X / 2);
            float s1 = (float)System.Math.Sin(vec.X / 2);
            float c2 = (float)System.Math.Cos(vec.Y / 2);
            float s2 = (float)System.Math.Sin(vec.Y / 2);
            float c3 = (float)System.Math.Cos(vec.Z / 2);
            float s3 = (float)System.Math.Sin(vec.Z / 2);
            return new Quaternion(  c1*c2*c3-s1*s2*s3,
                                    s1*c2*c3+c1*s2*s3,
                                    c1*s2*c3+s1*c2*s3,
                                    c1*c2*s3+s1*s2*c3);
        }
    }
}
