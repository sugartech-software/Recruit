using UnityEngine;
using Motto.Domain;

public class MultipleObjectRemoverCommand : Command
{

    private GameObject[] gameObjects;



    public MultipleObjectRemoverCommand(GameObject[] gameObjects)
    {
        this.gameObjects = gameObjects;

    }

    public void DoExecute()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
        
    }

    public void Release(bool executed)
    {
        if (executed){
             foreach (var gameObject in gameObjects)
            {
                 GameObject.Destroy(gameObject);
            }  
        }
    }

    public void UndoExecute()
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }

}
