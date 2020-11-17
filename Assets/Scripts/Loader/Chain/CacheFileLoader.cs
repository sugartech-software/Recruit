using System;
using System.Collections;
using System.IO;
using Motto.Directory;
using UnityEngine;

public class CacheFileLoader : ChainLoader
{
    string cacheKey;
    bool isFromCache;

    /// <summary>
    /// When error occurs in FileLoader
    /// transfer action to CacheError Action
    /// </summary>
    public override IEnumerator ErrorLoad(string message)
    {
        //Debug.Log(message + " from " + "CacheFileLoader");
        yield return null;
    }



    /// <summary>
    /// it is called When the file from network or cache successfully loaded.
    /// it saves the file to the directory with cache key
    /// if it is from only network.
    /// if it is from cache nothing happens but only success function is called
    /// </summary>
    /// <returns>The finish.</returns>
    /// <param name="data">Data.</param>
    public override IEnumerator FinishLoad(byte[] data)
    {
        if (!isFromCache)
        {
            yield return InnerProgressLoad(0, "Dosya kaydediliyor.");
            yield return FileManager.Instance.SaveDirectory(cacheKey, data);
        }
        else
        {
            isFromCache = false;
        }
    }

    /// <summary>
    /// Load the specified source.
    /// </summary>
    /// <returns>The load.</returns>
    /// <param name="source">Source.</param>
    public override IEnumerator BeforeLoad(string source)
    {
        //create query string from url
        cacheKey = CacheStreamKey(source);

        /*//query from image dictionary
        //if we have a texture with this key 
        //we can take it and break;
        if (ImageDictionary.ContainsKey(imageSourceQuery))
        {
            finish(ImageDictionary[imageSourceQuery]);
            yield break;
        }*/

        //query from directory
        //if we dont have file reset the imageSourceQuery
        string sourceQuery = cacheKey;
        if (File.Exists(cacheKey))
        {
            sourceQuery = "file://" + cacheKey;
            isFromCache = true;
        }
        else
        {
            sourceQuery = source;
        }

        sourceKey = sourceQuery;
        yield return null;
    }



    public static string CacheStreamKey(string imageSource)
    {

        if (imageSource == null)
            return null;
        imageSource = Uri.UnescapeDataString(imageSource);
        imageSource = imageSource.Replace("https://", "");
        imageSource = imageSource.Replace("http://", "");

        int lastPointIndex = imageSource.LastIndexOf('.');
        if (lastPointIndex > -1)
        {
            imageSource = imageSource.Substring(0, lastPointIndex).Replace(".", "") + imageSource.Substring(lastPointIndex, imageSource.Length - lastPointIndex);
        }

        imageSource = imageSource.Replace(",", "");
        string[] imageSourceSplits = imageSource.Split('/');
        string imageSourceQuery = (string)imageSource.Clone();


        if (imageSourceSplits.Length > 0)
        {
            imageSourceQuery = "";
            foreach (String imageSpart in imageSourceSplits)
            {
                if (!imageSpart.Equals("http:")
                 && !imageSpart.Equals("https:")
                 && !imageSpart.Equals("file:"))
                {
                    imageSourceQuery += imageSpart + "_";
                }
            }
            imageSourceQuery = imageSourceQuery.Remove(imageSourceQuery.Length - 1, 1);
            imageSourceQuery = imageSourceQuery.Replace(" ", "");
        }

        return Path.Combine(Application.persistentDataPath, imageSourceQuery);
    }

    public override IEnumerator ProgressLoad(float progress, string message)
    {
        yield return null;
    }
}

