using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public GameObject chooseVehicle;
    public static int chosenVehicle = -1;
    public GameObject[] Vehicles;

    private Vehicle vehicle;
    private GameObject vehicleObject;
    private Rigidbody2D vehicleRb;
    private Transform vehicleTransform;

    private bool allowFire = true;
    private bool allowAbility1 = true;
    private bool allowAbility2 = true;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Instantiate(chooseVehicle, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    public void SetChosenVehicle(int nr)
    {
        chosenVehicle = nr;
    }

    private void SetVehicle()
    {
        Debug.Log("Setting vehicle " + chosenVehicle);
        CmdSpawnVehicle(chosenVehicle);
        chosenVehicle = -1;
    }

    [Command]
    private void CmdSpawnVehicle(int choice)
    {
        vehicleObject = Instantiate(Vehicles[choice], this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(vehicleObject, connectionToClient);
        RpcGetVehicle(vehicleObject);
    }

    [ClientRpc]
    private void RpcGetVehicle(GameObject refr)
    {
        if (!isLocalPlayer) return;
        Debug.Log("recieved");
        vehicleObject = refr;
        vehicleRb = vehicleObject.GetComponent<Rigidbody2D>();
        vehicle = vehicleObject.GetComponent<Vehicle>();
        vehicleTransform = vehicleObject.GetComponent<Transform>();
        vehicle.CmdCreateHealthBar();
        vehicle.SetCamera();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if (vehicle == null && chosenVehicle != -1)
            SetVehicle();

        if (vehicle == null || vehicle.dead)
            return;

        //moving
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        if (vertical != 0) vehicleRb.AddForce(vehicleTransform.up * vehicle.thrust * vertical);
        if (horizontal != 0) vehicleRb.AddTorque(-horizontal * vehicle.rotateSpeed);

        //shooting
        int shot = (int)(Input.GetAxisRaw("Fire1"));
        if ((shot != 0) && allowFire)
        {
            Debug.Log("shots fired");
            allowFire = false;
            vehicle.CmdDoFire();
            Invoke("RefreshFire", vehicle.fireRate);
        }

        //Ability1
        if (Input.GetKeyDown(KeyCode.O) && allowAbility1)
        {
            Debug.Log("Ability 1");
            allowAbility1 = false;
            vehicle.Ability1();
            Invoke("RefreshAbility1", vehicle.ability1Cooldown);
        }

        //Ability2
        if (Input.GetKeyDown(KeyCode.P) && allowAbility2)
        {
            Debug.Log("Ability 2");
            allowAbility2 = false;
            vehicle.Ability2();
            Invoke("RefreshAbility2", vehicle.ability2Cooldown);
        }
    }

    private void RefreshFire()
    {
        allowFire = true;
    }
    private void RefreshAbility1()
    {
        allowAbility1 = true;
    }
    private void RefreshAbility2()
    {
        allowAbility2 = true;
    }

    private void OnDestroy()
    {
        if (vehicleObject != null)
            Destroy(vehicleObject);
    }

}
