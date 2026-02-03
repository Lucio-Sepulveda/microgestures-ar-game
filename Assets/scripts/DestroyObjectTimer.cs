using UnityEngine;

public class DestroyObjectTimer : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;


    private float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
    }
}
