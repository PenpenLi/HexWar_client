using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;

public class AddOutlinePanel : ScriptableWizard {

	public GameObject obj;

	public float outline = 0.01f;

	public bool fixNormals = false;

	public Texture lightningTexture;
	
	[MenuItem ("小Q/武器转档")]
	static void CreateWizard () {
		
		ScriptableWizard.DisplayWizard<AddOutlinePanel> ("小Q专用");
		//If you don't want to use the secondary button simply leave it out:
		//如果你不想使用辅助按钮可以忽略它：
		//ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
	}
	
	void OnWizardCreate () {

		if (obj == null) {

			EditorUtility.DisplayDialog("错误警告","必须拖入物体","OK");

			return;
		}

		Mesh resultMesh;
		GameObject resultObj;
		Material resultMaterial;

		GameObjectTools.MeshAddOutline(obj,lightningTexture,outline,out resultMesh,out resultObj,out resultMaterial, fixNormals);

		string path = EditorUtility.SaveFilePanelInProject ("保存文件", "", "prefab", "aaaa");

		int start = path.LastIndexOf ("/");
		int end = path.LastIndexOf (".");
		string saveName = path.Substring (start + 1, end - start - 1);

		string qian = path.Substring (0, start + 1);

		AssetDatabase.CreateAsset (resultMesh, qian + saveName + "_mesh.asset");

		AssetDatabase.CreateAsset (resultMaterial, qian + saveName + "_mat.mat");

		PrefabUtility.CreatePrefab (qian + saveName + ".prefab", resultObj);

		AssetBundleTools.SetAssetBundleName(qian + saveName + ".prefab",saveName);

		GameObject.DestroyImmediate (resultObj);
	}
}
