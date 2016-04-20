using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;
using UnityEngine.UI;
using xy3d.tstd.lib.screenScale;

namespace xy3d.tstd.lib.superList{

	public class SuperScaleScrollRect : MonoBehaviour {

		[SerializeField]
		private RectTransform childTransform;

		private float childWidth;

		private float childHeight;

		private Vector2 containerHalfRect;

		[SerializeField]
		private float minScale;

		[SerializeField]
		private float maxScale;

		// Use this for initialization
		void Start () {

			SuperScrollRect scrollRect = gameObject.AddComponent<SuperScrollRect>();

			scrollRect.content = childTransform;

			scrollRect.horizontal = true;

			scrollRect.vertical = true;

			scrollRect.movementType = ScrollRect.MovementType.Clamped;

			containerHalfRect = (transform as RectTransform).rect.size / 2;

			SuperFunction.Instance.AddEventListener(ScreenScale.Instance.gameObject,ScreenScale.SCALE_CHANGE,ScaleChange);

			childWidth = childTransform.rect.width;

			childHeight = childTransform.rect.height;

			if(minScale == 0){

				float widthScale = containerHalfRect.x * 2 / childWidth * 1.001f;

				float heightScale = containerHalfRect.y * 2 / childHeight * 1.001f;

				minScale = Mathf.Max(widthScale,heightScale);
			}

			if(maxScale == 0){

				maxScale = 1;
			}

			SuperDebug.Log("SuperScaleScrollRect  minScale:" + minScale + "   maxScale:" + maxScale);
		}

		private void ScaleChange(SuperEvent e){

			float scale = (float)e.data[0];

			float tmpScale = scale * childTransform.localScale.x;

			if(tmpScale < minScale){

				scale = minScale / childTransform.localScale.x;

			}else if(tmpScale > maxScale){

				scale = maxScale / childTransform.localScale.x;
			}

			Vector2 pos = (Vector2)e.data[1];

			pos = pos * (containerHalfRect.x * 2 / Screen.width);

			Vector2 resultPos = GetAnchoredPosFix(pos,scale);

			if(scale < 1){

				if(resultPos.x + childWidth / 2 * childTransform.localScale.x * scale < containerHalfRect.x){

	//				Debug.Log("右面漏了");

					resultPos = new Vector2(containerHalfRect.x - childWidth / 2 * childTransform.localScale.x * scale,resultPos.y);

				}else if(resultPos.x + containerHalfRect.x - childWidth / 2 * childTransform.localScale.x * scale > 0){

	//				Debug.Log("左面漏了");
					
					resultPos = new Vector2(childWidth / 2 * childTransform.localScale.x * scale - containerHalfRect.x,resultPos.y);
				}

				if(resultPos.y + childHeight / 2 * childTransform.localScale.x * scale < containerHalfRect.y){
					
	//				Debug.Log("上面漏了");

					resultPos = new Vector2(resultPos.x,containerHalfRect.y - childHeight / 2 * childTransform.localScale.x * scale);

				}else if(resultPos.y + containerHalfRect.y - childHeight / 2 * childTransform.localScale.x * scale > 0){
					
	//				Debug.Log("下面漏了");

					resultPos = new Vector2(resultPos.x,childHeight / 2 * childTransform.localScale.x * scale - containerHalfRect.y);
				}
			}
			
			childTransform.anchoredPosition = resultPos;

			childTransform.localScale = childTransform.localScale * scale;
		}

		private Vector2 GetAnchoredPosFix(Vector2 _pos,float _scale){

			Vector2 mPos = _pos - containerHalfRect;
			
			Vector2 dv = mPos - childTransform.anchoredPosition;
			
			dv = dv * _scale;
			
			Vector2 resultPos = mPos - dv;

			return resultPos;
		}
	}
}