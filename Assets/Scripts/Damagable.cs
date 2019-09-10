using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Damagable : NetworkBehaviour
{
    public int collisionDmgMulitiplier = 1;

    public int maxHealth;
    [SyncVar(hook = "OnChangeHealth")] public int currHealth;
    [SyncVar] public bool dead = false;

    private void OnChangeHealth(int health)
    {
        currHealth = health;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            TakeDamage((int)collision.relativeVelocity.magnitude * collisionDmgMulitiplier);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            TakeDamage(1);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        amount = Mathf.Min(amount, currHealth);
        currHealth -= amount;
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (currHealth <= 0)
        {
            dead = true;
            Destroy(this.gameObject);
        }
    }

}
