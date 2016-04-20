using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleDamageNumUnit
    {
        public float uFix = 0;
		public float vFix = 0;
		public float xFix = 0;
		
		public int groupIndex = -1;

        private Vector4 vec = new Vector4();
        private Vector4 stateVec = new Vector4();
		
		public BattleDamageNumUnit nextUnit;

		private int index;

		private string fixStr;

		private string stateStr;

        private float alpha = 0;

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private bool isChange = true;

        public bool IsChange
        {
            get { return isChange; }
            set { isChange = value; }
        }
		
		public BattleDamageNumUnit(int _index)
		{
			index = _index;

			string tmpIndex = index.ToString();

			fixStr = "fix" + tmpIndex;

			stateStr = "state" + tmpIndex;
		}
		
		public void SetGroup(int _groupIndex){
			
			groupIndex = _groupIndex;
		}

		public void GetFix(Material _material)
		{
			vec.x = xFix;
			vec.y = uFix;
			vec.z = vFix;

			_material.SetVector(fixStr,vec);
		}

        public Vector4 GetFix()
        {
            vec.x = xFix;
            vec.y = uFix;
            vec.z = vFix;
            return vec;
        }

		public void GetState(Material _material)
		{
			stateVec.x = alpha;
			stateVec.y = groupIndex;
			stateVec.z = state;

			_material.SetVector(stateStr,stateVec);
		}

        public Vector4 GetState()
        {
            stateVec.x = alpha;
            stateVec.y = groupIndex;
            stateVec.z = state;
            return stateVec;
        }
    }
}
