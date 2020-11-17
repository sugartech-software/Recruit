using System;
using System.Collections;


public abstract class ChainAsyncLoad<T>: AsyncLoad<T>
{

    protected T data;
    protected AsyncLoad<T> asyncLoad;
    protected ChainAsyncLoad<T> childLoader;
    
    protected string sourceKey;
    protected bool breakChain;
 
    public ChainAsyncLoad<T> Child => childLoader;

   
    public  IEnumerator Load(string source, AsyncLoad<T> asyncLoad)
    {
        this.asyncLoad = asyncLoad;
        yield return Load(source);
    }

    public IEnumerator Load(string source)
    {
        breakChain = false;
        sourceKey = source;

        BeforeLoad(source);
    
        if (childLoader != null && !breakChain)
            yield return childLoader.Load(sourceKey);
    }


    public IEnumerator FinishLoad(T data)
    {
        this.data = data;
        yield return FinishLoad(this, data);
       
        yield return asyncLoad?.FinishLoad(data);
        
    }

    public IEnumerator ErrorLoad(T data, string message)
    {
        yield return ErrorLoad(message);

        yield return asyncLoad?.ErrorLoad(data, message);
    }

    public IEnumerator ProgressLoad(T data, string message)
    {

        yield return ProgressLoad(data,  message);

        yield return asyncLoad?.ProgressLoad(data, message);
        
    }


    public ChainAsyncLoad<T> AddChild(ChainAsyncLoad<T> chainLoader)
    {
        chainLoader.asyncLoad = this;
        this.childLoader = chainLoader;
        return chainLoader;
    }


    public abstract IEnumerator BeforeLoad(string source);

    public abstract IEnumerator FinishLoad(ChainAsyncLoad<T> chainAsyncLoad, T data);

    public abstract IEnumerator ErrorLoad(string errorMessage);

    public abstract IEnumerator ProgressLoad(float progress, string message);

   
}

