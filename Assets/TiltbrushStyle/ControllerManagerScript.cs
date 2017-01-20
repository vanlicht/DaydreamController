using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ControllerManagerScript : MonoBehaviour {

	public Text hudText;
	public GameObject thumbTouchObj;
	public GameObject uiContainer;
	public GameObject trackPad;

	private Vector3 ballAccel;
	private Vector3 ballLastVelocity;
	private Vector3 initTrackPos;
	private Vector3 trackPadSize;
	private float onePercentTrackPadSize;
	private Vector3 previousThumbPos;
	private bool isAnimating;

	void Start () {
		DOTween.Init();
		isAnimating = false;
		initTrackPos = trackPad.transform.localPosition;
		trackPadSize = trackPad.GetComponent<Renderer> ().bounds.size;
		onePercentTrackPadSize = trackPadSize.x;
		thumbTouchObj.SetActive (false);
	}


	void Update () {
		Quaternion ori = GvrController.Orientation;
		gameObject.transform.localRotation = ori;

		Vector3 v = GvrController.Orientation * Vector3.forward;

		//

		if (GvrController.IsTouching) {
			thumbTouchObj.SetActive (true);
			Vector2 touchPos = GvrController.TouchPos;
			float xPos = (touchPos.x * onePercentTrackPadSize) + (initTrackPos.x - (onePercentTrackPadSize / 2.0f));
			float yPos = thumbTouchObj.transform.localPosition.y;
			float zPos = ((1.0f - touchPos.y) * onePercentTrackPadSize) + (initTrackPos.z - (onePercentTrackPadSize / 2.0f));
			thumbTouchObj.transform.localPosition = new Vector3 (xPos, yPos, zPos);

			// Test For Swipe
			float dist = Vector3.Distance(thumbTouchObj.transform.localPosition, previousThumbPos);
			//hudText.text = "x:" + dist + " y:" + trackPadSize.x + " z:" + zPos;

			if (dist > 0.1 && !isAnimating) {
				isAnimating = true;
				//swipe
				float rotAmount;
				if (previousThumbPos.x > xPos) {
					rotAmount = -90.0f;
				} else {
					rotAmount = 90.0f;
				}
				Vector3 currentRotation = uiContainer.transform.localRotation.eulerAngles;
				uiContainer.transform.DOLocalRotate (new Vector3 (0, 0, currentRotation.z + rotAmount), 0.35f).OnComplete (RotAnimationComplete);
			}

			previousThumbPos = thumbTouchObj.transform.localPosition;
		} 
		else {
			thumbTouchObj.SetActive (false);
			previousThumbPos = new Vector3();
		}
	}

	private void RotAnimationComplete () {
		isAnimating = false;
	}
}
