using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBricks : MonoBehaviour
{
    //Will allow to make the mesh with a higher or lower number of triangles
    [SerializeField]
    int detailLevel;

    //The length of the straight lines constituting the edge of the bricks
    [SerializeField]
    float faceLength;

    //The length of the side of the brick
    [SerializeField]
    float sideLength;


    private void OnEnable()
    {
        //Our mesh
        var brickMesh = new Mesh
        {
            name = "Brick Mesh"
        };

        //The vertices allows us to constituate the triangles
        brickMesh.vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 0f, 0f)
        };

        //The normals will allow us to use light properly
        brickMesh.normals = new Vector3[]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
        };

        //The order in which to use the vertices to draw the triangles
        brickMesh.triangles = new int[]
        {
            0,
            1,
            2
        };

        //I have no uv nor tangents yet as I don't use basemaps or normalmaps yet.


        brickMesh.vertices = updateVertices(brickMesh.vertices);
        brickMesh.triangles = updateTriangles(brickMesh.triangles);


        //Then we attribute this mesh to the mesh filter
        GetComponent<MeshFilter>().mesh = brickMesh;


        for (int i = 0; i < brickMesh.vertices.Length; i++)
        {
            Debug.Log("vertices " + i + " = " + brickMesh.vertices[i]);
        }

        for (int i = 0; i < brickMesh.triangles.Length; i++)
        {
            Debug.Log("triangles " + i + " = " + brickMesh.triangles[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3[] updateVertices(Vector3[] vertices)
    {
        var updatedVertices = new Vector3[vertices.Length + 2];

        
        for (int i = 0; i < vertices.Length; i++)
        {
            updatedVertices[i] = vertices[i];
        }

        updatedVertices[vertices.Length] = new Vector3(vertices[vertices.Length - 2].x + Mathf.Sin(180 / detailLevel) * faceLength, vertices[vertices.Length - 2].y + Mathf.Cos(180 / detailLevel) * faceLength, 0f);
        updatedVertices[vertices.Length + 1] = new Vector3(vertices[vertices.Length - 2].x + Mathf.Cos(180 / detailLevel) * faceLength, vertices[vertices.Length - 2].y + Mathf.Sin(180 / detailLevel) * faceLength, 0f);


        return updatedVertices;      
    }

    private int[] updateTriangles(int[] triangles)
    {
        var updatedTriangles = new int[triangles.Length + 3];
        for (int i = 0; i < triangles.Length; i++)
        {
            updatedTriangles[i] = triangles[i];
        }

        updatedTriangles[triangles.Length] = triangles.Length - 4;
        updatedTriangles[triangles.Length + 1] = triangles.Length - 2;
        updatedTriangles[triangles.Length + 2] = triangles.Length - 1;

        return updatedTriangles;
    }
}
