#if USE_ASSETBUNDLE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.assetBundleManager;

using System;

namespace xy3d.tstd.lib.assetManager{

	public class AssetManagerUnit<T> : IAssetManagerUnit where T:UnityEngine.Object {

		private AssetManagerData data;
	
		private int type = -1;
	
		private List<Action<T>> callBackList = new List<Action<T>>();
		private string name;

		public AssetManagerUnit(string _name){

			name = _name;

			data = AssetManager.Instance.GetData (name);
		}

		public void Load(Action<T> _callBack){

			callBackList.Add (_callBack);

			if (type == -1) {

				type = 0;

				StartLoad();
			}
		}

		private void StartLoad(){

			int loadNum = 2;

			AssetBundle assetBundle = null;

			Action<AssetBundle> callBack = delegate(AssetBundle _assetBundle) {

				assetBundle = _assetBundle;

				GetAssetBundle(ref loadNum,assetBundle);
			};

			AssetBundleManager.Instance.Load (data.assetBundle, callBack);

			if (data.assetBundleDep != null) {

				callBack = delegate(AssetBundle _assetBundle) {

					GetAssetBundle(ref loadNum,assetBundle);
				};

				foreach(string path in data.assetBundleDep){

					loadNum++;

					AssetBundleManager.Instance.Load (path, callBack);
				}
			}

			GetAssetBundle(ref loadNum,assetBundle);
		}

		private void GetAssetBundle(ref int _loadNum,AssetBundle _assetBundle){

			_loadNum--;

			if (_loadNum == 0) {

				if(AssetManager.LOADASYNC){

					AssetManager.Instance.script.Load<T>(name,_assetBundle,LoadOver);

				}else{

					T asset = null;

					try{

						asset = _assetBundle.LoadAsset<T>(name);

					}catch(Exception e){

						SuperDebug.Log("Asset加载错误:" + e.Message);

					}finally{

						LoadOver(asset);
					}
				}
			}
		}

		private void LoadOver(T _asset){

			AssetBundleManager.Instance.Unload(data.assetBundle);

			if(data.assetBundleDep != null){
			
				foreach(string depName in data.assetBundleDep){
					
					AssetBundleManager.Instance.Unload(depName);
				}
			}
			
			AssetManager.Instance.RemoveUnit(name);
			
			foreach(Action<T> callBack in callBackList){
				
				callBack(_asset);
			}
			
			callBackList.Clear();
		}
	}
}
#endif