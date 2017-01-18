using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class tTouchTracking : MonoBehaviour
{
    #region Public Fields
    [Tooltip("The sensitivity to gvr touch events.")] /// Allows you to control how sensitive the paged scroll rect is to events from the gvr controller.
    public float ScrollSensitivity = 1.0f;

    [Tooltip("The speed that the rect snaps to a page.")] /// The speed that the scroll rect snaps to a page when the gvr touchpad is released.
    public float SnapSpeed = 6.0f;

    [Tooltip("Determines whether the user must be pointing at the scroll rect with the controller to be able to scroll.")] /// If true, the user must be pointing at the scroll rect with the controller to be able to scroll.
    public bool onlyScrollWhenPointing = true;

    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    //T: later can use e.g. OnSwipeLeft.Invoke(); to invoke/ start the selected script in the editor (similar to Event trigger on the Button)
    #endregion

    #region Private Fields
    /// Keep track of the last few frames of touch positions, and the initial position
    private bool isTrackingTouches = false;
    private Vector2 initialTouchPos;
    private Vector2 previousTouchPos;
    private float previousTouchTimestamp;
    private Vector2 overallVelocity;

    private bool canScroll = false;
    private bool isScrolling = false;
    private float scrollOffset = float.MaxValue;

    /// Touch Delta is required to be higher than the click threshold to avoid detecting clicks as swipes.
	private const float kClickThreshold = 0.125f;

    /// overallVelocity must be greater than the swipe threshold to detect a swipe.
	private const float kSwipeThreshold = 0.75f;

    /// The difference between two timestamps must be greater than this value to be considered different. Helps reduce noise.
	private const float kTimestampDeltaThreshold = 1.0e-7f;

    /// If the difference between the target scroll offset
	/// and the current scroll offset is greater than the moving threshold,
	/// then we are considered to be moving. This coeff is multiplied by the spacing
	/// to get the moving threshold.
	private const float kIsMovingThresholdCoeff = 0.1f;

    /// Values used for low-pass-filter to improve the accuracy of our tracked velocity.
    private const float kCuttoffHz = 10.0f;
    private const float kRc = (float)(1.0 / (2.0 * Mathf.PI * kCuttoffHz));

    private enum SnapDirection
    {
        Left,
        Right,
        Closest
    }
    #endregion



    #region Public Parameters
    /// Returns true if scrolling is currently allowed
    public bool CanScroll
    {
        get
        {
            return canScroll;
        }
        set
        {
            if (canScroll == value)
            {
                return;
            }

            canScroll = value;

            if (!canScroll)
            {
                StopScrolling();
                StopTouchTracking();
            }
        }
    }

    private void StopTouchTracking()
    {
        throw new NotImplementedException();
    }

    private void StopScrolling()
    {
        if (!isScrolling){
            return;
        }
        if(overallVelocity.x > kSwipeThreshold)
        {
            /// If I was swiping to the right.
            SnapToPageInDirection(SnapDirection.Left);
        }else if(overallVelocity.x < -kSwipeThreshold)
        {
            SnapToPageInDirection(SnapDirection.Right);
        }
        else
        {
            SnapToPageInDirection(SnapDirection.Closest);
        }
        isScrolling = false;
    }

    private void SnapToPageInDirection(SnapDirection left)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Main Methods
    void Update()
    {
        if (!CanScroll)
        {
            return;
        }

        if(!isScrolling & GvrController.IsTouching)
        {
            if (!isTrackingTouches)
            {
                StartTouchTracking();
            }
            else
            {
                Vector2 touchDelta = GvrController.TouchPos - initialTouchPos;
                float xDeltaMagnitude = Mathf.Abs(touchDelta.x);
                float yDeltaMagnitude = Mathf.Abs(touchDelta.y);

                if (xDeltaMagnitude > kClickThreshold && xDeltaMagnitude > yDeltaMagnitude)
                {
                    StartScrolling();
                }
            }
            
        }
    }

    private void StartTouchTracking()
    {
        throw new NotImplementedException();
    }

    private void StartScrolling()
    {
        if (isScrolling)
        {
            return;
        }


    }
    #endregion
}
