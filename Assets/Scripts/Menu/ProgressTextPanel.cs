using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTextPanel : MonoBehaviour
{


    public GameObject protextTextInstance;

    private List<ProgressText> messages;
    private bool needsUpdate = true;

    // Update is called once per frame
    void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
        }
    }

    public ProgressText AddMessage(string message)
    {
        gameObject.SetActive(true);
        GameObject progressObject = Instantiate(protextTextInstance, transform);
        progressObject.SetActive(true);
        progressObject.GetComponent<ProgressText>().Text = message;
        needsUpdate = true;


        int childCount = transform.childCount;
        Transform childTransform = null;
        for (int i = 0; i < childCount; i++)
        {
            childTransform = transform.GetChild(i);
            childTransform.localPosition = new Vector3(0, (i) * -20, 0);
        }
        needsUpdate = false;

        return progressObject.GetComponent<ProgressText>();
    }

    public ProgressText ShowMessage(string message, float time = -1)
    {
        Clear();
        ProgressText m = AddMessage(message);
        m.Type = ProgressTextEnum.Error;
        if (time > 0)
            m.TimeAlive = time;
        return m;
    }

    public void RemoveMessage(ProgressText message)
    {
        Destroy(message.gameObject);
        if (transform.childCount < 1)
        {
            gameObject.SetActive(false);
        }
        needsUpdate = true;
    }

    public void Clear()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);
    }
}
