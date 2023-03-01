using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance;


    [HideInInspector]
    public List<Weapon> weaponList = new List<Weapon>();

    [HideInInspector]
    public List<Human> humanList = new List<Human>();
    [HideInInspector]
    public List<Zombie> zombieList = new List<Zombie>();

    [HideInInspector]
    public Vector2 maxPosition = Vector2.one;
    [HideInInspector]
    public Vector2 minPosition = -Vector2.one;

    public float edgePadding = 1f;

    public Human humanPreFab;

    public Zombie zombiePreFab;

    public Weapon weaponPreFab;

    public int numHumans = 10;

    public int numZombies = 1;

    public int numWeapons = 2;

    public int countDownTime = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Camera cam = Camera.main;

        if(cam != null)
        {
            Vector3 camPosition = cam.transform.position;
            float halfHight = cam.orthographicSize;
            float halfWidth = halfHight * cam.aspect;

            maxPosition.x = camPosition.x + halfWidth - edgePadding;
            maxPosition.y = camPosition.y + halfHight - edgePadding;

            minPosition.x = camPosition.x - halfWidth + edgePadding;
            minPosition.y = camPosition.y - halfHight + edgePadding;
        }
        
        for (int i = 0; i < numHumans; i++)
        {
            humanList.Add(Spawn(humanPreFab));
        }
        for (int i = 0; i < numZombies; i++)
        {
            zombieList.Add(Spawn(zombiePreFab));
        }
        for (int i=0; i < numWeapons; i++)
        {
            weaponList.Add(Spawn(weaponPreFab));
        }
        
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePosition);
            float worldX = worldpos.x;
            float worldY = worldpos.y;

            Human newHuman = Instantiate(humanPreFab, new Vector3(worldX, worldY), Quaternion.identity);
            humanList.Add(newHuman);

        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePosition);
            float worldX = worldpos.x;
            float worldY = worldpos.y;

            Zombie newZombie = Instantiate(zombiePreFab, new Vector3(worldX, worldY), Quaternion.identity);
            zombieList.Add(newZombie);

        }
        if (mouse.middleButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePosition);
            float worldX = worldpos.x;
            float worldY = worldpos.y;

            Weapon newWeapon = Instantiate(weaponPreFab, new Vector3(worldX, worldY), Quaternion.identity);
            weaponList.Add(newWeapon);

        }

    }

    private T Spawn<T>(T prefabToSpawn) where T : Agent
    {
        float xPos = Random.Range(minPosition.x, maxPosition.x);
        float yPos = Random.Range(minPosition.y, maxPosition.y);

        return Instantiate(prefabToSpawn, new Vector3(xPos, yPos), Quaternion.identity);
    }

    
    public Human GetClosestHuman(Zombie sourcePlayer)
    {
        float minDistance = float.MaxValue;
        Human closestHuman = null;
        foreach (Human other in humanList)
        {
            float sqrDistance = Vector3.SqrMagnitude(sourcePlayer.physicsObject.Position - other.physicsObject.Position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestHuman = other;
                minDistance = sqrDistance;
            }
        }

        return closestHuman;
    }

    public Zombie GetClosestZombie(Human sourcePlayer)
    {
        float minDistance = float.MaxValue;
        Zombie closestZombie = null;
        foreach (Zombie other in zombieList)
        {
            float sqrDistance = Vector3.SqrMagnitude(sourcePlayer.physicsObject.Position - other.physicsObject.Position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestZombie = other;
                minDistance = sqrDistance;
            }
        }

        return closestZombie;
    }

    public Weapon GetClosestWeapon(Human sourcePlayer)
    {
        float minDistance = float.MaxValue;
        Weapon closestWeapon = null;
        foreach (Weapon other in weaponList)
        {
            float sqrDistance = Vector3.SqrMagnitude(sourcePlayer.physicsObject.Position - other.physicsObject.Position);

            if (sqrDistance < float.Epsilon)
            {
                continue;
            }

            if (sqrDistance < minDistance)
            {
                closestWeapon = other;
                minDistance = sqrDistance;
            }
        }

        return closestWeapon;
    }

    public void KillHuman(Human deadHuman)
    {
        
        deadHuman.spriteRenderer.enabled = false;
        humanList.Remove(deadHuman);
        Destroy(deadHuman.gameObject);
        Zombie newZombie = Instantiate(zombiePreFab, deadHuman.physicsObject.Position, Quaternion.identity);
        newZombie.Spawn();
        zombieList.Add(newZombie);
    }
    
    public void KillWeapon(Weapon deadWeapon)
    {
        deadWeapon.spriteRenderer.enabled = false;
        weaponList.Remove(deadWeapon);
        Destroy(deadWeapon.gameObject);
    }

    public void KillZombie(Zombie deadZombie)
    {
        deadZombie.spriteRenderer.enabled = false;
        zombieList.Remove(deadZombie);
        Destroy(deadZombie.gameObject);
    }
}
