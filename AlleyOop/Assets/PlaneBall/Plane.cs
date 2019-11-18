using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    public Plane() { }

    public Plane(float extents, Vector3 position, Vector3 normal)
    {
        this.extents = extents;
        this.position = position;
        this.normal = normal;
    }

    public Plane(Transform t)
    {
        this.t = t;
    }

    public Vector3 GetNormal()
    {
        return normal;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    Transform t;

    public float extents = 5f;
    public Vector3 position = Vector3.zero;
    public Vector3 normal = Vector3.up;
}
