using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Collider2D[] mapBoundsColliders;

    [Header("Spawn Timing")]
    [SerializeField] private float initialSpawnInterval = 4f;
    [SerializeField] private float minSpawnInterval = 0.75f;
    [SerializeField] private float intervalDecreasePerSecond = 0.05f;

    [Header("Spawn Rules")]
    [SerializeField] private float minDistanceFromPlayer = 3f;
    [SerializeField] private int maxSpawnAttempts = 20;

    private float spawnTimer;
    private float currentSpawnInterval;
    private Bounds combinedMapBounds;
    private bool hasValidMapBounds;

    private void Start()
    {
        if (playerPosition == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPosition = player.transform;
            }
        }

        hasValidMapBounds = TryGetCombinedBounds(out combinedMapBounds);

        currentSpawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval);
        spawnTimer = currentSpawnInterval;
    }

    private void Update()
    {
        if (enemyPrefab == null || !hasValidMapBounds)
        {
            return;
        }

        currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - (intervalDecreasePerSecond * Time.deltaTime));

        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f)
        {
            return;
        }

        TrySpawnEnemy();
        spawnTimer = currentSpawnInterval;
    }

    private void TrySpawnEnemy()
    {
        for (int i = 0; i < Mathf.Max(1, maxSpawnAttempts); i++)
        {
            Vector3 spawnPosition = GetRandomPointInBounds(combinedMapBounds);

            if (IsTooCloseToPlayer(spawnPosition))
            {
                continue;
            }

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            return;
        }
    }

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0f);
    }

    private bool IsTooCloseToPlayer(Vector3 spawnPosition)
    {
        if (playerPosition == null)
        {
            return false;
        }

        return Vector2.Distance(spawnPosition, playerPosition.position) < minDistanceFromPlayer;
    }

    private bool TryGetCombinedBounds(out Bounds bounds)
    {
        bounds = default;
        if (mapBoundsColliders == null || mapBoundsColliders.Length == 0)
        {
            return false;
        }

        bool hasBounds = false;
        foreach (Collider2D mapCollider in mapBoundsColliders)
        {
            if (mapCollider == null)
            {
                continue;
            }

            if (!hasBounds)
            {
                bounds = mapCollider.bounds;
                hasBounds = true;
            }
            else
            {
                bounds.Encapsulate(mapCollider.bounds);
            }
        }

        return hasBounds;
    }
}
