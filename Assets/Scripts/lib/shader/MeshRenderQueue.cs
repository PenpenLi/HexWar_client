using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.shader{

	public class MeshRenderQueue : MonoBehaviour {

		[SerializeField]
		private int renderQueue;

		// Use this for initialization
		void Awake () {
		
			GetComponent<Renderer>().material.renderQueue = renderQueue;

			GameObject.Destroy(this);
		}
	}
}