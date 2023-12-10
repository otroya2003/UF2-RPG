using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject obj;
    public int cantidad = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            GameObject[] inventario = GameObject.FindGameObjectWithTag("general-events").GetComponent<InventoryController>().getSlots();

            for (int e = 0; e < inventario.Length; e++)
            {
                if (!inventario[e])
                {
                    GameObject.FindGameObjectWithTag("general-events").GetComponent<InventoryController>().setSlot(obj, e, cantidad);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
