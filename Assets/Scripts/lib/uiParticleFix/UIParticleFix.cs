using UnityEngine;
using System.Collections;

public class UIParticleFix : MonoBehaviour {

	// Use this for initialization
	void Start () {

		ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();

		float scale = transform.lossyScale.x / 0.015625f;
		
		if(scale > 1){
			
			scale = 1;
		}

		renderer.material.SetFloat("_Scaling",scale);
			
		if(renderer.renderMode == ParticleSystemRenderMode.Billboard){

			Camera uiCamera = gameObject.GetComponentInParent<Canvas>().worldCamera;
				
			renderer.material.SetVector("_Center", renderer.gameObject.transform.position);  
			renderer.material.SetMatrix("_Camera", uiCamera.worldToCameraMatrix);  
			renderer.material.SetMatrix("_CameraInv", uiCamera.worldToCameraMatrix.inverse);  
		}
	}
}
