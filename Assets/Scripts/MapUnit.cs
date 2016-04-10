﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MapUnit : MonoBehaviour {

	[SerializeField]
	private MeshRenderer mainMr;

	[SerializeField]
	private MeshRenderer offMr;

	[SerializeField]
	private GameObject offGo;

	public int index;

	// Use this for initialization
	void Start () {
	
	}

	public void SetMainColor(Color _color){

		mainMr.material.SetColor ("_Color", _color);
	}

	public void SetOffColor(Color _color){

		offMr.material.SetColor ("_Color", _color);
	}

	public void SetOffVisible(bool _value){

		offGo.SetActive (_value);
	}

	public void OnMouseDown(){

		Debug.Log (index);

		SendMessageUpwards ("MapUnitClick", this, SendMessageOptions.DontRequireReceiver);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
