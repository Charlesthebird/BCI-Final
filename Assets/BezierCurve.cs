using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {
    public class Segment
    {
        public ControlPoint startCP;
        public ControlPoint endCP;
        // positions goes from 0 -> (interpolationCount - 1)
        // (positions[0] == startPoint.pos) and (positions[n-1] == endPoint.pos)
        public Vector2[] positions;
        public float[] lengths;
    }
    public List<Segment> segments;
    int interpolationCount = 100;


    private void OnDrawGizmos()
    {
        if (segments == null) return;
        for(int i=0; i<segments.Count; i++)
        {
            Gizmos.color = new Color(i / (float)segments.Count , .5f, i);
            Gizmos.DrawSphere(segments[i].startCP.pos, .2f);
            for(int j=0; j<interpolationCount - 1; j++)
            {
                Gizmos.DrawLine(segments[i].positions[j],
                    segments[i].positions[j + 1]);
            }
            //(interpolationCount - 1) is the edge case where we find the distance to the first position in the next segment
            var nextSegIndex = (i + 1) % segments.Count;
            var nextPos = segments[nextSegIndex].positions[0];
            var curPos = segments[i].positions[interpolationCount - 1];
            Gizmos.DrawLine(curPos, nextPos);
        }
    }

    public IEnumerator AddCP(Vector3 localPos, Vector3 leftTangent, Vector3 rightTangent)
    {
        // create the gameObject
        var cpObj = ControlPoint.CreateCPObject(localPos, leftTangent, rightTangent, this.transform);
        yield return new WaitForEndOfFrame();
        // initialize the segment references
        segments.Add(new Segment());
        var numSegments = segments.Count;
        var p = segments[numSegments - 1];
        p.startCP = cpObj.GetComponent<ControlPoint>();
        p.endCP = segments[0].startCP;
        p.positions = new Vector2[interpolationCount];
        p.lengths = new float[interpolationCount];
        // if this was not the first segment, then update the last segment
        var prevSegIndex = ((numSegments - 2) + numSegments) % numSegments;
        segments[prevSegIndex].endCP = p.startCP;
        // now update all the new positions and lengths
        UpdateSegment(numSegments - 1);
    }

    void UpdateSegment(int segIndex)
    {
        var seg = segments[segIndex];
        // loop through all interpolationCount positions
        // lerp between the two points for now (will be replaced with linear function and then catmull rom
        for (int i = 0; i < interpolationCount; i++) {
            // get the variables for the 4-point bezier function
            var t = i / (float)interpolationCount;
            var p1 = seg.startCP.pos;
            var p2 = seg.startCP.rightTangent;
            var p3 = seg.endCP.leftTangent;
            var p4 = seg.endCP.pos;
            // use the bezier function to find the current position at location t = i/interpolationCount
            //P = (1−t)3P1 + 3(1−t)2tP2 + 3(1−t)t2P3 + t3P4
            seg.positions[i] = (Mathf.Pow((1 - t), 3) * p1) +
                (3 * Mathf.Pow((1 - t), 2) * t * p2) +
                (3 * (1 - t) * Mathf.Pow(t, 2) * p3) +
                (Mathf.Pow(t, 4) * p4);
            //seg.positions[i] = Vector2.Lerp(seg.startCP.pos, seg.endCP.pos, i / (float)interpolationCount);
        }
        // loop through all interpolationCount lengths 
        Vector3 nextPos, curPos;
        for (int i = 0; i < interpolationCount - 1; i++) {
            curPos = seg.positions[i];
            nextPos = seg.positions[i + 1];
            seg.lengths[i] = Vector2.Distance(curPos, nextPos);
        }
        //(interpolationCount - 1) is the edge case where we find the distance to the first position in the next segment
        var nextSegIndex = (segIndex + 1) % segments.Count;
        nextPos = segments[nextSegIndex].positions[0];
        curPos = seg.positions[interpolationCount - 1];
        seg.lengths[interpolationCount - 1] = Vector2.Distance(curPos, nextPos);
    }

    void Awake ()
    {
        segments = new List<Segment>();
        var p1 = new Vector3(3, 0, 0);
        var p2 = new Vector3(6, 4, 0);
        var p3 = new Vector3(5, 2, 0);
        StartCoroutine(AddCP(p1, p3, p2 + Vector3.up));
        StartCoroutine(AddCP(p2, p1, p3 + Vector3.up));
        StartCoroutine(AddCP(p3, p2, p1 + Vector3.up));
    }

    // Update is called once per frame
    void Update () {
        // could be optimized if not needed every frame
        for(int i=0; i<segments.Count; i++)
        {
            UpdateSegment(i);
        }
    }
}
