using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventario : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;
    public GameObject ventanaInventario;
    public Transform drop;

    [Header("Item seleccionado")] 
    
    private ItemSlot itemSeleccionado;

    private int indiceItem;

    public TextMeshProUGUI nombreItem;
    public TextMeshProUGUI descripcionItem;
    public TextMeshProUGUI statItem;
    public TextMeshProUGUI statNombreItem;

    public GameObject botonUsar;
    public GameObject botonDrop;
    public GameObject botonEquipar;
    public GameObject botonDesequipar;

    private int itemEquipado;
    private Player_Controller controller;
    private Jugador_Necesidades necesidades;


    [Header("Eventos")] 
    
    public UnityEvent abrirInventario;
    public UnityEvent cerrarInventario;

    public static Inventario instancia;

    private void Awake()
    {
        instancia = this;
        controller = GetComponent<Player_Controller>();
        necesidades = GetComponent<Jugador_Necesidades>();
    }

    private void Start()
    {
        ventanaInventario.SetActive(false);

        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].indice = i;
            uiSlots[i].LimpiarSlot();
        }

        LimpiarItemSeleccionado();
    }

    public void Toggle()
    {
        if (ventanaInventario.activeInHierarchy)
        {
            ventanaInventario.SetActive(false);
            cerrarInventario.Invoke();
            controller.ActivarCursor(false);
        }
        else
        {
            ventanaInventario.SetActive(true);
            abrirInventario.Invoke();
            LimpiarItemSeleccionado();
            controller.ActivarCursor(true);
        }
    }

    public bool EstaAbierto()
    {
        return ventanaInventario.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item.puedeStackear)
        {
            ItemSlot slot = GetItemStack(item);
            if (slot != null)
            {
                slot.cantidad++;
                UpdateUI();
                return;
            }

            TirarItem(item);
        }

        ItemSlot slotVacio = GetSlotVacio();
        if (slotVacio != null)
        {
            slotVacio.item = item;
            slotVacio.cantidad = 1;
            UpdateUI();
            return;
        }

    }

    private void TirarItem(ItemData item)
    {
        Instantiate(item.prefabDrop, drop.position, 
            Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                uiSlots[i].Set(slots[i]);
            }
            else
            {
                uiSlots[i].LimpiarSlot();
            }
        }
    }

    ItemSlot GetItemStack(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].cantidad < item.maxNumStack)
            {
                return slots[i];
            }
        }
        return null;
    }

    private ItemSlot GetSlotVacio()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    private void LimpiarItemSeleccionado()
    {
        itemSeleccionado = null;
        nombreItem.text = string.Empty;
        descripcionItem.text = string.Empty;
        statNombreItem.text = string.Empty;
        statItem.text = string.Empty;
        botonDrop.SetActive(false);
        botonUsar.SetActive(false);
        botonEquipar.SetActive(false);
        botonDesequipar.SetActive(false);
    }

    public void SelectItem(int indice)
    {
        if (slots[indice].item == null)
        {
            return;
        }

        itemSeleccionado = slots[indice];
        indiceItem = indice;
        nombreItem.text = itemSeleccionado.item.nombre;
        descripcionItem.text = itemSeleccionado.item.descripción;
        
        statNombreItem.text = string.Empty;
        statItem.text = string.Empty;
        for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
        {
            statNombreItem.text += itemSeleccionado.item.stats[i].tipo.ToString()+ "\n";

            statItem.text += itemSeleccionado.item.stats[i].valor.ToString() +"\n";
        }
        
        
        botonUsar.SetActive(itemSeleccionado.item.tipo == ItemType.Consumible);
        botonEquipar.SetActive(itemSeleccionado.item.tipo == ItemType.Equipable && !uiSlots[indice].equipado);
        botonDesequipar.SetActive(itemSeleccionado.item.tipo == ItemType.Equipable && uiSlots[indice].equipado);
        botonDrop.SetActive(true);
        
    }

    public void BotonUsar()
    {
        if (itemSeleccionado.item.tipo == ItemType.Consumible)
        {
            for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
            {
                switch (itemSeleccionado.item.stats[i].tipo)
                {
                    case TipoStatConsumible.Hambre:
                        necesidades.Comer(itemSeleccionado.item.stats[i].valor);
                        break;
                        
                    case TipoStatConsumible.Sed:
                        necesidades.Beber(itemSeleccionado.item.stats[i].valor);
                        break;
                    case TipoStatConsumible.Vida:
                        necesidades.Curar(itemSeleccionado.item.stats[i].valor);
                        break;
                    case TipoStatConsumible.Sueño:
                        necesidades.Dormir(itemSeleccionado.item.stats[i].valor);
                        break;
                }
            }
            EliminarItemSeleccionado();
        }
        
    }

    public void BotonEquipar()
    {
        if (uiSlots[itemEquipado].equipado)
        {
            Desequipar(itemEquipado);
        }

        uiSlots[indiceItem].equipado = true;
        itemEquipado = indiceItem;
        Equipo_Manager.instancia.Equipar(itemSeleccionado.item);
        UpdateUI();
        SelectItem(indiceItem);
    }

    public void BotonDesequipar()
    {
        Desequipar(indiceItem);
    }

    private void Desequipar(int indice)
    {
        uiSlots[indice].equipado = false;
        Equipo_Manager.instancia.Desequipar();
        UpdateUI();
        if (indiceItem == indice)
        {
            SelectItem(indice);
        }
    }

    public void BotonDrop()
    {
        TirarItem(itemSeleccionado.item);
        EliminarItemSeleccionado();
    }

    private void EliminarItemSeleccionado()
    {
        itemSeleccionado.cantidad--;
        if (itemSeleccionado.cantidad == 0)
        {
            if (uiSlots[indiceItem].equipado == true)
            {
                Desequipar(indiceItem);
            }

            itemSeleccionado.item = null;
            LimpiarItemSeleccionado();
        }
        
        UpdateUI();
    }

    public void EliminarItem(ItemData item)
    {
        
    }

    public bool TieneItems(ItemData item, int cantidad)
    {
        return false;
    }

    public void BotonAbrirInventario(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }
    
}

public class ItemSlot
{
    public ItemData item;
    public int cantidad;
    
}