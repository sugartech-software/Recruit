using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class ZipMemoryStreamFactory : StreamFactory
{
    private Dictionary<String, byte[]> dictionaryStream = null;

    public override bool Exists(string path)
    { 
        return dictionaryStream.ContainsKey(path);
    }

    public Dictionary<String, byte[]> Entries => dictionaryStream;

    public override Stream OpenStream(string path)
    {

        int lastSeperator = -1;
        String pathToSelect = path;
        if ((lastSeperator = path.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase)) > 0)
        {
            pathToSelect = path.Substring(lastSeperator + 1);
        }

        byte[] ms = new byte[0];
        if( dictionaryStream.TryGetValue(pathToSelect, out ms))
        {
            return new MemoryStream(ms);
        }
        return null;
    }

    public void AddEndtry(string key, byte[] ms)
    {
        int lastSeperator = -1;
        String keyToAdd = key;
        if (( lastSeperator = key.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase)) > 0){
            keyToAdd = key.Substring(lastSeperator + 1);
        }

        if (dictionaryStream == null)
        {
            dictionaryStream = new Dictionary<string, byte[]>();
        }
        dictionaryStream.Add(keyToAdd, ms);
    }

    internal bool Contains(string fullName)
    {
       return dictionaryStream != null && dictionaryStream.ContainsKey(fullName);
    }
}
