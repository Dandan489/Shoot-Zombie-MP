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
    private float nextAttack = 0f;
    private float attackSpeed = 1f;
    private Rigidbody2D rigi;
    public GameObject blood;

    public Transform target;
    NavMeshAgent agent;

    private void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rigi = gameObject.GetComponent<Rigidbody2D>();
        FindTarget();
    }

    private void Update(){
        FindTarget();
        agent.SetDestination(target.position);
    }

    private void FixedUpdate() {
        Vector2 dire = new Vector2(target.position.x, target.position.y) - rigi.position;
        float angle = Mathf.Atan2(dire.y, dire.x) * Mathf.Rad2Deg;
        rigi.rotation = angle;
    }
    
    private void FindTarget()
    {
        float minDistance = float.MaxValue;
        Transform closestPlayer = null;
        foreach (Transform player in GameManager.instance.player_transform)
        {
            if (Vector2.Distance(transform.position, player.position) < minDistance)
            {
                minDistance = Vector2.Distance(transform.position, player.position);
                closestPlayer = player;
            }
        }
        target = closestPlayer;
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
