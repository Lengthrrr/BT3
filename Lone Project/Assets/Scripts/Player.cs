using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Math;

public class Player : MonoBehaviour
{


    [SerializeField]
    private float speedMax;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerDirection;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform camObject;

    [SerializeField]
    private GameObject attackObject;

    public Transform camFollow;
    public Animator playerAnim;
    public Vector3 worldPosition;
    public Vector3 cursorPosRelative;
    private float changeFactor;
    private float timeSpeed;

    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private Transform attackSpawn;
    [SerializeField]
    private int maxClip;
    public int bulletsLeft;
    private bool canFire = true;

    [SerializeField]
    private Quaternion rotation;

    [SerializeField]
    private GameController gc;
    [SerializeField]
    private float playerSpeedCurr;

    [SerializeField]
    private float lastShotTime;

    private bool reloading;

    public Vector3 planeSize;


    void Start()
    {
        planeSize = GameObject.Find("Plane").GetComponent<Transform>().localScale;

        bulletsLeft = maxClip;

        reloading = false;

        rb = GetComponent<Rigidbody>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        // Attack Spawn is where the player's bullets spawn from
        attackSpawn = GameObject.Find("AttackSpawn").GetComponent<Transform>();
        // This is the main camera that follows the player
        camObject = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Standard get functions for private attributes
    public float GetSpeedMax() { return speedMax; }
    public float GetCurrSpeed() { return rb.velocity.magnitude; }
    public Quaternion getRotation() { return rotation; }

    void Update()
    {
        playerSpeedCurr = rb.velocity.magnitude;

        // Finds where the mouse is on gamespace (worldPosition)
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        // Finds where the mouse is relative to the player (cursorPosRelative)
        cursorPosRelative = worldPosition - this.transform.position;

        // Makes the player always face the mouse (Sometimes this doesn't work tho :/)
        // Should probs fix this eventaully ahah
        cursorPosRelative.y = 0;
        rotation = Quaternion.LookRotation(cursorPosRelative);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1000);

        // Makes the camera follow the player
        // Need to set bounds so when there is a map you aren't wasting portions of the screen
        camObject.transform.position = new Vector3 ( this.transform.position.x, 10, this.transform.position.z );

        // Makes the player move towards the camera, faster the further the mouse is
        rb.velocity = speedMax * (cursorPosRelative).normalized;
        rb.velocity = rb.velocity * cursorPosRelative.magnitude * speed;

        // Limits the speed of the player
        if (rb.velocity.magnitude > speedMax) { 
            float currSpeed = rb.velocity.magnitude;
            rb.velocity = new Vector3(rb.velocity.x * speedMax / currSpeed, rb.velocity.y, rb.velocity.z * speedMax / currSpeed);
        }

        // Lets you fire again after enough time
        if (canFire == false) {
            if (gc.getTime() - lastShotTime > fireRate) { 
                canFire = true;
            }
        }

        // Finishes your reload after enough time has elapsed
        if (reloading == true)
        {
            if (gc.getTime() - lastShotTime > reloadTime)
            {
                reloading = false;
                bulletsLeft = maxClip;
            }
        }

    }

    public void Fire()
    {
        // Occurs when the player clicks
        //Debug.Log("Attempting to fire an Attack");

        // If the player has no bullets, or can't fire (such as immediately after they fired)
        // then you just return this function and don't end up spawning a bullet
        if (bulletsLeft < 1)
        {
            return;
        }
        if (canFire == false) {
            return;
        }

        // Reduce your clip and make it so you can't fire immediately again
        canFire = false;
        bulletsLeft -= 1;
        lastShotTime = gc.getTime();

        // Summon the bullet in front of the player
        Instantiate(attackObject, attackSpawn.position, Quaternion.identity);

        // Start the timer which before the player can fire again
        FireDelay();

        // If you have no bullets, reload the weapon
        if (bulletsLeft == 0) {
            reloading = true;
            reloadFunc();
        }
    }

    private void reloadFunc() {
        // Blank function, for any sounds or animations
        reloading = true;
    }

    public void FireDelay() {
        // Blank function, for any sounds or animations
        canFire = false;
    }
    

    
}
