using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.audio;
using xy3d.tstd.lib.assetManager;
using UnityEngine.UI;
using xy3d.tstd.lib.superGraphicRaycast;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.screenScale{

	public class ScreenScale : MonoBehaviour {

		private static ScreenScale _Instance;

		public static ScreenScale Instance{

			get{

				if(_Instance == null){

					GameObject go = new GameObject("ScreenScaleGameObject");

					_Instance = go.AddComponent<ScreenScale>();
				}

				return _Instance;
			}
		}

		public const string SCALE_CHANGE = "ScaleChange";

		private float distance = -1;

		// Update is called once per frame
		void Update () {

			if(Input.touchCount > 1){

				if(distance == -1){

					distance = Vector2.Distance(Input.GetTouch(0).position,Input.GetTouch(1).position);

				}else{

					float tmpDistance = Vector2.Distance(Input.GetTouch(0).position,Input.GetTouch(1).position);

					Vector2 pos = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;

					SuperEvent e = new SuperEvent(SCALE_CHANGE);

					e.data = new object[]{tmpDistance / distance,pos};

					SuperFunction.Instance.DispatchEvent(gameObject,e);

					distance = tmpDistance;
				}

			}else if(distance != -1){

				distance = -1;
			}
		}
	}
}