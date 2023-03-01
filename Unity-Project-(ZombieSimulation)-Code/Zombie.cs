using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Agent
{
    public enum ZombieState
    {
        Hunting,
        Wandering,
        Counting,
        Dead
    }

    private ZombieState currentState = ZombieState.Counting;

    public ZombieState CurrentState => currentState;

    public float countDownTimer = 0f;

    public float visionDistance = 4f;

    public SpriteRenderer spriteRenderer;

    public Sprite itSprite;
    public Sprite countingSprite;
    public Sprite notItSprite;

    protected override void CalculateSteeringForces()
    {
        if (AgentManager.Instance.humanList.Count > 0)
        {
            switch (currentState)
            {
                case ZombieState.Hunting:
                    {

                        Human targetPlayer = AgentManager.Instance.GetClosestHuman(this);
                        if (IsTouching(targetPlayer))
                        {
                            StateTransition(ZombieState.Counting);
                            targetPlayer.Kill();
                        }
                        else
                        {
                            float distToItPlayer = Vector3.SqrMagnitude(targetPlayer.physicsObject.Position - physicsObject.Position);
                            if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                            {
                                Seek(targetPlayer.physicsObject.Position);
                            }
                            else
                            {
                                StateTransition(ZombieState.Wandering);
                            }
                            
                        }

                        break;
                    }
                case ZombieState.Counting:
                    {
                        countDownTimer -= Time.deltaTime;
                        if (countDownTimer <= 0f)
                        {
                            StateTransition(ZombieState.Hunting);
                        }
                        break;
                    }
                case ZombieState.Wandering:
                    {

                        Human targetPlayer = AgentManager.Instance.GetClosestHuman(this);

                        float distToItPlayer = Vector3.SqrMagnitude(targetPlayer.physicsObject.Position - physicsObject.Position);

                        if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                        {
                            StateTransition(ZombieState.Hunting);
                        }
                        else
                        {
                            Wander();
                        }

                        break;
                    }
            }

        }
        StayInBounds(4f);

        AvoidAllObsticals();

    }

    private void StateTransition(ZombieState newZombieState)
    {
        currentState = newZombieState;

        switch (newZombieState)
        {
            case ZombieState.Hunting:
                {
                    physicsObject.useFriction = false;
                    spriteRenderer.sprite = itSprite;
                    physicsObject.useFriction = false;
                    break;
                }
            case ZombieState.Counting:
                {
                    countDownTimer = AgentManager.Instance.countDownTime;
                    spriteRenderer.sprite = countingSprite;

                    physicsObject.useFriction = true;
                    break;
                }
            case ZombieState.Wandering:
                {
                    physicsObject.useFriction = false;
                    spriteRenderer.sprite = notItSprite;
                    break;
                }
            case ZombieState.Dead:
                {
                    AgentManager.Instance.KillZombie(this);
                    break;
                }
        }

    }

    public void Spawn()
    {
        StateTransition(ZombieState.Counting);
    }

    private bool IsTouching(Human otherPlayer)
    {
        if (AgentManager.Instance.humanList != null)
        {
            float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - otherPlayer.physicsObject.Position);

            float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(otherPlayer.physicsObject.radius, 2);

            return sqrDistance < sqrRadii;
        }
        else
        {
            return false;
        }

    }

    public void Kill()
    {
        StateTransition(ZombieState.Dead);
    }

}
