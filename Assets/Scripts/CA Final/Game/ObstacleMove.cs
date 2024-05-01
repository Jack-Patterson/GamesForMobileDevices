using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Game
{
    public class ObstacleMove : MonoBehaviour
    {
        public float speed;

        void Update()
        {
            transform.Translate(Vector3.back * (speed * Time.deltaTime), Space.World);
            
            if (transform.position.z < -10)
            {
                Destroy(gameObject);
            }
        }
    }
}