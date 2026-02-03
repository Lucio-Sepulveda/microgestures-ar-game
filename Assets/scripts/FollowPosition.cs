using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform followTarget;

    [Header("Settings")]
    public Vector3 localOffset = new Vector3(0f, 0.15f, 0.10f);
    public float positionSmooth = 10f;
    public float rotationSmooth = 10f;

    [Header("Thresholds")]
    [Tooltip("Distancia mínima de movimiento de la mano para actualizar la posición.")]
    public float positionThreshold = 0.01f;

    [Tooltip("Cambio mínimo de rotación en grados para actualizar la rotación.")]
    public float rotationThreshold = 0.5f;

    // Estado previo para comparación
    private Vector3 lastTargetPos;
    private float lastTargetYaw;

    void Start()
    {
        // Inicializar valores
        lastTargetPos = followTarget.position;
        lastTargetYaw = followTarget.rotation.eulerAngles.y;
    }

    void LateUpdate()
    {
        // Obtener yaw únicamente
        Vector3 targetEuler = followTarget.rotation.eulerAngles;
        float targetYaw = targetEuler.y;

        Quaternion yawOnly = Quaternion.Euler(0, targetYaw, 0);

        // Pos objetivo con offset
        Vector3 desiredPos = followTarget.position + yawOnly * localOffset;

        // --- CHECK UMBRAL DE MOVIMIENTO ---
        bool movePosition =
            Vector3.Distance(followTarget.position, lastTargetPos) > positionThreshold;

        bool moveRotation =
            Mathf.Abs(Mathf.DeltaAngle(targetYaw, lastTargetYaw)) > rotationThreshold;

        // --- APLICAR POSICIÓN ---
        if (movePosition)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                Time.deltaTime * positionSmooth
            );

            lastTargetPos = followTarget.position;
        }

        // --- APLICAR ROTACIÓN ---
        if (moveRotation)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                yawOnly,
                Time.deltaTime * rotationSmooth
            );

            lastTargetYaw = targetYaw;
        }
    }
}
