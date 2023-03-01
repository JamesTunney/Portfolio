using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Agent
{
    public enum HumanState
    {
        NotIt,
        Defender,
        Dead
    }

    private HumanState currentState = HumanState.NotIt;

    public HumanState CurrentState => currentState;

    public float countDownTimer = 0f;

    public float visionDistance = 4f;

    public SpriteRenderer spriteRenderer;

    public Sprite defenderSprite;
    public Sprite notItSprite;

    protected override void CalculateSteeringForces()
    {
        if (AgentManager.Instance.zombieList.Count > 0)
        {
            switch (currentState)
            {
                case HumanState.NotIt:
                    {

                        Zombie currentIt = AgentManager.Instance.GetClosestZombie(this);
                        float distToItPlayer = Vector3.SqrMagnitude(physicsObject.Position - currentIt.physicsObject.Position);
                        if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                        {
                            Flee(currentIt.physicsObject.Position);
                        }
                        else
                        {
                            if (AgentManager.Instance.weaponList.Count > 0)
                            {
                                Weapon nearWeapon = AgentManager.Instance.GetClosestWeapon(this);
                                float distToWeapon = Vector3.SqrMagnitude(physicsObject.Position - nearWeapon.physicsObject.Position);

                                if (UsingWeapon(nearWeapon))
                                {
                                    StateTransition(HumanState.Defender);
                                    AgentManager.Instance.KillWeapon(nearWeapon);
                                }
                                else if (distToWeapon < Mathf.Pow(visionDistance, 2))
                                {
                                    Seek(nearWeapon.physicsObject.Position);
                                }
                                else
                                {
                                    Wander();
                                }
                            }
                            else
                            {
                                Wander();
                            }


                        }
                        Separate(AgentManager.Instance.humanList);
                        break;
                    }
                case HumanState.Defender:
                    {

                        countDownTimer -= Time.deltaTime;
                        if (countDownTimer <= -10f)
                        {
                            Zombie currentIt = AgentManager.Instance.GetClosestZombie(this);
                            float distToItPlayer = Vector3.SqrMagnitude(currentIt.physicsObject.Position - physicsObject.Position);
                            if (IsTouching(currentIt))
                            {
                                currentIt.Kill();
                            }
                            else
                            {
                                if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                                {
                                    Seek(currentIt.physicsObject.Position);
                                }
                                else
                                {
                                    Wander();
                                }
                            }

                        }


                        break;
                    }
            }

        }
        StayInBounds(4f);

        AvoidAllObsticals();
    }

    private void StateTransition(HumanState newHumanState)
    {
        currentState = newHumanState;

        switch (newHumanState)
        {
            case HumanState.NotIt:
                {
                    physicsObject.useFriction = false;
                    spriteRenderer.sprite = notItSprite;
                    break;
                }
            case HumanState.Defender:
                {
                    physicsObject.useFriction = false;
                    spriteRenderer.sprite = defenderSprite;
                    countDownTimer = AgentManager.Instance.countDownTime;
                    break;
                }
            case HumanState.Dead:
                {
                    AgentManager.Instance.KillHuman(this);
                    break;
                }
        }

    }


    private bool IsTouching(Zombie otherPlayer)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - otherPlayer.physicsObject.Position);

        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(otherPlayer.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }

    private bool UsingWeapon(Weapon otherPlayer)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - otherPlayer.physicsObject.Position);

        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(otherPlayer.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }

    public void Kill()
    {
        if(currentState == HumanState.Defender)
        {
            if(Random.Range(1, 10) > 7)
            {
                StateTransition(HumanState.Dead);
            }
        }
        else
        {
            StateTransition(HumanState.Dead);
        }
        
    }
    public void Deffend()
    {
        StateTransition(HumanState.Defender);
    }

}