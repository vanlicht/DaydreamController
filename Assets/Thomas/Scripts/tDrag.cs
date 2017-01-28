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
            Vector3 inistantiatePos = pointerData.pointerCurrentRaycast.gameObject.transform.position;
            clone = (GameObject) Instantiate(quadGrid, inistantiatePos, quadGrid.transform.rotation);
            isDraggable = true;
        }
    }

    void OnDragging()
    {

        int layerMask = 1 << 8;
        RaycastHit hitInfo;
        var headPos = laser.transform.position;
        var gazeDir = laser.transform.forward;

        //Raycast only hit the layer of SnappingGrid
        if (Physics.Raycast(headPos, gazeDir, out hitInfo, Mathf.Infinity, layerMask))
        {
            Vector3 deltaPos = hitInfo.point;
            if (isDraggable && GvrController.ClickButton) // should change to clickdown
            {
                this.transform.position = deltaPos;
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