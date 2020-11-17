using System.Collections;
using Motto.Application;
using Motto.Domain;


public abstract class AsyncAssetLoader<T, K> where K : BaseEntity
{

    public AsyncAssetLoader()
    {
    }

    protected T asset;
    protected K entity;
    protected bool isInProgress;
    protected AsyncLoad<T> asyncLoad;
    protected ChainLoader chainLoader;
    protected OnLoadingListener loadingListener;

    public bool IsInProgress
    {
        get
        {
            return isInProgress;
        }
    }

    public void Load(string fileUrl, K entity, OnLoadingListener loadingListener)
    {
        Load(fileUrl, entity, null, loadingListener);
    }

    public void Load(string fileUrl, K entity)
    {
        Load(fileUrl, entity, null, null);
    }

    public void Load(string fileUrl, K entity, AsyncLoad<T> asyncLoad, OnLoadingListener loadingListener)
    {
        GameManager.Instance.StartCoroutine(LoadWithEnumerator(fileUrl, entity, asyncLoad, loadingListener));
    }


    public virtual IEnumerator LoadWithEnumerator(string fileUrl, K entity, AsyncLoad<T> asyncLoad)
    {
        this.entity = entity;
        this.isInProgress = true;
        this.asyncLoad = asyncLoad;
        yield return chainLoader?.Load(fileUrl, FinishLoad, ErrorLoad, ProgressLoad);
    }

    public virtual IEnumerator LoadWithEnumerator(string fileUrl, K entity, AsyncLoad<T> asyncLoad, OnLoadingListener loadingListener)
    {

        this.loadingListener = loadingListener;
        loadingListener?.Start();
        yield return LoadWithEnumerator(fileUrl, entity, asyncLoad);
    }


    protected IEnumerator FinishLoad(byte[] data)
    {
        yield return ConvertData(data);

        isInProgress = false;

        yield return asyncLoad?.FinishLoad(asset);

        loadingListener?.End();
    }

    protected IEnumerator ErrorLoad(string errorMessage)
    {
        ProgressText progressText = GameManager.Instance.ProgressManager.AddMessage(errorMessage);

        progressText.Text = errorMessage;
        progressText.Type = ProgressTextEnum.Error;
        isInProgress = false;
        loadingListener?.Error(0, errorMessage);
        return asyncLoad?.ErrorLoad(asset, errorMessage);
    }


    protected IEnumerator ProgressLoad(float progress, string message)
    {
        loadingListener?.Progress(progress, entity?.name + ":" + message);
        return asyncLoad?.ProgressLoad(asset, entity?.name + ":" + message);
    }

    public abstract IEnumerator ConvertData(byte[] data);



}
