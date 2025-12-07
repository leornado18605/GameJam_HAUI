using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private ObjectPooling pool;

    [Header("Player Settings")]
    [SerializeField] private LayerMask playerLayer;
    private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRadius = 5f;

    [Header("Block Areas")]
    public LayerMask houseLayer;     // Layer của House
    public float houseCheckRadius = 3f; // nếu có nhà trong bán kính này → ko spawn

    private Coroutine spawnRoutine;

    private void Start()
    {
        FindPlayerByLayer();
    }

    private void FindPlayerByLayer()
    {
        Collider[] hits = Physics.OverlapSphere(Vector3.zero, 1000f, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player!");
        }
    }

    private void OnEnable()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (player != null)
            {
                if (IsNearHouse())
                    continue; // ❌ Không spawn nếu gần House

                SpawnNearPlayer();
            }
        }
    }

    private bool IsNearHouse()
    {
        Collider[] hits = Physics.OverlapSphere(player.position, houseCheckRadius);

        foreach (var h in hits)
        {
            if (h.CompareTag("House"))
            {
                // Debug.Log("Gần House → Không spawn");
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void SpawnNearPlayer()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPos = new Vector3(
            player.position.x + randomPos.x,
            player.position.y,
            player.position.z + randomPos.y
        );

        pool.Spawn(spawnPos, Quaternion.identity);
    }
}