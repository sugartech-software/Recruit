using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;


public abstract class TemplateChainLoader<T> : ChainLoader
{
    Func<T, IEnumerator> finishLoad;
    Action<string> errorLoad;
    Action<float, string> progressLoad;

    protected T convertedData;
    protected string cacheSource;
    protected string errorMessage;

 
    public bool isInProgress;

    public IEnumerator Load(string source, Func< T, IEnumerator> finish = null, Action<string> error = null, Action<float, string> progress = null)
    {
        finishLoad = finish;
        errorLoad = error;
        progressLoad = progress;
        cacheSource = CacheFileLoader.CacheStreamKey(source);
        isInProgress = true;

        yield return childLoader.Load(source);
    }

    public override IEnumerator BeforeLoad(string source)
    {
        yield return null;
    }

    public override IEnumerator ErrorLoad(string errorMessage)
    {
        errorLoad?.Invoke(errorMessage);
        isInProgress = false;
        yield return null;
    }

    public override IEnumerator FinishLoad(byte[] data)
    {
        if (finishLoad != null)
        {
            yield return ConvertData(data);

            if (errorMessage != null){
                isInProgress = false;
                yield return InnerErrorLoad(errorMessage);
            }

            if(errorMessage == null)
            {
                isInProgress = false;
                yield return finishLoad(convertedData);
            }
        }
        
    }

    public override IEnumerator ProgressLoad(float progress, string message)
    {
        progressLoad?.Invoke(progress, message);

        yield return null;
    }

    public abstract IEnumerator ConvertData(byte[] data);
   
}
