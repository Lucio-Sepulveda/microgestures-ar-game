using UnityEngine;

public class GameZoneInfo : MonoBehaviour
{
    [SerializeField] private MeshRenderer screenRenderer;

    public float Width { get; private set; }
    public float Height { get; private set; }

    public float HalfWidth { get; private set; }
    public float HalfHeight { get; private set; }

    void Awake()
    {
        if (screenRenderer == null)
            screenRenderer = GetComponentInChildren<MeshRenderer>();

        Vector3 size = screenRenderer.bounds.size;
        Width = size.x;
        Height = size.y;

        HalfWidth = Width * 0.5f;
        HalfHeight = Height * 0.5f;
    }
}
