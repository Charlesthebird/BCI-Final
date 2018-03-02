﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Properties")]
    public float speed = 8.0f;
    public float turboSpeedup = 2.0f;

    // control the possible movement area for the player ship
    float topMoveExtent = 4.4f;
    float bottomMoveExtent = -4.4f;
    float leftMoveExtent = -8.2f;
    float rightMoveExtent = 8.2f;

    [Header("References")]
    public GameObject laser1Obj;
    public AudioSource[] audioSources;

    Transform weaponSpawn;


    // Use this for initialization
    void Start () {
        weaponSpawn = transform.Find("WeaponSpawn");
	}
	
	// Update is called once per frame
	void Update () {
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var b = Instantiate(laser1Obj) as GameObject;
            b.transform.position = weaponSpawn.position;
            audioSources.Where(s => s.name == "Laser1Fire").Single().Play();
        }

    }
}
