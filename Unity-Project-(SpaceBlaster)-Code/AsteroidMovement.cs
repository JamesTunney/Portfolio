using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentLocation = transform.position;
        float changedPosition = currentLocation.y - speed * Time.deltaTime;
        transform.position = new Vector3(currentLocation.x, changedPosition, currentLocation.z);
    }
}
