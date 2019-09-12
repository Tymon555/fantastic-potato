using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vehicle : Damagable
{

    public float fireRate;
    public float thrust;
    public float rotateSpeed;
    public float projectileSpeed;
    public float projectileTime;
    public float ability1Cooldown;
    public float ability2Cooldown;

    public GameObject weaponPrefab;
    public GameObject healthBar;

    private HealthBarFollow barFollow;
    private GameObject bar;

    public virtual void Awake()
    {
        //override this and set public values of this and damagable
    }

    public void SetCamera()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
    }

    public override void TakeDamage(int amount)
    {
        if (!isServer)
            return;
        base.TakeDamage(amount);
        barFollow.ShortenScale(amount * (3f / maxHealth));
    }

    private void OnDestroy()
    {
        if (bar != null)
             Destroy(bar);
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

    public virtual void Ability1()
    {
        //virtual commands are not supported - override this and call a command
    }

    public virtual void Ability2()
    {
        //^
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
