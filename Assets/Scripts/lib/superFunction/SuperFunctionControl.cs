using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.superFunction{

	public class SuperFunctionControl : MonoBehaviour {

		public bool isDestroy = false;

		void OnDestroy(){

			if(!isDestroy){

				SuperFunction.Instance.DestroyGameObject(gameObject);
			}
		}
	}
}