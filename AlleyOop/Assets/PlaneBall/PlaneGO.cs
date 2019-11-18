using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGO : MonoBehaviour
{
    public float extents = 5f;
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    public Vector3 normal
    {
        get
        {
            return transform.up;
        }
    }
    public Vector3 right
    {
        get
        {
            return transform.right;
        }
    }

    void Update()
    {
        Debug.DrawRay(position, normal);
    }
}
