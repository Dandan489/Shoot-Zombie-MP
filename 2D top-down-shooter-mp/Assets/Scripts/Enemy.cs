using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Enemy : Fighter
{
    public int damage = 2;
    public int score = 1;
    public int coindropup;
    public int coindropdn;
    [SyncVar]
    private float nextAttack = 0f;
    private float attackSpeed = 2f;
    private Rigidbody2D rigi;
    public GameObject blood;

    public Transform target;
    NavMeshAgent agent;

    private void Start() {
        nextAttack = Time.time;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        Player.instance.FindTarget(gameObject.transform);
    }

    private void Update(){
        Player.instance.FindTarget(gameObject.transform);
        agent.SetDestination(target.position);
    }

    private void FixedUpdate() {
        Vector2 dire = new Vector2(target.position.x, target.position.y) - rigi.position;
        float angle = Mathf.Atan2(dire.y, dire.x) * Mathf.Rad2Deg;
        rigi.rotation = angle;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player" && Time.time >= nextAttack)
        {
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = gameObject.transform.position,
                pushForce = 0
            };
            collision.collider.SendMessage("ReceiveDamage", dmg);

            nextAttack = Time.time + attackSpeed;
        }
    }

    protected override void Death()
    {
        base.Death();
        EnemySpwaner.instance.enemycnt--;
        GameObject Blood = Instantiate(blood, transform.position, transform.rotation);
        NetworkServer.Spawn(Blood);
        GameManager.instance.OnEnemyDie(Random.Range(coindropdn, coindropup), score, gameObject);
    }
}
