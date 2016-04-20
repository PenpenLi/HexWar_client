using UnityEngine;
using System.Collections;
using System;

public class AssemblyManagerDownLoader : MonoBehaviour {

	public void Init(string _path,Action<byte[]> _callBack){

		SuperDebug.Log("加载代码文件:" + _path);

		StartCoroutine(LoadDll(_path,_callBack));
	}

	IEnumerator LoadDll(string _path,Action<byte[]> _callBack){

		WWW www = new WWW(_path);

		yield return www;

		if(string.IsNullOrEmpty(www.error)){

			_callBack(www.bytes);

		}else{

			SuperDebug.Log(www.error);

			_callBack(null);
		}
	}
}
