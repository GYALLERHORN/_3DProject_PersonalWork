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
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // ȸ������ �ð�ȸ���� +��.
        // �Ʒ� ���� �������� Player������Ʈ�� ��°�� �����̸� �̻��ϴϱ� x�� camȸ���� camContainer���� �ñ��.

        camCurYRot = mouseDelta.x * lookSensitivity;
        transform.eulerAngles += new Vector3(0, camCurYRot, 0);
    }

    public void OnMoveInput(InputAction.CallbackContext context) // �÷��̾� �̵� �Է� : WASD
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

    public void OnLookInput(InputAction.CallbackContext context) // ķ ȸ�� �Է� : Mouse Delta
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnAccelerateInput(InputAction.CallbackContext context) // ���� �Է� : LShift
    {
        if (!canAccel || context.phase == InputActionPhase.Canceled)
        {
            accelRateInput = 1f;
        }
        if (context.phase == InputActionPhase.Performed)
        {
            if (canAccel && curMovementInput.y >= 0f) // LShift���� ä�� �÷��̾� ���� ������ ����
            {
                accelRateInput = 2.0f; // ���� ����
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
