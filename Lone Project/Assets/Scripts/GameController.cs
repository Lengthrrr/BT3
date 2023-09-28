using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    [SerializeField]
    private float timeRate;

    [SerializeField]
    private float baseTime; // Must be less than one


    [SerializeField]
    private Player playerScript;
    [SerializeField]
    private blackCatAffected playerIsBlack;



    // The amount of time that has elapsed in game for the player
    [SerializeField]
    private float gameTime;
    [SerializeField]
    private double gameTimeDouble;

    [SerializeField]
    private List<GameObject> blackObjects;
    [SerializeField]
    private bool newSecond = false;
    [SerializeField]
    private float oldTime;

    public bool rewindTime = false;


    // Standard get functions
    public float GetTimeRate() { return timeRate; }
    // How many seconds have elapsed since starting the scene
    public float getTime() { return gameTime; }


    // Start is called before the first frame update
    void Start()
    {
        // Sets the current in game time to zero
        gameTime = 0;

        // Finds the player
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        playerIsBlack = GameObject.Find("Player").GetComponent<blackCatAffected>();
        //Application.targetFrameRate = (int)(60 / baseTime);
        //timeRate = playerScript.speed;
    }

    // Update is called once per frame
    void Update()
    {
        //timeRate = playerObject.playerSpeed / maxPlayerSpeed;
        timeRate = CalculateTimeSpeed();


        oldTime = gameTime;

        // Moves the in game time forward based on the player's speed
        gameTime += Time.deltaTime * timeRate;
        gameTimeDouble = System.Math.Round(gameTime, 2);


        if (Input.GetKeyDown(KeyCode.Return))
        {
            rewindTime = true;
        }
        //else if (Input.GetKeyUp(KeyCode.Return)) {

        if (playerIsBlack.pointsInTime.Count == 0)
        {
            rewindTime = false;
        }
        //}
        //if (newSecond) {
            //blackRecord();
            //newSecond = false;
        //} else if (Mathf.Floor(oldTime) != Mathf.Floor(gameTime)) {
            //newSecond = true;
            //Debug.Log("One Second has passed");
        //}
    }

    private float CalculateTimeSpeed() 
    {
        // Calculates as a decimal how fast the game is running (1 is full speed, 0 is paused)
        float playerSpeedDecimal = playerScript.GetCurrSpeed() / playerScript.GetSpeedMax();
        float result = playerSpeedDecimal * (1 - baseTime) + baseTime;
        return result;
    }

    public void blackCatThis(GameObject blackObject) {
        blackObjects.Add(blackObject);
    }

    public bool getRewind() { return rewindTime; }
}
