using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipo_Manager : MonoBehaviour
{
    public Equipo equipoActual;
    public Transform camaraEquipo;

    public static Equipo_Manager instancia;

    private Player_Controller controller;

    private void Awake()
    {
        instancia = this;
        controller = GetComponent<Player_Controller>();
        
    }

    public void ClickNormal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && equipoActual != null && 
            controller.puedeMirar == true)
        {
            equipoActual.OnAttackInput();
        }
    }

    public void ClickAlt(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && equipoActual != null &&
            controller.puedeMirar == true)
        {
            equipoActual.onAttackAltInput();
        }
    }

    public void Equipar(ItemData item)
    {
        Desequipar();
        equipoActual = Instantiate(item.equipoPrefab, camaraEquipo).GetComponent<Equipo>();
    }

    public void Desequipar()
    {
        if (equipoActual != null)
        {
            Destroy(equipoActual.gameObject);
            equipoActual = null;
        }
    }
}
