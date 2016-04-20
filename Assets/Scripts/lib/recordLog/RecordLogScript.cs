using UnityEngine;
using System.Collections;

public class RecordLogScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void OnApplicationQuit(){
		
		RecordLog.Stop();
	}
}
