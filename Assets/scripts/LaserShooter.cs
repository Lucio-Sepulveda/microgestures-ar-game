using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    [Header("Prefab & Spawn")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Settings")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float laserSpeed = 1f;
    [SerializeField] private float lifeTime = 2f;

    [Header("Collision")]
    [SerializeField] private string targetTag = "Invader"; 
    // Cambiá por "Player" cuando lo use la nave enemiga

    private float cooldownTimer = 0f;

    private List<GameObject> pool;

    // ---------------------------------------------------------

    void Awake()
    {
        CreatePool();
    }

    void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    // ---------------------------------------------------------
    // 🔫 FUNCIÓN PARA DISPARAR (se llama desde otro script)
    // ---------------------------------------------------------
    public void Shoot()
    {
        if (cooldownTimer > 0) return;

        GameObject laser = GetLaserFromPool();
        if (laser == null) return;

        laser.transform.position = firePoint.position;
        //laser.transform.rotation = firePoint.rotation;
        laser.SetActive(true);

        // Configurar láser
        Laser laserComp = laser.GetComponent<Laser>();
        laserComp.Fire(firePoint, laserSpeed, lifeTime, targetTag);

        cooldownTimer = fireCooldown;
    }

    // ---------------------------------------------------------
    // POOL
    // ---------------------------------------------------------
    private void CreatePool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(laserPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private GameObject GetLaserFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        GameObject extra = Instantiate(laserPrefab);
        extra.SetActive(false);
        pool.Add(extra);
        return extra;
    }
}
