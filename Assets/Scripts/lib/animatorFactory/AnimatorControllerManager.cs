using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using xy3d.tstd.lib.assetManager;

namespace xy3d.tstd.lib.animatorFactoty{

	public class AnimatorControllerManager : ScriptableObject {

		public static readonly string PATH = "Assets/Arts/animation";

		public static readonly string FILE_NAME = "animators.asset";

		private static AnimatorControllerManager _Instance;

		public static AnimatorControllerManager Instance{

			get{

				return _Instance;
			}
		}

		[SerializeField]
		private string[] names;

		[SerializeField]
		private RuntimeAnimatorController[] animators;

		public static void Init(Action _callBack){

			Action<AnimatorControllerManager> callBack = delegate(AnimatorControllerManager obj) {

				LoadOK (obj,_callBack);
			};

			AssetManager.Instance.GetAsset<AnimatorControllerManager>(PATH + "/" + FILE_NAME,callBack);
		}

		private static void LoadOK(AnimatorControllerManager _asset,Action _callBack){

			_Instance = _asset;

			if(_callBack != null){

				_callBack();
			}
		}

		public void Save(string[] _names,RuntimeAnimatorController[] _animators){

			names = _names;
			animators = _animators;
		}

		public RuntimeAnimatorController GetAnimator(string _name){

			int index = Array.IndexOf(names,_name);

			if(index != -1){

				return animators[index];

			}else{

				return null;
			}
		}
	}
}