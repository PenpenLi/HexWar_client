using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.wwwManager{

	public class WWWManager{

		private struct WWWRequest{

			public string path;
			public bool isRemote;
			public Action<WWW> callBack;
			public float timeout;
			public Action timeoutCallBack;

			public WWWRequest(string _path,bool _isRemote,Action<WWW> _callBack,float _timeout,Action _timeoutCallBack){

				path = _path;
				isRemote = _isRemote;
				callBack = _callBack;
				timeout = _timeout;
				timeoutCallBack = _timeoutCallBack;
			}
		}

		private static WWWManager _Instance;
		
		public static WWWManager Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new WWWManager ();
				}
				
				return _Instance;
			}
		}

		private GameObject go;
		private WWWManagerScript script;

		private List<WWWRequest> list = new List<WWWRequest>();

		private const int maxLoadNum = 10;
		private int loadNum = 0;

		public delegate bool fixUrlDelegate(ref string _path);

		public WWWManager(){

			go = new GameObject();

			go.name = "WWWManagerGameObject";

			GameObject.DontDestroyOnLoad(go);

			script = go.AddComponent<WWWManagerScript>();

			script.SetLoadOverCallBack(OneLoadOK);
		}

		public void SetUrlFixFun(fixUrlDelegate _callBack){

			script.SetUrlFixFun(_callBack);
		}

		public void Load(string _path,Action<WWW> _callBack){

			LoadReal(_path,false,_callBack,0.0f,null);
		}

		public void LoadRemote(string _path,Action<WWW> _callBack){

			LoadReal(_path,true,_callBack,0.0f,null);
		}

		public void LoadRemote(string _path,Action<WWW> _callBack,float _timeout,Action _timeoutCallBack){
			
			LoadReal(_path,true,_callBack,_timeout,_timeoutCallBack);
		}

		private void LoadReal(string _path, bool _isRemote, Action<WWW> _callBack, float _timeout, Action _timeoutCallBack){
			
			if(loadNum < maxLoadNum){
				
				loadNum++;

				script.Load(_path,_isRemote,_callBack,_timeout,_timeoutCallBack);
				
			}else{

				WWWRequest request = new WWWRequest(_path,_isRemote,_callBack,_timeout,_timeoutCallBack);

				list.Add(request);
			}
		}

		private void OneLoadOK(){

//			SuperDebug.Log("一个WWW加载完成了  还剩下" + pathList.Count + "个");

			if(list.Count > 0){

				WWWRequest request = list[0];

				list.RemoveAt(0);

				string path = request.path;
				bool isRemote = request.isRemote;
				Action<WWW> callBack = request.callBack;
				float timeout = request.timeout;
				Action timeoutCallBack = request.timeoutCallBack;

				script.Load(path,isRemote,callBack,timeout,timeoutCallBack);

			}else{

				loadNum--;
			}
		}
	}
}