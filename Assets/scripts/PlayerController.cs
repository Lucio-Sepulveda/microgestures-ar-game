using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ------------------------------------------------------------
    //  References
    // ------------------------------------------------------------
    [Header("References")]
    [SerializeField] private Transform gameZone;
    [SerializeField] private GameZoneInfo gameZoneInfo;   // obtiene ancho de la pantalla

    [SerializeField] private LaserShooter laserShooter;

    [SerializeField] private AudioSource shootAudioSource;

    // ------------------------------------------------------------
    //  Movement Settings
    // ------------------------------------------------------------
    [Header("Movement Settings (relative to GameZone size)")]
    [SerializeField] private float moveSpeedRatio = 0.25f;      // % del ancho por segundo
    [SerializeField] private float dashSpeedRatio = 0.8f;       // % del ancho por segundo
    [SerializeField] private float dashDistanceRatio = 0.3f;    // % del ancho por dash
    [SerializeField] private float dashCooldown = 0.8f;

    private float moveSpeed;      // se calcula
    private float dashSpeed;      // se calcula
    private float dashDistance;   // se calcula

    // ------------------------------------------------------------
    //  Internal State
    // ------------------------------------------------------------
    private enum MoveState { Idle, MovingLeft, MovingRight, Dashing }
    private MoveState currentState = MoveState.Idle;
    private MoveState previousState = MoveState.Idle;

    private bool canDash = true;
    private float dashTargetLocalX;  

    // ------------------------------------------------------------
    //  Unity Methods
    // ------------------------------------------------------------
    void Start()
    {
        RecalculateMovementValues();
    }

    void Update()
    {
        switch (currentState)
        {
            case MoveState.MovingLeft:
                MoveContinuous(-1);
                break;

            case MoveState.MovingRight:
                MoveContinuous(1);
                break;

            case MoveState.Dashing:
                PerformDash();
                break;
        }
    }

    public void Shoot()
    {
        laserShooter.Shoot();

        var pitch = Random.Range(0.9f, 1.1f);
        shootAudioSource.pitch = pitch;
        shootAudioSource.Play();

    }

    // Recalcular cuando cambie el tamaño de la GameZone
    public void RecalculateMovementValues()
    {
        float width = gameZoneInfo.Width;

        moveSpeed = width * moveSpeedRatio;
        dashSpeed = width * dashSpeedRatio;
        dashDistance = width * dashDistanceRatio;
    }

    // ------------------------------------------------------------
    //  Public API (lo que usarán tus microgestos)
    // ------------------------------------------------------------

    public void MoveRight()
    {
        if (currentState == MoveState.MovingRight && canDash)
        {
            previousState = currentState;
            StartDash(1);
            return;
        }

        // Start continuous movement
        currentState = MoveState.MovingRight;
        OnMoveStart();
    }

    public void MoveLeft()
    {
        if (currentState == MoveState.MovingLeft && canDash)
        {
            previousState = currentState;
            StartDash(-1);
            return;
        }

        currentState = MoveState.MovingLeft;
        OnMoveStart();
    }

    public void StopMovement()
    {
        currentState = MoveState.Idle;
        OnStop();
    }

    // ------------------------------------------------------------
    //  Continuous Movement
    // ------------------------------------------------------------
    private void MoveContinuous(int direction)  // -1 left, +1 right
    {
        Vector3 newPos = transform.position + gameZone.right * (direction * moveSpeed * Time.deltaTime);
        transform.position = ClampInsideGameZone(newPos);
    }

    // ------------------------------------------------------------
    //  Dash System
    // ------------------------------------------------------------
    private void StartDash(int direction)
    {
        canDash = false;
        currentState = MoveState.Dashing;

        Vector3 local = gameZone.InverseTransformPoint(transform.position);
        dashTargetLocalX = Mathf.Clamp(
            local.x + direction * dashDistance,
            -gameZoneInfo.HalfWidth,
            +gameZoneInfo.HalfWidth
        );

        OnDashStart();

        StartCoroutine(DashCooldownRoutine());
    }

    private void PerformDash()
    {
        Vector3 local = gameZone.InverseTransformPoint(transform.position);
        float newX = Mathf.MoveTowards(local.x, dashTargetLocalX, dashSpeed * Time.deltaTime);

        Vector3 worldTarget = gameZone.TransformPoint(new Vector3(newX, local.y, local.z));
        transform.position = worldTarget;

        if (Mathf.Abs(newX - dashTargetLocalX) < 0.001f)
        {
            currentState = previousState;
            OnDashEnd();
        }
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        OnDashCooldownComplete();
    }

    // ------------------------------------------------------------
    //  Boundaries
    // ------------------------------------------------------------
    private Vector3 ClampInsideGameZone(Vector3 worldPos)
    {
        Vector3 local = gameZone.InverseTransformPoint(worldPos);

        local.x = Mathf.Clamp(local.x, -gameZoneInfo.HalfWidth * 0.98f, gameZoneInfo.HalfWidth * 0.98f);

        return gameZone.TransformPoint(local);
    }

    // ------------------------------------------------------------
    //  Game Juice Hooks
    //  (llamadas para que vos les pongas sonidos, partículas, etc.)
    // ------------------------------------------------------------

    protected virtual void OnMoveStart()
    {
        // sonido, partículas de movimiento, vibración XR, etc.
    }

    protected virtual void OnStop()
    {
        // animación idle, apagar partículas
    }

    protected virtual void OnDashStart()
    {
        // partículas explosivas, sonido fuerte, etc.
    }

    protected virtual void OnDashEnd()
    {
        // animación cuando termina el dash
    }

    protected virtual void OnDashCooldownComplete()
    {
        // brillo de "dash ready", sonido de recharge
    }
}
