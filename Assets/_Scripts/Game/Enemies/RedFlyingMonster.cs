using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlyingMonster : GameElement {
    public GameObject destroyPrefab;
    public BezierCurve curve;
    float curveDist = 0;
    public float speed = .05f;
    public float curviness = 2.0f;
    GameObject curveObj;
    public int numPoints = 10;

    private IEnumerator Start()
    {
        //curve = GameObject.FindObjectOfType<BezierCurve>();
        //return;
        var pathParent = GameObject.Find("EnemyPaths").transform;
        curveObj = new GameObject();
        yield return new WaitForEndOfFrame();
        curveObj.transform.parent = pathParent;
        curveObj.name = "EnemyPath_" + name;
        curve = curveObj.AddComponent<BezierCurve>();
        // make a for loop to add the curve control points
        var curveLength = 80.0f;
        for(int i=0; i<numPoints; i++)
        {
            //var newPos = new Vector3(transform.position.x, transform.position.y + ((i / (float)numPoints) * curveLength)));
            //curve.AddCP();
        }



        var p1 = transform.position;
        var p2 = transform.position - new Vector3(3, 5, 0);
        var p3 = p2 - new Vector3(-5f, 5, 0);
        var p4 = p3 - new Vector3(5f, 5, 0);
        var p5 = p4 - new Vector3(-3f, 10, 0);
        curve.AddCP(p1, p1 + Vector3.up * curviness, p1 + Vector3.down * curviness);
        curve.AddCP(p2, p2 + Vector3.up * curviness, p2 + Vector3.down * curviness);
        curve.AddCP(p3, p3 + Vector3.up * curviness, p3 + Vector3.down * curviness);
        curve.AddCP(p4, p4 + Vector3.up * curviness, p4 + Vector3.down * curviness);
        curve.AddCP(p5, p5 + Vector3.up * curviness, p5 + Vector3.down * curviness);
        curve.AddCP(p1, p1 + Vector3.up * curviness, p1 + Vector3.down * curviness);
        //================================//
        //var p1 = new Vector3(3, 0, 0);
        //var p2 = new Vector3(6, 4, 0);
        //var p3 = new Vector3(5, 2, 0);
        //StartCoroutine(curve.AddCP(p1, p3, p2 + Vector3.up));
        //StartCoroutine(curve.AddCP(p2, p1, p3 + Vector3.up));
        //StartCoroutine(curve.AddCP(p3, p2, p1 + Vector3.up));
    }


    // Use this for initialization
    void Update () {
        if (curve == null) return;
        //curve.RefreshCurve();
        curveDist += speed * Time.deltaTime;
        curveDist -= (int)curveDist;
        transform.position = curve.GetPosAtDist(curveDist);
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            Debug.Log("boom");
        }
        var w = coll.GetComponent<PlayerWeapon>();
        if(w != null)
        {
            // destroy the laser
            Destroy(w.gameObject);
            // destroy this enemy
            Instantiate(destroyPrefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if(curveObj != null) Destroy(curveObj);
    }
}
