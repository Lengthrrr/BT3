using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; //important
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform mapCentre, neo, firePoint;
    public bool isChase, targetSighted;
    public float range, elapsedTime, trackingAttackRange, chaseRange, rotationSpeed = 1f, fireTime; // order (slowest to fastest): explosive, tracking, bouncing
    private bool canHear, inShootFOV, canFire = true;
    public GameObject bullet, plane;
    private Vector3 planeSize;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        planeSize = plane.GetComponent<MeshRenderer>().bounds.size;
    }


    void Update()
    {
        // The agent is allowed to fire after every fireTime seconds
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= fireTime && !canFire) {
            canFire = true;
            elapsedTime = 0;
        }

        agent.isStopped = false;

        // If tracking agent and in attack range, attack neo
        if (gameObject.tag == "Tracking" && Vector2.Distance(transform.position, neo.position) <= trackingAttackRange)
        {
            agent.isStopped = true;

            if (canFire)
            {
                //Debug.Log("BANG " + transform.position);
                Instantiate(bullet, firePoint.position, transform.rotation * Quaternion.Euler(0, 90, 90));
                canFire = false;
                elapsedTime = 0;
            }

        }
        // If explosive/bouncing agent and neo is in sight, chase and attack neo
        else if (gameObject.tag == "Not Tracking" && canSeeTarget() || isChase)
        {
            isChase = true;
            agent.SetDestination(neo.position);
            if (canSeeTarget() && canFire && !canHear && inShootFOV)
            {
                Instantiate(bullet, firePoint.position, transform.rotation * Quaternion.Euler(0, 90, 90));
                canFire = false;
                elapsedTime = 0;
                // Shoot ever 2 or 4 seconds, depending on the bullet type
            }

            if (Vector2.Distance(transform.position, neo.position) <= agent.stoppingDistance + 5)
            {
                // Rotate towards Neo to keep him in sight
                // Object rotation code based off code from https://www.youtube.com/watch?v=2XEiHf1N_EY

                // Calculate the direction to the target.
                Vector3 directionToTarget = neo.position - transform.position;
                //Debug.DrawRay(directionToTarget, Vector2.up, UnityEngine.Color.blue, 1.0f); //so you can see with gizmos
                // Create a rotation that looks at the target.
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                // Smoothly interpolate the current rotation towards the target rotation.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, neo.position) > chaseRange)
            {
                isChase = false;
            }
        }
        else if (!isChase)
        {
            //Debug.Log("SCATTER");

            // Scatter code from: https://github.com/JonDevTutorial/RandomNavMeshMovement/blob/main/RandomMovement.cs

            // If the agent is close to its current destination or has reached it, it needs a new destination.
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(mapCentre.position, range, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.forward, UnityEngine.Color.blue, 1.0f); //so you can see with gizmos
                    agent.SetDestination(point);
                }
            }
        }

    }

    bool canSeeTarget()
    {
        // From https://discussions.unity.com/t/field-of-view-using-raycasting/3096/3

        RaycastHit hit;
        Vector3 rayDirection = neo.position - transform.position;
        float seeFieldOfViewDegrees = 135f;
        float shootFieldOfViewDegrees = 30f;

        if ((Vector3.Angle(rayDirection, transform.forward)) < seeFieldOfViewDegrees * 0.5f)
        { // Detect if player is within the see field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, agent.stoppingDistance + 6))
            {
                if (hit.transform == neo)
                {
                    //Debug.Log("I see you");

                    if ((Vector3.Angle(rayDirection, transform.forward)) < shootFieldOfViewDegrees * 0.5f)
                    { // Detect if player is within the shoot field of view
                        inShootFOV = true;
                    } else
                    {
                        inShootFOV = false;
                    }
                    canHear = false;
                    return true;
                }
            }
        } 
        else if (Physics.Raycast(transform.position, rayDirection, out hit, agent.stoppingDistance))
        {
            // If the player is very close behind the player and in view the enemy will detect the player
            if (hit.transform == neo)
            {
                //Debug.Log("I hear you");
                canHear = true;
                inShootFOV = false;
                return true;
            }
        }

        //Debug.Log("Where are you?");

        return false;

    }

    bool RandomPoint(Vector3 centre, float range, out Vector3 result)
    {
        // From documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html

        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = centre + new Vector3((Random.value - 0.5f) * planeSize.x, (Random.value - 0.5f) * 5, (Random.value - 0.5f) * planeSize.z);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;

    }


}