using System;
using System.Collections;

public abstract class Loader
{
    public abstract IEnumerator Load(string source, Func<byte[], IEnumerator> finish = null, Func<string, IEnumerator> error = null, Func<float, string, IEnumerator> progress = null);
}

