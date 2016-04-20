using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;

public class HeroCreaterPanel : ScriptableWizard {

	public RuntimeAnimatorController animatorController;
	public GameObject skeleton;

	public bool addCollider;

	public GameObject[] parts = new GameObject[1];
	public GameObject[] replaceParts = new GameObject[3];
	public float outline = 0.01f;
	public bool fixNormals = false;

	[MenuItem ("小Q/角色主体文件生成")]
	static void CreateWizard () {

		ScriptableWizard.DisplayWizard<HeroCreaterPanel> ("小Q专用");
		//If you don't want to use the secondary button simply leave it out:
		//如果你不想使用辅助按钮可以忽略它：
		//ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
	}

	void OnWizardCreate () {

		if (skeleton == null) {

			EditorUtility.DisplayDialog("错误警告","必须拖入骨骼文件！！！","OK");

			return;
		}

		if (parts.Length == 0 || parts[0] == null) {

			EditorUtility.DisplayDialog("错误警告","至少有一个皮肤文件！！！","OK");
			
			return;
		}

//		if (animatorController == null) {
//
//			EditorUtility.DisplayDialog("错误警告","必须拖入动作文件！！！","OK");
//
//			return;
//		}

		List<GameObject> importParts = new List<GameObject> ();

		for (int i = 0; i < parts.Length; i++) {

			if(parts[i] != null){

				GameObject tmpObj = GameObject.Instantiate(parts[i]);

				tmpObj.name = parts[i].name;

				importParts.Add(tmpObj);
			}
		}

		List<GameObject> importReplaceParts = new List<GameObject> ();
		
		for (int i = 0; i < replaceParts.Length; i++) {
			
			if(replaceParts[i] != null){

				GameObject tmpObj = GameObject.Instantiate(replaceParts[i]);
				
				tmpObj.name = replaceParts[i].name;

				importReplaceParts.Add(tmpObj);
			}
		}

		Material resultMaterial;
		Mesh resultMesh;

		GameObject resultObj = GameObject.Instantiate (skeleton);

		resultObj.name = skeleton.name;//这行代码不能删  否则骨骼运动就停止了

		GameObjectTools.CombineMeshs (ref resultObj, importParts, importReplaceParts, animatorController, outline, out resultMesh, out resultMaterial, addCollider, fixNormals);

		string path = EditorUtility.SaveFilePanelInProject ("保存文件", "", "prefab", "aaaa");

		if(!string.IsNullOrEmpty(path)){

			int start = path.LastIndexOf ("/");
			int end = path.LastIndexOf (".");
			string saveName = path.Substring (start + 1, end - start - 1);
			
			string qian = path.Substring (0, start + 1);
			
			AssetDatabase.CreateAsset (resultMesh, qian + saveName + "_mesh.asset");
			
			AssetDatabase.CreateAsset (resultMaterial, qian + saveName + "_mat.mat");
			
			PrefabUtility.CreatePrefab (qian + saveName + ".prefab", resultObj);

			AssetBundleTools.SetAssetBundleName(qian + saveName + ".prefab",saveName);
		}

		for (int i = 0; i < importParts.Count; i++) {

			GameObject.DestroyImmediate (importParts [i]);
		}

		for (int i = 0; i < importReplaceParts.Count; i++) {
			
			GameObject.DestroyImmediate (importReplaceParts [i]);
		}
		
		GameObject.DestroyImmediate (resultObj);
	}
}
