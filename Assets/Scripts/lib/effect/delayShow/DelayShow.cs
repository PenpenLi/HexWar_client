using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.effect{

	public class DelayShow : MonoBehaviour {
		
		[SerializeField]private float delayTime = 1.0f;

		private bool isDelayActive = false;	//用来标记当前显示该对象是否是因为延迟显示功能显示的

        private int index = -1;

		// Use this for initialization
		void OnEnable () {		

			if (!isDelayActive) {

				gameObject.SetActive(false);

                index = SuperTween.Instance.DelayCall(delayTime, DelayFunc);
			}
		}

		void OnDisable()
		{
			if (isDelayActive) {

				isDelayActive = false;		
			}
		}

        void OnDestroy()
        {
            if (index != -1)
            {
                SuperTween.Instance.Remove(index);
            }
        }

		void DelayFunc()
		{
			isDelayActive = true;
            gameObject.SetActive(true);
		}
	}
}