using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstical : MonoBehaviour
{
    public float radius = 1f;

    public Vector3 Position => transform.position;

    private void OnDrawGizmoSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, radius);
    }
}
