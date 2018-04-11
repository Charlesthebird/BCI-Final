using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameElement : MonoBehaviour
{
    [HideInInspector]
    public SceneController sceneController;

    protected void Awake()
    {
        sceneController = GameObject.FindObjectOfType<SceneController>();
    }

}
