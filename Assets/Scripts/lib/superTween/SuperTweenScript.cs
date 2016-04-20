using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace xy3d.tstd.lib.superTween{

	public class SuperTweenScript : MonoBehaviour {

		private Dictionary<int,SuperTweenUnit> dic = new Dictionary<int, SuperTweenUnit>();

		private Dictionary<Action<float>,SuperTweenUnit> toDic = new Dictionary<Action<float>, SuperTweenUnit>();
		private int index;

		private List<SuperTweenUnit> endList = new List<SuperTweenUnit>();

		public int To(float _startValue,float _endValue,float _time,Action<float> _delegate,Action _endCallBack, bool isFixed){

			lock (dic) {

				if (toDic.ContainsKey (_delegate)) {

					SuperTweenUnit unit = toDic [_delegate];

					unit.Init (unit.index, _startValue, _endValue, _time, _delegate, _endCallBack, isFixed);

					return unit.index;

				} else {

					int result = GetIndex ();

					SuperTweenUnit unit = new SuperTweenUnit ();

					unit.Init (result, _startValue, _endValue, _time, _delegate, _endCallBack, isFixed);

					dic.Add (result, unit);

					toDic.Add (_delegate, unit);

					return result;
				}
			}
		}

		public void SetTag(int _index,string _tag){

			lock (dic) {

				if (dic.ContainsKey (_index)) {
						
					SuperTweenUnit unit = dic [_index];

					unit.tag = _tag;
				}
			}
		}

		public void Remove(int _index, bool _toEnd){

			lock (dic) {
			
				if (dic.ContainsKey (_index)) {

					SuperTweenUnit unit = dic [_index];

					dic.Remove (_index);

					if (unit.dele != null) {
						
						toDic.Remove (unit.dele);
					}

					if (_toEnd) {

						if (unit.dele != null) {
							unit.dele (unit.endValue);
						}
						
						if (unit.endCallBack != null) {
							
							unit.endCallBack ();
						}
					}

					unit.isRemoved = true;
				}
			}
		}

        public void RemoveAll(bool _toEnd)
        {
			lock (dic) {

				Dictionary<int,SuperTweenUnit> tmpDic = dic;

				dic = new Dictionary<int, SuperTweenUnit> ();
				toDic = new Dictionary<Action<float>, SuperTweenUnit> ();

				Dictionary<int,SuperTweenUnit>.ValueCollection.Enumerator enumerator = tmpDic.Values.GetEnumerator ();

				while (enumerator.MoveNext()) {

					SuperTweenUnit unit = enumerator.Current;
				
					if (_toEnd) {
						if (unit.dele != null) {
							unit.dele (unit.endValue);
						}

						if (unit.endCallBack != null) {
							
							unit.endCallBack ();
						}
					}

					unit.isRemoved = true;
				}
			}
        }

		public void RemoveWithTag(string _tag, bool _toEnd)
		{
			lock (dic) {

				List<SuperTweenUnit> list = new List<SuperTweenUnit> ();

				Dictionary<int,SuperTweenUnit>.ValueCollection.Enumerator enumerator = dic.Values.GetEnumerator ();

				while (enumerator.MoveNext()) {

					SuperTweenUnit unit = enumerator.Current;

					if (unit.tag != null && unit.tag.Equals (_tag)) {
						list.Add (unit);
					}
				}

				for (int i = 0; i < list.Count; i++) {

					SuperTweenUnit unit = list [i];

					dic.Remove (unit.index);

					if (unit.dele != null) {
						
						toDic.Remove (unit.dele);
					}

					if (_toEnd) {
						
						if (unit.dele != null) {
							unit.dele (unit.endValue);
						}
						
						if (unit.endCallBack != null) {
							
							unit.endCallBack ();
						}
					}
					
					unit.isRemoved = true;
				}
			}
		}

		public int DelayCall(float _time,Action _endCallBack){

			lock (dic) {

				int result = GetIndex ();

				SuperTweenUnit unit = new SuperTweenUnit ();

				unit.Init (result, 0, 0, _time, null, _endCallBack, false);

				dic.Add (result, unit);

				return result;
			}
		}

        public int DelayCall(float _time, Action _endCallBack, bool isFixed)
        {
			lock (dic) {

				int result = GetIndex ();

				SuperTweenUnit unit = new SuperTweenUnit ();

				unit.Init (result, 0, 0, _time, null, _endCallBack, isFixed);

				dic.Add (result, unit);

				return result;
			}
        }

		public int NextFrameCall(Action _endCallBack){

			lock (dic) {

				int result = GetIndex ();
					
				SuperTweenUnit unit = new SuperTweenUnit ();

				unit.Init (result, 0, 0, 0, null, _endCallBack, false);
					
				dic.Add (result, unit);
					
				return result;
			}
		}

		// Update is called once per frame
		void Update () {

			lock (dic) {

				if (dic.Count > 0) {
					
					float nowTime = Time.time;
					
					float nowUnscaleTime = Time.unscaledTime;

					Dictionary<int,SuperTweenUnit>.Enumerator enumerator = dic.GetEnumerator ();

					while (enumerator.MoveNext()) {

						SuperTweenUnit unit = enumerator.Current.Value;

						float tempTime = 0;

						if (unit.isFixed) {
							tempTime = nowUnscaleTime;
						} else {
							tempTime = nowTime;
						}

						if (tempTime > unit.startTime + unit.time) {
							if (unit.dele != null) {

								unit.dele (unit.endValue);

								toDic.Remove (unit.dele);
							}

							endList.Add (unit);

						} else if (unit.dele != null) {

							float value = unit.startValue + (unit.endValue - unit.startValue) * (tempTime - unit.startTime) / unit.time;
							unit.dele (value);
						}
					}

					if (endList.Count > 0) {

						for (int i = 0; i < endList.Count; i ++) {

							SuperTweenUnit unit = endList [i];

							dic.Remove (unit.index);

							if (!unit.isRemoved && unit.endCallBack != null) {
								
								unit.endCallBack ();
							}
						}
						
						endList.Clear ();
					}
				}
			}
		}

		private int GetIndex(){

			int result = index;

			index++;

			return result;
		}
	}
}