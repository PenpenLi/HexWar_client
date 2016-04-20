using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.publicTools;

namespace xy3d.tstd.lib.scene{

	public class Scene : MonoBehaviour {

		[SerializeField] public Texture2D[] farTextures;
		[SerializeField] public Texture2D[] nearTextures;
		
		[SerializeField] public float fieldOfView;
		
		[SerializeField] public Color ambientLight;
		[SerializeField] public float ambientIntensity;
		
		[SerializeField] public bool fog;
		[SerializeField] public Color fogColor;
		[SerializeField] public float fogStartDistance;
		[SerializeField] public float fogEndDistance;

		void OnEnable(){

			LightmapData[] lightmaps = new LightmapData[farTextures.Length];

			for(int i = 0 ; i < lightmaps.Length ; i++){

				lightmaps[i] = new LightmapData();

				lightmaps[i].lightmapFar = farTextures[i];
				lightmaps[i].lightmapNear = nearTextures[i];
			}

			LightmapSettings.lightmaps = lightmaps;

			GameObject cameraGameObject = PublicTools.FindChildForce(gameObject,"CameraGameObject");

			if(Camera.main != null){

				Camera.main.fieldOfView = fieldOfView;

				if(cameraGameObject != null){

					Camera.main.transform.position = cameraGameObject.transform.position;
					Camera.main.transform.rotation = cameraGameObject.transform.rotation;
				}
			}

			RenderSettings.ambientLight = ambientLight;
			RenderSettings.ambientIntensity = ambientIntensity;

			if(fog){

				RenderSettings.fog = true;
				RenderSettings.fogMode = FogMode.Linear;
				RenderSettings.fogColor = fogColor;
				RenderSettings.fogStartDistance = fogStartDistance;
				RenderSettings.fogEndDistance = fogEndDistance;
			}
		}

		void OnDisable(){
			
			LightmapSettings.lightmaps = new LightmapData[0];

			RenderSettings.ambientLight = Color.white;
			RenderSettings.ambientIntensity = 1;

			RenderSettings.fog = false;
		}
	}
}