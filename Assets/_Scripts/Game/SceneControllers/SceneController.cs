﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SceneController : MonoBehaviour {
    public UnityEvent onAwake;    

    [HideInInspector]
    public GameObject introContainer;
    [HideInInspector]
    public GameObject levelContainer;
    [HideInInspector]
    public GameObject uiContainer;
    [HideInInspector]
    public GameObject screenFade;
    [HideInInspector]
    public OpenBCIListener bci;

    public AudioSource[] audioSources;

    void Awake () {
        onAwake.Invoke();

        introContainer = GameObject.Find("IntroContainer");
        levelContainer = GameObject.Find("LevelContainer");
        uiContainer = GameObject.Find("UIContainer");
        screenFade = GameObject.Find("ScreenFade");
        bci = GameObject.Find("BCIListener").GetComponent<OpenBCIListener>();

        // set up the game to start
        levelContainer.SetActive(false);
        uiContainer.SetActive(false);
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }
    // necessary to run the start game in a coroutine because the uicontainer needs a frame to render before enabling the level objects
    IEnumerator StartLevelCoroutine()
    {
        introContainer.SetActive(false);
        uiContainer.SetActive(true);
        yield return new WaitForEndOfFrame();
        levelContainer.SetActive(true);
    }

    public void PlaySound(string soundName)
    {
        audioSources.Where(s => s.name == soundName).Single().Play();
    }
}
