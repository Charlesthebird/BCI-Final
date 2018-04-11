using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : GameElement {
    [Header("Properties")]
    public float speed = 8.0f;
    public float turboSpeedup = 2.0f;
    public float weaponCooldownTime = 0.1f;

    // control the possible movement area for the player ship
    float topMoveExtent = 4.4f;
    float bottomMoveExtent = -4.4f;
    float leftMoveExtent = -8.2f;
    float rightMoveExtent = 8.2f;

    [Header("References")]
    public GameObject laser1Obj;
    public AudioSource[] audioSources;

    Transform weaponSpawn;
    bool cooledDown = true;


    // Use this for initialization
    void Start () {
        weaponSpawn = transform.Find("WeaponSpawn");
	}
	
	// Update is called once per frame
	void Update () {
        // time wasn't good to change
        //float baseSpeed = .5f;
        //var recentMax = 0.0f;
        //if(sceneController.bci.cachedEEGData.Count > 0)
        //    recentMax = sceneController.bci.cachedEEGData.Max(d => d.GetBeta());
        //Time.timeScale = Mathf.Min(2.0f, baseSpeed + (sceneController.bci.curBetaAverage / recentMax));

        //Debug.Log(sceneController.bci.curBetaValue);
        //var h = Input.GetAxis("Horizontal");
        //var v = Input.GetAxis("Vertical");
        
        // MOVEMENT
        // button up/down gives snappier controls on laptop
        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) movement.y = speed;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) movement.y = -speed;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) movement.x = -speed;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) movement.x = speed;
        // handle turbo
        if (Input.GetKey(KeyCode.LeftShift)) movement = movement * turboSpeedup;

        // make the movement framerate independent
        movement *= Time.deltaTime;

        // check movement extents to stay in bounds
        if ((transform.position + movement).x > rightMoveExtent ||
            (transform.position + movement).y > topMoveExtent ||
            (transform.position + movement).x < leftMoveExtent ||
            (transform.position + movement).y < bottomMoveExtent)
        {
            movement = Vector3.zero;
        }

        // apply the movement
        transform.Translate(movement);


        // WEAPONS
        if(cooledDown && Input.GetKey(KeyCode.Space))
        {
            var b = Instantiate(laser1Obj) as GameObject;
            b.transform.position = weaponSpawn.position;
            audioSources.Where(s => s.name == "Laser1Fire").Single().Play();
            StartCoroutine(WaitForCooldown());
        }
    }
    IEnumerator WaitForCooldown()
    {
        cooledDown = false;
        yield return new WaitForSeconds(weaponCooldownTime);
        cooledDown = true;
    }
}
