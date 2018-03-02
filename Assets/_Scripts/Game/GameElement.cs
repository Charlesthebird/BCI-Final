using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameElement : MonoBehaviour
{
    [HideInInspector]
    public SceneController sceneController;

    void Awake()
    {
        sceneController = GameObject.FindObjectOfType<SceneController>();
    }

}
