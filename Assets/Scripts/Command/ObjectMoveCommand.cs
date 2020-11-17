using UnityEngine;
using Motto.Domain;

public class ObjectMoveCommand : Command
{

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private GameObject gameObject;


    public ObjectMoveCommand(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.previousPosition = gameObject.transform.position;
        this.previousRotation = gameObject.transform.rotation;
    }

    public void DoExecute()
    {
        gameObject.transform.position = currentPosition;
        gameObject.transform.rotation = currentRotation;
    }

    public void Release(bool executed)
    {
        if (executed)
        {

        }
    }

    public void UndoExecute()
    {
        gameObject.transform.position = previousPosition;
        gameObject.transform.rotation = previousRotation;
    }

    public void Update()
    {
        this.currentPosition = gameObject.transform.position;
        this.currentRotation = gameObject.transform.rotation;
    }
}
