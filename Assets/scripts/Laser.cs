using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed;
    private float timer;
    private float lifetime;
    private Transform firePoint;

    private Renderer rend;
    private Material mat;
    private float emissionBase;

    private string targetTag;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;

        mat.EnableKeyword("_EMISSION");
        emissionBase = 1f;
    }

    // -----------------------------------------
    // 🔥 Se llama cuando se dispara
    // -----------------------------------------
    public void Fire(Transform firePoint, float speed, float lifetime, string targetTag)
    {
        this.firePoint = firePoint;
        this.speed = speed;
        this.lifetime = lifetime;
        this.targetTag = targetTag;
        timer = 0f;
    }

    // -----------------------------------------
    void Update()
    {
        // Mover láser
        transform.position += firePoint.forward * speed * Time.deltaTime;

        // Auto-destrucción
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        // Efecto de emisión
        float e = emissionBase + Mathf.Sin(Time.time * 40f) * 0.5f;
        mat.SetColor("_EmissionColor", mat.color * e);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Laser hit: " + other.name);
        if (!other.CompareTag(targetTag))
            return;

        // Desactivar láser
        gameObject.SetActive(false);

        // Ejecutar Die() si existe en el objeto golpeado
        var dieInterface = other.GetComponent<IDamageable>();
        if (dieInterface != null)
        {
            dieInterface.Die();
            return;
        }
    }
}
