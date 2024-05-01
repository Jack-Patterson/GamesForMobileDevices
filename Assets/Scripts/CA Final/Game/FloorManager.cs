using UnityEngine;
using UnityEngine.Serialization;

namespace GamesForMobileDevices.CA_Final.Game
{
    public class FloorManager : MonoBehaviour
    {
        private const float Speed = 5f;
        private Vector3 _startPosition;
        private const float ResetPosition = -20f;
        private float _startPositionOffset = 10f;

        void Start()
        {
            _startPosition = new Vector3(0,0,80);
        }

        void Update()
        {
            transform.Translate(Vector3.back * (Speed * Time.deltaTime));
            
            if (transform.position.z <= ResetPosition)
            {
                transform.position = _startPosition;
            }
        }
    }
}
