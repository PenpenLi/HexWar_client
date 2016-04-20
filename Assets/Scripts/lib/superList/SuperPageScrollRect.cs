using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.superList;

public class SuperPageScrollRect : SuperScrollRect {

	[SerializeField]
	private int num;

	[SerializeField]
	private float speedFix;

	private float step;

	private float halfStep;

	void Awake(){
		
		step = 1f / (num - 1);
		
		halfStep = step * 0.5f;
	}

	public override void OnEndDrag(PointerEventData eventData){

		base.OnEndDrag (eventData);

		StopMovement ();

		if (horizontalNormalizedPosition > 0 && horizontalNormalizedPosition < 1) {

			for (int i = 0; i < num; i++) {

				if (horizontalNormalizedPosition < i * step + halfStep) {

					SuperTween.Instance.To(horizontalNormalizedPosition,i * step,Mathf.Abs(i * step - horizontalNormalizedPosition) * speedFix,SetPos,null);

					break;
				}
			}
		}
	}

	private void SetPos(float _value){

		horizontalNormalizedPosition = _value;
	}
}
