using UnityEngine;
using Motto.Domain;

public class ObjectRemoverCommand : Command
{

    private GameObject gameObject;



    public ObjectRemoverCommand(GameObject gameObject)
    {
        this.gameObject = gameObject;

    }

    public void DoExecute()
    {
        gameObject.SetActive(false);
    }

    public void Release(bool executed)
    {
        if (executed)
            GameObject.Destroy(gameObject);
    }

    public void UndoExecute()
    {
        gameObject.SetActive(true);
    }

}
