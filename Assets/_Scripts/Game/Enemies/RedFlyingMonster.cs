using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlyingMonster : GameElement
{
    public GameObject destroyPrefab;
    public BezierCurve curve;
    float curveDist = 0;
    public float speed = .05f;
    GameObject curveObj;


    [Tooltip("the frequency of the curve")]
    public int numPoints = 10;
    int cachedNumPoints;

    Transform pathParent;

    [Tooltip("the height of the curve")]
    public float curveLength = 25.0f;
    float cachedCurveLength;

    [Tooltip("the width of the curve")]
    public float curveDeviation = 0.0f;
    float cachedCurveDeviation;

    public float curviness = .5f;
    float cachedCurviness;

    float bciScale = 1f;



    protected new void Awake()
    {
        base.Awake();
        pathParent = GameObject.Find("EnemyPaths").transform;
        curveObj = new GameObject();
    }
    private void Start()
    {
        curveObj.transform.SetParent(pathParent);
        curveObj.transform.position = transform.position;
        curveObj.name = "EnemyPath_" + name;
        curve = curveObj.AddComponent<BezierCurve>();
        RebuildCurve();

        //var p1 = transform.position;
        //var p2 = transform.position - new Vector3(3, 5, 0);
        //var p3 = p2 - new Vector3(-5f, 5, 0);
        //var p4 = p3 - new Vector3(5f, 5, 0);
        //var p5 = p4 - new Vector3(-3f, 10, 0);
        //curve.AddCP(p1, p1 + Vector3.up * curviness, p1 + Vector3.down * curviness);
        //curve.AddCP(p2, p2 + Vector3.up * curviness, p2 + Vector3.down * curviness);
        //curve.AddCP(p3, p3 + Vector3.up * curviness, p3 + Vector3.down * curviness);
        //curve.AddCP(p4, p4 + Vector3.up * curviness, p4 + Vector3.down * curviness);
        //curve.AddCP(p5, p5 + Vector3.up * curviness, p5 + Vector3.down * curviness);
        //curve.AddCP(p1, p1 + Vector3.up * curviness, p1 + Vector3.down * curviness);
        //================================//
        //var p1 = new Vector3(3, 0, 0);
        //var p2 = new Vector3(6, 4, 0);
        //var p3 = new Vector3(5, 2, 0);
        //StartCoroutine(curve.AddCP(p1, p3, p2 + Vector3.up));
        //StartCoroutine(curve.AddCP(p2, p1, p3 + Vector3.up));
        //StartCoroutine(curve.AddCP(p3, p2, p1 + Vector3.up));
    }


    // Use this for initialization
    void Update()
    {
        // check if curve exists
        if (curve == null)
        {
            Debug.Log("ERROR!!! NO ENEMY PATH: " + name);
            return;
        }

        // check for any path updates
        if (cachedNumPoints != numPoints || cachedCurveLength != curveLength)
        {
            RebuildCurve();
        }
        // update the curve deviation if it has changed or the curviness has changed
        else if (cachedCurveDeviation != curveDeviation || cachedCurviness != curviness)
        {
            UpdateCurveDeviation();
        }

        // move the enemy
        curveDist += speed * Time.deltaTime;
        curveDist -= (int)curveDist;
        transform.position = curve.GetPosAtDist(curveDist);

        // update the curve deviation to test it
        //curveDeviation = Mathf.Sin(Time.time) * 3.0f;

        // ----------------------- update deviation based on bci 
        curveDeviation = bciScale * Mathf.Clamp(sceneController.bci.curBetaAverage, .2f, 1.5f);



        curve.RefreshCurve();
    }

    void UpdateCurveDeviation()
    {
        Vector3 newPos;
        for (int i = 0; i < numPoints; i++)
        {
            if (i % 2 == 0)
                newPos = new Vector3(curve.transform.position.x + curveDeviation, curve.transform.position.y - ((i / (float)numPoints) * curveLength));
            else
                newPos = new Vector3(curve.transform.position.x - curveDeviation, curve.transform.position.y - ((i / (float)numPoints) * curveLength));
            //curve.segments[i].startCP.transform.position = newPos;
            curve.segments[i].startCP.transform.position = newPos;
            curve.segments[i].startCP.leftTangentTransform.localPosition = Vector3.up * curviness;
            curve.segments[i].startCP.rightTangentTransform.localPosition = Vector3.down * curviness;
        }
        //curve.RefreshCurve();
        cachedCurveDeviation = curveDeviation;
        cachedCurviness = curviness;
    }

    void RebuildCurve()
    {
        // destroy everything and clear list
        if (curve.segments != null)
        {
            for (int i = 0; i < curve.segments.Count; i++)
                Destroy(curve.segments[i].startCP.gameObject);
            curve.segments.Clear();
        }
        // add control points back in
        Vector3 newPos;
        for (int i = 0; i < numPoints; i++)
        {
            if (i % 2 == 0)
                newPos = new Vector3(curve.transform.position.x + curveDeviation, curve.transform.position.y - ((i / (float)numPoints) * curveLength));
            else
                newPos = new Vector3(curve.transform.position.x - curveDeviation, curve.transform.position.y - ((i / (float)numPoints) * curveLength));
            //curve.AddCP(newPos, (Vector3.up * curveDeviation), (Vector3.down * curveDeviation));
            curve.AddCP(newPos, (Vector3.up * curviness), (Vector3.down * curviness));
        }
        curve.RefreshCurve();
        // cache the number of points and the curve length and the curve deviation that was used to build this curve
        cachedNumPoints = numPoints;
        cachedCurveLength = curveLength;
        cachedCurveDeviation = curveDeviation;
        cachedCurviness = curviness;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var w = coll.GetComponent<PlayerWeapon>();
        if (w != null)
        {
            sceneController.player.AddToScore(7);
            // destroy the laser
            Destroy(w.gameObject);
            // destroy this enemy
            Instantiate(destroyPrefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (curveObj != null) Destroy(curveObj);
    }
}
