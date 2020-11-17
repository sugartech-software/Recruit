using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MemoryCacheLoader : ChainLoader
{

    static Dictionary<string, byte[]> memoryData = new Dictionary<string, byte[]>();
    string memoryKey;

    public override IEnumerator BeforeLoad(string source)
    {
        //create query string from url
         memoryKey = CacheFileLoader.CacheStreamKey(source);


        //query from image dictionary
        //if we have a texture with this key 
        //we can take it and break;
        if (memoryData.ContainsKey(memoryKey))
        {
            InnerFinishLoad(memoryData[memoryKey]);
            yield break;
        }
    }

    public override IEnumerator ErrorLoad(string errorMessage)
    {
        Debug.LogError("Error Load  MemoryCacheLoader: " + errorMessage);
        yield return null;
    }
  

    public override IEnumerator FinishLoad(byte[] data)
    {
        if (!memoryData.ContainsKey(memoryKey))
        {
            memoryData.Add(memoryKey, data);
        }
        yield return null;
    }

    public override IEnumerator ProgressLoad(float progress, string message)
    {
         return null;
    }

}

