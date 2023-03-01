using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour
{

    //declare variables
    public float speed = 1f;

    public Vector2 direction = Vector2.right;

    public Vector2 velocity = Vector2.zero;

    private Vector2 movementInput;

    public Bullet playerShot;

    Vector3 currentLocation;

    public List<Bullet> bulletList = new List<Bullet>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //declare variables
        direction = movementInput;

        //update velocity
        velocity = direction * speed * Time.deltaTime;

        //get current location
        currentLocation = transform.position;

        //keep rotation proper
        if (direction != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: (Vector3)direction);
        }

        //manage wraping around if position excedds a specific value. 
        if (currentLocation.y >= 5)
        {
            transform.position = new Vector3(currentLocation.x, y: -4.9f, currentLocation.z);
        }
        else if (currentLocation.y <= -5)
        {
            transform.position = new Vector3(currentLocation.x, y: 4.9f, currentLocation.z);
        }
        else if (currentLocation.x <= -10)
        {
            transform.position = new Vector3(x: -9.9f, currentLocation.y, currentLocation.z);
        }
        else if (currentLocation.x >= 10)
        {
            transform.position = new Vector3(x: 9.9f, currentLocation.y, currentLocation.z);
        }
        else
        {
            transform.position += (Vector3)velocity;
        }

        /*
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Fires");
            Bullet currentObject;
            currentObject = (Bullet)Instantiate(playerShot, position: new Vector3(currentLocation.x, currentLocation.y, currentLocation.z), Quaternion.Euler(0, 0, 0));
            currentObject.direction = direction;
            bulletList.Add(currentObject);
        }
        */
    }
    //manages player input. 
    public void OnMove(InputAction.CallbackContext moveContext)
    {
        movementInput = moveContext.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext fireContext)
    {
        
    }
}
