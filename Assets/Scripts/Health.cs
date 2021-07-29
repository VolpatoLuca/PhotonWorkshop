using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : Photon.MonoBehaviour
{
    [SerializeField] private Image healthImage;

    [PunRPC] public void ReduceHealth(float amount)
    {
        ModifyHealth(-amount);
    }

    private void ModifyHealth(float amount)
    {
        if (photonView.isMine)
        {
            healthImage.fillAmount += amount;
        }
        else
        {
            healthImage.fillAmount += amount;
        }
    }
}
