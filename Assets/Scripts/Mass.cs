using UnityEngine;


class Mass
{
    public Mass(Vector3 position, float mass = 0.1f)
    {
        this.position = position;
        this.mass = mass;
        F = V = Vector3.zero;
    }

    public float mass = 0.1f;

    public Vector3 F;
    public Vector3 A;
    public Vector3 V;

    public Vector3 position;

    public bool isStatic = false;

    public void Simulate(float dt)
    {
        if (isStatic)
        {
            F = Vector3.zero;
            return;
        }

        // air force
        F += -V.normalized * V.sqrMagnitude * 0.2f;
        A = F / mass;
        A += Vector3.down * 9.8f;//gravity
        V += A * dt;

        RaycastHit hitInfo;
        if (Physics.SphereCast(position, 0.005f, V.normalized, out hitInfo, dt * V.magnitude * 1.5f))
        {
            V = Vector3.Reflect(V.normalized, hitInfo.normal) * V.magnitude * 0.5f * Mathf.Clamp01(hitInfo.distance * 300);
            //if (V.magnitude < 0.01) V = Vector3.zero;
        }

        position += V * dt;

        // clear force
        F = Vector3.zero;
    }

}