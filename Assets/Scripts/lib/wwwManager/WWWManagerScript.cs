using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.wwwManager
{

	public class WWWManagerScript : MonoBehaviour
	{

#if PLATFORM_ANDROID

		private static string path = Application.streamingAssetsPath + "/";

#elif PLATFORM_IOS

		private static string path = "file:" + Application.streamingAssetsPath + "/";

#else 

		private static string path = "file:///" + Application.streamingAssetsPath + "/";

#endif

		private WWWManager.fixUrlDelegate fixUrlDelegate;

		private Action loadOverCallBack;

		public void SetLoadOverCallBack(Action _callBack){

			loadOverCallBack = _callBack;
		}

		public void SetUrlFixFun (WWWManager.fixUrlDelegate _callBack)
		{
			fixUrlDelegate = _callBack;
		}

		public void Load (string _path, bool _isRemote, Action<WWW> _callBack, float _timeout, Action _timeoutCallBack)
		{
//			Action ddd = delegate() {
//
//				StartCoroutine (LoadCorotine (_path, _isRemote, _callBack));
//			};
//
//			SuperTween.Instance.DelayCall(4,ddd);

			StartCoroutine (LoadCorotine (_path, _isRemote, _callBack, _timeout, _timeoutCallBack));
		}

		private IEnumerator LoadCorotine (string _path, bool _isRemote, Action<WWW> _callBack, float _timeout, Action _timeoutCallBack)
		{
			string finalPath;

			if (!_isRemote) {

				if (fixUrlDelegate == null) {

					finalPath = path + _path;

				} else {

					bool b = fixUrlDelegate (ref _path);

					if (b) {

						finalPath = _path;

					} else {

						finalPath = path + _path;
					}
				}

			} else {

				finalPath = _path;
			}

			if(_timeout == 0){

				using(WWW www = new WWW (finalPath)){

					yield return www;

		//			SuperDebug.Log ("资源加载成功:" + _path);

					if (www.error != null) {

						SuperDebug.Log ("WWW download had an error:" + www.error + "  finalPath:" + finalPath);
					}

					_callBack (www);
				}

			}else{

				using(WWW www = new WWW (finalPath)){

					float timer = 0.0f; 

					bool failed = false;

					while(!www.isDone){

						if(timer > _timeout){

							failed = true;

							break;
						}
						
						timer += Time.unscaledDeltaTime;

						yield return null;
					}

					if(failed){

						_timeoutCallBack();

					}else{

						_callBack (www);
					}
				}
			}
			
			loadOverCallBack();
		}
	}
}