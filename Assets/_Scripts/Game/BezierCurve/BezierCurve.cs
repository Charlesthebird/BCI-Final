using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float totalCurveLength = 0;
    public List<Segment> segments;
    int interpolationCount = 100;


    // dist is in range 0 -> 1
    public Vector2 GetPosAtDist(float dist)
    {
        if (segments == null || segments.Count == 0) return Vector2.one * 100;
        for (int i=0; i< segments.Count; i++)
        {
            // if this is the right segment, return the position that it finds
            if (dist <= segments[i].lengths[interpolationCount - 1])
            {
                for(int j=0; j<interpolationCount; j++)
                {
                    if(dist <= segments[i].lengths[j])
                    {
                        var prevLen = GetPrevLen(i, j);
                        var curLen = segments[i].lengths[j];
                        if (i == 0 && j == 0) prevLen = 0;
                        var remainder = (curLen - dist) / (curLen - prevLen);

                        var prevPos = GetPrevPos(i, j);
                        var curPos = segments[i].positions[j];
                        return Vector3.Lerp(curPos, prevPos, remainder);
                    }
                }
            }
        }
        return Vector2.zero;
    }

    void Awake()
    {
        //var p1 = new Vector3(3, 0, 0);
        //var p2 = new Vector3(6, 4, 0);
        //var p3 = new Vector3(5, 2, 0);
        //AddCP(p1, p3, p2 + Vector3.up);
        //AddCP(p2, p1, p3 + Vector3.up);
        //AddCP(p3, p2, p1 + Vector3.up);
    }

    public void RefreshCurve()
    {
        if(segments == null) segments = new List<Segment>();
        // Updates all lengths and positions
        UpdateSegmentPositions();
        UpdateSegmentLengths();
        // Normalizes all lengths and positions (to the range 0 -> 1)
        NormalizeSegments();
    }

    void NormalizeSegments()
    {
        if (segments == null || segments.Count == 0) return;
        // get the total length of the curve as a ratio from 0 to 1
        // the total curve length should be the last lengths value
        totalCurveLength = segments[segments.Count - 1].lengths[interpolationCount - 1];
        for (var i = 0; i < segments.Count; i++)
        {
            for (var j = 0; j < interpolationCount; j++)
            {
                segments[i].lengths[j] = segments[i].lengths[j] / totalCurveLength;
            }
        }
    }


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

    public void AddCP(Vector3 localPos, Vector3 leftTangent, Vector3 rightTangent)
    {
        StartCoroutine(AddCPCoroutine(localPos, leftTangent, rightTangent));
    }
    IEnumerator AddCPCoroutine(Vector3 localPos, Vector3 leftTangent, Vector3 rightTangent)
    {
        if (segments == null) segments = new List<Segment>();
        // create the gameObject
        var cpObj = ControlPoint.CreateCPObject(localPos, leftTangent, rightTangent, this);
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
        RefreshCurve();
    }

    void UpdateSegmentPositions()
    {
        for (int j = 0; j < segments.Count; j++)
        {
            var seg = segments[j];
            // loop through all interpolationCount positions
            for (int i = 0; i < interpolationCount; i++)
            {
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
        }
    }

    void UpdateSegmentLengths()
    {
        for (int j = 0; j < segments.Count; j++)
        {
            var seg = segments[j];
            // loop through all interpolationCount lengths 
            Vector3 prevPos, curPos;
            for (int i = 0; i < interpolationCount; i++)
            {
                curPos = seg.positions[i];
                prevPos = GetPrevPos(j, i);
                seg.lengths[i] = GetPrevLen(j, i) + Vector2.Distance(prevPos, curPos);
            }
        }
    }

    Vector3 GetPrevPos(int segIndex, int i)
    {
        var prevI = i - 1;
        if (prevI >= 0) return segments[segIndex].positions[prevI];
        else
        {
            var prevSegI = segIndex - 1;
            if (prevSegI >= 0) return segments[prevSegI].positions[interpolationCount - 1];
            else
            {
                return segments[segments.Count - 1].positions[interpolationCount - 1];
            }
        }
    }
    float GetPrevLen(int segIndex, int i)
    {
        var prevI = i - 1;
        if (prevI >= 0) return segments[segIndex].lengths[prevI];
        else
        {
            var prevSegI = segIndex - 1;
            if (prevSegI >= 0) return segments[prevSegI].lengths[interpolationCount - 1];
            else
            {
                return segments[segments.Count - 1].lengths[interpolationCount - 1];
            }
        }
    }
}
