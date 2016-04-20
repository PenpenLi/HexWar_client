using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.assetManager;

using System;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.gameObjectFactory
{

	public class GameObjectFactoryUnit
	{

		private string name;
		private GameObject data;

		private bool dataIsCopy;

		private int type = -1;

		private int _useNum = 0;

		private GameObject preloadedData;

		public int useNum {

			get {

				return _useNum;
			}
		}

		private List<Action<GameObject>> callBackList = new List<Action<GameObject>> ();
		private List<bool> getCopyList = new List<bool> ();
		private List<bool> addUseNumList = new List<bool> ();

		public GameObjectFactoryUnit (string _name)
		{
			name = _name;
		}

		public void PreloadGameObject(Action _callBack){

			Action<GameObject> callBack = delegate(GameObject obj) {

				preloadedData = GameObject.Instantiate<GameObject>(obj);

				preloadedData.SetActive(false);

				_callBack();
			};

			GetGameObject(callBack,false,true);
		}

		public GameObject GetGameObject (Action<GameObject> _callBack, bool _getCopy, bool _addUseNum)
		{
			if (_addUseNum) {

				AddUseNum ();
			}

			if(_getCopy && preloadedData != null){

				DelUseNum();

				GameObject tmpGameObject = preloadedData;
				
				preloadedData = null;

				tmpGameObject.SetActive(true);

				if(_callBack != null){

					_callBack(tmpGameObject);
				}

				return tmpGameObject;
			}

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);
				getCopyList.Add (_getCopy);
				addUseNumList.Add (_addUseNum);

				AssetManager.Instance.GetAsset<GameObject> (name, GetResouece);

				return null;

			} else if (type == 0) {

				callBackList.Add (_callBack);
				getCopyList.Add (_getCopy);
				addUseNumList.Add (_addUseNum);

				return null;

			} else {

				if (_getCopy) {

					GameObject result = GameObject.Instantiate<GameObject> (data);

					FixResult (result,_addUseNum);

					if (_callBack != null) {

						_callBack (result);
					}

					return result;

				} else {

					if (_callBack != null) {

						_callBack (data);
					}

					return data;
				}
			}
		}

		private void GetResouece (GameObject _go)
		{
			data = _go;

			dataIsCopy = false;

			type = 1;

			for (int i = 0; i < callBackList.Count; i++) {

				if (callBackList [i] != null) {

					if (getCopyList [i]) {

						GameObject result = GameObject.Instantiate (data);

						FixResult (result,addUseNumList[i]);

						callBackList [i] (result);

					} else {

						callBackList [i] (data);
					}
				}
			}

			callBackList.Clear ();
			getCopyList.Clear ();
			addUseNumList.Clear ();
		}
		
		public GameObject GetGameObject (string _bodyPath, Action<GameObject> _callBack, string[] _partsPaths, string[] _jointNames, float[] _partsScale, bool _getCopy, bool _addUseNum)
		{
			if (_addUseNum) {

				AddUseNum ();
			}

			if (type == -1) {
				
				type = 0;
				
				callBackList.Add (_callBack);

				getCopyList.Add (_getCopy);

				addUseNumList.Add (_addUseNum);
				
				StartLoad (_bodyPath, _partsPaths, _jointNames, _partsScale);

				return null;
				
			} else if (type == 0) {
				
				callBackList.Add (_callBack);

				getCopyList.Add (_getCopy);
				
				addUseNumList.Add (_addUseNum);

				return null;
				
			} else {

				if(_getCopy){

					GameObject result = GameObject.Instantiate (data);

					result.SetActive (true);

					FixResult (result,_addUseNum);

					if (_callBack != null) {

						_callBack (result);
					}

					return result;

				}else{

					if (_callBack != null) {
						
						_callBack (data);
					}
					
					return data;
				}
			}
		}

		private void StartLoad (string _bodyPath, string[] _partsPaths, string[] _jointNames, float[] _partsScale)
		{
			GameObject body = null;
			GameObject[] parts = new GameObject[_partsPaths.Length];
			
			int loadNum = 3;
			
			Action<GameObject> callBack = delegate(GameObject _go) {
				
				body = _go;
				
				GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
			};
			
			GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, true);

			Action<GameObject[]> callBack2 = delegate(GameObject[] _gameObjects) {

				parts = _gameObjects;

				GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
			};

			GameObjectFactory.Instance.GetGameObjects (_partsPaths, callBack2, false, false);

			GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
		}

		private void GetSomeOfResource (ref int _loadNum, GameObject _body, GameObject[] _parts, string[] _jointNames, float[] _partScales)
		{

			_loadNum--;

			if (_loadNum == 0) {

				GetAllResource (_body, _parts, _jointNames, _partScales);
			}
		}

		private void GetAllResource (GameObject _body, GameObject[] _parts, string[] _jointNames, float[] _partScales)
		{
			data = _body;

			dataIsCopy = true;

			data.name = name;

			GameObjectTools.CombinePart (data, _parts, _jointNames, _partScales);
			
			type = 1;
			
			for (int i = 0; i < callBackList.Count; i++) {

				if (callBackList [i] != null) {

					if(getCopyList[i]){
				
						GameObject result = GameObject.Instantiate (data);

						FixResult (result,addUseNumList[i]);

						callBackList [i] (result);

					}else{

						callBackList [i] (data);
					}
				}
			}

			callBackList.Clear ();

			getCopyList.Clear ();

			addUseNumList.Clear ();

			data.SetActive (false);
		}

		private void FixResult (GameObject _go,bool _addUseNum)
		{
			if (_addUseNum) {

				GameObjectControl gameObjectControl = _go.AddComponent<GameObjectControl> ();

				gameObjectControl.delUseNum = DelUseNum;
			}

			_go.name = name;
		}

		private void AddUseNum ()
		{
			_useNum++;
		}

		private void DelUseNum ()
		{
			_useNum--;
		}

		public void Dispose ()
		{
			if (data != null) {

				if (dataIsCopy) {

					GameObject.Destroy (data);
				}

				data = null;
			}
		}
	}
}