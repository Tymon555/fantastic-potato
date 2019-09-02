using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthBarFollow : NetworkBehaviour
{
    public Transform playerObject;
    [SyncVar(hook = "OnSetScale")] private Vector3 scale;

    private void Awake()
    {
        scale = new Vector3(3f, 0.2f, 1f);
    }

    void Update()
    {
        if(playerObject != null)
            transform.position = playerObject.position + new Vector3(0f, 2f, 0f);
    }

    public void SetPlayerObject(Transform obj)
    {
        playerObject = obj;
    }

    public void ShortenScale(float amount)
    {
        scale -= new Vector3(amount, 0f, 0f);
    }

    private void OnSetScale(Vector3 vec)
    {
        this.scale = vec;
        this.transform.localScale = vec;
    }
}
