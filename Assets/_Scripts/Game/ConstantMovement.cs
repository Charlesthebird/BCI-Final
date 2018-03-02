using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    public float speed = -.5f;

    // Update is called once per frame
    void Update()
    {
        var p = transform.localPosition;
        transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
    }
}
