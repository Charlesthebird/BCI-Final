using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameIfKeyPressed : GameElement {
    	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey)
        {
            sceneController.screenFade.GetComponent<Animator>().Play("LevelFadeInAndStart");
        }
	}
}
