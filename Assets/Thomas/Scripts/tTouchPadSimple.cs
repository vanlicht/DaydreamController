using UnityEngine;
using System.Collections;

public class tTouchPadSimple : MonoBehaviour
{
    #region Private Fields
    bool isScrolling;
    bool isTrackingTouches;
    #endregion
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!isScrolling && GvrController.IsTouching)
        {
            //Start tracking
        }
        
        //Debug.Log(GvrController.IsTouching);
        //Debug.Log(GvrController.TouchPos);
	}
}
