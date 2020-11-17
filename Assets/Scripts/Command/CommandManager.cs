using System;
using System.Collections.Generic;
using UnityEngine;
public class CommandManager : MonoBehaviour
{

    private CommandManager()
    {

    }

    private static CommandManager instance;

    public static CommandManager Instance
    {
        get
        {
            if (GameObject.Find("CommandManager") == null)
            {
                GameObject gameObject = new GameObject("CommandManager");
                instance = gameObject.AddComponent<CommandManager>();
            }

            return instance;
        }
    }

    private List<Command> commandList = new List<Command>();
    private int commandIndex = -1;

    public void DoExecute(Command command)
    {

        if (commandIndex < commandList.Count - 1)
        {
            for (int i = commandList.Count - 1; i > commandIndex; i--)
            {
                commandList[i].Release(false);
                commandList.RemoveAt(i);
            }
        }

        commandList.Add(command);
        DoExecute();

        if (commandList.Count > 5)
        {
            commandList[0].Release(true);
            commandList.RemoveAt(0);
            commandIndex = commandList.Count - 1;
        }
    }

    public void AddExecute(Command command)
    {

        commandIndex++;
        if (commandIndex < commandList.Count - 1)
        {
            for (int i = commandList.Count - 1; i > commandIndex - 1; i--)
            {
                if (i >= 0)
                {
                    commandList[i].Release(false);
                    commandList.RemoveAt(i);
                }
            }
        }

        commandList.Add(command);

        if (commandList.Count > 5)
        {
            commandList[0].Release(true);
            commandList.RemoveAt(0);
            commandIndex = commandList.Count - 1;
        }
    }

    public void DoExecute()
    {
        commandIndex++;
        if (commandIndex >= commandList.Count)
        {
            commandIndex = commandList.Count - 1;
            return;
        }

        commandList[commandIndex].DoExecute();
        //Debug.Log("Command Execute DO " + commandList[commandIndex]);
    }

    public void UndoExecute()
    {
        if (commandIndex <= -1)
        {
            commandIndex = -1;
            return;
        }

        commandList[commandIndex].UndoExecute();
        // Debug.Log("Command Execute UNDO " + commandList[commandIndex]);
        commandIndex--;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
        {
            DoExecute();
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            UndoExecute();
        }
    }
}
