using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentLocation = transform.position;

        float changedPosition = currentLocation.x - speed * Time.deltaTime;

        transform.position = new Vector3(changedPosition, currentLocation.y, currentLocation.z);
    }


}
