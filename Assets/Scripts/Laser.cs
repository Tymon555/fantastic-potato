using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Laser : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServer)
            Destroy(this.gameObject);
    }
}
