using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProTurn;
    [SerializeField] private TextMeshProUGUI _textMeshProLVLJ1;
    [SerializeField] private TextMeshProUGUI _textMeshProLVLJ2;
    [SerializeField] private TextMeshProUGUI _textMeshProEXPJ1;
    [SerializeField] private TextMeshProUGUI _textMeshProEXPJ2;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.turn_txt += SetTextTurn;
        Enemy1Controller.lvlJ1_txt += SetTextNivelJ1;
        Enemy1Controller.lvlJ2_txt += SetTextNivelJ2;
        Enemy1Controller.expJ1_txt += SetTextExpJ1;
        Enemy1Controller.expJ2_txt += SetTextExpJ2;

        _textMeshProTurn.text = "Turno jugador 1";
        _textMeshProLVLJ1.text = "Nivel J1: 1";
        _textMeshProLVLJ2.text = "Nivel J2: 1";
        _textMeshProEXPJ1.text = "Exp J1: 0";
        _textMeshProEXPJ2.text = "Exp J2: 0";
    }
    public void SetTextTurn(bool turno)
    {

        _textMeshProTurn.text = turno ? "Turno jugador 1" : "Turno jugador 2";
    }
    public void SetTextNivelJ1(int cantidad)
    {

        _textMeshProLVLJ1.text = "Nivel J1: "+ cantidad;
    }
    public void SetTextNivelJ2(int cantidad)
    {

        _textMeshProLVLJ2.text = "Nivel J2: " + cantidad;
    }
    public void SetTextExpJ1(int cantidad)
    {

        _textMeshProEXPJ1.text = "Exp J1: " + cantidad;
    }
    public void SetTextExpJ2(int cantidad)
    {

        _textMeshProEXPJ2.text = "Exp J2: " + cantidad;
    }
}
