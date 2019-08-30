using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public int depth = -10;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            Vector2 difference = (transform.position - playerTransform.position);
            float dist = difference.magnitude;
            if(dist > 3)
            {
                transform.position = playerTransform.position + ((Vector3)difference * (3f/dist)) + new Vector3(0f, 0f, depth);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }
}