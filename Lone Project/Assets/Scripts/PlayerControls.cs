using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

  [SerializeField]
private Vector2 cursorPos;
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

public Transform camFollow;
public Animator playerAnim;
public Vector3 worldPosition;
public Vector3 cursorPosRelative;
private float changeFactor;

// Start is called before the first frame update

    // Update is called once per frame
void Update()
{
//camFollow.position = (this.position.x, this.position.y + 10, this.position.z);
Vector3 mousePos = Input.mousePosition;
mousePos.z = Camera.main.nearClipPlane;
worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
// worldPosition is where the cursor is in the world
cursorPosRelative = worldPosition - this.transform.position;

rb.velocity = new Vector3(cursorPosRelative.x * speed, cursorPosRelative.y * speed, rb.velocity.z);

playerSpeed = Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2);
// Call x 
//playerDirection = ;

if (playerSpeed > speedMax) {
changeFactor = speedMax / playerSpeed;
rb.velocity *= changeFactor;
// gives number < 1 which is how much the players speed needs to be multiplied by

}
}
}
