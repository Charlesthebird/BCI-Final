using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    public GameObject[] objectTypes;
    public float[] lowFreq;
    public float[] highFreq;

    Vector2 extents;

    // Use this for initialization
    void Start () {
        extents = GetComponent<BoxCollider2D>().bounds.extents;
        for(int i=0; i<objectTypes.Length; i++)
        {
            StartCoroutine(StartSpawning(i));
        }
    }
	
	IEnumerator StartSpawning(int enemyType)
    {
        while (true)
        {
            var g = Instantiate(objectTypes[enemyType]) as GameObject;
            g.transform.localScale = Vector3.one;
            g.transform.parent = this.transform;
            g.transform.position = transform.position + new Vector3(Random.Range(-extents.x, extents.x), 
                Random.Range(-extents.y,extents.y),0);
            yield return new WaitForSeconds(Random.Range(lowFreq[enemyType], highFreq[enemyType]));
        }
    }
}
