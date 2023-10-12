using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.5f;
    private float lastCheckTime;
    [SerializeField]
    private float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    public bool foundDestination = true;

    public static InteractionManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate) // Update메서드에서 일정한 시간 간격을 두고 실행
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 화면 중앙에 ray발사
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format($"Press [E] {curInteractable.GetInteractPrompt()}");
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            if (curInteractGameObject.CompareTag("Destination"))
            {
                GameManager.instance.GameEnd();
            }
            curInteractable.OnInteract();
            curInteractable = null;
            curInteractGameObject = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
