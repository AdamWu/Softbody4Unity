using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    public int numIterations = 10;
    public int massCount = 10;
    public float massSize = 0.1f;

    public float ks = 200f;
    public float kd = 0.8f;

    Mass[,] arrMass;
    List<Spring> lstSpring = new List<Spring>();

    // mesh
    private MeshFilter meshFilter;
    private Vector3[] vertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null) vertices = meshFilter.sharedMesh.vertices;

        arrMass = new Mass[massCount, massCount];
        for (int i = 0; i < massCount; i ++)
        {
            for (int j = 0; j < massCount; j ++)
            {
                Vector3 position = transform.position + new Vector3(i, 0, j) * 0.1f;
                Mass mass = new Mass(position);
                arrMass[i, j] = mass;
            }
        }
        for (int i = 0; i < massCount; i++)
        {
            for (int j = 0; j < massCount; j++)
            {
                // struct spring
                if (i < massCount - 1) CreateSpring(i, j, i + 1, j, ks);
                if (j < massCount - 1) CreateSpring(i, j, i, j + 1, ks);

                // shear spring
                if (i < massCount - 1 && j < massCount - 1) CreateSpring(i, j, i + 1, j + 1, ks * 0.7f);
                if (i < massCount - 1 && j > 0) CreateSpring(i, j, i + 1, j - 1, ks * 0.7f);

                // blend spring
                if (i < massCount - 2) CreateSpring(i, j, i + 2, j, ks * 0.5f);
                if (j < massCount - 2) CreateSpring(i, j, i, j + 2, ks * 0.5f);
            }
        }
        arrMass[0, 0].isStatic = true;
        //arrMass[massCount-1, 0].isStatic = true;
    }
    
    Spring CreateSpring(int i, int j, int m, int n, float ks)
    {
        Mass massA = arrMass[i, j];
        Mass massB = arrMass[m, n];
        float len = (massA.position - massB.position).magnitude;
        Spring spring = new Spring(massA, massB, len, ks);
        lstSpring.Add(spring);
        return spring;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = 0.01f;
        float dtStep = dt / numIterations;
        for (int step = 0; step < numIterations; step++)
        {
            for (int i = 0; i < lstSpring.Count; i++)
            {
                lstSpring[i].Simulate(dtStep);
            }
            for (int i = 0; i < massCount; i++)
            {
                for (int j = 0; j < massCount; j++)
                {
                    arrMass[i, j].Simulate(dtStep);
                }
            }
        }

        if (meshFilter != null)
        {
            for (int i = 0; i < massCount; i++)
            {
                for (int j = 0; j < massCount; j++)
                {
                    vertices[j * massCount + i] = arrMass[j, i].position - transform.position;
                }
            }
            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.RecalculateTangents();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (arrMass != null)
        {
            for (int i = 0; i < massCount; i++)
            {
                for (int j = 0; j < massCount; j++)
                {
                    Mass mass = arrMass[i, j];
                    Gizmos.DrawSphere(mass.position, 0.05f);
                }
            }
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < lstSpring.Count; i++)
        {
            Spring spring = lstSpring[i];
            Gizmos.DrawLine(spring.massA.position, spring.massB.position);
        }
    }
}
