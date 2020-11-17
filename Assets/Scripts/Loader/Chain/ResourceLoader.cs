using System;
using System.Collections;
using UnityEngine;

public class ResourceLoader : ChainLoader
{
    Action<Texture2D> finishTextureLoad;
    Action errorTextureLoad;

    public ResourceLoader()
    {
    }

    public IEnumerator LoadTexture(string source, Action<Texture2D> finish = null, Action error = null) {
        finishTextureLoad = finish;
        errorTextureLoad = error;
        yield return childLoader.Load(source);
    }

    public override IEnumerator BeforeLoad(string source)
    {
        if(source != null && source.StartsWith("@@"))
        {
            string cleanSOurce = source.Substring(2);
            UnityEngine.Object resource =   Resources.Load(cleanSOurce);
            breakChain = false;
        }
        yield return null;
    }

    public override IEnumerator ErrorLoad(string errorMessage)
    {
        if(errorTextureLoad != null) {
            errorTextureLoad();
        }
        yield return null;
    }

    public override IEnumerator FinishLoad(byte[] data)
    {
        if(finishTextureLoad != null) {
            Texture2D texture2D = new Texture2D(1, 1);
            if (texture2D.LoadImage(data))
            {
                finishTextureLoad(texture2D);
            }
        }
        yield return null;
    }

    public override IEnumerator ProgressLoad(float progress, string message)
    {
         return null;
    }
}

