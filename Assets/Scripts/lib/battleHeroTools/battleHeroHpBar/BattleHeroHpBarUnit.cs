using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroHpBarUnit
    {
        private const float hpChangeTime = 1;//血条变化需要时间

        private float height = 0;

        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(1, 1, 1);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);  
        private Matrix4x4 matrix = new Matrix4x4();

        private Vector4 posVec = new Vector4();
        private Vector4 stateInfoVec = new Vector4();
        private Vector4 fixVec = new Vector4();

        private Vector4 scaleVec4 = new Vector4(1, 1, 1, 1);

        public bool show = false;

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

        public float angerUFix;
        public float angerXFix;

//        private float angerNum;

        public float hpUFix;
        public float hpXFix;

//        private float targetHp;
        private float _hp;
        private float maxHp;

        private GameObject go;

		private int index;

		private string positionsStr;

		private string fixStr;

		private string stateInfoStr;

		private string scaleFixStr;

        public BattleHeroHpBarUnit(int _index)
        {
			index = _index;

			string indexStr = index.ToString();

			positionsStr = "positions" + indexStr;

			fixStr = "fix" + indexStr;

			stateInfoStr = "stateInfo" + indexStr;

			scaleFixStr = "scaleFix" + indexStr;
        }

        public float Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                hpUFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
                hpXFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth;

                _hp = value;
            }
        }

        public void SetHp(float _targetHp)
        {
            show = true;
            Alpha = 1;
            int id = SuperTween.Instance.To(Hp, _targetHp, hpChangeTime, UpdateHp, UpdateHpEnd);
            SuperTween.Instance.SetTag(id, "battle_tag");
        }

        private void UpdateHpEnd()
        {
            show = false;
        }

        private void UpdateHp(float value)
        {
            Hp = value;
        }

        public void Init(float _nowhp, float _maxHp, int _nowAnger, float _height, GameObject _go)
        {
            maxHp = _maxHp;
            Hp = _nowhp;
            go = _go;
            height = _height;
            UpdateAnger(_nowAnger);
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

		public void GetStateInfoVec(Material _material)
		{
			float tempAlpha = Alpha;
			if ((!BattleHeroHpBar.Instance.showForce) && (!show))
			{
				tempAlpha = 0;
			}
			stateInfoVec.x = tempAlpha;
			stateInfoVec.y = State;

			_material.SetVector(stateInfoStr,stateInfoVec);
		}

        public Vector4 GetStateInfoVec()
        {
            float tempAlpha = Alpha;
            if ((!BattleHeroHpBar.Instance.showForce) && (!show))
            {
                tempAlpha = 0;
            }
            stateInfoVec.x = tempAlpha;
            stateInfoVec.y = State;
            return stateInfoVec;
        }

        public Matrix4x4 GetMatrix()
        {
            
            if (go != null)
            {
                //rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z);
                //scale.x = Math.Abs(go.transform.localScale.x);
                //scale.y =  Math.Abs(go.transform.localScale.y);
                //scale.z =  Math.Abs(go.transform.localScale.z);
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

		public void GetFixVec(Material _material)
		{
			if (go != null)
			{
				fixVec.x = hpUFix;
				fixVec.y = hpXFix;
				fixVec.z = angerUFix;
				fixVec.w = angerXFix;
			}

			_material.SetVector(fixStr,fixVec);
		}

        public Vector4 GetFixVec()
        {
            if (go != null)
            {
                fixVec.x = hpUFix;
                fixVec.y = hpXFix;
                fixVec.z = angerUFix;
                fixVec.w = angerXFix;
            }
            return fixVec;
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

        public void resetHp(float _nowHp, float _maxHp)
        {
            maxHp = _maxHp;
            Hp = _nowHp;
        }

        public void UpdateAnger(float value)
        {
            angerUFix = (value / 3 - 1) * BattleHeroHpBar.angerBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
            angerXFix = (value / 3 - 1) * BattleHeroHpBar.angerBarWidth;

        }
    }
}