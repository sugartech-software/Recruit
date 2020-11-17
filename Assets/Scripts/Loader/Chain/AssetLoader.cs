using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetLoader : ChainLoader
{

    static IDictionary<string, AssetBundle> assetDictionary = new Dictionary<string, AssetBundle>();
    static AssetLoader instance;


    public static AssetLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AssetLoader();
                instance.AddChild(new CacheFileLoader())
                        .AddChild(new FileChainLoader(new FileLoader()));
            }
            return instance;
        }
    }

    private AssetLoader()
    {

    }

    Action<AssetBundle> finishAssetLoad;
    Action errorAssetLoad;


    public IEnumerator LoadAsset(string source, Action<AssetBundle> finish = null, Action error = null) {
        finishAssetLoad = finish;
        errorAssetLoad = error;
        yield return Load(source);
    }

    public override IEnumerator BeforeLoad(string source)
    {
        sourceKey = source;
        if (assetDictionary.ContainsKey(sourceKey))
        {
            finishAssetLoad(assetDictionary[sourceKey]);
            breakChain = true;
        }
        yield return null;
    }

    public override IEnumerator ErrorLoad(string errorMessage)
    {
        if(errorAssetLoad != null) {
            errorAssetLoad();
        }
        yield return null;
    }

    public override IEnumerator FinishLoad(byte[] data)
    {
        if(finishAssetLoad != null) {
            AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(data);
            yield return createRequest;

            AssetBundle bundle = createRequest.assetBundle;
            assetDictionary.Add(sourceKey, bundle);
            
            finishAssetLoad(bundle);
        }
        yield return null;
    }

    public override IEnumerator ProgressLoad(float progress, string message)
    {
        yield return  null;
    }

}

