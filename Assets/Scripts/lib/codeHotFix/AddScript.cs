using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Events;

public class AddScript : MonoBehaviour {

	public string scriptName;

	[HideInInspector] public AddAtt[] atts;

	[HideInInspector] public Button[] buttons;

	[HideInInspector] public string[] buttonMethodNames;

	public void Init(){
		
		Type type = AssemblyManager.Instance.assembly.GetType(scriptName);

		Component script = gameObject.AddComponent(type);

		foreach(AddAtt addAtt in atts){

			addAtt.Init(script,type);
		}

		if(scriptName == "xy3d.tstd.lib.effect.Gradient"){

			Outline outline = gameObject.GetComponent<Outline>();

			if(outline != null){

				Color effectColor = outline.effectColor;
				Vector2 effectDistance = outline.effectDistance;
				bool useGraphicAlpha = outline.useGraphicAlpha;

				GameObject.Destroy(outline);

				outline = gameObject.AddComponent<Outline>();

				outline.effectColor = effectColor;
				outline.effectDistance = effectDistance;
				outline.useGraphicAlpha = useGraphicAlpha;
			}

			Shadow shadow = gameObject.GetComponent<Shadow>();

			if(shadow != null){

				Color effectColor = shadow.effectColor;
				Vector2 effectDistance = shadow.effectDistance;
				bool useGraphicAlpha = shadow.useGraphicAlpha;
				
				GameObject.Destroy(shadow);
				
				shadow = gameObject.AddComponent<Shadow>();
				
				shadow.effectColor = effectColor;
				shadow.effectDistance = effectDistance;
				shadow.useGraphicAlpha = useGraphicAlpha;
			}
		}

		if(buttons != null){

			for(int i = 0 ; i < buttons.Length ; i++){

				UnityAction callBack = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction),script,buttonMethodNames[i]);
				
				buttons[i].onClick.AddListener(callBack);
			}
		}

		GameObject.Destroy(this);
	}
}
