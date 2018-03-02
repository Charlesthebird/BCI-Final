using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCreator : MonoBehaviour {
    public GameObject grassRow;

    void OnTriggerExit2D(Collider2D coll)
    {
        Instantiate(grassRow, transform.position, transform.rotation, transform);
    }

}
