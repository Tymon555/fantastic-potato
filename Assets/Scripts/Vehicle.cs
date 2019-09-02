using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vehicle : Player
{
    protected float laserSpeed = 10f;
    protected float laserTime = 1f;
    private float fireRate = 1f;

    private Rigidbody2D vehicleRb;
    public GameObject laserPrefab;
    private bool allowFire = true;
 

    [Command]
    void CmdDoFire(float lifeTime)
    {
        //allowFire = false;
        GameObject laser = (GameObject)Instantiate(
            laserPrefab,
            transform.position,
            transform.rotation);
        Physics2D.IgnoreCollision(laser.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        var bullet2D = laser.GetComponent<Rigidbody2D>();
        bullet2D.velocity = transform.up * laserSpeed;
        Destroy(laser, lifeTime);

        NetworkServer.Spawn(laser);
        //Invoke("refreshFire", fireRate);
    }
    void Start()
    {
        vehicleRb = GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate()
    {
        if (dead) this.gameObject.SetActive(false);
        if (!isLocalPlayer || dead) return;
        base.UpdateMovement(vehicleRb);

        //weapons 
        int laser = (int)(Input.GetAxisRaw("Fire1"));
        if((laser != 0) && allowFire)
        {
            Debug.Log("shots fired");
            allowFire = false;
            CmdDoFire(laserTime);
            Invoke("refreshFire", fireRate);
        }
    }
    private void refreshFire()
    {
        allowFire = true;
    }
}
