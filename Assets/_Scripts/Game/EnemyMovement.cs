using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {


    public float speed = 5f;
    public float mag;
    // Update is called once per frame
    void Update()
    {
        var p = transform.localPosition;
        transform.Translate(new Vector3(Mathf.Sin(Time.time) * mag, speed * Time.deltaTime, 0));
    }
}
