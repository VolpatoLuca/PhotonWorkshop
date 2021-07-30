using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
        transform.parent = null;
    }

    private void LateUpdate()
    {
        if (!player)
        {
            print("A");
            Destroy(gameObject);
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(transform.position, player.transform.position + offset, 6 * Time.deltaTime);
            transform.position = newPos;

        }
    }
}
