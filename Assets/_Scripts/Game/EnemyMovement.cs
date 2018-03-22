using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float speed = 5f;
    public float mag;

    [Header("Properties")]
    public float weaponCooldownTime = .8f;

    [Header("References")]
    public GameObject EnemyShip1Laser;
    Transform weaponSpawn;
    bool cooledDown = true;

    // Use this for initialization
    void Start()
    {
        weaponSpawn = transform.Find("WeaponSpawn");
    }
    // Update is called once per frame
    void Update()
    {
        var p = transform.localPosition;
        transform.Translate(new Vector3(Mathf.Sin(Time.time) * mag, speed * Time.deltaTime, 0));

        // WEAPONS
        if (cooledDown)
        {
            var b = Instantiate(EnemyShip1Laser) as GameObject;
            b.transform.position = weaponSpawn.position;
            StartCoroutine(WaitForCooldown());
        }
    }
    IEnumerator WaitForCooldown()
    {
        cooledDown = false;
        yield return new WaitForSeconds(weaponCooldownTime);
        cooledDown = true;
    }
}
