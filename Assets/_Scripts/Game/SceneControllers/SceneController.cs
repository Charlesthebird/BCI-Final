using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PostProcessing;

public class SceneController : MonoBehaviour {
    public UnityEvent onAwake;    

    [HideInInspector]
    public GameObject introContainer;
    [HideInInspector]
    public GameObject levelContainer;
    [HideInInspector]
    public GameObject uiContainer;
    [HideInInspector]
    public GameObject gameOverContainer;
    [HideInInspector]
    public GameObject screenFade;
    [HideInInspector]
    public OpenBCIListener bci;
    [HideInInspector]
    public TextMeshProUGUI scoreText;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Camera mainCamera;

    PostProcessingBehaviour postProc;

    public AudioSource[] audioSources;

    void Awake () {
        onAwake.Invoke();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        postProc = mainCamera.GetComponent<PostProcessingBehaviour>();
        player = GameObject.Find("Player").GetComponent<Player>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        gameOverContainer = GameObject.Find("GameOverContainer");
        introContainer = GameObject.Find("IntroContainer");
        levelContainer = GameObject.Find("LevelContainer");
        uiContainer = GameObject.Find("UIContainer");
        screenFade = GameObject.Find("ScreenFade");
        bci = GameObject.Find("BCIListener").GetComponent<OpenBCIListener>();

        // set up the game to start
        levelContainer.SetActive(false);
        uiContainer.SetActive(false);
        gameOverContainer.SetActive(false);
    }

    private void Update()
    {
        // update the postproc settings
        // chromatic aberration
        var newCASettings = postProc.profile.chromaticAberration.settings;
        newCASettings.intensity = Mathf.Min(3.0f, bci.curBetaAverage);
        postProc.profile.chromaticAberration.settings = newCASettings;
        // vignette
        var newVignetteSettings = postProc.profile.vignette.settings;
        newVignetteSettings.intensity = Mathf.Min(.5f, bci.curBetaAverage);
        postProc.profile.vignette.settings = newVignetteSettings;
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
