using System;
using System.Collections;
using Motto.Application;

public abstract class ChainLoader : Loader
{

    private long time ;
    private bool showTime;

    Func<byte[], IEnumerator> finishLoad;
    Func<string, IEnumerator> errolLoad;
    Func<float, string, IEnumerator> progressLoad;

    protected ChainLoader childLoader;
    protected ChainLoader parentLoader;
    protected string sourceKey;
    protected bool breakChain;

    public ChainLoader Parent => parentLoader;
 
    public ChainLoader Child => childLoader;

   
    public override IEnumerator Load(string source, Func<byte[], IEnumerator> finish = null, Func<string, IEnumerator> error = null, Func<float, string, IEnumerator> progress = null)
    {
        
        finishLoad = finish;
        errolLoad = error;
        progressLoad = progress;

        yield return Load(source);
    }

    protected IEnumerator InnerFinishLoad(byte[] data)
    {
        if(showTime){
            time = System.DateTime.Now.Ticks - time;
            yield return InnerProgressLoad(0, this.GetType().Name + ":" + time);
        }
       
        yield return FinishLoad(data);
        if (parentLoader != null)
        {
            yield return parentLoader.InnerFinishLoad(data);
        }else if(finishLoad != null)
        {
            yield return finishLoad(data);
        }
    }

    protected IEnumerator InnerErrorLoad(string message)
    {

        yield return ErrorLoad(message);

        if (parentLoader != null)
        {
            yield return parentLoader.InnerErrorLoad(message);
        }else if( errolLoad != null)
        {
            yield return errolLoad(message);
        }
    }

    protected IEnumerator InnerProgressLoad(float progress, string message)
    {

        yield return ProgressLoad(progress, message);

        if (parentLoader != null)
        {
            yield return parentLoader.InnerProgressLoad(progress, message);
        }
        else if (progressLoad != null)
        {
           yield return progressLoad(progress, message);
        }
    }


    public IEnumerator Load(string source)
    {
        breakChain = false;
        sourceKey = source;
        
        if(showTime){
            time = System.DateTime.Now.Ticks;
        }
        
        yield return BeforeLoad(source);

        if (childLoader != null && !breakChain)
            yield return childLoader.Load(sourceKey);
    }


    public ChainLoader AddChild(ChainLoader chainLoader)
    {
        chainLoader.parentLoader = this;
        this.childLoader = chainLoader;
        return chainLoader;
    }

    public abstract IEnumerator BeforeLoad(string source);

    public abstract IEnumerator FinishLoad(byte[] data);

    public abstract IEnumerator ErrorLoad(string errorMessage);

    public abstract IEnumerator ProgressLoad(float progress,  string progressMessage);
   
}

