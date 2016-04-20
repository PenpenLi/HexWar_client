using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.superFunction{

	public class SuperFunction{

		private static SuperFunction _Instance;

		public static SuperFunction Instance{

			get{

				if(_Instance == null){

					_Instance = new SuperFunction();
				}

				return _Instance;
			}
		}

		private Dictionary<int,SuperFunctionUnit> dic;
		private Dictionary<GameObject,Dictionary<string,List<SuperFunctionUnit>>> dic2;
//		private int dispatchEventIndex = 0;
//		private List<Action> delegateList = new List<Action>();
		private int index = 0;

		public SuperFunction(){

			dic = new Dictionary<int, SuperFunctionUnit>();
			dic2 = new Dictionary<GameObject,Dictionary<string,List<SuperFunctionUnit>>>();
		}

		public int AddEventListener(GameObject _target,string _eventName,Action<SuperEvent> _callBack){

			int result = GetIndex();

			SuperFunctionUnit unit = new SuperFunctionUnit(_target,_eventName,_callBack,result);

			dic.Add(result,unit);

			Dictionary<string,List<SuperFunctionUnit>> tmpDic;

			if(dic2.ContainsKey(_target)){

				tmpDic = dic2[_target];

			}else{

				_target.AddComponent<SuperFunctionControl>();

				tmpDic = new Dictionary<string,List<SuperFunctionUnit>>();

				dic2.Add(_target,tmpDic);
			}

			List<SuperFunctionUnit> tmpList;

			if(tmpDic.ContainsKey(_eventName)){

				tmpList = tmpDic[_eventName];

			}else{

				tmpList = new List<SuperFunctionUnit>();

				tmpDic.Add(_eventName,tmpList);
			}

			tmpList.Add(unit);

			return result;
		}

		public void RemoveEventListener(int _index){

			if(dic.ContainsKey(_index)){

				SuperFunctionUnit unit = dic[_index];

				dic.Remove(_index);

				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[unit.target];

				List<SuperFunctionUnit> tmpList = tmpDic[unit.eventName];

				tmpList.Remove(unit);

				if(tmpList.Count == 0){

					tmpDic.Remove(unit.eventName);

					if(tmpDic.Count == 0){

						SuperFunctionControl control = unit.target.GetComponent<SuperFunctionControl>();

						control.isDestroy = true;

						GameObject.Destroy(control);

						dic2.Remove(unit.target);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target){

			if(dic2.ContainsKey(_target)){

				SuperFunctionControl control = _target.GetComponent<SuperFunctionControl>();
				
				control.isDestroy = true;
				
				GameObject.Destroy(control);

				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];

				dic2.Remove(_target);

				foreach(List<SuperFunctionUnit> list in tmpDic.Values){

					foreach(SuperFunctionUnit unit in list){

						dic.Remove(unit.index);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target,string _eventName){

			if(dic2.ContainsKey(_target)){
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];

				if(tmpDic.ContainsKey(_eventName)){

					List<SuperFunctionUnit> list = tmpDic[_eventName];

					foreach(SuperFunctionUnit unit in list){
						
						dic.Remove(unit.index);
					}

					tmpDic.Remove(_eventName);

					if(tmpDic.Count == 0){

						SuperFunctionControl control = _target.GetComponent<SuperFunctionControl>();
						
						control.isDestroy = true;
						
						GameObject.Destroy(control);

						dic2.Remove(_target);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target,string _eventName,Action<SuperEvent> _callBack){

			if(dic2.ContainsKey(_target)){
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];
				
				if(tmpDic.ContainsKey(_eventName)){
					
					List<SuperFunctionUnit> list = tmpDic[_eventName];

					for(int i = 0 ; i < list.Count ; i++){

						if(list[i].callBack == _callBack){

							dic.Remove(list[i].index);

							list.RemoveAt(i);

							break;
						}
					}

					if(list.Count == 0){

						tmpDic.Remove(_eventName);

						if(tmpDic.Count == 0){

							SuperFunctionControl control = _target.GetComponent<SuperFunctionControl>();
							
							control.isDestroy = true;
							
							GameObject.Destroy(control);
							
							dic2.Remove(_target);
						}
					}
				}
			}
		}

		public void DispatchEvent(GameObject _target,SuperEvent _event){

			if (dic2.ContainsKey (_target)) {
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];
				
				if(tmpDic.ContainsKey(_event.eventName)){

					List<SuperFunctionUnit> tmpList = tmpDic[_event.eventName];
					
					SuperEvent[] eventList = new SuperEvent[tmpList.Count];

					SuperFunctionUnit[] unitList = new SuperFunctionUnit[tmpList.Count];

					for(int i = 0 ; i < tmpList.Count ; i++){

						SuperFunctionUnit unit = tmpList[i];

						SuperEvent tmpEvent = new SuperEvent(_event.eventName);

						tmpEvent.target = _target;

						tmpEvent.data = _event.data;

						tmpEvent.index = unit.index;

						eventList[i] = tmpEvent;

						unitList[i] = unit;
					}

					for(int i = 0 ; i < eventList.Length ; i++){

						unitList[i].callBack(eventList[i]);
					}
				}
			}
		}

		public void DestroyGameObject(GameObject _target){

			Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];
			
			dic2.Remove(_target);
			
			foreach(List<SuperFunctionUnit> list in tmpDic.Values){
				
				foreach(SuperFunctionUnit unit in list){
					
					dic.Remove(unit.index);
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