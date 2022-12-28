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
            Vector3 position = transform.position + new Vector3(i, 0, 0) * 0.15f;
            Mass mass = new Mass(position);
            lstMass.Add(mass);
        }
        for (int i = 0; i < lstMass.Count -1; i ++)
        {
            float len = (lstMass[i].position - lstMass[i + 1].position).magnitude;
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < lstMass.Count; i++)
        {
            Mass mass = lstMass[i];
            Gizmos.DrawSphere(mass.position, 0.05f);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < lstSpring.Count; i++)
        {
            Spring spring = lstSpring[i];
            Gizmos.DrawLine(spring.massA.position, spring.massB.position);
        }
    }
}
