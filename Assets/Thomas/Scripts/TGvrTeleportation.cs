using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TGvrTeleportation: MonoBehaviour, IPointerClickHandler // IPointerDownHandler, IPointerExitHandler, IPointerUpHandler 
{
    [SerializeField]
    GameObject player;
    public GameObject laserSrouce;
    

    private void Start()
    {
        
    }

    //private void Update()
    //{
    //    var headPos = laserSrouce.transform.position;
    //    var gazeDir = laserSrouce.transform.forward;
    //    RaycastHit hitInfo;

    //    if (Physics.Raycast(headPos, gazeDir, out hitInfo))
    //    {
    //        Debug.Log(hitInfo.point);
    //        //(hitInfo.transform.gameObject.tag == "Interaction")
    //        //{
    //        //    Vector3 deltaPos = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.transform.position.z);
    //        //    Debug.Log(deltaPos);
    //        //    //hitInfo.transform.position = deltaPos;
    //        //}
    //    }
    //}
    public void OnPointerClick(PointerEventData data)
    {
        PointerEventData pointerData = data;
        if (pointerData.pointerCurrentRaycast.gameObject.tag == "Teleport")
        {
            Vector3 worldPos = pointerData.pointerCurrentRaycast.worldPosition;
            Debug.Log(worldPos);
            Vector3 playerPos = new Vector3(worldPos.x, player.transform.position.y, worldPos.z);
            player.transform.position = playerPos;
            
        }
        else
        {
            return;
        }
        
    }

}
