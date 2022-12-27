using UnityEngine;


class Spring
{
    public Spring(Mass massA, Mass massB, float restLen, float ks=100f, float kd=0.8f)
    {
        this.massA = massA;
        this.massB = massB;
        this.restLen = restLen;
        this.ks = ks;
        this.kd = kd;
    }

    private Mass massA;
    private Mass massB;

    private float restLen = 0f;

    private float ks = 100f;
    private float kd = 0.8f;

    public void Simulate(float dt)
    {
        Vector3 delta = massB.transform.position - massA.transform.position;
        Vector3 force = ks * delta.normalized * (delta.magnitude - restLen);

        // spring resistance
        Vector3 deltaV = massA.V - massB.V;
        Vector3 forceR = -kd * delta.normalized * Vector3.Dot(deltaV, delta.normalized);

        massA.F += force + forceR;
        massB.F -= force + forceR;
    }
}