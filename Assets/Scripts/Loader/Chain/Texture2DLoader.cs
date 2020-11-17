using System;
using System.Collections;
using UnityEngine;

public class Texture2DLoader : ChainLoader
{
    Action<Texture2D> finishTextureLoad;
    Action errorTextureLoad;

    public Texture2DLoader()
    {
    }

    public IEnumerator LoadTexture(string source, Action<Texture2D> finish = null, Action error = null) {
        finishTextureLoad = finish;
        errorTextureLoad = error;
        yield return childLoader.Load(source);
    }

    public override IEnumerator BeforeLoad(string source)
    {
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

