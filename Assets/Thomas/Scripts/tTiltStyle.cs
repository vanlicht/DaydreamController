using UnityEngine;
using System.Collections;
using System;

public class tTiltStyle : MonoBehaviour
{
    public GameObject cubePanel;

    bool isTrackingTouches;
    bool isScrolling;
    private Vector2 initialTouchPos;
    private Vector2 previousTouchPos;
    private float previousTouchTimestamp;
    private Vector2 overallVelocity;
    private float kTimestampDeltaThreshold;
    private const float kCuttoffHz = 10.0f;
    private const float kRc = (float)(1.0 / (2.0 * Mathf.PI * kCuttoffHz));

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    //Check if touch tracking
        if(!isTrackingTouches && GvrController.IsTouching)
        {
            Debug.Log("!isTrackingTouches + IsTouching");
            StartTouchTracking();
        }
        
        if (GvrController.TouchUp)
        {
            Debug.Log("TouchUp");
            StopTouchTracking();
        }

        if (isTrackingTouches && GvrController.IsTouching)
        {
            Debug.Log("isTrackingTouches + IsTouching");
            TrackTouch();
        }

    }

    private void TrackTouch()
    {
        if (!isTrackingTouches)
        {
            Debug.LogWarning("StartTouchTracking must be called before touches can be tracked.");
            return;
        }

        float timeElapsedSeconds = (Time.time - previousTouchTimestamp);

        // If the timestamp has not changed, do not update.
        if (timeElapsedSeconds < kTimestampDeltaThreshold)
        {
            return;
        }

        // Update velocity
        Vector2 touchDelta = GvrController.TouchPos - previousTouchPos;
        Vector2 velocity = touchDelta / timeElapsedSeconds;
        float weight = timeElapsedSeconds / (kRc + timeElapsedSeconds);
        overallVelocity = Vector2.Lerp(overallVelocity, velocity, weight);

        Debug.Log("T: velocity: " + velocity);
        //Debug.Log("T: weight: " + weight);

        //Update the previous touch
        previousTouchPos = GvrController.TouchPos;
        previousTouchTimestamp = Time.time;
    }

    private void StartTouchTracking()
    {
        isTrackingTouches = true;
        initialTouchPos = GvrController.TouchPos;
        previousTouchPos = initialTouchPos;
        previousTouchTimestamp = Time.time;
        overallVelocity = Vector2.zero;

    }

    private void StopTouchTracking()
    {
        isTrackingTouches = false;
        
        initialTouchPos = Vector2.zero;
        previousTouchPos = Vector2.zero;
        previousTouchTimestamp = 0f;
        overallVelocity = Vector2.zero;
    }
}
