using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedGameObjectControls : GameElement {


    public void SetSelfActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void SetIntroTextActiveTrue()
    {
        sceneController.screenFade.SetActive(true);
    }
    public void StartLevel()
    {
        sceneController.StartLevel();
    }
}
