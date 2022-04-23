using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSpotClickDetect : MonoBehaviour
{
    void OnMouseDown()
    {
        // Avoid click in UI and spot at same time
        if (EventSystem.current.IsPointerOverGameObject()) return;
            
        GameManager.Instance.InvokeOnClickBuildingSpot(gameObject);
    }
}
