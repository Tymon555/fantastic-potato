using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public int depth = -10;
    public float noFollowDist = 2f;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            Vector2 difference = (transform.position - playerTransform.position);
            float dist = difference.magnitude;
            if(dist > noFollowDist)
            {
                transform.position = playerTransform.position + ((Vector3)difference * (noFollowDist/dist)) + new Vector3(0f, 0f, depth);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
    }
}