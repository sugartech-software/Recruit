using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoaderScene : IScene
{
    // Start is called before the first frame update
    void Start()
    {  
        LevelManager.Instance.LoadSceneAsync<MainScene>();
    }

    public override  void SceneLoadPostProcess(Scene scene){
        
    }

   
}
