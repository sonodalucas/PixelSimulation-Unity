using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        
        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 10);
        vertices[2] = new Vector3(10, 10);
        vertices[3] = new Vector3(10, 0);
        
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);
        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;
        
        // MeshUtils.CreateEmptyMeshArrays(1, out Vector3[] vertices, 
        //     out Vector2[] uv, out int[] triangles);
        //
        // MeshUtils.AddToMeshArrays(vertices, uv, triangles, 0, Vector3.zero, 0f, new Vector2(10, 10), 
        //     new Vector2(0, 0), new Vector2(1, 1));

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
