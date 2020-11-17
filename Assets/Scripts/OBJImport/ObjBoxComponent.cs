using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjBoxComponent : MonoBehaviour
{

    public ObjBox box;

    public static ObjBoxComponent FromGameObject(GameObject gameObject, ObjBox objBox)
    {
        ObjBoxComponent instance = null;
        if(gameObject != null)
        {
            instance = gameObject.AddComponent<ObjBoxComponent>();
        }
        instance.box = objBox;
        return instance;
    }
 
}

public class ObjBox
{

    public Vector3 center;
    public Vector3 minExtend;
    public Vector3 maxExtend;
    public Vector3 size;

    public ObjBox(Vector3 center, Vector3 minExtend, Vector3 maxExtend)
    {
        this.center = center;
        this.minExtend = minExtend;
        this.maxExtend = maxExtend;
        this.size = this.maxExtend - this.minExtend;
    }

    public void Translate(Vector3 translateAmount)
    {
        this.center += translateAmount;
        this.minExtend += translateAmount;
        this.maxExtend += translateAmount;
        //this.size = this.maxExtend - this.minExtend;
    }

    public static ObjBox FromBoxes(List<ObjBox> boxes)
    {
        ObjBox box = null;

        Vector3 minExtend = Vector3.zero;
        Vector3 maxExtend = Vector3.zero;

        float minimumYaxis = float.MaxValue;
        float minimumXaxis = float.MaxValue;
        float minimumZaxis = float.MaxValue;

        float maximumXaxis = float.MinValue;
        float maximumYaxis = float.MinValue;
        float maximumZaxis = float.MinValue;

        Vector3 center = Vector3.zero;

        foreach (var v in boxes)
        {
            center += v.center;
            if (minimumYaxis > v.minExtend.y)
            {
                minimumYaxis = v.minExtend.y;
            }
            if (minimumXaxis > v.minExtend.x)
            {
                minimumXaxis = v.minExtend.x;
            }
            if (minimumZaxis > v.minExtend.z)
            {
                minimumZaxis = v.minExtend.z;
            }
            if (maximumYaxis < v.maxExtend.y)
            {
                maximumYaxis = v.maxExtend.y;
            }
            if (maximumXaxis < v.maxExtend.x)
            {
                maximumXaxis = v.maxExtend.x;
            }
            if (maximumZaxis < v.maxExtend.z)
            {
                maximumZaxis = v.maxExtend.z;
            }
        }
        center /= boxes.Count;
        minExtend = new Vector3(minimumXaxis, minimumYaxis, minimumZaxis);
        maxExtend = new Vector3(maximumXaxis, maximumYaxis, maximumZaxis);

        box = new ObjBox(center, minExtend, maxExtend);
        return box;

    }

    public static ObjBox FromVertices(List<Vector3> vertices)
    {
        float minimumYaxis = float.MaxValue;
        float minimumXaxis = float.MaxValue;
        float minimumZaxis = float.MaxValue;

        float maximumYaxis = float.MinValue;
        float maximumXaxis = float.MinValue;
        float maximumZaxis = float.MinValue;

        Vector3 center = Vector3.zero;
        foreach (Vector3 v in vertices)
        {
            center += v;
            if (minimumYaxis > v.y)
            {
                minimumYaxis = v.y;
            }
            if (minimumXaxis > v.x)
            {
                minimumXaxis = v.x;
            }
            if (minimumZaxis > v.z)
            {
                minimumZaxis = v.z;
            }
            if (maximumYaxis < v.y)
            {
                maximumYaxis = v.y;
            }
            if (maximumXaxis < v.x)
            {
                maximumXaxis = v.x;
            }
            if (maximumZaxis < v.z)
            {
                maximumZaxis = v.z;
            }
        }
        center /= vertices.Count;
        Vector3 minExtend = new Vector3(minimumXaxis, minimumYaxis, minimumZaxis);
        Vector3 maxExtend = new Vector3(maximumXaxis, maximumYaxis, maximumZaxis);


        ObjBox box = new ObjBox(center, minExtend, maxExtend);
        return box;
    }
}
