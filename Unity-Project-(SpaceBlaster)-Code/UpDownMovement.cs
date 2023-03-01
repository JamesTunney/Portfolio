using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float speed = 1f;

    bool goDown = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentLocation = transform.position;

        if (goDown)
        {
            float changedPosition = currentLocation.y - speed * Time.deltaTime;

            transform.position = new Vector3(currentLocation.x, changedPosition, currentLocation.z);
            currentLocation = transform.position;
            if (currentLocation.y <= -4.3f)
            {
                goDown = false; 
            }
        }
        else
        {
            float changedPosition = currentLocation.y + speed * Time.deltaTime;

            transform.position = new Vector3(currentLocation.x, changedPosition, currentLocation.z);
            currentLocation = transform.position;
            if (currentLocation.y >= 4.3f)
            {
                goDown = true;
            }
        }
        
    }
}
