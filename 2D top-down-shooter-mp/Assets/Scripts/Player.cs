using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : Fighter
{
    public float moveSpeed = 5f;
    public Vector3 startingPos;
    private Rigidbody2D rigi;
    private Vector2 movement;
    private Vector2 mousePos;
    private Camera cam;

    private void Start() {
        if (!isLocalPlayer) return;
        gameObject.transform.position = startingPos;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update(){
        if (!isLocalPlayer) return;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate(){
        if (!isLocalPlayer) return;
        rigi.MovePosition(rigi.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 dire = mousePos - rigi.position;
        float angle = Mathf.Atan2(dire.y, dire.x) * Mathf.Rad2Deg - 90f;

        rigi.rotation = angle;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isLocalPlayer) return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        if (!isLocalPlayer) return;
        Debug.Log("Ur dead");
        Application.Quit();
    }
}
