using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetInteractPrompt()
    {
        return string.Format($"{itemData.name}");
    }

    public void OnInteract()
    {
        gameObject.SetActive(false);
    }
}
