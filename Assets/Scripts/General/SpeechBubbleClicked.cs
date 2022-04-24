using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeechBubbleClicked : MonoBehaviour
{
    public GameManager.SpeechBubbleType BubbleType;
    void OnMouseDown()
    {
        // Avoid click in UI and speechbubble at same time
        if (EventSystem.current.IsPointerOverGameObject()) return;

        GameManager.Instance.InvokeOnClickSpeechBubble(gameObject, BubbleType);
    }
}
