using UnityEngine;
using System.Collections;

public class AnimationBindCamera : MonoBehaviour {

	private Transform recTrans;

	private Vector3 recPos;

	private Quaternion recRotation;

	public void BindCamera(){

		recTrans = Camera.main.transform.parent;

		recPos = Camera.main.transform.localPosition;

		recRotation = Camera.main.transform.localRotation;

		Camera.main.transform.SetParent (transform, false);

		Camera.main.transform.localPosition = Vector3.zero;

		Camera.main.transform.localRotation = Quaternion.identity;
	}

	public void UnbindCamera(){

		Camera.main.transform.SetParent (recTrans, false);

		Camera.main.transform.localPosition = recPos;

		Camera.main.transform.localRotation = recRotation;
	}
}
