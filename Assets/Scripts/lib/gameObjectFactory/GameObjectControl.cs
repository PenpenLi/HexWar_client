using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;
using System;

namespace xy3d.tstd.lib.gameObjectFactory{

	public class GameObjectControl : MonoBehaviour {

		public Action delUseNum;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnDestroy(){

			if (delUseNum != null) {

				delUseNum();
			}
		}
	}
}