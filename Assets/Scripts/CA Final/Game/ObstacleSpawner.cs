using UnityEngine;

namespace GamesForMobileDevices.CA_Final.Game
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public GameObject[] obstacles;
        public float spawnRate = 2f;
        private float _nextSpawnTime = 0f;
        public float obstacleSpeed = 5f;

        void Update()
        {
            if (Time.time >= _nextSpawnTime && !GameManager.instance.IsGameOver)
            {
                SpawnObstacle();
                _nextSpawnTime = Time.time + spawnRate;
            }
        }

        void SpawnObstacle()
        {
            GameObject prefabToSpawn = obstacles[Random.Range(0, obstacles.Length)];
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 0, transform.position.z);
            GameObject obstacle = Instantiate(prefabToSpawn, positionToSpawn,
                prefabToSpawn.transform.rotation);
            obstacle.AddComponent<ObstacleMove>().speed = obstacleSpeed;
        }
    }
}