using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

#if UNITY_HAS_GOOGLEVR && (UNITY_ANDROID || UNITY_EDITOR)
public class tTouchPadSimple : MonoBehaviour
{
    #region Fields
    bool isScrolling;
    bool isTrackingTouches;
    private Vector2 initialTouchPos;
    private const float kClickThreshold = 0.125f;
    private Vector2 previousTouchPos;
    private float previousTouchTimestamp;
    public float ScrollSensitivity = 1f;

    //?
    private Vector2 overallVelocity;

    //???
    private float scrollOffset = float.MaxValue;
    private float targetScrollOffset;
    private Coroutine activeSnapCoroutine;
    private IPageProvider pageProvider;
    #endregion

    #region Parameters
    /// Returns the amount that the
    /// rect has been scrolled in local coordinates.
    public float ScrollOffset
    {
        get
        {
            return scrollOffset;
        }
        private set
        {
            if (value != ScrollOffset)
            {
                scrollOffset = value;
                //OnScrolled();
            }
        }
    }
    #endregion

    #region Main Methods
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if isScrolling is OFF
        if (!isScrolling && GvrController.IsTouching)
        {
            //Start tracking: if not tracking touch yet, start
            if (!isTrackingTouches)
            {
                StartTouchTracking();
            }
            else //if already tracking touch
            {
                //query x and y magnitude
                Vector2 touchDelta = GvrController.TouchPos - initialTouchPos;
                float xDeltaMagnitude = Mathf.Abs(touchDelta.x);
                float yDeltaMagnitude = Mathf.Abs(touchDelta.y);

                //start scrolling if conditions met
                if(xDeltaMagnitude > kClickThreshold && xDeltaMagnitude > yDeltaMagnitude)
                {
                    StartScrolling();
                }
            }
        }

        //if isScrolling is ON
        if(isScrolling && GvrController.IsTouching)
        {
            Vector2 touchDelta = GvrController.TouchPos - previousTouchPos;
            if(Mathf.Abs(touchDelta.x) > 0)
            {
                // Translate directly based on the touch value
                float spacingCoeff = -pageProvider.GetSpacing();
                targetScrollOffset += touchDelta.x * spacingCoeff * ScrollSensitivity;
            }
            LerpTowardsOffset(targetScrollOffset);
        }

        
        //Debug.Log(GvrController.IsTouching);
        //Debug.Log(GvrController.TouchPos);
	}

    
    #endregion

    #region Utility Methods
    private void StartTouchTracking()
    {
        isTrackingTouches = true;
        initialTouchPos = GvrController.TouchPos;
        previousTouchPos = initialTouchPos;
        previousTouchTimestamp = Time.time;
        overallVelocity = Vector2.zero;
    }

    private void StartScrolling()
    {
        if (isScrolling)
        {
            return;
        }

        targetScrollOffset = scrollOffset;

        if(activeSnapCoroutine == null)
        {
            StopCoroutine(activeSnapCoroutine);
        }

        isScrolling = true;
    }

    /// Returns false if the ScrollOffset is already the same as the targetOffset.
    private bool LerpTowardsOffset(float targetOffset)
    {
        //if (ScrollOffset == targetOffset)
        //{
        //    return false;
        //}

        //float diff = Mathf.Abs(ScrollOffset - targetScrollOffset);
        //float threshold = pageProvider.GetSpacing() * kSnapScrollOffsetThresholdCoeff;
        //if (diff < threshold)
        //{
        //    ScrollOffset = targetScrollOffset;
        //}
        //else
        //{
        //    ScrollOffset = Mathf.Lerp(ScrollOffset, targetOffset, SnapSpeed * Time.deltaTime);
        //}

        //ScrollOffset = Mathf.Lerp(ScrollOffset, targetOffset, SnapSpeed * Time.deltaTime);
        return true;
    }
    #endregion
}
#endif  // UNITY_HAS_GOOGLEVR &&(UNITY_ANDROID || UNITY_EDITOR