using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.superTween{

	public class SuperTween{

		private static SuperTween _Instance;

		public static SuperTween Instance{

			get{

				if(_Instance == null){

					_Instance = new SuperTween();
				}

				return _Instance;
			}
		}

		private GameObject go;

		private SuperTweenScript script;

		public SuperTween(){

			go = new GameObject("SuperTweenGameObject");

			GameObject.DontDestroyOnLoad(go);

			script = go.AddComponent<SuperTweenScript>();
		}

		public int To(float _startValue,float _endValue,float _time,Action<float> _delegate,Action _endCallBack){

			return script.To(_startValue,_endValue,_time,_delegate,_endCallBack, false);
		}

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, Action _endCallBack, bool isFixed)
        {
            return script.To(_startValue, _endValue, _time, _delegate, _endCallBack, isFixed);
        }

		public void Remove(int _index){

			script.Remove(_index,false);
		}

		public void Remove(int _index, bool _toEnd){

			script.Remove(_index,_toEnd);
		}

		public void SetTag(int _index,string _tag){

			script.SetTag(_index,_tag);
		}

        public void RemoveAll(bool _toEnd)
        {
			script.RemoveAll(_toEnd);
        }

		public void RemoveWithTag(string _tag, bool _toEnd)
		{
			script.RemoveWithTag(_tag,_toEnd);
		}

		public int DelayCall(float _time,Action _endCallBack){

			return script.DelayCall(_time,_endCallBack);
		}

        public int DelayCall(float _time, Action _endCallBack, bool isFixed)
        {
            return script.DelayCall(_time, _endCallBack, isFixed);
        }

		public int NextFrameCall(Action _endCallBack){

			return script.NextFrameCall(_endCallBack);
		}
	}
}
