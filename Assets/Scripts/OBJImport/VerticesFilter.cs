using System;
using UnityEngine;

public interface VerticesFilter
{

    Vector3[] filter(Vector3[] vertices);

    Boolean endFilter(Vector3[] vertices);
}

