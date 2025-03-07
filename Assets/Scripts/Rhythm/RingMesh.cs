// I have no idea how this works XD, i just stole this lol
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ShrinkingRing : MonoBehaviour
{
    public int segments = 64; // Number of segments (higher = smoother)
    public float outerRadius = 0.5f; // Starting outer radius
    public float innerRadius = 1f; // Starting inner radius
    private float shrinkSpeed; // Shrink speed per second

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private float scaleFactor = 1f;

    private Hits parentScript;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
	meshRenderer = GetComponent<MeshRenderer>();
        GenerateRingMesh();

	parentScript = GetComponentInParent<Hits>();

	meshRenderer.material.color = parentScript.color;
	shrinkSpeed = parentScript.speed;
    }

    void Update()
    {
        if (scaleFactor > 0)
        {
            scaleFactor -= shrinkSpeed * Time.deltaTime;
            scaleFactor = Mathf.Max(0, scaleFactor);
            GenerateRingMesh();
        }
    }

    void GenerateRingMesh()
    {
        int vertexCount = segments * 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[segments * 6];

        float angleStep = 2f * Mathf.PI / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;
            float cosA = Mathf.Cos(angle);
            float sinA = Mathf.Sin(angle);

            float scaledOuter = outerRadius * scaleFactor;
            float scaledInner = innerRadius * scaleFactor;

            // Outer and inner vertices (shrinking)
            vertices[i * 2] = new Vector3(cosA * scaledOuter, sinA * scaledOuter, 0f);
            vertices[i * 2 + 1] = new Vector3(cosA * scaledInner, sinA * scaledInner, 0f);

            // Triangles
            int triIndex = i * 6;
            int nextIndex = (i + 1) % segments;

            triangles[triIndex] = i * 2;
            triangles[triIndex + 1] = nextIndex * 2;
            triangles[triIndex + 2] = i * 2 + 1;

            triangles[triIndex + 3] = nextIndex * 2;
            triangles[triIndex + 4] = nextIndex * 2 + 1;
            triangles[triIndex + 5] = i * 2 + 1;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

