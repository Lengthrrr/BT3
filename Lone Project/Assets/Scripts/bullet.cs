using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    // Which team the bullet is on, for homing and friendly fire checks
    [SerializeField]
    private string team;

    // How fast the bullet should move
    [SerializeField]
    private float speed;

    // WTF is a quaternion
    [SerializeField]
    private Quaternion facingDirection;

    private Rigidbody rb;

    // Used to ensure the bullet is travelling at the correct speed
    [SerializeField]
    private GameController gameManager;
    [SerializeField]
    private float timeRate;

    // Types include: homing, time-ignoring, explosive etc.
    [SerializeField]
    private string bulletType;

    [SerializeField]
    private float bulletLifeTime;
    [SerializeField]
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {

        // When the bullet is spawned, it faces the same direction as the player
        facingDirection = GameObject.Find("Player").GetComponent<Player>().getRotation();
        transform.rotation = Quaternion.Slerp(GameObject.Find("Player").GetComponent<Transform>().rotation, facingDirection, 1);


        // Sets rb to the rigidbody of the gameobject
        rb = this.GetComponent<Rigidbody>();

        // Finds the game controller (used to find what speed the game is moving at)
        gameManager = GameObject.Find("GameController").GetComponent<GameController>();

        // Makes the bullet eventually time out even if it doesn't hit anything
        spawnTime = gameManager.getTime();
    }

    // Update is called once per frame
    void Update()
    {
        // If the bullet ignores time, then it is always moving at full speed otherwise it follows time speed
        if (bulletType == "Time-Ignoring")
        {
            timeRate = 1;
        }
        else
        {
            timeRate = gameManager.GetTimeRate();
        }

        //Sets velocity of the bullet to move forward
        rb.velocity = transform.forward * speed * timeRate;

        if (gameManager.getTime() - spawnTime > bulletLifeTime) {
            Debug.Log("Bullet Timed Out");
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        // If the bullet hits anything, destroy it
        //Debug.Log("Bullet Destroyed");
        Destroy(this.gameObject);
    }
}
