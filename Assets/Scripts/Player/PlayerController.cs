using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    [Range(1f, 10f)]
    public float accelRateInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    private float camCurYRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    [HideInInspector]
    public bool canAccel = true;
    private bool isFlashOn = false;

    [SerializeField]
    private GameObject flashLight;
    private Rigidbody _rigidbody;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    private void Move()
    {
        Vector3 direction = (transform.forward * curMovementInput.y * accelRateInput + transform.right * curMovementInput.x) * Time.fixedDeltaTime;
        direction *= moveSpeed;
        direction.y = _rigidbody.velocity.y;

        _rigidbody.velocity = direction;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 회전값은 시계회전이 +다.
        // 아래 위를 볼때마다 Player오브젝트가 통째로 움직이면 이상하니까 x축 cam회전은 camContainer에게 맡긴다.

        camCurYRot = mouseDelta.x * lookSensitivity;
        transform.eulerAngles += new Vector3(0, camCurYRot, 0);
    }

    public void OnMoveInput(InputAction.CallbackContext context) // 플레이어 이동 입력 : WASD
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) // 캠 회전 입력 : Mouse Delta
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnAccelerateInput(InputAction.CallbackContext context) // 가속 입력 : LShift
    {
        if (!canAccel || context.phase == InputActionPhase.Canceled)
        {
            accelRateInput = 1f;
        }
        if (context.phase == InputActionPhase.Performed)
        {
            if (canAccel && curMovementInput.y >= 0f) // LShift누른 채로 플레이어 전진 상태일 때만
            {
                accelRateInput = 2.0f; // 가속 적용
            }
        }
    }

    public void OnFlashLightInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isFlashOn)
            {
                flashLight.SetActive(true);
                isFlashOn = true;
            }
            else
            {
                flashLight.SetActive(false);
                isFlashOn = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), 0.3f);
    }
}
