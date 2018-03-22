using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {
    public float delay = 1.0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterCoroutine());
	}
    IEnumerator DestroyAfterCoroutine()
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
