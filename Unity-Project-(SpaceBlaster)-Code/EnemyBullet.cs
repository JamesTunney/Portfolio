using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 9f;

    public Vector2 direction = Vector2.right;

    public Vector2 velocity = Vector2.zero;

    private Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(-1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //declare variables
        if (direction == new Vector2(0f, 0f))
        {
            direction = new Vector2(1f, 0f);
        }
        //update velocity
        velocity = direction * speed * Time.deltaTime;

        //get current location
        Vector3 currentLocation = transform.position;

        //keep rotation proper
        if (direction != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: (Vector3)direction);
        }

        transform.position += (Vector3)velocity;
    }
}
