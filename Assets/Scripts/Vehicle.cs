using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vehicle : Player
{
    protected float laserSpeed = 10f;
    protected float laserTime = 1f;

    private Rigidbody2D rb2D;
    public GameObject laserPrefab;
 

    [Command]
    void CmdDoFire(float lifeTime)
    {
        GameObject laser = (GameObject)Instantiate(
            laserPrefab,
            transform.position + transform.right,
            Quaternion.identity);

        var bullet2D = laser.GetComponent<Rigidbody2D>();
        bullet2D.velocity = transform.right * laserSpeed;
        Destroy(laser, lifeTime);

        NetworkServer.Spawn(laser);
    }
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate()
    {
        base.UpdateMovement(rb2D);

        //weapons 
        int laser = (int)(Input.GetAxisRaw("Fire1"));
        if(laser != 0)
        {
            Debug.Log("shots fired");
            CmdDoFire(laserTime);
        }
    }
}
