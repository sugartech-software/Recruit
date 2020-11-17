using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Motto.Directory;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader
{
    private const string CACHE_FILE = "cache_textures";
    private  Dictionary<string, Texture2D> imageDictionary = new Dictionary<string, Texture2D>();

    private ChainLoader chainLoader;

    //public ChainLoader Loader => chainLoader;
    public ChainLoader Loader
    {
        get
        {

            ChainLoader cl = new Texture2DLoader();
            cl.AddChild(new MemoryCacheLoader())
                        .AddChild(new CacheFileLoader())
                        .AddChild(new FileChainLoader(new FileLoader()));
            return cl;

        }
    }

    private ImageLoader()
    {
        chainLoader = new Texture2DLoader();
        chainLoader.AddChild(new MemoryCacheLoader())
                    .AddChild(new CacheFileLoader())
                    .AddChild(new FileChainLoader(new FileLoader()));
    }

    private static ImageLoader instance = null;

    public static ImageLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ImageLoader();
            }
            return instance;
        }
    }

    public  Dictionary<string, Texture2D> ImageDictionary => imageDictionary;

    internal IEnumerator GetImage(string imageSource, Action<Texture2D> finish = null, Action error = null)
    {
        bool isFromDirectory = false;
        //create query string from url
        string imageSourceQuery = CacheStreamKey(imageSource);


        //query from image dictionary
        //if we have a texture with this key 
        //we can take it and break;
        if (ImageDictionary.ContainsKey(imageSourceQuery))
        {
            finish(ImageDictionary[imageSourceQuery]);
            yield break;
        }

        //query from directory
        //if we dont have file reset the imageSourceQuery
        if (File.Exists(imageSourceQuery))
        {
            imageSourceQuery = "file://" + imageSourceQuery;
            isFromDirectory = true;
        }
        else
        {
            imageSourceQuery = imageSource;
        }

        //
        //Debug.Log("Request URL is " + imageSourceQuery);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageSourceQuery);
        yield return www.SendWebRequest();

        //
        if (!www.isHttpError && !www.isNetworkError)
        {
            //Debug.Log("Error while downloading data: " + www.error);
            Texture2D resultTexture = DownloadHandlerTexture.GetContent(www);
            if (finish != null)
            {
                finish(resultTexture);
            }

            ///create image source query from url
            imageSourceQuery = CacheStreamKey(imageSource);


            ///put the result texture image dictionary
            /// it cahches the result texture
            /// 
            if (!imageDictionary.ContainsKey(imageSourceQuery))
            {
                ImageDictionary.Add(imageSourceQuery, resultTexture);
            }


            //save it to file 
            //so we can get it from directory if we dont have any network access
            if (!isFromDirectory)
            {
                yield return FileManager.Instance.SaveDirectory(imageSourceQuery, resultTexture.EncodeToPNG());
            }
        }
        else
        {
            if (error != null)
            {
                error();
            }
        }

    }


    private string CacheStreamKey(string imageSource)
    {

        imageSource = Uri.UnescapeDataString(imageSource);
        imageSource = imageSource.Replace("https://", "");
        imageSource = imageSource.Replace("http://", "");

        int lastPointIndex = imageSource.LastIndexOf('.');

        if(lastPointIndex > -1)
            imageSource = imageSource.Substring(0, lastPointIndex).Replace(".", "") + imageSource.Substring(lastPointIndex, imageSource.Length - lastPointIndex );
       
        imageSource = imageSource.Replace(",", "");
        string[] imageSourceSplits = imageSource.Split('/');
        string imageSourceQuery = (string)imageSource.Clone();


        if (imageSourceSplits.Length > 0)
        {
            imageSourceQuery = "";
            foreach (String imageSpart in imageSourceSplits)
            {
                if (!imageSpart.Equals("http:")
                 && !imageSpart.Equals("https:"))
                {
                    imageSourceQuery += imageSpart + "_";
                }
            }
            imageSourceQuery = imageSourceQuery.Remove(imageSourceQuery.Length - 1, 1);
        }

        return Path.Combine(Application.persistentDataPath, imageSourceQuery);
    }



}

