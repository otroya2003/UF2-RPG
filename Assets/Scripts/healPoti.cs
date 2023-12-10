using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPoti : MonoBehaviour
{
    [SerializeField] private CharacterStats m_stats;
    [SerializeField] private CharacterStats m_stats2;


    public void heal()
    {
        if (GameManager.turno)
        {
            m_stats.health += 100;
        }
        else
        {
            m_stats2.health += 100;
        }

        Component[] inventory = GameObject.FindGameObjectWithTag("inventario").GetComponentsInChildren<Transform>();
        GameObject.FindGameObjectWithTag("general-events").GetComponentInChildren<InventoryController>().removeItem(inventory);
    }
}
