using UnityEngine;
using System.Collections;
using UnityEditor;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.scene;

public class SceneTools  {

	[MenuItem("场景/构造场景Prefab")]
	public static void Start(){

		GameObject go = Selection.activeGameObject;

		GameObject prefab = GameObject.Instantiate(go);

		LightmapData[] datas = LightmapSettings.lightmaps;

		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers){

			if(renderer.lightmapIndex != -1){

				GameObject tg = PublicTools.FindChild(prefab,renderer.gameObject.name);

				LightmapGameObject ll = tg.AddComponent<LightmapGameObject>();

				ll.lightmapIndex = renderer.lightmapIndex;

				ll.lightmapScaleOffset = renderer.lightmapScaleOffset;
			}
		}

		Scene scene = prefab.AddComponent<Scene>();

		scene.farTextures = new Texture2D[datas.Length];
		scene.nearTextures = new Texture2D[datas.Length];
		
		for(int i = 0 ; i < datas.Length ; i++){
			
			scene.farTextures[i] = datas[i].lightmapFar;
			scene.nearTextures[i] = datas[i].lightmapNear;
		}
		
		scene.fieldOfView = Camera.main.fieldOfView;
		
		scene.ambientLight = RenderSettings.ambientLight;
		scene.ambientIntensity = RenderSettings.ambientIntensity;
		
		scene.fog = RenderSettings.fog;
		scene.fogColor = RenderSettings.fogColor;
		scene.fogStartDistance = RenderSettings.fogStartDistance;
		scene.fogEndDistance = RenderSettings.fogEndDistance;
		
		string path = "Assets/Arts/map/" + go.name + ".prefab";

		PrefabUtility.CreatePrefab(path,prefab);

		GameObject.DestroyImmediate(prefab);

		AssetBundleTools.SetAssetBundleName(path,go.name);

		SuperDebug.Log("场景Prefab构造完成!");
	}
}
