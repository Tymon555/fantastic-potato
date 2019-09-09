using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vehicle : NetworkBehaviour
{

    public int collisionDmgMulitiplier = 1;
    public int maxHealth;

    public float fireRate;
    public float thrust;
    public float rotateSpeed;
    public float projectileSpeed;
    public float projectileTime;
    public float ability1Cooldown;
    public float ability2Cooldown;

    public GameObject weaponPrefab;
    public GameObject healthBar;

    [SyncVar(hook = "OnChangeHealth")] public int currHealth;
    [SyncVar] public bool dead = false;

    private HealthBarFollow barFollow;
    private GameObject bar;

    public virtual void Awake()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
        //override this and set public values
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        amount = Mathf.Min(amount, currHealth);
        currHealth -= amount;
        CheckIfDead();
        barFollow.ShortenScale(amount * (3f / maxHealth));
        //override if has armour or something
    }

    private void CheckIfDead()
    {
        if(currHealth <= 0)
        {
            dead = true;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            TakeDamage((int)collision.relativeVelocity.magnitude * collisionDmgMulitiplier);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Weapon")
        {
            TakeDamage(1);
        }
    }

    private void OnDestroy()
    {
        if (bar != null)
             Destroy(bar);
    }

    private void OnChangeHealth(int health)
    {
        currHealth = health;
    }


    [Command]
    public void CmdDoFire()
    {
        GameObject projectile = (GameObject)Instantiate(
            weaponPrefab,
            transform.position,
            transform.rotation);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        var bullet2D = projectile.GetComponent<Rigidbody2D>();
        bullet2D.velocity = transform.up * projectileSpeed;
        Destroy(projectile, projectileTime);

        NetworkServer.Spawn(projectile);
    }

    [Command]
    public virtual void CmdAbility1()
    {
        //override
    }

    [Command]
    public virtual void CmdAbility2()
    {
        //override
    }

    [Command]
    public void CmdCreateHealthBar()
    {
        bar = Instantiate(
               healthBar,
               transform.position + new Vector3(0f, 2f, 0f),
               Quaternion.identity);
        barFollow = bar.GetComponent<HealthBarFollow>();
        barFollow.SetPlayerObject(transform);
        NetworkServer.Spawn(bar);
    }
}
