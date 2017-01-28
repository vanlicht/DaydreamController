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
        if (Physics.Raycast(headPos, gazeDir, out hitInfo))
        {
            if (hitInfo.transform.gameObject.tag == "SnappingGrid")
            {
                Vector3 deltaPos = hitInfo.point;
                if (isDraggable && GvrController.IsTouching) // should change to clickdown
                {
                    this.transform.position = deltaPos;
                }
                if(GvrController.IsTouching == false)
                {
                    isDraggable = false;
                    GetComponent<Collider>().enabled = true;
                    DestroyImmediate(clone);
                }
            }
        }
    }
}
