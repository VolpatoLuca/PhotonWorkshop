using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed : MonoBehaviour
{
    public float DestroyTime = 4;

    private void OnEnable()
    {
        Destroy(gameObject, DestroyTime);
    }
}
