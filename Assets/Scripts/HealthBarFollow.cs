using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform playerObject;

    void Update()
    {
        if(playerObject != null)
            transform.position = playerObject.position + new Vector3(0f, 2f, 0f);
    }

    public void SetPlayerObject(Transform obj)
    {
        playerObject = obj;
    }
}
