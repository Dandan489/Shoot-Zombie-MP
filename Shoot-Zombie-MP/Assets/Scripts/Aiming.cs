using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private LineRenderer lineR;
    private RaycastHit2D hit;
    public Transform gun;

    private void Start()
    {
        lineR = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineR.SetPosition(0, gun.position);
        hit = Physics2D.Raycast(transform.position, transform.right);
        if (hit.collider != null)
        {
            lineR.SetPosition(1, hit.point);
        }
    }
}
