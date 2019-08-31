using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public int collisionDmgMulitiplier = 1;
    public float thrust = 1f;
    public float rotateSpeed = 0.5f;
    [SyncVar] public int health = 20;
    public bool dead = false;
    public GameObject healthBar;

    private Rigidbody2D playerRb;
    private GameObject bar;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate()
    {
        UpdateMovement(playerRb);
    }
    public void UpdateMovement(Rigidbody2D rb2D)
    {
        if (!isLocalPlayer || dead) return;
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        if (vertical != 0) rb2D.AddForce(transform.up * thrust * vertical);
        if (horizontal != 0) rb2D.AddTorque(-horizontal * rotateSpeed);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
        CmdCreateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        amount = Mathf.Min(amount, health);
        health -= amount;
        if (health == 0) dead = true;
        bar.transform.localScale -= new Vector3(amount * 0.2f, 0f, 0f);
        
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
        if(collision.tag == "Laser")
        {
            TakeDamage(1);
        }
    }
    [Command]
    void CmdCreateHealthBar()
    {
        bar = Instantiate(
            healthBar,
            transform.position + new Vector3(0f, 2f, 0f),
            Quaternion.identity);
        bar.GetComponent<HealthBarFollow>().SetPlayerObject(this.transform);
        NetworkServer.Spawn(bar);
    }
}
