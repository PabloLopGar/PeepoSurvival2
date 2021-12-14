using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{

    public int daño;
    
    public float ratioDaño;

    private List<IReciboDaño> cosasADañar = new List<IReciboDaño>();

    void Start()
    {
        StartCoroutine(RepartirDaño());
    }

    
    void Update()
    {
        
    }

    IEnumerator RepartirDaño()
    {
        while (true)
        {
            for (int i = 0; i < cosasADañar.Count; i++)
            {
                cosasADañar[i].RecibirDaño(daño);
            }
            yield return new WaitForSeconds(ratioDaño);   
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IReciboDaño>() != null)
        {
            cosasADañar.Add(other.gameObject.GetComponent<IReciboDaño>());
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<IReciboDaño>() != null)
        {
            cosasADañar.Remove(other.gameObject.GetComponent<IReciboDaño>());
        }
    }
}
