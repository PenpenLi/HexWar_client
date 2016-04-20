using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIconUnit
    {
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(1, 1, 1);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();

        private Vector4 posVec = new Vector4();
        private Vector4 fixVec = new Vector4();

        private Vector4 scaleVec4 = new Vector4(1, 1, 1, 1);

        private float height = 0;

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
		
		public float uFix = 0;
		public float vFix = 0;
		
		public float alpha = 0;

		public Action<BattleSkillIconUnit, Action> endBack;

        public Action callBack;

        private GameObject go;

		private int index;

		private string positionsStr;

		private string fixStr;

		private string scaleFixStr;

		public BattleSkillIconUnit(int _index){

			index = _index;

			string indexStr = index.ToString();

			positionsStr = "positions" + indexStr;

			fixStr = "fix" + indexStr;

			scaleFixStr = "scaleFix" + indexStr;
		}

        public void Init(float _height, GameObject _go)
        {
            go = _go;
            height = _height;
        }

		public void GetPositionsVec(Material _material)
		{
			if (go != null)
			{
				posVec.x = go.transform.position.x;
				posVec.y = go.transform.position.y + height;
				posVec.z = go.transform.position.z;
				posVec.w = 1;
			}

			_material.SetVector(positionsStr,posVec);
		}

        public Vector4 GetPositionsVec()
        {
            if (go != null)
            {
                posVec.x = go.transform.position.x;
                posVec.y = go.transform.position.y + height;
                posVec.z = go.transform.position.z;
                posVec.w = 1;
            }
            return posVec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (go != null)
            {
                //rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z);
                //scale.x = Math.Abs(go.transform.localScale.x);
                //scale.y = Math.Abs(go.transform.localScale.y);
                //scale.z = Math.Abs(go.transform.localScale.z);
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

		public void GetScaleFix(Material _material)
		{
			if (go != null)
			{
                //scaleVec4.x = Math.Abs(go.transform.localScale.x);
                //scaleVec4.y = Math.Abs(go.transform.localScale.y);
                //scaleVec4.z = Math.Abs(go.transform.localScale.z);
                //scaleVec4.w = 1;
			}

			_material.SetVector(scaleFixStr,scaleVec4);
		}

        public Vector4 GetScaleFix()
        {
            if (go != null)
            {
                //scaleVec4.x = Math.Abs(go.transform.localScale.x);
                //scaleVec4.y = Math.Abs(go.transform.localScale.y);
                //scaleVec4.z = Math.Abs(go.transform.localScale.z);
                //scaleVec4.w = 1;
            }
            return scaleVec4;
        }

		public void GetFixVec(Material _material)
		{
			if (go != null)
			{
				fixVec.x = uFix;
				fixVec.y = vFix;
				fixVec.z = alpha;
				fixVec.w = State;
			}

			_material.SetVector(fixStr,fixVec);
		}

        public Vector4 GetFixVec()
        {
            if (go != null)
            {
                fixVec.x = uFix;
                fixVec.y = vFix;
                fixVec.z = alpha;
                fixVec.w = State;
            }
            return fixVec;
        }
    }
}
