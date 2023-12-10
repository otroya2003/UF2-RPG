using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool turno;
    private RectTransform uiBar;
    private float barWidth;
    private float barWidthMax;

    public static Action<bool> turn_txt;

    private GameObject inventario;
    private bool inventoryVisible = false;

    private void OnEnable()
    {
        uiBar = GameObject.FindGameObjectWithTag("bar-movements").GetComponent<RectTransform>();
        barWidthMax = uiBar.anchorMax.x;
        barWidth = barWidthMax;
        uiBar.anchorMax = new Vector2(barWidth, uiBar.anchorMax.y);
    }
    private void Start()
    {
        inventario = GameObject.FindGameObjectWithTag("inventario-com");
        inventario.SetActive(false);
        turno = true;

        PlayerController._useMovement += numberMovesLose;
        Player2Controller._useMovement += numberMovesLose;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryVisible = !inventoryVisible;
            inventario.SetActive(inventoryVisible);
            if (inventoryVisible) 
            { 
                GameObject.FindGameObjectWithTag("general-events").GetComponentInChildren<InventoryController>().showInventory(); 
            }
        }

    }


    private void numberMovesLose()
    {
        barWidth = (barWidth - 0.2f) >= 0 ? (barWidth - 0.2f) : 0;

        if (barWidth <= 0.2)
        {
            turno = !turno;
            barWidth = barWidthMax;
            turn_txt.Invoke(turno);
        }

        uiBar.anchorMax = new Vector2(barWidth, uiBar.anchorMax.y);
    }
}
