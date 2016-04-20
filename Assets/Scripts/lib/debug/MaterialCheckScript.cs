using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.textureFactory;
using UnityEngine.UI;
using xy3d.tstd.lib.assetManager;
using System;
using xy3d.tstd.lib.superGraphicRaycast;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.superList;
using System.Collections.Generic;
using xy3d.tstd.lib.superTween;
using System.Reflection;
using xy3d.tstd.lib.superFunction;
using xy3d.tstd.lib.superRaycast;
using System.Threading;

public class MaterialCheckScript : MonoBehaviour {

	void Start(){

	}

	void Update(){

		Check(gameObject);
	}

	private void Check(GameObject _go){

		Image image = _go.GetComponent<Image>();

		if(image != null){

			if(image.material.name.Equals("Default UI Material")){

				Debug.LogError("error!  " + _go.name);
			}
		}

		Text text = _go.GetComponent<Text>();
		
		if(text != null){
			
			if(text.material.name.Equals("Default UI Material")){
				
				Debug.LogError("error!  " + _go.name);
			}
		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			Check(_go.transform.GetChild(i).gameObject);
		}
		 
	}
}
