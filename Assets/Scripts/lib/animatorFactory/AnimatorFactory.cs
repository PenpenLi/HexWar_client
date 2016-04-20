using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.animatorFactoty{

	public class AnimatorFactory {

		private static AnimatorFactory _Instance;
		
		public static AnimatorFactory Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new AnimatorFactory ();
				}
				
				return _Instance;
			}
		}
		
		public Dictionary<string,AnimatorFactoryUnit> dic;
		
		public AnimatorFactory(){
			
			dic = new Dictionary<string, AnimatorFactoryUnit>();
		}

		public RuntimeAnimatorController GetAnimator(string _path,Action<RuntimeAnimatorController> _callBack){

//#if USE_ASSETBUNDLE
//
//			RuntimeAnimatorController result = AnimatorControllerManager.Instance.GetAnimator(_path);
//
//			_callBack(result);
//
//			return result;
//
//#else

			AnimatorFactoryUnit unit;
			
			if (!dic.ContainsKey (_path)) {
				
				unit = new AnimatorFactoryUnit (_path);
				
				dic.Add(_path,unit);
				
			} else {
				
				unit = dic [_path];
			}
			
			return unit.GetAnimator (_path, _callBack);

//#endif
		}

		public void AddUseNum(RuntimeAnimatorController _animatorController){
			
			if (dic.ContainsKey (_animatorController.name)) {
				
				dic [_animatorController.name].AddUseNum ();
			}
		}

		public void DelUseNum(RuntimeAnimatorController _animatorController){

			if (dic.ContainsKey (_animatorController.name)) {

				dic [_animatorController.name].DelUseNum ();
			}
		}

		public void Dispose(bool _force){
			
			List<string> delKeyList = new List<string> ();
			
			foreach (KeyValuePair<String,AnimatorFactoryUnit> pair in dic) {
				
				if (_force || pair.Value.useNum == 0) {
					
					pair.Value.Dispose ();
					
					delKeyList.Add (pair.Key);
				}
			}
			
			for(int i = 0 ; i < delKeyList.Count ; i++){
				
				dic.Remove (delKeyList[i]);
			}
		}
	}
}