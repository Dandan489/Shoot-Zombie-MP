using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : Fighter
{
    public static Player instance = null;
    public float moveSpeed = 5f;
    public Vector3 startingPos;
    private Rigidbody2D rigi;
    private Vector2 movement;
    private Vector2 mousePos;
    private Camera cam;
    private bool downed = false;
    public HudUpdate HU;
    public GameObject Hud;

    private void Start() {
        gameObject.transform.position = startingPos;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main;
        instance = this;
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
        CmdNew_Player();
        Hud.SetActive(true);
        HU.player = gameObject.GetComponent<Player>();
        HU.weapon = gameObject.GetComponent<Shooting>();
        HU.weapon1 = gameObject.transform.Find("Gun").Find("weapon_gun").gameObject;
        HU.weapon2 = gameObject.transform.Find("Gun").Find("weapon_machine").gameObject;
        gameObject.GetComponent<Shooting>().HU = HU;
        Debug.Log("Player Loaded");
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
        TitleText.instance.Show("get hit1");
        base.ReceiveDamage(dmg);
        TitleText.instance.Show("get hit2");
        HU.OnHitpointChange();
        TitleText.instance.Show("get hit3");
    }

    protected override void Death()
    {
        if (!isLocalPlayer) return;
        TitleText.instance.Show("Ur dead");
        downed = true;
        CmdKill_Player();
    }

    [Command]
    private void CmdNew_Player()
    {
        GameManager.instance.New_Player(gameObject.transform);
    }

    [Command]
    private void CmdKill_Player()
    {
        GameManager.instance.Kill_Player(gameObject.transform);
    }

    public void FindTarget(Transform zombie)
    {
        CmdFindTarget(zombie);
    }

    [Command]
    private void CmdFindTarget(Transform zombie)
    {
        float minDistance = float.MaxValue;
        Transform closestPlayer = null;
        foreach (Transform player in GameManager.instance.player_transform)
        {
            if (Vector2.Distance(zombie.position, player.position) < minDistance)
            {
                minDistance = Vector2.Distance(transform.position, player.position);
                closestPlayer = player;
            }
        }
        RpcSetTarget(closestPlayer, zombie);
    }

    [ClientRpc]
    private void RpcSetTarget(Transform player, Transform zombie)
    {
        zombie.gameObject.GetComponent<Enemy>().target = player;
    }
}
