using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    
    [Header("Camara")]

    public Transform camaraContainer;
    
    public float maxXMirar;
    
    public float minXMirar;
    
    private float camCurXRotation;
    
    public float sensCam;

    [Header("Movimiento")] 
    
    public float moveSpeed;

    private Vector2 inputActualMovimiento;  

    private Vector2 mouseDelta;

    private Rigidbody rigidbody;

    [Header("Salto")] 
    
    public float fuerzaSalto;

    public LayerMask capasSuelo;

    public bool puedeMirar;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        puedeMirar = true;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (puedeMirar == true)
        {
            CamaraMirar();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * inputActualMovimiento.y + transform.right * inputActualMovimiento.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;
        rigidbody.velocity = dir;
    }

    public void CamaraInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void CamaraMirar()
    {
        camCurXRotation += mouseDelta.y * sensCam;
        camCurXRotation = Mathf.Clamp(camCurXRotation, minXMirar, maxXMirar);
        camaraContainer.localEulerAngles = new Vector3(-camCurXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * sensCam, 0);
    }

    public void CapturarInputMovimiento(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            inputActualMovimiento = context.ReadValue<Vector2>();
        }
        else
        {
            inputActualMovimiento = Vector2.zero;
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        { new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.02f), Vector3.down),
          new Ray(transform.position + (-transform.forward * 0.2f)+ (Vector3.up * 0.02f), Vector3.down),
          new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.02f), Vector3.down),
          new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.02f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, capasSuelo))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void CapturarTeclaSalto(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                rigidbody.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            }
        }
    }

    public void ActivarCursor(bool activar)
    {
        Cursor.lockState = activar ? CursorLockMode.None : CursorLockMode.Locked;
        puedeMirar = !activar;
    }
    
    
    
}
