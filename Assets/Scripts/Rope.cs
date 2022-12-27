using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    List<Mass> lstMass = new List<Mass>();
    List<Spring> lstSpring = new List<Spring>();

    void Start()
    {
        for (int i = 0; i < 10; i ++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = i.ToString();
            Destroy(go.GetComponent<Collider>());
            go.transform.SetParent(transform);
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            go.transform.localPosition = new Vector3(i * 0.15f, 0, 0);
            Mass mass = go.AddComponent<Mass>();
            lstMass.Add(mass);
        }
        for (int i = 0; i < lstMass.Count -1; i ++)
        {
            float len = (lstMass[i].transform.position - lstMass[i + 1].transform.position).magnitude;
            Spring spring = new Spring(lstMass[i], lstMass[i + 1], len);
            lstSpring.Add(spring);
        }
        lstMass[0].isStatic = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        float dt = 0.01f;
        for(int i = 0; i < lstSpring.Count; i++)
        {
            lstSpring[i].Simulate(dt);
        }
        for (int i = 0; i < lstMass.Count; i++)
        {
            lstMass[i].Simulate(dt);
        }
    }
}
