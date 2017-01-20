using UnityEngine;

namespace GVR.Input {

	public class BallRotationController : MonoBehaviour {
		
		void LateUpdate() {
			transform.localRotation = GvrController.Orientation;
		}
	}
}
