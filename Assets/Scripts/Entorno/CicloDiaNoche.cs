using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{

    [Range(0.0f, 1.0f)]
    public float tiempo;

    public float duracionDia;
    public float horaInicio = 0.4f;
    public float ratioTiempo;
    public Vector3 mediodia;

    [Header("Sol")] 
    public Light sol;

    public Gradient colorSol;

    public AnimationCurve intensidadSol;

    [Header("Luna")] 
    public Light luna;

    public Gradient colorLuna;

    public AnimationCurve intensidadLuna;

    [Header("Otros")] 
    public AnimationCurve intensidadGlobal;

    public AnimationCurve reflejosGlobales;
    
    
    void Start()
    {

        ratioTiempo = 1.0f / duracionDia;
        tiempo = horaInicio;

    }

    
    void Update()
    {

        tiempo += ratioTiempo * Time.deltaTime;

        if (tiempo >= 1.0f)
        {
            tiempo = 0.0f;
        }

        sol.transform.eulerAngles = (tiempo - 0.25f) * mediodia * 4.0f;
        luna.transform.eulerAngles = (tiempo - 0.75f) * mediodia * 4.0f;

        sol.intensity = intensidadSol.Evaluate(tiempo);
        luna.intensity = intensidadLuna.Evaluate(tiempo);

        sol.color = colorSol.Evaluate(tiempo);
        luna.color = colorLuna.Evaluate(tiempo);

        if (sol.intensity == 0 && sol.gameObject.activeInHierarchy)
        {
            
            sol.gameObject.SetActive(false);
            
        }
        else if (sol.intensity > 0 && !sol.gameObject.activeInHierarchy)
        {
            sol.gameObject.SetActive(true);
        }
        
        if (luna.intensity == 0 && luna.gameObject.activeInHierarchy)
        {
            
            luna.gameObject.SetActive(false);
            
        }
        else if (luna.intensity > 0 && !luna.gameObject.activeInHierarchy)
        {
            luna.gameObject.SetActive(true);
        }

        RenderSettings.ambientIntensity = intensidadGlobal.Evaluate(tiempo);
        RenderSettings.reflectionIntensity = intensidadGlobal.Evaluate(tiempo);

    }
}
