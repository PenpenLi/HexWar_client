using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.UI;
using xy3d.tstd.lib.superList;
using UnityEngine.Events;

public class PrefabCheckTools{

    [MenuItem("Prefab检测/删除多余的CanvasRenderer并检查是否有脚本丢失,按钮是否有空事件")]
	public static void Start()
    {
		string[] strs = AssetDatabase.GetAllAssetPaths();

		strs = AssetDatabase.FindAssets("t:prefab");

		string[] paths = new string[strs.Length];

		for(int i = 0 ; i < strs.Length ; i++){

			paths[i] = AssetDatabase.GUIDToAssetPath(strs[i]);
		}

		foreach(string path in paths){

			bool skip = false;

			foreach(string kk in CodeHotFixTools.ignorePaths){

				if(path.IndexOf(kk) != -1){

					skip = true;

					break;
				}
			}

			if(!skip){

				Check(path);
			}
		}
    }

	private static void Check(string _path){

		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_path);

		Fix(prefab);
	}

	private static void Fix(GameObject _go){

		CanvasRenderer r = _go.GetComponent<CanvasRenderer>();

		if(r != null){

			Image img = _go.GetComponent<Image>();

			Text text = _go.GetComponent<Text>();

			RawImage rawImg = _go.GetComponent<RawImage>();

			if(img == null && text == null && rawImg == null){

				GameObject.DestroyImmediate(r,true);
			}
		}

		MonoBehaviour[] b = _go.GetComponents<MonoBehaviour>();

		foreach(MonoBehaviour m in b){

			if(m == null){

				SuperDebug.LogErrorFormat("发现脚本丢失  root:{0}--->{1}",_go.transform.root.name,_go.name);

				break;
			}
		}

		Button bt = _go.GetComponent<Button>();
		
		if(bt != null){
			
			int num = bt.onClick.GetPersistentEventCount();
			
			for(int i = 0 ; i < num ; i++){
				
				UnityEngine.Object t = bt.onClick.GetPersistentTarget(i);
				
				string methodName = bt.onClick.GetPersistentMethodName(i);
				
				if(!(t is MonoBehaviour)){

					Debug.LogError("Button target gameObject is not a MonoBehaviour!  GameObject.name:" + _go.name + "   root.name:" + _go.transform.root.gameObject.name);

				}else{

					MonoBehaviour script = t as MonoBehaviour;

					MethodInfo mi = script.GetType().GetMethod(methodName);

					if(mi == null){

						Debug.LogError("Button target method is not found in target!  GameObject.name:" + _go.name + "   root.name:" + _go.transform.root.gameObject.name);
					}
				}
			}
		}

//		SuperList superList = _go.GetComponent<SuperList>();
//		
//		if(superList != null){
//
//			Mask mask = _go.GetComponent<Mask>();
//
//			if(mask != null){
//
//				GameObject.DestroyImmediate(mask);
//
//				Image img = _go.GetComponent<Image>();
//
//				GameObject.DestroyImmediate(img);
//
//				_go.AddComponent<RectMask2D>();
//
//				_hasChange = true;
//			}
//		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			Fix(_go.transform.GetChild(i).gameObject);
		}
	}
}
