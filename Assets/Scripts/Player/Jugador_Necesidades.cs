using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Jugador_Necesidades : MonoBehaviour , IReciboDaño
{

    public Necesidad vida;
    public Necesidad hambre;
    public Necesidad sed;
    public Necesidad dormir;

    public float noComidaCantidadVidaQueDecae;
    public float noAguaCantidadVidaQueDecae;

    public UnityEvent alRecibirDaño;

    void Start()
    {
        vida.valorActual = vida.valorComienzo;
        hambre.valorActual = hambre.valorComienzo;
        sed.valorActual = sed.valorComienzo;
        dormir.valorActual = dormir.valorComienzo;
    }

    
    void Update()
    {
        
        hambre.Restar(hambre.ratioPerdida * Time.deltaTime);
        sed.Restar(sed.ratioPerdida * Time.deltaTime);
        dormir.Añadir(dormir.ratioRegeneracion * Time.deltaTime);

        if (hambre.valorActual == 0.0f)
        {
            vida.Restar(noComidaCantidadVidaQueDecae * Time.deltaTime);
        }

        if (sed.valorActual == 0.0f)
        {
            vida.Restar(noAguaCantidadVidaQueDecae * Time.deltaTime);
        }

        if (vida.valorActual == 0.0f)
        {
            Morir();
        }
        
        vida.barraUi.fillAmount = vida.GetPorcentage();
        sed.barraUi.fillAmount = sed.GetPorcentage();
        hambre.barraUi.fillAmount = hambre.GetPorcentage();
        dormir.barraUi.fillAmount = dormir.GetPorcentage();

    }

    public void Curar(float cantidad)
    {
        vida.Añadir(cantidad);
    }

    public void Comer(float cantidad)
    {
        hambre.Añadir(cantidad);
    }

    public void Beber(float cantidad)
    {
        sed.Añadir(cantidad);
    }

    public void Dormir(float cantidad)
    {
        dormir.Restar(cantidad);
    }

    public void RecibirDaño(int cantidad)
    {
        vida.Restar(cantidad);
        alRecibirDaño?.Invoke();
    }

    public void Morir()
    {
        Debug.Log("Has muerto bitch");
    }
    
}

[System.Serializable]
public class Necesidad
{
    [HideInInspector]
    public float valorActual;
    public float valorMax;
    public float valorComienzo;
    public float ratioRegeneracion;
    public float ratioPerdida;
    public Image barraUi;

    public void Añadir(float cantidad)
    {
        valorActual = Mathf.Min(valorActual + cantidad, valorMax);
    }

    public void Restar(float cantidad)
    {
        valorActual = Mathf.Max(valorActual - cantidad, 0.0f);
    }

    public float GetPorcentage()
    {
        return valorActual / valorMax;
    }
    
}

public interface IReciboDaño
{

    void RecibirDaño(int cantidad);


}