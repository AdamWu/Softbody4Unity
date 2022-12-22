using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    public Transform endPoint;

    public float mass = 0.1f;
    public float kd = 2f;
    public float kResistent = 0.3f;//阻力系数
    public Vector3 windForce;//测试风力

    private Vector3 a;
    private Vector3 v;
    private Vector3 p;


    void Awake()
    {
        transform.position = target.position;
        p = transform.position;
    }
    
    void Update()
    {
        Vector3 delta = target.position - p;
        Vector3 force = delta * kd;
        Vector3 forceResistent = -v.normalized * v.sqrMagnitude * kResistent;
        force += forceResistent;
        force += windForce * Random.Range(-0.4f, 1.0f);//模拟简单风力

        a = force / mass;
        v += a * Time.deltaTime;
        p += v * Time.deltaTime;

        Debug.LogFormat("force {0} v {1} p {2}", force.x, v.x, p.x);
        
        transform.rotation = Quaternion.FromToRotation(-Vector3.up, (p+endPoint.localPosition - transform.position).normalized);
        transform.position = target.position;
    }
}
