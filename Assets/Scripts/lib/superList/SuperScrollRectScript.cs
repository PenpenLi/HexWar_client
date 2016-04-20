using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.superList{

	public class SuperScrollRectScript : MonoBehaviour {

		private static SuperScrollRectScript _Instance;

		public static SuperScrollRectScript Instance{

			get{

				if(_Instance == null){

					GameObject go = new GameObject("SuperScrollRectGameObject");

					_Instance = go.AddComponent<SuperScrollRectScript>();
				}

				return _Instance;
			}
		}

		public int canDrag = 1;
	}
}