using System.Collections.Generic;
using UnityEngine;

public class InvaderSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InvaderPool invaderPool;
    [SerializeField] private GameZoneInfo gameZone;  // usamos GameZoneInfo

    [Header("Spawn Settings")]
    [SerializeField] private int invadersPerRow = 7;

    [SerializeField] private float horizontalSpacingFactor = 0.1f;
    // porcentaje del ancho del quad

    [SerializeField] private float rowHeightOffsetFactor = 0.45f;
    // altura relativa (0 = centro, 1 = borde superior)

    [Header("Probabilidades (0 a 1)")]
    [Range(0f, 1f)] public float probabilityBlue = 0.33f;
    [Range(0f, 1f)] public float probabilityGreen = 0.33f;
    [Range(0f, 1f)] public float probabilityRed = 0.34f;

    [Header("Spawner Limits")]
    [SerializeField] private int maxInvadersTotal = 40;
    private int spawnedInvadersCount = 0;

    // ----------------------------------------------------------------------

    public List<GameObject> SpawnRow()
    {
        if (spawnedInvadersCount >= maxInvadersTotal)
            return new List<GameObject>();

        List<GameObject> row = new List<GameObject>();

        float spacing = gameZone.Width * horizontalSpacingFactor;

        float totalWidth = (invadersPerRow - 1) * spacing;

        Vector3 basePos =
            gameZone.transform.position +
            gameZone.transform.up * (gameZone.Height * rowHeightOffsetFactor);

        Vector3 leftStart = basePos - gameZone.transform.right * (totalWidth / 2f);

        // ------------------------------------------------------------

        for (int i = 0; i < invadersPerRow; i++)
        {
            if (spawnedInvadersCount >= maxInvadersTotal)
                break;

            Vector3 spawnPos = leftStart + gameZone.transform.right * (i * spacing);

            GameObject inv = SpawnRandomColor(spawnPos);

            if (inv != null)
            {
                row.Add(inv);
                //spawnedInvadersCount++;
            }
        }

        return row;
    }

    // ----------------------------------------------------------------------

    private GameObject SpawnRandomColor(Vector3 pos)
    {
        float r = Random.value;

        if (r < probabilityBlue)
            return invaderPool.GetBlueInvader(pos);

        if (r < probabilityBlue + probabilityGreen)
            return invaderPool.GetGreenInvader(pos);

        return invaderPool.GetRedInvader(pos);
    }

    // ----------------------------------------------------------------------

    public void NotifyInvaderDestroyed()
    {
        spawnedInvadersCount--;
        if (spawnedInvadersCount < 0)
            spawnedInvadersCount = 0;
    }
}
