using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SpawnPointManager : MonoBehaviour
    {
        public static SpawnPointManager Instance { get; private set; }

        [SerializeField] private List<Transform> spawnPoints;

        public Transform GetDefault => transform;

        public Transform GetByIndexOrDefault(int index) =>
            (spawnPoints == null || index < 0 || index >= spawnPoints.Count) ? GetDefault : spawnPoints[index];

        public Transform GetRandomSpawnPoint => (spawnPoints == null || spawnPoints.Count == 0)
            ? GetDefault
            : spawnPoints[Random.Range(0, spawnPoints.Count)];

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}