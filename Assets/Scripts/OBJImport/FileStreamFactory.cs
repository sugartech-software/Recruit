using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class FileStreamFactory : StreamFactory
{
    FileInfo _objectInfo = null;

    public FileStreamFactory()
    {
    }

    public FileStreamFactory(string path)
    {
        _objectInfo = new FileInfo(path);
    }

    public override bool Exists(string pathName)
    {
        if (!string.IsNullOrEmpty(pathName) && File.Exists(pathName))
        {
            return true;
        }
        return false;
    }

    public override Stream OpenStream(string path)
    {
        if (File.Exists(Path.Combine(_objectInfo.Directory.FullName, path)))
        {
            return new FileStream(Path.Combine(_objectInfo.Directory.FullName, path), FileMode.Open);
        }
        return new FileStream(path, FileMode.Open);
    }
}
