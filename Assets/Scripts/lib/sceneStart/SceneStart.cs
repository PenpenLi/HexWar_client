using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.assetManager;
using xy3d.tstd.lib.gameObjectFactory;

namespace xy3d.tstd.lib.sceneStart{

	public class SceneStart : MonoBehaviour {

		[SerializeField]
		private string prefabPath;

		// Use this for initialization
		void Awake () {
		
			GameObjectFactory.Instance.GetGameObject(prefabPath,GetGo,true);
		}

		private void GetGo(GameObject _go){

			GameObject.Destroy(gameObject);
		}
	}
}