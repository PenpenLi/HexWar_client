using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AssetManagerScript : MonoBehaviour {

//	private List<Func<bool>> delList = new List<Func<bool>>();

	public void Load<T>(string _name, AssetBundle _assetBundle, Action<T> _callBack)where T:UnityEngine.Object{

//		AssetBundleRequest request = _assetBundle.LoadAssetAsync<T>(_name);
//
//		Func<bool> del = delegate() {
//
//			if(request.isDone){
//
//				T asset = (T)request.asset;
//				
//				_callBack(asset);
//
//				return true;
//
//			}else{
//
//				return false;
//			}
//		};
//
//		delList.Add(del);

		StartCoroutine (LoadCorotine (_name, _assetBundle, _callBack));
	}

//	void Update(){
//
//		for(int i = delList.Count - 1 ; i > -1 ; i--){
//
//			bool result = delList[i]();
//
//			if(result){
//
//				delList.RemoveAt(i);
//			}
//		}
//	}

	private IEnumerator LoadCorotine<T>(string _name, AssetBundle _assetBundle, Action<T> _callBack)where T:UnityEngine.Object{

		AssetBundleRequest request = _assetBundle.LoadAssetAsync<T>(_name);

		yield return request;

		T asset = (T)request.asset;

		_callBack(asset);
	}
}
