using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Motto.Application;
using Motto.Domain;
using UnityEngine.Events;

public class LevelManager
{
    public static int roomType;

    private Type sceneType;

    private List<Type> previousSceneTypes = new List<Type>();



    private static Scene currentScene = default(Scene);
    private static Scene previousScene = default(Scene);

    public static Scene Scene
    {
        get
        {
            return currentScene;
        }
    }

    private static LevelManager instance;

    private LevelManager()
    {
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
    }

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LevelManager();

            return instance;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(roomType - 10);
    }

    public void LoadPreviousScene()
    {
        popSceneType();
        GameManager.Instance.StartCoroutine(LoadAsyncScene(sceneType.Name, null));
    }


    public void LoadAsyncScene(string name)
    {
        sceneType = typeof(IScene);
        GameManager.Instance.StartCoroutine(LoadAsyncScene(name, LoadSceneMode.Single, null));
    }

    public IEnumerator LoadAsyncScene(string name, AsyncLoad<AsyncOperation> processor)
    {
        yield return LoadAsyncScene(name, LoadSceneMode.Single, processor);
    }


    public IEnumerator LoadAsyncScene(string name, LoadSceneMode sceneMode, AsyncLoad<AsyncOperation> processor)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, sceneMode);

        while (asyncLoad?.progress < 0.9f)
        {
            yield return processor?.ProgressLoad(asyncLoad, asyncLoad?.progress.ToString());
        }

        yield return processor?.ProgressLoad(asyncLoad, asyncLoad?.progress.ToString());

        yield return processor?.FinishLoad(asyncLoad);
    }



    public void LoadSceneAsync<T>() where T : IScene
    {
        LoadSceneAsync<T>(typeof(T).Name);
    }


    public void LoadSceneAsync<T>(AsyncLoad<AsyncOperation> levelLoadProcessor) where T : IScene
    {
        LoadSceneAsync<T>(typeof(T).Name, levelLoadProcessor);
    }


    public void LoadSceneAsync<T>(string path) where T : IScene
    {
        LoadSceneAsync<T>(path, null);
    }


    public void LoadSceneAsync<T>(string path, AsyncLoad<AsyncOperation> levelLoadProcessor) where T : IScene
    {

        pushSceneType(typeof(T));

        LevelManager.previousScene = currentScene;
        LevelManager.currentScene = default(UnityEngine.SceneManagement.Scene);
        GameManager.Instance.StartCoroutine(LoadAsyncScene(path, levelLoadProcessor));
    }


    public void LoadSceneAsync<T>(string path, LoadSceneMode sceneMode, AsyncLoad<AsyncOperation> levelLoadProcessor) where T : IScene
    {
        pushSceneType(typeof(T));

        LevelManager.previousScene = currentScene;
        LevelManager.currentScene = default(UnityEngine.SceneManagement.Scene);
        GameManager.Instance.StartCoroutine(LoadAsyncScene(path, sceneMode, levelLoadProcessor));
    }

    public IEnumerator LoadSceneAsyncYield<T>(string path, AsyncLoad<AsyncOperation> levelLoadProcessor) where T : IScene
    {
        pushSceneType(typeof(T));

        LevelManager.previousScene = currentScene;
        LevelManager.currentScene = default(UnityEngine.SceneManagement.Scene);

        yield return LoadAsyncScene(path, levelLoadProcessor);
    }


    private void SceneManager_SceneLoaded(Scene sceneP, LoadSceneMode sceneMode)
    {
        LevelManager.currentScene = sceneP;

        GameObject gameObject = GameObject.Find(sceneType.Name);
        if (gameObject == null)
        {
            gameObject = new GameObject(sceneType.Name);
            SceneManager.MoveGameObjectToScene(gameObject, sceneP);
        }

        IScene passSceneInstance = gameObject.GetComponent<IScene>();

        if (passSceneInstance == null)
            passSceneInstance = (IScene)gameObject.AddComponent(sceneType);

        if (passSceneInstance != null)
            passSceneInstance.AsyncSceneLoadProcess(LevelManager.Scene, null);
    }


    private void pushSceneType(Type addedSceneType)
    {
        previousSceneTypes.Add(sceneType);
        sceneType = addedSceneType;
    }

    private void popSceneType()
    {
        int index = previousSceneTypes.Count - 1;
        index = index < 0 ? 0 : index;
        sceneType = previousSceneTypes[index];
        previousSceneTypes.RemoveAt(index);
    }
}
