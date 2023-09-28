using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkBlocks : MonoBehaviour
{

    public GameObject cubeBase;

    private void Start()
    {
        if (this.transform.localScale.x == 1f) 
        {
            transform.position = new Vector3(this.transform.position.x + 0.25f, this.transform.position.y, this.transform.position.z);
            Transform temp = this.transform;
            temp.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z);
            Instantiate(cubeBase, temp);
        }

        if (this.transform.localScale.x == 1.5f)
        {
            transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y, this.transform.position.z);
            Transform temp = this.transform;
            temp.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z);
            Instantiate(cubeBase, temp);
            temp.position = new Vector3(this.transform.position.x - 1.0f, this.transform.position.y, this.transform.position.z);
            Instantiate(cubeBase, temp);
        }

        if (this.transform.localScale.z == 1f)
        {
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.25f);
            Transform temp = this.transform;
            temp.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.5f);
            Instantiate(cubeBase, temp);
        }

        if (this.transform.localScale.z == 1.5f)
        {
            transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.5f);
            Transform temp = this.transform;
            temp.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.5f);
            Instantiate(cubeBase, temp);
            temp.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 1.0f);
            Instantiate(cubeBase, temp);
        }

        this.transform.localScale = new Vector3(0.5f, 1.0f, 0.5f);
    }
}
