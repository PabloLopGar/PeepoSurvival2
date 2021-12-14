using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icono;
    public TextMeshProUGUI cantidadText;
    private ItemSlot slotActual;
    private Outline outline;
    public int indice;
    public bool equipado;


    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipado;
    }

    public void Set(ItemSlot slot)
    {
        slotActual = slot;
        icono.gameObject.SetActive(true);
        icono.sprite = slot.item.icono;
        cantidadText.text = slot.cantidad > 1 ? slot.cantidad.ToString() : string.Empty;
        if (outline != null)
            outline.enabled = equipado;
    }

    public void LimpiarSlot()
    {
        slotActual = null;
        icono.gameObject.SetActive(false);
        cantidadText.text = string.Empty;
    }

    public void PresionarBoton()
    {
        Inventario.instancia.SelectItem(indice);   
    }
}
