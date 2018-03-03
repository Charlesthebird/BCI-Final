using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCreatorAndPlayerWeaponDestroyer : MonoBehaviour {
    public GameObject grassRow;

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "BackgroundGrass")
        {
            var g = Instantiate(grassRow, transform.position, transform.rotation, transform) as GameObject;
            g.transform.position = coll.transform.position + new Vector3(0,2,0);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "PlayerWeapon")
            Destroy(coll.gameObject);
    }

}
