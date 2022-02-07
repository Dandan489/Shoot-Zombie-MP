using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int health;
    public int max_health;
    
    protected Vector2 pushDirection;

    protected virtual void ReceiveDamage(Damage dmg){
        health -= dmg.damageAmount;
        pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
        gameObject.GetComponent<Rigidbody2D>().AddForce(pushDirection);

        if(health <= 0)
            Death();
    }

    protected virtual void Death(){
        
    }
}
