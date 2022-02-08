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
    private bool downed;
    public HudUpdate HU;

    private void Start() {
        GameManager.instance.New_Player(gameObject.transform);
        if (!isLocalPlayer) return;
        gameObject.transform.position = startingPos;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Player Loaded");
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
        HU.player = gameObject.GetComponent<Player>();
        HU.weapon = gameObject.GetComponent<Shooting>();
        HU.weapon1 = gameObject.transform.Find("Gun").Find("weapon_gun").gameObject;
        HU.weapon2 = gameObject.transform.Find("Gun").Find("weapon_machine").gameObject;
        gameObject.GetComponent<Shooting>().HU = HU;
    }

    private void Update(){
        if (!isLocalPlayer || downed) return;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate(){
        if (!isLocalPlayer || downed) return;
        rigi.MovePosition(rigi.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 dire = mousePos - rigi.position;
        float angle = Mathf.Atan2(dire.y, dire.x) * Mathf.Rad2Deg - 90f;

        rigi.rotation = angle;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        HU.OnHitpointChange();
    }



    protected override void Death()
    {
        Debug.Log("Ur dead");
        downed = true;
        GameManager.instance.Kill_Player(gameObject.transform);
    }
}
