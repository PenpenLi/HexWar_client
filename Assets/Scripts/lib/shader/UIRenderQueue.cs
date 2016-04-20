using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace xy3d.tstd.lib.shader{

	public class UIRenderQueue : MonoBehaviour {

		private static Dictionary<string,Material> dic = new Dictionary<string, Material>();

		[SerializeField]
		private int renderQueue;

		[SerializeField]
		private bool isIndependent;

		// Use this for initialization
		void Awake () {
		
			MaskableGraphic graphic = GetComponent<MaskableGraphic>();

			if(isIndependent){

				Material material = Material.Instantiate<Material>(graphic.material);

				material.renderQueue = renderQueue;

				graphic.material = material;

			}else{

				string materialName = graphic.material.name + "_" + renderQueue.ToString();

				if(dic.ContainsKey(materialName)){

					graphic.material = dic[materialName];

				}else{

					Material material = Material.Instantiate<Material>(graphic.material);

					material.name = materialName;

					material.renderQueue = renderQueue;

					dic.Add(materialName,material);

					graphic.material = material;
				}
			}

			GameObject.Destroy(this);
		}
	}
}