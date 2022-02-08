using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 2;
    public float push = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "enemy"){
            Damage dmg = new Damage{
                damageAmount = damage,
                origin = gameObject.transform.position,
                pushForce = push
            };
            other.SendMessage("ReceiveDamage", dmg);
        }
        Destroy(gameObject);
    }
}
