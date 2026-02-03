using UnityEngine;


public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private LaserShooter laserShooter;
    [SerializeField] GameObject DieEffectPrefab;

    void IDamageable.Die()
    {
        Instantiate(DieEffectPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
