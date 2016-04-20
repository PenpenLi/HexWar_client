using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.assetManager;
using System;

namespace xy3d.tstd.lib.textureFactory{

	public class TextureFactoryUnit<T> :ITextureFactoryUnit where T:UnityEngine.Object {

		private string name;
		private T data;
		private int type = -1;

		private bool isDispose = false;

		private List<Action<T>> callBackList = new List<Action<T>>();

		public TextureFactoryUnit(string _name){
			
			name = _name;
		}

		public T GetTexture(Action<T> _callBack){

			if (type == -1) {
				
				type = 0;
				
				callBackList.Add (_callBack);
				
				AssetManager.Instance.GetAsset<T> (name,GetAsset);

				return default(T);
				
			} else if (type == 0) {
				
				callBackList.Add (_callBack);

				return default(T);
				
			} else {

				if(_callBack != null){

					_callBack(data);
				}

				return data;
			}
		}

		private void GetAsset(T _data){

			if(isDispose){

				Resources.UnloadAsset(_data);

				return;
			}

			data = _data;

			type = 1;

			foreach(Action<T> callBack in callBackList){

				if(callBack != null){

					callBack(data);
				}
			}

			callBackList.Clear();
		}

		public void Dispose(){

			if (type == 1) {
				
				Resources.UnloadAsset(data);

				data = null;

			}else{

				isDispose = true;
			}
		}
	}
}
