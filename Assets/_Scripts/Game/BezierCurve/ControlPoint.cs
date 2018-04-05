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

    public BezierCurve curve;



    private void Start()
    {
        UpdateReferences();
    }
    private void UpdateReferences()
    {
        // update the references to the position and tangents
        pos = transform.position;
        leftTangent = ltTransform.position;
        rightTangent = rtTransform.position;
    }

    public void SetWorldPosition(Vector3 newPos)
    {
        transform.position = newPos;
        UpdateReferences();
        curve.RefreshCurve();
    }
    public void SetLeftTangentWorldPos(Vector3 newPos)
    {
        ltTransform.position = newPos;
        UpdateReferences();
        curve.RefreshCurve();
    }
    public void SetRightTangentWorldPos(Vector3 newPos)
    {
        rtTransform.position = newPos;
        UpdateReferences();
        curve.RefreshCurve();
    }

    public static GameObject CreateCPObject(Vector3 localPos, Vector3 leftTangent, Vector3 rightTangent, BezierCurve curve)
    {
        // set up the control point gameObject
        var g = new GameObject();
        g.transform.parent = curve.transform;
        g.transform.localPosition = localPos;
        g.name = "Control Point - " + g.transform.GetSiblingIndex();
        var cp = g.AddComponent<ControlPoint>();
        cp.curve = curve;
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
