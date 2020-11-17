using System;
using System.Collections;

public class FileChainLoader : ChainLoader
{
    FileLoader fileLoader;

    public FileChainLoader(){
        this.fileLoader = new FileLoader();
    }

    public FileChainLoader(FileLoader fileLoader)
    {
        this.fileLoader = fileLoader;
    }

    public override IEnumerator BeforeLoad(string source)
    {
        yield return InnerProgressLoad(0, "Yükleniyor.");
        yield return fileLoader.Load(source, InnerFinishLoad, InnerErrorLoad, InnerProgressLoad);
    }

    public override IEnumerator ErrorLoad(string errorMessage)
    {
        yield return null;
    }

    public override IEnumerator FinishLoad(byte[] data)
    {
        yield return InnerProgressLoad(100, "Yüklendi.");
    }

    public override IEnumerator ProgressLoad(float progres, string message)
    {
        yield return null;
    }
}

