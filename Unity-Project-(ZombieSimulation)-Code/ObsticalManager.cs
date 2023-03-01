using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalManager : MonoBehaviour
{
    public static ObsticalManager Instance;

    public List<Obstical> Obsticals = new List<Obstical>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
