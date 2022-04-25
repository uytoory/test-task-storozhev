using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [HideInInspector] public Queue<GameObject> pool;
    [SerializeField] private int number = 20;
    [SerializeField] private GameObject bulletPrefab = null;

    private void Start()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < number; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public GameObject SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = pool.Dequeue();

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);

        IPoolObject poolObject = bullet.GetComponent<IPoolObject>();
        if (poolObject != null)
        {
            poolObject.OnSpawn();
        }

        pool.Enqueue(bullet);
        return bullet;
    }
}
