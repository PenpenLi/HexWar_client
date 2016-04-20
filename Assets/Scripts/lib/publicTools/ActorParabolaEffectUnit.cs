using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;
using System.Collections.Generic;

namespace xy3d.tstd.lib.publicTools
{

	public class ActorParabolaEffectUnit 
	{

        public static List<ActorParabolaEffectUnit> apeList = new List<ActorParabolaEffectUnit>();

        //private Vector3[] intervalList = new Vector3[7]{new Vector3(0,12,0), new Vector3(0,10,20), new Vector3(0,10,-20), new Vector3(20,8,0), 
        //    new Vector3(-20,8,0), new Vector3(30,6,0), new Vector3(-30,6,0)};

        private Vector3[] intervalList = new Vector3[7]{new Vector3(0,12,0), new Vector3(10,10,10), new Vector3(-10,10,-10), new Vector3(20,8,20), 
            new Vector3(-20,8,-20), new Vector3(30,6,030), new Vector3(-30,6,-30)};

	    private int effectNum;

	    private Vector3 endPos;

	    private GameObject effectGO;

	    private ActorParabolaUnit[] apList;
        private Action<ActorParabolaEffectUnit, int, int> callBack;
        private int targetIndex;
        private int bulletIndex;

        public ActorParabolaEffectUnit(GameObject _effectGO, Vector3 _startPos, Vector3 _endPos, int _effectNum, float time, Action<ActorParabolaEffectUnit, int, int> _callBack, int _targetIndex, int _bulletIndex)
	    {
	        effectNum = _effectNum;

	        endPos = _endPos;
	        effectGO = _effectGO;
            callBack = _callBack;
            targetIndex = _targetIndex;
            bulletIndex = _bulletIndex;

	        apList = new ActorParabolaUnit[effectNum];

	        int index = (effectNum % 2 == 1) ? 0 : 1;

	        for (int i = 0; i < effectNum; i++)
	        {
                GameObject ins;
                if (i == 0)
                {
                    ins = effectGO;
                }
                else
                {
                    ins = GameObject.Instantiate(effectGO);
                }
	            Vector3 interval = intervalList[index + i];

                ActorParabolaUnit ap = new ActorParabolaUnit(ins, _startPos, endPos, interval);
	            apList[i] = ap;
	        }

            Action toCall = delegate()
            {
                callBack(this, targetIndex, bulletIndex);
            };

            Action<float> SetPercent = delegate(float value)
            {
                for (int i = 0; i < effectNum; i++)
                {
                    ActorParabolaUnit ap = apList[i];
                    ap.SetPercent(value);
                }
            };

            int id = SuperTween.Instance.To(0, 1, time, SetPercent, toCall);
            SuperTween.Instance.SetTag(id, "battle_tag");
	    }

	    public void Destroy()
	    {
	        for (int i = 0; i < effectNum; i++)
	        {
	            ActorParabolaUnit ap = apList[i];
	            ap.Destroy();
	            apList[i] = null;
	        }

	        apList = null;
	    }
	}
}
