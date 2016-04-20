using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;
using System;
using System.Collections.Generic;

namespace xy3d.tstd.lib.superRaycast{

	public class SuperRaycast : MonoBehaviour {

		public const string GetMouseButtonDown = "GetMouseButtonDown";
		public const string GetMouseButton = "GetMouseButton";
		public const string GetMouseButtonUp = "GetMouseButtonUp";
		public const string GetMouseEnter = "GetMouseEnter";
		public const string GetMouseExit = "GetMouseExit";

		private const string CHECK_LAYER_NAME = "Default";

		private static SuperRaycast _Instance;

		private Dictionary<string,int> dic = new Dictionary<string, int>();

		public static void Init(){

			GameObject go = new GameObject("SuperRaycastGameObject");
			
			_Instance = go.AddComponent<SuperRaycast>();
		}

		public static void SetIsOpen(bool _isOpen, string _str){
			if(_Instance == null){
				//SuperDebug.Log("SuperRaycast _Instance is null!  value:" + _isOpen + "   str:" + _str + "    " + _Instance);
				return;
			}

			//SuperDebug.Log("SuperRaycastReal  value:" + _isOpen + "   str:" + _str + "    " + _Instance);
			
			_Instance.m_isOpen = _Instance.m_isOpen + (_isOpen ? 1 : -1);
			
			if(_Instance.m_isOpen == 0){
				
				if(!_Instance.isProcessingUpdate){
					
					_Instance.objs.Clear();
					
				}else{
					
					_Instance.needClearObjs = true;
				}
				
			}else if(_Instance.m_isOpen > 1){

				SuperDebug.Log("SuperRaycast error!!!!!!!!!!!!!");

				foreach(KeyValuePair<string,int> pair in _Instance.dic){

					SuperDebug.Log("key:" + pair.Key + "  value:" + pair.Value);
				}
				
				SuperDebug.LogError("3D SuperRaycast SetOpen error!");
			}

			if(_Instance.dic.ContainsKey(_str)){

				if(_isOpen){

					_Instance.dic[_str]++;

				}else{

					_Instance.dic[_str]--;
				}

				if(_Instance.dic[_str] == 0){

					_Instance.dic.Remove(_str);
				}

			}else{

				if(_isOpen){

					_Instance.dic.Add(_str,1);

				}else{

					_Instance.dic.Add(_str,-1);
				}
			}
		}

		public static string filterTag{

			get{

				if(_Instance == null){
					
					return null;
					
				}else{
					
					return _Instance.m_filterTag;
				}
			}

			set{

				if(_Instance == null){
					
					return;

				}else{

					_Instance.m_filterTag = value;
				}
			}
		}

		public static bool filter{

			get{

				if(_Instance == null){
					
					return false;
					
				}else{
					
					return _Instance.m_filter;
				}
			}

			set{

				if(_Instance == null){
					
					return;
					
				}else{
					
					_Instance.m_filter = value;
				}
			}
		}

		void Awake(){

			layerIndex = 1 << LayerMask.NameToLayer(CHECK_LAYER_NAME);
		}
		
		private int layerIndex;

		private int m_isOpen = 0;

		private bool m_filter = false;
		
		private string m_filterTag;

		private List<GameObject> objs = new List<GameObject>();

		private bool isProcessingUpdate = false;

		private bool needClearObjs = false;

		void Update(){
			
			if(m_isOpen > 0){

				isProcessingUpdate = true;

				RaycastHit[] hits = null;

				if(Input.GetMouseButtonDown(0)){

					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);

					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}
						
						objs.Add(hit.collider.gameObject);

						SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
							
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,enterEvent);
							
						SuperEvent e = new SuperEvent(GetMouseButtonDown);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}

				}

				if(Input.GetMouseButton(0)){

					if(hits == null){

						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						
						hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);
					}
					
					List<GameObject> newObjs = new List<GameObject>();

					List<GameObject> enterObjs = new List<GameObject>();
					
					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}
						
						newObjs.Add(hit.collider.gameObject);
						
						if(!objs.Contains(hit.collider.gameObject)){

							enterObjs.Add(hit.collider.gameObject);
							
//							SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
//							
//							SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,enterEvent);
							
						}else{
							
							objs.Remove(hit.collider.gameObject);
						}
						
						SuperEvent e = new SuperEvent(GetMouseButton);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}
					
					foreach(GameObject go in objs){
						
						SuperEvent exitEvent = new SuperEvent(GetMouseExit);
						
						SuperFunction.Instance.DispatchEvent(go,exitEvent);
					}
					
					objs = newObjs;

					foreach(GameObject go in enterObjs){
						
						SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
						
						SuperFunction.Instance.DispatchEvent(go,enterEvent);
					}

				}

				if(Input.GetMouseButtonUp(0)){

					if(hits == null){

						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						
						hits = Physics.RaycastAll(ray,float.MaxValue,layerIndex);
					}

					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(m_filter && !hit.collider.gameObject.CompareTag(m_filterTag)){
							
							continue;
						}

						SuperEvent e = new SuperEvent(GetMouseButtonUp);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}
					
//					foreach(GameObject go in objs){
//						
//						SuperEvent exitEvent = new SuperEvent(GetMouseExit);
//						
//						SuperFunction.Instance.DispatchEvent(go,exitEvent);
//					}
					
					objs.Clear();
				}

				if(needClearObjs){

					needClearObjs = false;

					objs.Clear();
				}
				
				isProcessingUpdate = false;
			}
		}
	}
}
