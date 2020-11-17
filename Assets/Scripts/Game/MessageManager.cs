using UnityEngine;
using System.Collections;
using Motto.Application;

public class MessageManager : MonoBehaviour
{

    private static ProgressTextPanel messageTextPanel;

    public static ProgressTextPanel Instance
    {
        get
        {
            if (messageTextPanel == null)
            {

                GameObject go = GameObject.Find("MessagePanelCanvas");

                if (go == null)
                {
                    go = Resources.Load<GameObject>("UI/MessagePanelCanvas");
                }

                GameObject goInstance = Instantiate(go);
                goInstance.name = "MessagePanel";
                messageTextPanel = goInstance.GetComponentInChildren<ProgressTextPanel>();

            }
            return messageTextPanel;
        }
    }
}
