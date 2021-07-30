using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private GameObject playerCanvas;
    public float healthAmount;
    private PlayerController pController;


    private void Start()
    {
        pController = GetComponent<PlayerController>();
        if (photonView.isMine)
        {
            GameManager.instance.localPlayer = gameObject;
        }
    }

    [PunRPC]
    public void ReduceHealth(float amount)
    {
        ModifyHealth(-amount);
    }

    private void CheckHealth()
    {
        healthImage.fillAmount = healthAmount / 100f;
        if (photonView.isMine && healthAmount <= 0)
        {
            GameManager.instance.EnableRespawn();
            photonView.RPC("Dead", PhotonTargets.AllBuffered);
        }
    }

    [PunRPC]
    private void Dead()
    {
        pController.isDead = true;
        pController.sr.enabled = false;
        pController.bc.enabled = false;
        playerCanvas.SetActive(false);
        pController.rb.gravityScale = 0;
    }
    [PunRPC]
    private void Respawn()
    {
        pController.isDead = false;
        pController.sr.enabled = true;
        pController.bc.enabled = true;
        healthAmount = 100;
        healthImage.fillAmount = 1;
        playerCanvas.SetActive(true);
        pController.rb.gravityScale = 1;
    }

    private void ModifyHealth(float amount)
    {
        //if (photonView.isMine)
        //{
        //    healthImage.fillAmount += amount;
        //}
        //else
        //{
        //    healthImage.fillAmount += amount;
        //}
        healthAmount += amount;
        CheckHealth();
    }

    [PunRPC]
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
