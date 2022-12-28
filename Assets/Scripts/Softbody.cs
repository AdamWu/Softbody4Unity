using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Softbody : MonoBehaviour
{
    public int numIterations = 10;
    public int massCount = 5;
    public float massSize = 0.05f;
    public float linkRadius = 0.1f;

    List<Mass> lstMass = new List<Mass>();
    List<Spring> lstSpring = new List<Spring>();

    public float ks = 200f;
    public float kd = 0.8f;

    // mesh
    private MeshFilter meshFilter;
    private Vector3[] vertices;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null) vertices = meshFilter.sharedMesh.vertices;

        for (int i = 0; i < massCount; i ++)
        {
            for (int j = 0; j < massCount; j ++)
            {
                for (int k = 0; k < massCount; k++)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    go.name = string.Format("{0}-{1}-{2}", i, j, k);
                    Destroy(go.GetComponent<Collider>());
                    go.transform.SetParent(transform);
                    go.transform.localScale = Vector3.one * massSize;
                    go.transform.localPosition = new Vector3(i, k, j) * 0.1f;
                    Mass mass = go.AddComponent<Mass>();
                    lstMass.Add(mass);
                }
            }
        }
        for (int i = 0; i < lstMass.Count; i++)
        {
            Mass massA = lstMass[i];
            for (int j = i+1; j < lstMass.Count; j++)
            {
                Mass massB = lstMass[j];
                float len = (massA.transform.position - massB.transform.position).magnitude;
                if (len < linkRadius)
                {
                    Spring spring = new Spring(massA, massB, len, ks);
                    lstSpring.Add(spring);
                }
            }
        }

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
            for (int i = 0; i < lstMass.Count; i++)
            {
                lstMass[i].Simulate(dtStep);
            }
        }

        if (meshFilter != null)
        {
            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.RecalculateTangents();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < lstSpring.Count; i++)
        {
            Spring spring = lstSpring[i];
            Debug.DrawLine(spring.massA.transform.position, spring.massB.transform.position, Color.red);
        }
    }
}
