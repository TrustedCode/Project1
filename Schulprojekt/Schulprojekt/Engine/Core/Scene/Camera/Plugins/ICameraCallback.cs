using OpenTK;

namespace Animation_Engine.Engine.Core.Scene.Camera.Plugins
{
    public interface ICameraMovementCallback
    {
        /// <summary>
        /// Modifies the movement
        /// </summary>
        /// <param name="viewPosition"></param>
        /// <param name="motionVec"></param>
        /// <returns>True if modified</returns>
        bool Move(ref Vector3 viewPosition, Vector3 motionVec);

        /// <summary>
        /// Modifies the movement
        /// </summary>
        /// <param name="viewPosition"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        /// <returns>True if modified</returns>
        bool Move(ref Vector3 viewPosition, Vector3 direction, float speed);
    }

    public interface ICameraRotationCallback
    {
        /// <summary>
        /// Modifies the rotation
        /// </summary>
        /// <param name="x"></param>
        /// <returns>True if modified</returns>
        bool RotateX(ref Quaternion _quat, float x);

        /// <summary>
        /// Modifies the rotation
        /// </summary>
        /// <param name="y"></param>
        /// <returns>True if modified</returns>
        bool RotateY(ref Quaternion _quat, float y);

        /// <summary>
        /// Modifies the rotation
        /// </summary>
        /// <param name="z"></param>
        /// <returns>True if modified</returns>
        bool RotateZ(ref Quaternion _quat, float z);

        /// <summary>
        /// Modifies the rotation
        /// </summary>
        /// <param name="vec"></param>
        /// <returns>True if modified</returns>
        bool Rotate(ref Quaternion _quat, Vector3 vec);

        /// <summary>
        /// Modifies the rotation
        /// </summary>
        /// <param name="quat"></param>
        /// <returns>True if modified</returns>
        bool Rotate(ref Quaternion _quat, Quaternion quat);
    }
}
