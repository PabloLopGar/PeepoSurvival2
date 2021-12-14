using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{

    public float checkRate = 0.05f;
    private float ultimaVezComprobado;
    public float maxDistancia;
    public LayerMask layer;
    private Camera cam;
    private GameObject objetoActual;
    private IInteractuable interactuableActual;
    public TextMeshProUGUI mensaje;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Time.time - ultimaVezComprobado > checkRate)
        {
            ultimaVezComprobado = Time.time;
            Ray rayo = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(rayo, out hit, maxDistancia, layer))
            {
                if (hit.collider.gameObject != objetoActual)
                {
                    objetoActual = hit.collider.gameObject;
                    interactuableActual = hit.collider.GetComponent<IInteractuable>();
                    SetMensaje();
                }
            }
            else
            {
                objetoActual = null;
                interactuableActual = null;
                mensaje.gameObject.SetActive(false);
            }
        }
    }

    private void SetMensaje()
    {
        mensaje.gameObject.SetActive(true);
        mensaje.text = string.Format("<b> [E] </b> {0}", 
            interactuableActual.GetMensajeInteraccion());
    }

    public void AlInteractuar(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && interactuableActual != null)
        {
            interactuableActual.Interactuar();
            objetoActual = null;
            interactuableActual = null;
            mensaje.gameObject.SetActive(false);
        }
    }
    
}

public interface IInteractuable
{

    string GetMensajeInteraccion();

    void Interactuar();

}
