using System.Collections.Generic;
using UnityEngine;

public class InvaderGroupController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InvaderSpawner spawner;
    [SerializeField] private GameZoneInfo gameZone; // ← ahora usamos GameZoneInfo

    [Header("Movement Settings")]
    [SerializeField] private float horizontalSpeed = 0.3f;
    [SerializeField] private float verticalStepFactor = 0.08f; 
    // porcentaje de la altura del quad que bajan por paso

    [SerializeField] private int minInvadersToSpawnNewRow = 20;

    private List<List<GameObject>> rows = new List<List<GameObject>>();
    private bool movingRight = true;

    // ----------------------------------------------------------------------

    void Start()
    {
        SpawnNewRow();
    }

    void Update()
    {
        MoveGroups();

        if (CountActiveInvaders() == 0)
        {
            SpawnNewRow();
        }
    }

    // ----------------------------------------------------------------------

    private void SpawnNewRow()
    {
        List<GameObject> newRow = spawner.SpawnRow();

        if (newRow.Count > 0)
            rows.Add(newRow);
    }

    private void CheckForNewRow()
    {
        int activeCount = CountActiveInvaders();

        if (activeCount < minInvadersToSpawnNewRow)
            SpawnNewRow();
    }

    // ----------------------------------------------------------------------

    private void MoveGroups()
    {
        Vector3 moveDir = movingRight ? gameZone.transform.right : -gameZone.transform.right;

        foreach (var row in rows)
        {
            foreach (var inv in row)
            {
                if (inv != null && inv.activeInHierarchy)
                    inv.transform.position += moveDir * horizontalSpeed * Time.deltaTime;
            }
        }

        if (ReachedBorder())
        {
            movingRight = !movingRight;

            float verticalStep = gameZone.Height * verticalStepFactor;

            foreach (var row in rows)
            {
                foreach (var inv in row)
                {
                    if (inv != null && inv.activeInHierarchy)
                    {
                        inv.transform.position -= gameZone.transform.up * verticalStep;
                    }
                }
            }

            CheckForNewRow();
        }
    }

    // ----------------------------------------------------------------------

    private bool ReachedBorder()
    {
        foreach (var row in rows)
        {
            foreach (var inv in row)
            {
                if (inv == null) continue;

                Vector3 localPos = gameZone.transform.InverseTransformPoint(inv.transform.position);

                if (Mathf.Abs(localPos.x) > (gameZone.Width / 2f) * 0.9f)
                    return true;
            }
        }
        return false;
    }

    private int CountActiveInvaders()
    {
        int c = 0;
        foreach (var row in rows)
            foreach (var inv in row)
                if (inv != null && inv.activeInHierarchy)
                    c++;
        return c;
    }
}
