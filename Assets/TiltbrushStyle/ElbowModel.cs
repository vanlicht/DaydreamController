using UnityEngine;

public class ElbowModel : MonoBehaviour {
	Vector3 origin;
	private Vector3 addoffset;

	void Start() {
		origin = gameObject.transform.position;
		addoffset = new Vector3 (0.25f, 0f, 0f);
	}

	void Update() {
		//var yaw = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
		var pitch = Quaternion.AngleAxis(transform.rotation.eulerAngles.x, Vector3.right);
		gameObject.transform.position = origin + pitch * Vector3.right * addoffset.x;
	}
}