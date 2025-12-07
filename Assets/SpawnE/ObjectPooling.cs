using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int preloadCount = 10;

    private Queue<GameObject> pool;

    private void Awake()
    {
        pool = new Queue<GameObject>();
        Preload();
    }

    private void Preload()
    {
        for (int i = 0; i < preloadCount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Spawn object tại vị trí và rotation của transform spawnPoint
    /// </summary>
    public GameObject Spawn(Transform spawnPoint)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            // Pool hết → tự mở rộng
            obj = Instantiate(prefab);
        }

        obj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        obj.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Spawn object tại vị trí Vector3
    /// </summary>
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Trả object về lại Pool
    /// </summary>
    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}