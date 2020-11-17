using System;
using System.Collections;
using UnityEngine.Networking;

public class FileLoader : Loader
{
    public override IEnumerator Load(string source, Func<byte[], IEnumerator> finish = null, Func<string, IEnumerator> error = null, Func<float, string, IEnumerator> progress = null)
    {
                //
        //Debug.Log("Request URL is " + imageSourceQuery);
        UnityWebRequest www = new UnityWebRequest(source);//UnityWebRequestTexture.GetTexture(imageSourceQuery);
        www.downloadHandler = new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation asycOperation =  www.SendWebRequest();

        while(!asycOperation.isDone){
            yield return progress?.Invoke( 100 * asycOperation.progress, "");
        }
        yield return progress?.Invoke(100, "Tamamlandi");

        //
        if (!www.isHttpError && !www.isNetworkError)
        {
            //Debug.Log("Error while downloading data: " + www.error);

            if (finish != null)
            {
                yield return finish(www.downloadHandler.data);
            }
        }
        else
        {
            if (error != null)
            {
                yield return error(www.error);
            }
        }
    }

}
