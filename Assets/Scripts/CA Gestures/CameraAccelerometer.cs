using UnityEngine;

namespace GamesForMobileDevices.CA_Gestures
{
    public class CameraAccelerometer : MonoBehaviour
    {
        private readonly float _speed = 1f;

        private void Update()
        {
            if (TouchManager.instance.ShouldUseAccelerometer)
            {
                RotateCamera();
            }
        }

        private void RotateCamera()
        {
            Vector3 tilt = Input.acceleration;
            Quaternion target = Quaternion.Euler(-tilt.y * _speed * 90, tilt.x * _speed * 90, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * _speed);
        }
    }
}