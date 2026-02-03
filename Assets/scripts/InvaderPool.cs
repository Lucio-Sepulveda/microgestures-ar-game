using System.Collections.Generic;
using UnityEngine;

public class InvaderPool : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject blueInvaderPrefab;
    [SerializeField] private GameObject greenInvaderPrefab;
    [SerializeField] private GameObject redInvaderPrefab;

    [Header("Pool Settings")]
    [SerializeField] private const int MIN_POOL_SIZE = 20;
    [SerializeField] private Transform gameZone;

    private List<GameObject> blueInvaders = new List<GameObject>();
    private List<GameObject> greenInvaders = new List<GameObject>();
    private List<GameObject> redInvaders = new List<GameObject>();

    void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        // Crear invaders azules
        for (int i = 0; i < MIN_POOL_SIZE; i++)
        {
            GameObject blueInvader = Instantiate(blueInvaderPrefab, transform);
            blueInvader.SetActive(false);
            blueInvaders.Add(blueInvader);
        }

        // Crear invaders verdes
        for (int i = 0; i < MIN_POOL_SIZE; i++)
        {
            GameObject greenInvader = Instantiate(greenInvaderPrefab, transform);
            greenInvader.SetActive(false);
            greenInvaders.Add(greenInvader);
        }

        // Crear invaders rojos
        for (int i = 0; i < MIN_POOL_SIZE; i++)
        {
            GameObject redInvader = Instantiate(redInvaderPrefab, transform);
            redInvader.SetActive(false);
            redInvaders.Add(redInvader);
        }
    }

    public GameObject GetBlueInvader(Vector3 position)
    {
        return GetInvader(blueInvaders, blueInvaderPrefab, position);
    }

    public GameObject GetGreenInvader(Vector3 position)
    {
        return GetInvader(greenInvaders, greenInvaderPrefab, position);
    }

    public GameObject GetRedInvader(Vector3 position)
    {
        return GetInvader(redInvaders, redInvaderPrefab, position);
    }

    private GameObject GetInvader(List<GameObject> pool, GameObject prefab, Vector3 position)
    {
        // Buscar un invader inactivo en la pool
        foreach (GameObject invader in pool)
        {
            if (!invader.activeInHierarchy)
            {
                invader.transform.position = position;
                invader.SetActive(true);
                return invader;
            }
        }

        // Si no hay ninguno disponible, crear uno nuevo
        GameObject newInvader = Instantiate(prefab, transform);
        newInvader.transform.position = position;
        newInvader.SetActive(true);
        pool.Add(newInvader);
        return newInvader;
    }

    // Método auxiliar para devolver un invader a la pool
    public void ReturnInvader(GameObject invader)
    {
        invader.SetActive(false);
    }
}
