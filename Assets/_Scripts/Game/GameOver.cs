using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public float restartDelay = 1;
    bool readyToRestart = false;
    float readyTime;
    TextMeshProUGUI t;


    private void Start()
    {
        readyTime = Time.time + restartDelay;
        t = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    void Update () {
        if (Time.time < readyTime) return;
        t.text = "Game Over<br><size=20px>Press any button to restart</size>";
        // restart once any key is pressed and released
        if (Input.anyKey)
        {
            readyToRestart = true;
        }
        if(readyToRestart && !Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
