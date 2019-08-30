using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public GameObject playerObject;

    void Update()
    {
        transform.position = playerObject.transform.position + new Vector3(4f, 2f, 0f);
    }

    public void SetPlayerObject(GameObject obj)
    {
        playerObject = obj;
    }
}
