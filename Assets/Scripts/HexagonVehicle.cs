using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonVehicle : Vehicle
{
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
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

    public override void CmdAbility1()
    {
        Debug.Log("Ability 1");
    }

    public override void CmdAbility2()
    {
        Debug.Log("Ability 2");
    }
}
