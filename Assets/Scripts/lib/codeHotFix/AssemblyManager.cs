using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.IO;

public class AssemblyManager{

	private static AssemblyManager _Instance;

	public static AssemblyManager Instance{

		get{

			if(_Instance == null){

				_Instance = new AssemblyManager();
			}

			return _Instance;
		}
	}

	public Assembly assembly;

	private AssemblyManagerDownLoader downLoader;

	public void Init(string _path,Action<bool> _callBack){

		if(downLoader == null){

			GameObject go = new GameObject();

			downLoader = go.AddComponent<AssemblyManagerDownLoader>();
		}

		Action<byte[]> del = delegate(byte[] obj) {

			GetBytes(obj,_callBack);
		};

		downLoader.Init(_path,del);
	}

	private void GetBytes(byte[] _bytes,Action<bool> _callBack){

		if(_bytes != null){

			assembly = Assembly.Load(_bytes);

			_callBack(true);

		}else{

			assembly = Assembly.GetCallingAssembly();

			_callBack(false);
		}
	}
}
