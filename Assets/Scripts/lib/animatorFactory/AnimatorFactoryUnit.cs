using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.assetManager;

namespace xy3d.tstd.lib.animatorFactoty{

	public class AnimatorFactoryUnit{

		private string name;
		private RuntimeAnimatorController data;

		private int type = -1;

		private List<Action<RuntimeAnimatorController>> callBackList = new List<Action<RuntimeAnimatorController>> ();

		public int useNum;

		public AnimatorFactoryUnit (string _name)
		{
			name = _name;
		}

		public RuntimeAnimatorController GetAnimator (string _path, Action<RuntimeAnimatorController> _callBack)
		{
			if(type == -1){

				type = 0;

				callBackList.Add(_callBack);

				AssetManager.Instance.GetAsset<RuntimeAnimatorController>(name,GetAsset);

				return null;

			}else if(type == 0){

				callBackList.Add(_callBack);

				return null;

			}else{

				if(_callBack != null){

					_callBack(data);
				}

				return data;
			}
		}

		private void GetAsset(RuntimeAnimatorController _data){

			data = _data;

			data.name = name;

			type = 1;

			for(int i = 0 ; i < callBackList.Count ; i++){

				if(callBackList[i] != null){

					callBackList[i](data);
				}
			}

			callBackList.Clear();
		}

		public void DelUseNum(){

			useNum--;
		}

		public void AddUseNum(){
			
			useNum++;
		}

		public void Dispose(){

			Resources.UnloadAsset (data);
		}
	}
}