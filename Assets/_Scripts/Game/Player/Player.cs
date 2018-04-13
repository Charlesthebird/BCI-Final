using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : GameElement
{
    [Header("Properties")]
    public float speed = 8.0f;
    public float turboSpeedup = 2.0f;
    public float weaponCooldownTime = 0.1f;

    // control the possible movement area for the player ship
    float topMoveExtent = 4.4f;
    float bottomMoveExtent = -4.4f;
    float leftMoveExtent = -8.2f;
    float rightMoveExtent = 8.2f;

    [Header("References")]
    public GameObject damagePrefab;
    public GameObject destroyPrefab;
    public GameObject laser1Obj;
    public AudioSource[] audioSources;

    Transform weaponSpawn;
    bool weaponCooledDown = true;

    int score = 0;
    int maxShields = 4;
    int numShields = 4;
    bool shieldCooledDown = true;
    public float bciCooldownThreshold = .4f;
    public float shieldCooldownTime = 3.0f;



    // Use this for initialization
    void Start()
    {
        weaponSpawn = transform.Find("WeaponSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        // time wasn't good to change
        //float baseSpeed = .5f;
        //var recentMax = 0.0f;
        //if(sceneController.bci.cachedEEGData.Count > 0)
        //    recentMax = sceneController.bci.cachedEEGData.Max(d => d.GetBeta());
        //Time.timeScale = Mathf.Min(2.0f, baseSpeed + (sceneController.bci.curBetaAverage / recentMax));

        //Debug.Log(sceneController.bci.curBetaValue);
        //var h = Input.GetAxis("Horizontal");
        //var v = Input.GetAxis("Vertical");

        // MOVEMENT
        // button up/down gives snappier controls on laptop
        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > .2f) movement.y = speed;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -.2f) movement.y = -speed;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -.2f) movement.x = -speed;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > .2f) movement.x = speed;
        // handle turbo
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1)) movement = movement * turboSpeedup;

        // make the movement framerate independent
        movement *= Time.deltaTime;

        // check movement extents to stay in bounds
        if ((transform.position + movement).x > rightMoveExtent ||
            (transform.position + movement).y > topMoveExtent ||
            (transform.position + movement).x < leftMoveExtent ||
            (transform.position + movement).y < bottomMoveExtent)
        {
            movement = Vector3.zero;
        }

        // apply the movement
        transform.Translate(movement);

        // WEAPONS
        if (weaponCooledDown && (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
        {
            var b = Instantiate(laser1Obj) as GameObject;
            b.transform.position = weaponSpawn.position;
            audioSources.Where(s => s.name == "Laser1Fire").Single().Play();
            StartCoroutine(WaitForWeaponCooldown());
        }

        // SHIELDS
        if(shieldCooledDown && sceneController.bci.curBetaAverage < bciCooldownThreshold)
        {
            if (numShields < maxShields) numShields++;
            RefreshShields();
            StartCoroutine(WaitForShieldCooldown());
        }
    }
    IEnumerator WaitForShieldCooldown()
    {
        shieldCooledDown = false;
        yield return new WaitForSeconds(shieldCooldownTime);
        shieldCooledDown = true;
    }
    IEnumerator WaitForWeaponCooldown()
    {
        weaponCooledDown = false;
        yield return new WaitForSeconds(weaponCooldownTime);
        weaponCooledDown = true;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            numShields--;
            if(numShields < 0)
            {
                Debug.Log("PLAYER DESTROYED!");
                Instantiate(destroyPrefab, transform.position, Quaternion.identity, transform.parent);
                sceneController.gameOverContainer.SetActive(true);
                Destroy(this.gameObject);
                return;
            }
            Instantiate(damagePrefab, transform.position, Quaternion.identity, transform.parent);
            RefreshShields();
        }
    }
    void RefreshShields()
    {
        var rt = GameObject.Find("ShieldBar").GetComponent<RectTransform>();
        float left = 1f;
        float right = 420 - (numShields * 120.5f);
        float posY = 25f;
        float height = 50f;

        Vector2 temp = new Vector2(left, posY - (height / 2f));
        rt.offsetMin = temp;
        temp = new Vector3(-right, temp.y + height);
        rt.offsetMax = temp;
    }

    public void AddToScore(int points)
    {
        this.score += points;
        sceneController.scoreText.text = "Score: " + this.score;
    }
}
