using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HexagonVehicle : Vehicle
{
    public GameObject duckPrefab;

    private float duckSpeed = 10f;
    private float duckTime = 6f;

    public override void Awake()
    {
        base.Awake();
        maxHealth = 20;
        currHealth = maxHealth;

        fireRate = 1f;
        thrust = 1.5f;
        rotateSpeed = 1f;
        projectileSpeed = 12f;
        projectileTime = 1f;
        ability1Cooldown = 30f;
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

    public override void Ability1()
    {
        CmdAbility1();
    }

    [Command]
    private void CmdAbility1()
    {
        GameObject duck = Instantiate(
            duckPrefab,
            transform.position + transform.up * 2f,
            transform.rotation
        );
        //Physics2D.IgnoreCollision(duck.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        duck.GetComponent<Rigidbody2D>().velocity = transform.up * duckSpeed;
        Destroy(duck, duckTime);

        NetworkServer.Spawn(duck);
    }

    public override void Ability2()
    {
        //do something 
    }
}
