using UnityEngine;
using System.Collections;

public class ParticleFix : MonoBehaviour {

	private ParticleSystemRenderer particleRenderer;

	private Camera camera;

	public void Init(Camera _camera){

		camera = _camera;
	}

	void Start(){

		particleRenderer = GetComponent<ParticleSystemRenderer>();
	}

	// Use this for initialization
	void Update () {

		particleRenderer.material.SetVector("_Scaling",transform.localScale);
			
		if(particleRenderer.renderMode == ParticleSystemRenderMode.Billboard || particleRenderer.renderMode == ParticleSystemRenderMode.Stretch){
				
			particleRenderer.material.SetVector("_Center", GetComponent<Renderer>().gameObject.transform.position); 

			if(camera != null){

				particleRenderer.material.SetMatrix("_Camera", camera.worldToCameraMatrix);  
				particleRenderer.material.SetMatrix("_CameraInv", camera.worldToCameraMatrix.inverse);  
			}
		}
	}
}
