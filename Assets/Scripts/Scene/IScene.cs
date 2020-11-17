using Motto.Application;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class IScene : MonoBehaviour
{


    public abstract void SceneLoadPostProcess(Scene scene);

    public void AsyncSceneLoadProcess(Scene scene, AsyncOperation asyncOperation)
    {

        GameManager.Instance.OnLevelLoad();
        SceneLoadPostProcess(scene);
    }


    protected GameObject getCameraGrouo(Scene scene)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.layer == 18)
            {
                return gameObject;
            }
        }

        return null;
    }

    protected GameObject hasGameObject(Scene scene, string name)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.name.StartsWith(name))
            {
                return gameObject;
            }
        }

        return null;
    }
}

