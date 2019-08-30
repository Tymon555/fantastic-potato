using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float thrust = 1f;
    public float rotateSpeed = 0.5f;
    [SyncVar] public int health = 100;
    public bool dead = false;

    private Rigidbody2D playerRb;

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
    }

    [ClientRpc]
    void RpcDamage(int amount)
    {
        Debug.Log("Took damage:" + amount);
    }
    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        health -= amount;
        health = Mathf.Max(0, health);
        if (health == 0) dead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            TakeDamage(1);
            RpcDamage(1);
        }
    }
}
