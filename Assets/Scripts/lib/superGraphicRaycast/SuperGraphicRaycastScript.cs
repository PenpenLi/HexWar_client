using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.superGraphicRaycast{

	public class SuperGraphicRaycastScript : MonoBehaviour {

		private static SuperGraphicRaycastScript _Instance;
		
		public static SuperGraphicRaycastScript Instance{
			
			get{
				
				if(_Instance == null){
					
					
					GameObject go = new GameObject("SuperGraphicRaycastScriptGameObject");
					
					_Instance = go.AddComponent<SuperGraphicRaycastScript>();
				}
				
				return _Instance;
			}
		}
		
		public int isOpen = 1;
		
		public bool filter = false;
		
		public string filterTag;
	}
}