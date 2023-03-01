using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public abstract class Agent : MonoBehaviour
{
    public PhysicsObject physicsObject;

    public float maxSpeed = 5f;
    public float maxForce = 5f;

    private Vector3 totalForce = Vector3.zero;

    private float wanderAngle = 0f;

    public float maxWanderAngle = 45f;

    public float maxWanderChangePerSecond = 10f;

    public float personalSpace = 1f;

    public float visionRange = 2f;

    private void Awake()
    {
        if(physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateSteeringForces();

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);

        totalForce = Vector3.zero;
    }

    protected abstract void CalculateSteeringForces();

    protected void Seek(Vector3 targetPos, float weight = 1f)
    {
        Vector3 desiredVelocity = targetPos - physicsObject.Position;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        totalForce += seekingForce * weight;
    }

    protected void Flee(Vector3 targetPos, float weight = 1f)
    {
        Vector3 desiredVelocity = physicsObject.Position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        totalForce += fleeingForce * weight;
    }

    protected void Wander(float weight = 1f)
    {
        float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;
        wanderAngle += Random.Range(-maxWanderChange, maxWanderChange);

        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * physicsObject.Direction.normalized + physicsObject.Position;

        Seek(wanderTarget, weight);
    }

    protected void StayInBounds(float weight = 1f)
    {
        Vector3 futurePosition = GetFuturePosition();

        if (futurePosition.x > AgentManager.Instance.maxPosition.x || futurePosition.x < AgentManager.Instance.minPosition.x ||
            futurePosition.y > AgentManager.Instance.maxPosition.y || futurePosition.y < AgentManager.Instance.minPosition.y)
        {
            Seek(Vector3.zero, weight);
        }
    }

    public Vector3 GetFuturePosition(float timeToLookAhead = 1f)
    {
        return physicsObject.Position + physicsObject.Velocity * timeToLookAhead;
    }

    protected void Separate<T>(List<T> agents) where T : Agent
    {
        float sqrPersonalSpace = Mathf.Pow(personalSpace, 2);

        foreach (T other in agents)
        {
            float sqrDist = Vector3.SqrMagnitude(other.physicsObject.Position - physicsObject.Position);

            if(sqrDist < float.Epsilon)
            {
                continue;
            }

            if(sqrDist < sqrPersonalSpace)
            {
                float weight = sqrPersonalSpace / (sqrDist + 0.1f);
                Flee(other.physicsObject.Position, weight);
            }
        }
    }

    protected void AvoidObstacle(Obstical obstical)
    {
        Vector3 toObstacle = obstical.Position - physicsObject.Position;

        float fwdToObstical = Vector3.Dot(physicsObject.Direction, toObstacle);
        if(fwdToObstical < 0)
        {
            return;
        }

        float rightToObsticalDot = Vector3.Dot(physicsObject.Right, toObstacle);

        if(Mathf.Abs(rightToObsticalDot) > physicsObject.radius + obstical.radius)
        {
            return;
        }

        if(fwdToObstical > visionRange)
        {
            return;
        }

        Vector3 desiredVelocity;

        if(rightToObsticalDot > 0)
        {
            desiredVelocity = physicsObject.Right * -maxSpeed;
        }
        else
        {
            desiredVelocity = physicsObject.Right * maxSpeed;
        }

        float weight = visionRange / (fwdToObstical + 0.1f);

        Vector3 steeringForce = (desiredVelocity - physicsObject.Velocity) * weight;

        totalForce += steeringForce;

    }

    protected void AvoidAllObsticals()
    {
        foreach(Obstical obstical in ObsticalManager.Instance.Obsticals)
        {
            AvoidObstacle(obstical);
        }
    }
}
