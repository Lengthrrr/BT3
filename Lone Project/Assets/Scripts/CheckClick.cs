using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckClick : MonoBehaviour
{

    // This entire script just runs the "fire" function whenever the player left clicks
    // Later might make it also use your ability when you right click

    [SerializeField]
    private Player playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();   
    }

    void OnMouseDown() {
        playerScript.Fire();
    }
}
