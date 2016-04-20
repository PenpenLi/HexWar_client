using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superGraphicRaycast;
using xy3d.tstd.lib.superRaycast;

namespace xy3d.tstd.lib.publicTools{

	public class RaycastControlScript : MonoBehaviour {

		public void OpenRaycast(){

            SuperGraphicRaycast.SetIsOpen(true, "RaycastControlScript-OpenRaycast");

            SuperRaycast.SetIsOpen(true, "RaycastControlScript-OpenRaycast");
		}

		public void CloseRaycast(){

            SuperGraphicRaycast.SetIsOpen(false, "RaycastControlScript-CloseRaycast");

            SuperRaycast.SetIsOpen(false, "RaycastControlScript-CloseRaycast");
		}
	}
}