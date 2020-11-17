using UnityEngine;
using System.Collections;
using System;
using System.IO;

public abstract class StreamFactory
{
    public abstract bool Exists(string pathName);
    public abstract Stream OpenStream(string path);
}
