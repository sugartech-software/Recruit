using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MottoVector3
{


    public MottoVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector()
    {
        return new Vector3(x, y, z);
    }

    public float x;
    public float y;
    public float z;

    public static Vector3[] ToVectors(List<MottoVector3> points)
    {
        Vector3[] vector3s = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            vector3s[i] = points[i].ToVector();
        }
        return vector3s;
    }
}