using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ProgressTextEnum
{
    Error,
    Loading
}

public class ProgressText : MonoBehaviour
{

    public string Text;
    public ProgressTextEnum Type = ProgressTextEnum.Loading;

    public float TimeAlive = 0;
    private float timeAlive = 0;

    private Text textComponent;
    private int loadingProgress = 1;
    private int MAX_LOADING_PROGRESS = 4;
    private float time = 2;
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.text = Text;
    }

    // Update is called once per frame
    void Update()
    {

        if (Type == ProgressTextEnum.Loading)
        {
            if (time > 0.1)
            {
                string loadingPorgressState = "";
                for (int i = 0; i < loadingProgress; i++)
                {
                    loadingPorgressState += ".";
                }

                textComponent.text = Text + loadingPorgressState;

                loadingProgress++;
                loadingProgress %= MAX_LOADING_PROGRESS;

                time = 0;
            }
            time += Time.deltaTime;
        }
        else
        {
            textComponent.text = Text;
        }

        
        if(TimeAlive > 0){
            timeAlive += Time.deltaTime;
            if (timeAlive > TimeAlive){
                gameObject.transform.parent.gameObject.SetActive(false);
                Destroy(gameObject);

            }
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
