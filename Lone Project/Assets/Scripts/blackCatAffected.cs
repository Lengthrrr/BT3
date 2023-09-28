using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackCatAffected : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameController gc;

    private Rigidbody rb;

    public bool isRewinding = false;

    public Player asd;

    public List<PointInTime> pointsInTime;

    void Start()
    {

        pointsInTime = new List<PointInTime>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        rb = this.GetComponent<Rigidbody>();

        if (this.name == "Player") {
            asd = this.GetComponent<Player>();
        }


        //gc.blackCatThis(this.gameObject);
    }

    //public void recordStats() {
        //Debug.Log("VAss");
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (gc.getRewind())
        {
            StartRewind();
        }
        else if (gc.getRewind() == false) {
            StopRewind();
        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        } else {
            Record(); 
        }
    }

    private void StartRewind() 
    {
        isRewinding = true;
        rb.isKinematic = true;
        asd.enabled = false;
    }

    private void StopRewind() 
    {
        isRewinding = false;
        rb.isKinematic = false;
        asd.enabled = true;
    }

    private void Record()
    {

        if (pointsInTime.Count > Mathf.Round(3f / Time.fixedDeltaTime)) {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    private void Rewind() 
    {

        if (pointsInTime.Count == 0) {

            Destroy(this.gameObject);
        
        }

        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else {
            StopRewind();
        }
    }
}
