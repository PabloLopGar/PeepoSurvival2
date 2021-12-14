using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ItemController : MonoBehaviour, IInteractuable
{
    public ItemData Item;
    
    public string GetMensajeInteraccion()
    {
        return string.Format("Recoger {0}", Item.nombre);
    }

    public void Interactuar()
    {
        Inventario.instancia.AddItem(Item);
        Destroy(gameObject);
    }
}
