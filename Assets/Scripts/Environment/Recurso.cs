using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recurso : MonoBehaviour
{
    public ItemData itemARecibir;
    public int cantidadPorHit = 2;
    public int capacidad;
    public GameObject particula;

    public void Recolectar(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < cantidadPorHit; i++)
        {
            if (capacidad <= 0)
            {
                break;
            }

            capacidad -= 1;
            Inventario.instancia.AddItem(itemARecibir);
        }
        
        Destroy(Instantiate(particula,hitPoint,
            Quaternion.LookRotation(hitNormal,Vector3.up)),1.0f);

        if (capacidad <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    
}
