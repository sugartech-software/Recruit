using System;
using System.Collections;
using System.Collections.Generic;
using Motto.Domain;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Motto.Application
{

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {

                if (GameObject.Find("GameManager") == null)
                {
                    Initiate();
                }

                return instance;
            }
        }


        private Canvas canvas;
        public Canvas Canvas
        {
            get
            {
                if (canvas == null)
                {
                    foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
                    {
                        Canvas c = g.GetComponent<Canvas>();
                        if (c != null)
                            canvas = c;
                    }
                }
                return canvas;
            }
        }


        GameObject selectedObject;
        public GameObject SelectedObject
        {
            get
            {
                return selectedObject;
            }
            set
            {
                selectedObject = value;
            }
        }

        public bool IsSelected(GameObject checkObject)
        {
            return selectedObject != null && selectedObject == checkObject;
        }

        private int layer;
        public int Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        private ProgressTextPanel progressTextPanel;

        public ProgressTextPanel ProgressManager
        {
            get
            {
                if (progressTextPanel == null)
                {
                    GameObject go = Resources.Load<GameObject>("UI/ProgressTextPanelCanvas");

                    //GameObject goInstance = Instantiate(go, Canvas.gameObject.transform);
                    GameObject goInstance = Instantiate(go);
                    goInstance.name = "ProgressTextPanelCanvas";
                    progressTextPanel = goInstance.GetComponentInChildren<ProgressTextPanel>();
                }
                return progressTextPanel;
            }
        }


        public List<CatalogCategory> categories;

        public static void Initiate()
        {
            GameObject gameObject = new GameObject("GameManager");
            gameObject.AddComponent<GameManager>();

            instance = gameObject.GetComponent<GameManager>();
            GameObject.DontDestroyOnLoad(instance);

        }


        public void OnLevelLoad()
        {
            progressTextPanel = null;
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            foreach (var comp in components)
            {
                if (!(comp is GameManager))
                {
                    DestroyImmediate(comp);
                }
            }
        }
    }

}

