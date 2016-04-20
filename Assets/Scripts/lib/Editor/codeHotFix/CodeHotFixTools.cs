using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;

public class CodeHotFixTools{

	public static readonly string shellScene = "shell";
	public static readonly string[] scenes = new string[]{"game","login","main","splash","update"};
	public static readonly string[] ignorePaths = new string[]{"SRSuperDebugger","SRF","ThirdParties","CameraPath3"};

	[MenuItem("安卓代码热更新前的准备/一键！！！")]
	public static void FixAll () {
		
		FixAllPrefabs();
		
		FixAllScene();
		
		EditorApplication.OpenScene("Assets/_Scenes/" + shellScene + ".unity");
	}

	[MenuItem("安卓代码热更新前的准备/修改所有prefab")]
	public static void FixAllPrefabs () {

		string[] strs = AssetDatabase.FindAssets("t:prefab");

		for(int i = 0 ; i < strs.Length ; i++){
			
			string path = AssetDatabase.GUIDToAssetPath(strs[i]);

			bool skip = false;
			
			foreach(string kk in ignorePaths){
				
				if(path.IndexOf(kk) != -1){
					
					skip = true;
					
					break;
				}
			}

			if(!skip){

				GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				bool hasChange = false;

				FixPrefab(go,ref hasChange);
			}
		}

		Debug.Log("所有Prefab转档结束!");
	}

	[MenuItem("安卓代码热更新前的准备/修改选中的prefab")]
	public static void FixSelectedPrefab () {
	
		GameObject go = Selection.activeGameObject;

		bool hasChange = false;

		FixPrefab(go,ref hasChange);
	}

	[MenuItem("安卓代码热更新前的准备/修改所有场景")]
	public static void FixAllScene () {

		foreach(string scenePath in scenes){

			EditorApplication.OpenScene("Assets/_Scenes/" + scenePath + ".unity");

			FixScene();
		}

		Debug.Log("所有场景转档结束!");
	}

	[MenuItem("安卓代码热更新前的准备/修改当前场景")]
	public static void FixScene () {
		
		GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();

		bool _hasChange = false;

		foreach(GameObject go in gos){

			if(go.transform.parent == null){

				FixPrefab(go,ref _hasChange);
			}
		}

		if(_hasChange){
			
			EditorApplication.SaveScene();
		}
	}

	private static void FixPrefab(GameObject _go,ref bool _hasChange){

		AddButtonListener(_go,ref _hasChange);

		FixGo(_go,ref _hasChange,true);
	}

	private static void AddButtonListener(GameObject _go,ref bool _hasChange){

		Button bt = _go.GetComponent<Button>();

		if(bt != null){

			int num = bt.onClick.GetPersistentEventCount();

			List<int> delList = null;

			for(int i = 0 ; i < num ; i++){

				UnityEngine.Object t = bt.onClick.GetPersistentTarget(i);

				string methodName = bt.onClick.GetPersistentMethodName(i);

				if(t is MonoBehaviour){

					if(delList == null){

						delList = new List<int>();
					}

					delList.Add(i);

					MonoBehaviour target = t as MonoBehaviour;
					
					AddButtonListener addButtonListener = target.gameObject.AddComponent<AddButtonListener>();

					addButtonListener.button = bt;

					addButtonListener.scriptName = target.name;

					addButtonListener.methodName = methodName;

					_hasChange = true;
				}
			}

			if(delList != null){

				for(int i = 0 ; i < delList.Count ; i++){

					UnityEventTools.RemovePersistentListener(bt.onClick,delList[i] - i);
				}
			}
		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			AddButtonListener(_go.transform.GetChild(i).gameObject,ref _hasChange);
		}
	}

	private static void FixGo(GameObject _go,ref bool _hasChange,bool _isRoot){

		MonoBehaviour[] b = _go.GetComponents<MonoBehaviour>();

		foreach(MonoBehaviour bb in b){

			if(bb is AddScript || bb is AddButtonListener){

				continue;
			}

			string typeName = bb.GetType().FullName;

			if(typeName.IndexOf("UnityEngine") == -1){

				AddScript ss = _go.AddComponent<AddScript>();

				ss.scriptName = typeName;

				List<AddAtt> list = new List<AddAtt>();

				FieldInfo[] ff = bb.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

				foreach(FieldInfo fff in ff){
					
					object[] yu = fff.GetCustomAttributes(false);
					
					foreach(object ob in yu){
						
						if(ob is SerializeField){

							object vv = fff.GetValue(bb);

							if(vv != null){

								if(vv.GetType().BaseType == typeof(MonoBehaviour)){

									Debug.LogError("SerializeField is a MonoBehaviour! go.name:" + _go.name + "   root.name:" + _go.transform.root.gameObject.name + "   FieldInfo.name:" + fff.Name);
								}

								AddAtt att = null;

								try{

									att = new AddAtt(fff.Name,vv);

								}catch(Exception e){

									throw new Exception("error:" + e.ToString() + "   go:" + _go.name);
								}

								list.Add(att);
							}
						}
					}
				}

				ss.atts = list.ToArray();

				AddButtonListener[] buttonListeners = _go.GetComponents<AddButtonListener>();

				List<AddButtonListener> list2 = new List<AddButtonListener>();

				for(int i = 0 ; i < buttonListeners.Length ; i++){

					if(buttonListeners[i].scriptName == bb.name){

						list2.Add(buttonListeners[i]);
					}
				}

				ss.buttons = new Button[list2.Count];

				ss.buttonMethodNames = new string[list2.Count];

				for(int i = 0 ; i < list2.Count ; i++){

					ss.buttons[i] = list2[i].button;

					ss.buttonMethodNames[i] = list2[i].methodName;

					GameObject.DestroyImmediate(list2[i],true);
				}

				GameObject.DestroyImmediate(bb,true);
				_hasChange = true;
			}
		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			FixGo(_go.transform.GetChild(i).gameObject,ref _hasChange,false);
		}

		if(_isRoot && _hasChange){

			_go.AddComponent<AddScriptRoot>();
		}
	}
}
