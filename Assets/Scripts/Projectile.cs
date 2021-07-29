using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Photon.MonoBehaviour
{
    public float initForce = 4;
    private Rigidbody2D rb;
    [SerializeField] private float damageAmount = 0.1f;
    private SpriteRenderer sr;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (!sr.flipX)
        {
            rb.velocity = Vector2.right * initForce;
        }
        else
        {
            rb.velocity = Vector2.left * initForce;
        }
        StartCoroutine(DestroyByTime());
    }

    private IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(4f);
        photonView.RPC("DestroySelf", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    public void FlipX()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.isMine)
            return;

        if (collision.TryGetComponent(out PhotonView target) && (!target.isMine || target.isSceneView))
        {
            if (target.CompareTag("Player"))
            {
                target.RPC("ReduceHealth", PhotonTargets.AllBuffered, damageAmount);
            }
            photonView.RPC("DestroySelf", PhotonTargets.AllBuffered);
        }
    }


}
