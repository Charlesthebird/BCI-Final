using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 8.0f;
    public float turboSpeedup = 2.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //var h = Input.GetAxis("Horizontal");
        //var v = Input.GetAxis("Vertical");

        // button up/down gives snappier controls on laptop
        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) movement.y = speed;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) movement.y = -speed;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) movement.x = -speed;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) movement.x = speed;
        // handle turbo
        if (Input.GetKey(KeyCode.LeftShift)) movement = movement * turboSpeedup;

        transform.Translate(movement * Time.deltaTime);
    }
}
