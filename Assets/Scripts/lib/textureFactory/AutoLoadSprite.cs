using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.textureFactory
{

	public class AutoLoadSprite : MonoBehaviour
	{

		public const string LOAD_OVER_EVENT = "AutoLoadSpriteLoadOver";

		[SerializeField]
		private string
			spriteName;

		[SerializeField]
		private Image
			img;

		[SerializeField]
		private GameObject
			eventGo;

		// Use this for initialization
		void Start () {
		
			if (eventGo != null) {

				SpriteFactory.SetSprite (img, spriteName, false, true, false, LoadOver, LoadOver);

			} else {

				SpriteFactory.SetSprite (img, spriteName, false, true, false, null, null);
			}
		}

		private void LoadOver () {

			SuperEvent e = new SuperEvent (LOAD_OVER_EVENT);

			SuperFunction.Instance.DispatchEvent (eventGo, e);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}