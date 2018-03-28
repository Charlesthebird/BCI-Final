using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour {
    public Vector3 pos;
    public Vector3 leftTangent;
    public Vector3 rightTangent;

    public Transform ltTransform;
    public Transform rtTransform;

    private void Update()
    {
        // update the references to the position and tangents
        pos = transform.position;
        leftTangent = ltTransform.position;
        rightTangent = rtTransform.position;
    }

    public static GameObject CreateCPObject(Vector3 localPos, Vector3 leftTangent, Vector3 rightTangent, Transform parent)
    {
        // set up the control point gameObject
        var g = new GameObject();
        g.transform.parent = parent;
        g.transform.localPosition = localPos;
        g.name = "Control Point - " + g.transform.GetSiblingIndex();
        var cp = g.AddComponent<ControlPoint>();
        // set up the tangent gameObjects
        var ltObj = new GameObject();
        ltObj.transform.parent = cp.transform;
        ltObj.transform.position = leftTangent;
        ltObj.name = "Prev Tangent";
        cp.ltTransform = ltObj.transform;
        var rtObj = new GameObject();
        rtObj.transform.parent = cp.transform;
        rtObj.transform.position = rightTangent;
        rtObj.name = "Next Tangent";
        cp.rtTransform = rtObj.transform;
        return g;
    }
}
