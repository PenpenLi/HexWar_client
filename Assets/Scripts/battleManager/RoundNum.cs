using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class RoundNum : MonoBehaviour {

	[SerializeField]
	private Text text;

	[SerializeField]
	private AnimationCurve curve;

	[SerializeField]
	private RectTransform trans;

	[SerializeField]
	private float moveTime = 2;

	private Vector2 startPos;

	private Vector2 endPos;

	private float startTime;

	private Action callBack;

	public void Move(string _str,Action _callBack){

		text.text = _str;

		callBack = _callBack;

		startTime = Time.time;

		gameObject.SetActive (true);

		(transform as RectTransform).anchoredPosition = endPos;
	}

	void Start(){
		
		startPos = new Vector2 (trans.rect.width * 0.5f + (transform as RectTransform).sizeDelta.x * 0.5f, 0);
		
		endPos = new Vector2 (-trans.rect.width * 0.5f - (transform as RectTransform).sizeDelta.x * 0.5f, 0);

		gameObject.SetActive (false);
	}

	void Update(){

		float time = Time.time;

		float percent = (time - startTime) / moveTime;

		if (percent > 1) {

			gameObject.SetActive (false);

			Action tmpCallBack = callBack;

			callBack = null;

			tmpCallBack();

		} else {

			float value = curve.Evaluate (percent);

			(transform as RectTransform).anchoredPosition = Vector2.Lerp (startPos, endPos, value);
		}
	}

	// Use this for initialization

}
