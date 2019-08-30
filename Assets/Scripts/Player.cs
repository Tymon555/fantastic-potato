using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float thrust = 1f;
    public float rotateSpeed = 0.5f;

    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void FixedUpdate()
    {
        UpdateMovement(rb2D);
    }
    public void UpdateMovement(Rigidbody2D rb2D)
    {
        if (!isLocalPlayer) return;
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
}
