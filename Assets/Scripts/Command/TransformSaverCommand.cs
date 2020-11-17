using System.Collections.Generic;
using UnityEngine;

public class TransformSaverCommand : Command
{


    public static TransformSaverCommand Save(GameObject objectsToMove)
    {
        return new TransformSaverCommand(new GameObject[1] { objectsToMove });
    }
    public static TransformSaverCommand Save(GameObject[] objectsToMove)
    {
        return new TransformSaverCommand(objectsToMove);
    }

    public static TransformSaverCommand Save(TransformSaverCommand command)
    {
        End(command);
        return new TransformSaverCommand(command.objectsToMove.ToArray());
    }

    public static TransformSaverCommand End(TransformSaverCommand command)
    {
        if (command != null)
        {
            command.doExecute();
        }
        return null;
    }



    private List<GameObject> objectsToMove;
    private List<Vector3> prevObjectsPositions;
    private List<Vector3> currentObjectsPositions;
    private List<Quaternion> prevObjectsRotations;
    private List<Quaternion> currentObjectsRotations;

    private TransformSaverCommand(GameObject[] objectsToMove)
    {
        this.objectsToMove = new List<GameObject>();
        this.objectsToMove.AddRange(objectsToMove);

        this.prevObjectsPositions = new List<Vector3>();
        this.currentObjectsPositions = new List<Vector3>();
        this.prevObjectsRotations = new List<Quaternion>();
        this.currentObjectsRotations = new List<Quaternion>();
        foreach (var item in objectsToMove)
        {
            this.prevObjectsPositions.Add(item.transform.position);
            this.currentObjectsPositions.Add(item.transform.position);
            this.prevObjectsRotations.Add(item.transform.rotation);
            this.currentObjectsRotations.Add(item.transform.rotation);
        }
    }
    private void doExecute()
    {
        updateTransform();
        CommandManager.Instance.DoExecute(this);
    }

    private void updateTransform()
    {
        this.currentObjectsPositions.Clear();
        this.currentObjectsRotations.Clear();
        foreach (var item in objectsToMove)
        {
            this.currentObjectsPositions.Add(item.transform.position);
            this.currentObjectsRotations.Add(item.transform.rotation);
        }
    }

    private void popTransform(List<Quaternion> transformMatris, List<Vector3> transformPositions)
    {
        int i = 0;
        foreach (var item in objectsToMove)
        {
            item.transform.position = transformPositions[i];
            item.transform.rotation = transformMatris[i];
            i++;
        }
    }

    public void DoExecute()
    {
        popTransform(this.currentObjectsRotations, this.currentObjectsPositions);
    }

    public void Release(bool executed)
    {
        this.objectsToMove.Clear();
        this.currentObjectsPositions.Clear();
        this.prevObjectsPositions.Clear();
        this.currentObjectsRotations.Clear();
        this.prevObjectsRotations.Clear();
    }

    public void UndoExecute()
    {
        popTransform(this.prevObjectsRotations, this.prevObjectsPositions);
    }
}