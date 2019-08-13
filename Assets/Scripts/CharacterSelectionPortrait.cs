using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionPortrait : MonoBehaviour
{
    public TextMeshProUGUI playerIndicator, finalized, sName;
    public Image diplayImage, backdrop;

    public void Init(Color color)
    {
        backdrop.color = color;
        playerIndicator.color = color;
        finalized.color = color;
        sName.color = color;
    }
}
