using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fov = 90f;
    [SerializeField] private int rayCount = 150;
    [SerializeField] private float viewDistance = 10f;

    Vector3 origin;
    float angle;

    Mesh mesh;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void LateUpdate()
    {
        float angleIncrease = fov / rayCount;

        Vector3[] verticles = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[verticles.Length];
        int[] triangles = new int[rayCount * 3];

        verticles[0] = origin;    

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            float angRad = angle * (Mathf.PI / 180f);
            Vector3 v = new Vector3(Mathf.Cos(angRad), Mathf.Sin(angRad));
            Vector3 vertex;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin , v, viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                vertex = origin + v * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
            }
            verticles[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = verticles;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAngle(float angle)
    {
        this.angle = angle - fov / 2;
    }
}
