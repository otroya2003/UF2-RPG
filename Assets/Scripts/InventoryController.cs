using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject[] slots;

    private int max_slots = 32;
    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[max_slots];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject[] getSlots()
    {
        return this.slots;
    }

    public void setSlot(GameObject slot, int pos, int cant)
    {
        bool exist = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                if (slots[i].tag == slot.tag)
                {
                    int already_cant = slots[i].GetComponent<AtributsController>().getCantidad();
                    this.slots[i].GetComponent<AtributsController>().setCantidad(already_cant + cant);
                    exist = true;
                }
            }
        }

        if (!exist)
        {
            slot.GetComponent<AtributsController>().setCantidad(cant);
            this.slots[pos] = slot;
        }
    }

    public void showInventory()
    {
        Component[] inventory = GameObject.FindGameObjectWithTag("inventario").GetComponentsInChildren<Transform>();
        bool slotUsed = false;

        if (removeItem(inventory))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    slotUsed = false;

                    for (int e = 0; e < inventory.Length; e++)
                    {
                        GameObject child = inventory[e].gameObject;

                        if (child.tag == "slot" && child.transform.childCount <= 1 && !slotUsed)
                        {
                            GameObject item = Instantiate(slots[i], child.transform.position, Quaternion.identity);
                            item.transform.SetParent(child.transform, false);
                            item.transform.localPosition = new Vector3(0, 0, 0);
                            item.name = item.name.Replace("Clone", "");

                            slotUsed = true;
                        }
                    }
                }
            }
        }
    }

    public bool removeItem(Component[] inventory)
    {
        for (int e = 1; e < inventory.Length; e++)
        {
            GameObject child = inventory[e].gameObject;
            if (child.tag == "slot" && child.transform.childCount > 0)
            {
                for (int a = 0; a <= 0; a++)
                {
                    Destroy(child.transform.GetChild(a).transform.gameObject);
                }
            }
        }
        return true;
    }
}
