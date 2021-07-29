using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private TMP_Text childText;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }


    void Update()
    {
        if (childText)
            childText.color = button.interactable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, .4f);
    }
}
