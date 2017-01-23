using UnityEngine;
using System.Collections;
using System;

public class tTiltStyle : MonoBehaviour
{
    public GameObject cubePanel;
    public float animTime;
    public AnimationCurve animCurve;

    private bool isScrolling = false;
    private bool isTrackingTouches = false;
    private const float kClickThreshold = 0.125f;
    private const float kSwipeThreshold = 0.75f;
    private Vector2 initialTouchPos;
    private Vector2 previousTouchPos;
    private float previousTouchTimestamp;
    private Vector2 overallVelocity;
    private const float kTimestampDeltaThreshold = 1.0e-7f;
    private const float kCuttoffHz = 10.0f;
    private const float kRc = (float)(1.0 / (2.0 * Mathf.PI * kCuttoffHz));

    private enum SnapDirection
    {
        Left,
        Right,
        Closest
    }


    private void Update()
    {
        if (!isScrolling && GvrController.IsTouching)
        {
            Debug.Log("A1");
            if (!isTrackingTouches)
            {
                Debug.Log("B1");
                StartTouchTracking();
            }
            else
            {
                Debug.Log("B2");
                Vector2 touchDelta = GvrController.TouchPos - initialTouchPos;
                float xDeltaMagnitude = Mathf.Abs(touchDelta.x);
                float yDeltaMagnitude = Mathf.Abs(touchDelta.y);
                //Debug.Log("B2.1 xDeltaMagnitude : kClickThreshold" + xDeltaMagnitude + " : " + kClickThreshold);
                if (xDeltaMagnitude > kClickThreshold && xDeltaMagnitude > yDeltaMagnitude)
                {
                    StartScrolling();
                    Debug.Log("C1");
                }
            }
        }
        if (isScrolling && GvrController.IsTouching)
        {
            Debug.Log("A2");
            Vector2 touchDelta = GvrController.TouchPos - previousTouchPos;
            Debug.Log("A2.1: previousTouchPos : touchDelta.x" + previousTouchPos + touchDelta.x);
            if (Mathf.Abs(touchDelta.x) > 0)
            {
                //float spacingCoeff = -90f;
                //targetScrollOffset += touchDelta.x * spacingCoeff * *ScrollSensitivity;
            }
        }
        if (GvrController.TouchUp)
        {
            Debug.Log("A3");
            StopScrolling();
            StopTouchTracking();
        }
        if (isTrackingTouches && GvrController.IsTouching)
        {
            Debug.Log("A4");
            TrackTouch();
        }

    }

    private void TrackTouch()
    {
        Debug.Log("Z0");
        if (!isTrackingTouches)
        {
            Debug.LogWarning("StartTouchTracking must be called before touches can be tracked.");
            return;
        }
        float timeElapsedSeconds = Time.time - previousTouchTimestamp;
        // If the timestamp has not changed, do not update.
        if (timeElapsedSeconds < kTimestampDeltaThreshold)
        {
            Debug.Log("Z0_1: timestampe has not changed...");
            return;
        }
        // Update velocity
        Vector2 touchDelta = GvrController.TouchPos - previousTouchPos;
        Vector2 velocity = touchDelta / timeElapsedSeconds;
        float weight = timeElapsedSeconds / (kRc + timeElapsedSeconds);
        Debug.Log("Z0_2 overallVelocity: " + overallVelocity);
        overallVelocity = Vector2.Lerp(overallVelocity, velocity, weight);
        Debug.Log("Z0_3 overallVelocity: " + overallVelocity);
        previousTouchPos = GvrController.TouchPos;
        previousTouchTimestamp = Time.time;
    }

    private void StartScrolling()
    {
        Debug.Log("Z1");
        if (isScrolling)
        {
            return;
        }
        //targetScrollOffset = ScrollOffset;

        //if (activeSnapCoroutine != null)
        //{
        //    StopCoroutine(activeSnapCoroutine);
        //}
        isScrolling = true;
    }

    private void StopScrolling()
    {
        Debug.Log("Z2");
        if (!isScrolling)
        {
            return;
        }

        if (overallVelocity.x > kSwipeThreshold)
        {
            Debug.Log("Z2 swipe Right");
            PanelRotate(SnapDirection.Right);
        }
        else if (overallVelocity.x < -kSwipeThreshold)
        {
            Debug.Log("Z2 swipe Left");
            PanelRotate(SnapDirection.Left);
        }
        else
        {
            Debug.Log("Z2 swipe Stay");
        }

        isScrolling = false;
    }

    private void StartTouchTracking()
    {
        Debug.Log("Z3");
        isTrackingTouches = true;
        initialTouchPos = Vector2.zero;
        previousTouchPos = Vector2.zero;
        previousTouchTimestamp = Time.time;
        overallVelocity = Vector2.zero;
    }

    private void StopTouchTracking()
    {
        Debug.Log("Z4");
        isTrackingTouches = false;
        initialTouchPos = Vector2.zero;
        previousTouchPos = Vector2.zero;
        previousTouchTimestamp = 0f;
        overallVelocity = Vector2.zero;
    }


    private IEnumerator PanelRotate(SnapDirection snapDirection)
    {
        float t = 0;
        Vector3 initialRot = cubePanel.transform.localEulerAngles;
        Vector3 rightRot = initialRot + new Vector3(0, 0, -90f);
        Vector3 leftRot = initialRot + new Vector3(0, 0, 90f);


        do
        {
            float curveTime = animCurve.Evaluate(t / animTime);
            if (snapDirection == SnapDirection.Right)
            {
                cubePanel.transform.localEulerAngles = Vector3.Lerp(initialRot, rightRot, curveTime);
            }
            else if (snapDirection == SnapDirection.Left)
            {
                cubePanel.transform.localEulerAngles = Vector3.Lerp(initialRot, leftRot, curveTime);
            }
            else
            {
                yield return null;
            }

            t += Time.deltaTime;
            yield return null;
        } while (t < animTime);
    }
}
