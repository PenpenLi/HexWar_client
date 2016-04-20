using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace xy3d.tstd.lib.assetBundleManager{

	public class AssetBundleManager{

		public const string path = "assetbundle/";

		private static AssetBundleManager _Instance;

		public static AssetBundleManager Instance {

			get {

				if (_Instance == null) {

					_Instance = new AssetBundleManager ();
				}

				return _Instance;
			}
		}

		public Dictionary<string,AssetBundleManagerUnit> dic;

		public AssetBundleManager(){

			dic = new Dictionary<string, AssetBundleManagerUnit>();
		}

		public void Load(string _name,Action<AssetBundle> _callBack){

			AssetBundleManagerUnit unit;

			if (!dic.ContainsKey (_name)) {

				unit = new AssetBundleManagerUnit (_name);

				dic.Add (_name, unit);

			} else {

				unit = dic [_name];
			}

			unit.Load (_callBack);
		}

		public void Remove(string _name){

			dic.Remove (_name);
		}

		public void Unload(string _name){

//			SuperDebug.Log ("Unload assetBundle:" + _name);

			dic[_name].Unload();
		}
	}
}
