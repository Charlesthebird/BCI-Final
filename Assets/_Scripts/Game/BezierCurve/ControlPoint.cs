using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour {
    public Transform leftTangentTransform;
    public Transform rightTangentTransform;

    public BezierCurve curve;


    public void SetWorldPosition(Vector3 newPos)
    {
        transform.position = newPos;
        curve.RefreshCurve();
    }
    public void SetLeftTangentWorldPos(Vector3 newPos)
    {
        leftTangentTransform.position = newPos;
        curve.RefreshCurve();
    }
    public void SetRightTangentWorldPos(Vector3 newPos)
    {
        rightTangentTransform.position = newPos;
        curve.RefreshCurve();
    }

    public static GameObject CreateCPObject(Vector3 worldPos, Vector3 leftTangent, Vector3 rightTangent, BezierCurve curve)
    {
        // set up the control point gameObject
        var g = new GameObject();
        g.transform.parent = curve.transform;
        g.transform.position = worldPos;
        g.name = "Control Point - " + g.transform.GetSiblingIndex();
        var cp = g.AddComponent<ControlPoint>();
        cp.curve = curve;
        // set up the tangent gameObjects
        var ltObj = new GameObject();
        ltObj.transform.parent = cp.transform;
        ltObj.transform.localPosition = leftTangent;
        ltObj.name = "Prev Tangent";
        cp.leftTangentTransform = ltObj.transform;
        var rtObj = new GameObject();
        rtObj.transform.parent = cp.transform;
        rtObj.transform.localPosition = rightTangent;
        rtObj.name = "Next Tangent";
        cp.rightTangentTransform = rtObj.transform;
        return g;
    }
}
