using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public Image[] blackBorders;
    public Image[] coloredBorders;

    public CharacterSelectionPortrait prefab;

    public Color color;
    int playerNumber;

    private GridSquare _hoveredSquare;
    private CharacterSelectionPortrait _csp;

    public void Init(GridSquare initialSquare, Transform panel)
    {
        foreach (Image i in coloredBorders)
            i.color = color;
        playerText.color = color;
        playerText.text = "P" + playerNumber.ToString();

        _csp = GameObject.Instantiate<CharacterSelectionPortrait>(prefab, panel);
        _csp.Init(color);

        Move(initialSquare);
    }

    public void Move(GridSquare nSquare)
    {
        _hoveredSquare = nSquare;
        this.transform.position = nSquare.transform.position;

        _csp.diplayImage.sprite = nSquare.displayImage.sprite;
    }


}
