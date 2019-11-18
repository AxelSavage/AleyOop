using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    const float g = 1f;
    public bool useGravity = true;

    [SerializeField] float planeOffset = 1f;
    [SerializeField] float playSpace = .5f;

    public Sphere[] spheres;
    public Sphere s0;
    public Sphere s1;

    public Transform pT;

    public Plane p0;
    public Plane p1;
    public Plane p2;
    public Plane p3;
    public Plane p4;
    public Plane p5;

    Vector3 point = Vector3.zero;

    void OnValidate()
    {
        p0 = new Plane();
        p1 = new Plane();
        p2 = new Plane();
        p3 = new Plane();
        p4 = new Plane();
        p5 = new Plane();

        Vector3 pttp = pT.transform.position;
        p0.position = pttp + -pT.transform.forward * (2.5f - planeOffset);
        p1.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.right * -.5f;
        p2.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.right * .5f;
        p3.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.up * .5f;
        p4.position = pttp + -pT.transform.forward * (2.5f - planeOffset + playSpace);
        p5.position = pttp + -pT.transform.forward * (2.5f - planeOffset - playSpace);

        p0.normal = pT.up;
        p1.normal = Vector3.Cross(pT.up, pT.forward);
        p2.normal = -Vector3.Cross(pT.up, pT.forward);
        p3.normal = -pT.up;
        p4.normal = pT.forward;
        p5.normal = -pT.forward;

        s0.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.up * .2f;
    }

    bool AreSpheresColliding(Sphere s0, Sphere s1)
    {
        float x = s0.position.x - s1.position.x;
        float y = s0.position.y - s1.position.y;
        float centerDstSq = x * x + y * y;
        float radius = s0.radius + s1.radius;
        float radiusSq = radius * radius;
        return centerDstSq <= radiusSq;
    }

    bool SphereIsGrounded(Sphere s, PlaneGO p)
    {
        Vector3 circToPlane = p.position - s.position;
        Vector3 dir = p.normal * Vector3.Dot(circToPlane, p.normal);

        float dot = Vector3.Dot(s.position, p.right);
        float add = dot > 0 ? s.radius : -s.radius;
        float mult = dot + add;
        point = p.position + p.right * mult;
        float dst = Vector3.Distance(point, p.position);

        Debug.Log("dst: " + dst + "s.rad > dst: " + (s.radius > dst));
        bool r = s.radius > dst && dst < 5f;
        return r;
    }

    void Update()
    {
        // player input
        //Vector3 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //s0.velocity += dir * g * Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    s0.velocity += Vector3.up * 5f;
        //}


    }

        
    private void FixedUpdate()
    {
        Vector3 pttp = pT.transform.position;
        p0.position = pttp + -pT.transform.forward * (2.5f - planeOffset);
        p1.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.right * -.5f;
        p2.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.right * .5f;
        p3.position = pttp + -pT.transform.forward * (2.5f - planeOffset) + pT.transform.up * .5f;
        p4.position = pttp + -pT.transform.forward * (2.5f - planeOffset + playSpace);
        p5.position = pttp + -pT.transform.forward * (2.5f - planeOffset - playSpace);

        p0.normal = pT.up;
        p1.normal = Vector3.Cross(pT.up, pT.forward);
        p2.normal = -Vector3.Cross(pT.up, pT.forward);
        p3.normal = -pT.up;
        p4.normal = pT.forward;
        p5.normal = -pT.forward;

        Simulate(s0);
    }


    /*private Vector3 CollisionResolutionVector(Sphere s, Vector3 normal)
    {
        Vector3 circToPlane = (p.position + p.normal * (s.radius - .01f)) - s.position;
        Vector3 dir = p.normal * Vector3.Dot(circToPlane, p.normal);
        float dot = Vector3.Dot(dir, p.normal);

        bool r = dot < 0f;

        if (r)
        {
            return Vector3.zero;
        }
        else
        {
            return p.normal * Mathf.Abs(dot);
        }
    }*/

    void Simulate(Sphere s)
    {
        // gravity
        s.acceleration = -pT.up * g;

        s.velocity += s.acceleration * Time.fixedDeltaTime;

        Vector3 force = Vector3.zero;

        force += NormalForce(s, p0) * Mathf.Abs(s.velocity.y);
        /*force += NormalForce(s, p1) * Mathf.Abs(s.velocity.magnitude);
        force += NormalForce(s, p2) * Mathf.Abs(s.velocity.magnitude);
        force += NormalForce(s, p3) * Mathf.Abs(s.velocity.magnitude);*/

        s.velocity += force;
        s.position += s.velocity * Time.fixedDeltaTime;
        //s.position += CollisionResolutionVector(s, p0);

        // if outside of walls
        s.position += CollisionResolutionVector(s, p0);
        s.position += CollisionResolutionVector(s, p1);
        s.position += CollisionResolutionVector(s, p2);
        s.position += CollisionResolutionVector(s, p3);
        s.position += CollisionResolutionVector(s, p4);
        s.position += CollisionResolutionVector(s, p5);

        //s.position += pT.up * Time.deltaTime;
        //Debug.DrawRay(s.position, pT.up, Color.green);
        //Debug.DrawRay(s.position, pT.forward, Color.blue);
        //Debug.DrawRay(s.position, pT.right, Color.red);
    }

    private Vector3 CollisionResolutionVector(Sphere s, Plane p)
    {
        Vector3 circToPlane = (p.position + p.normal * (s.radius - .01f)) - s.position;
        Vector3 dir = p.normal * Vector3.Dot(circToPlane, p.normal);
        float dot = Vector3.Dot(dir, p.normal);

        bool r = dot < 0f;

        if (r)
        {
            return Vector3.zero;
        }
        else
        {
            return p.normal * Mathf.Abs(dot);
        }
    }

    private Vector3 NormalForce(Sphere s, Plane p)
    {
        Vector3 cp = (s.position + -p.normal * s.radius);
        Vector3 circToPlane = p.position - cp;

        float dot = Vector3.Dot(circToPlane, p.normal);
        Vector3 dir = p.normal * dot;

        Debug.DrawRay(p.position, dir, Color.red);
        if (dot < 0)
        {
            return Vector3.zero;
        }

        return dir.normalized;
    }

    private Vector3 NormalForce(Sphere s, Transform t)
    {
        Vector3 circToPlane = t.position - (s.position + -transform.up * s.radius);

        float dot = Vector3.Dot(circToPlane, t.up);
        Vector3 dir = t.up * dot;

        if (dot < 0)
        {
            return Vector3.zero;
        }

        // TODO test if functions ok ;;; )) heb u forgat
        //Gizmos.DrawRay(transform.position, Vector3.up);
        return dir.normalized;
    }

    private void OnDrawGizmos()
    { /*if (!Application.isPlaying) return;*/

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(p0.position, p0.normal * .2f);
        Gizmos.DrawRay(p1.position + pT.up * .25f, p1.normal * .2f);
        Gizmos.DrawRay(p2.position + pT.up * .25f, p2.normal * .2f);
        Gizmos.DrawRay(p3.position, p3.normal * .2f);

        Gizmos.color = Color.white;
        Vector3 cross = Vector3.Cross(pT.forward, pT.up);
        Gizmos.DrawRay(p0.position, cross * .5f);
        Gizmos.DrawRay(p0.position, cross * -.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(p1.position, pT.up * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(p2.position, pT.up * .5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(p3.position, cross * .5f);
        Gizmos.DrawRay(p3.position, cross * -.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(p4.position, .05f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(p5.position, .05f);
    }
}
