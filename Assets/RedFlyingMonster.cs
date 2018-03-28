using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlyingMonster : GameElement {
    public GameObject destroyPrefab;


	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
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
}
