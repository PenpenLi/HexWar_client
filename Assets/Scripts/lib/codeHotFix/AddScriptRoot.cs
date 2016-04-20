using UnityEngine;
using System.Collections;

public class AddScriptRoot : MonoBehaviour {

	// Use this for initialization
	void Awake () {

		gameObject.SetActive(false);

		FixGo(transform);
		
		gameObject.SetActive(true);

		GameObject.Destroy(this);
	}
	
	private void FixGo(Transform _go){

		AddScript[] scripts = _go.GetComponents<AddScript>();

		for(int i = 0 ; i < scripts.Length ; i++){

			scripts[i].Init();
		}
		
		for(int i = 0 ; i < _go.childCount ; i++){

			FixGo(_go.transform.GetChild(i));
		}
	}
}
