using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class tDrag : MonoBehaviour, IPointerDownHandler //IPointerClickHandler, 
{
    public GameObject quadGrid;
    public GameObject laser;
    GameObject clone;
    bool isDraggable = false;

    private void Update()
    {
        OnDragging();
    }

    public void OnPointerDown(PointerEventData data)
    {
        PointerEventData pointerData = data;
        if (pointerData.pointerCurrentRaycast.gameObject.tag == "Interaction")
        {
            Debug.Log("interactable Object...");
            pointerData.pointerCurrentRaycast.gameObject.GetComponent<Collider>().enabled = false;
            var targetObject = pointerData.pointerCurrentRaycast.gameObject;
            clone = Instantiate(quadGrid);
            isDraggable = true;
        }
    }

    void OnDragging()
    {
        
        var headPos = laser.transform.position;
        var gazeDir = laser.transform.forward;
        RaycastHit hitInfo;
        int layerMask = LayerMask.GetMask("SnapGridRaycast");
        if (Physics.Raycast(headPos, gazeDir, out hitInfo, layerMask))
        {
            Vector3 deltaPos = hitInfo.point;
            if (isDraggable && GvrController.ClickButton) // should change to clickdown
            {
                if (hitInfo.transform.gameObject.tag == "SnappingGrid")
                {
                    this.transform.position = deltaPos;
                }
                else
                {
                    OnUndraggable();
                }
            }
            else
            {
                OnUndraggable();
            }

            if(GvrController.ClickButtonUp == true ||  GvrController.IsTouching == false)
            {

                OnUndraggable();
            }
        }
        else
        {
            OnUndraggable();
        }
    }

    void OnUndraggable()
    {
        isDraggable = false;
        GetComponent<Collider>().enabled = true;
        DestroyImmediate(clone);
    }
}