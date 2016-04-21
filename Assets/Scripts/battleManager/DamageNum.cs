using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DamageNum : MonoBehaviour {

	[SerializeField]
	private Text text;

	[SerializeField]
	private CanvasGroup group;

	[SerializeField]
	private AnimationCurve curve;

	[SerializeField]
	private float moveTime;

	[SerializeField]
	private float height;

	private float startTime;

	private float startY;

	private Action callBack;

	public void Init(int _num,Action _callBack){

		startTime = Time.time;

		text.text = _num.ToString ();

		callBack = _callBack;

		startY = (transform as RectTransform).anchoredPosition.y;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		float time = Time.time;
		
		float percent = (time - startTime) / moveTime;
		
		if (percent > 1) {
			
			GameObject.Destroy(gameObject);
			
			if(callBack != null){

				callBack();
			}
			
		} else {
			
			float value = curve.Evaluate (percent);
			
			(transform as RectTransform).anchoredPosition = new Vector2((transform as RectTransform).anchoredPosition.x,startY + value * height);

			group.alpha = 1 - value;
		}
	}
}
