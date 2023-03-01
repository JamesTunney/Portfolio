using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 direction;

    public float mass = 1f;

    public float gravityStrength = 1f;

    public bool useGravity = false;

    public float frictionCoeff = 0.2f;

    public float radius = 1f;

    public bool useFriction;

    public bool bounceOffWalls = false;

    public Vector3 Velocity
    {
        get { return velocity; }
    }
    public Vector3 Direction
    {
        get { return direction; }
    }
    public Vector3 Position
    {
        get { return transform.position; }
    }

    public Vector3 Right => transform.right;


    // Start is called before the first frame update
    void Start()
    {
        direction = Random.insideUnitCircle.normalized; 
    }

    // Update is called once per frame
    void Update()
    {
        if (useGravity)
        {
            ApplyGravity(Vector3.down * gravityStrength);
        }

        if (useFriction)
        {
            ApplyFriction(frictionCoeff);
        }
        

        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        if(velocity.sqrMagnitude > Mathf.Epsilon)
        {
            direction = velocity.normalized;
        }

        acceleration = Vector3.zero;

        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);

        if (bounceOffWalls)
        {
            BounceOffWalls();
        }
        
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    private void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;

        ApplyForce(friction);
    }

    private void ApplyGravity(Vector3 gravityForce)
    {
        acceleration += gravityForce;
    }

    private void BounceOffWalls()
    {
        if(transform.position.x > AgentManager.Instance.maxPosition.x && velocity.x > 0)
        {
            velocity.x *= -1f;
        }
        if (transform.position.x < AgentManager.Instance.minPosition.x && velocity.x < 0)
        {
            velocity.x *= -1f;
        }
        if (transform.position.y > AgentManager.Instance.maxPosition.y && velocity.y > 0)
        {
            velocity.y *= -1f;
        }
        if (transform.position.y < AgentManager.Instance.minPosition.y && velocity.y < 0)
        {
            velocity.y *= -1f;
        }
    }

}
