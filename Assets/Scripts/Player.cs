using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public const int maxHealth = 20;

    public int collisionDmgMulitiplier = 1;
    public float thrust = 1f;
    public float rotateSpeed = 0.5f;
    [SyncVar(hook = "OnChangeHealth")] public int currHealth = maxHealth;
    [SyncVar] public bool dead = false;
    public GameObject healthBar;

    private Rigidbody2D playerRb;
    private GameObject bar;
    private HealthBarFollow barFollow;

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

        amount = Mathf.Min(amount, currHealth);
        currHealth -= amount;
        CheckIfDead();
        barFollow.ShortenScale(amount * (3f / maxHealth));
    }

    private void CheckIfDead()
    {
        if(currHealth <= 0)
        {
            dead = true;
            bar.SetActive(false);
            this.gameObject.SetActive(false);
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
        barFollow = bar.GetComponent<HealthBarFollow>();
        barFollow.SetPlayerObject(this.transform);
        NetworkServer.Spawn(bar);
    }

    private void OnDestroy()
    {
        if (bar != null)
             Destroy(bar);
    }

    void OnChangeHealth(int health)
    {
        currHealth = health;
    }
}
