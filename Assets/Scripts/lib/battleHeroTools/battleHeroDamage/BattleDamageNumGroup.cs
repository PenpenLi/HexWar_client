using System;
using System.Collections.Generic;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleDamageNumGroup
    {

        private const string picStr = "1234567890-";
	
		private const float criticalScale = 2;
		
		public int drawIndex;
		
		public int index;

        public BattleDamageNumUnit[] unitVec;
		
		

        public Vector3 pos = new Vector3();
        public Vector3 nonePos = new Vector3();
        private Vector3 scale = new Vector3(1, 1, 1);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();

		
		private float groupWidth;
		
		public BattleDamageNumGroup preGroup;
		public BattleDamageNumGroup nextGroup;

		private string positionsStr;

		private string scaleFixStr;

		private Vector4 posVec = new Vector4(0,0,0,1);

        public BattleDamageNum bdm;

        public float nowScale = 1;

        public Vector4 scaleVec4 = new Vector4(1, 1, 1, 1);

        private float alpha = 0;

        public float Alpha
        {
            get { return alpha; }
            set { 
                alpha = value;
                if (unitVec != null)
                {
                    foreach (BattleDamageNumUnit unit in unitVec)
                    {
                        unit.Alpha = alpha;
                    }
                }
            }
        }

        private int state;

        public int State
        {
            get { return state; }
            set { 
                state = value;
                if (unitVec != null)
                {
                    foreach (BattleDamageNumUnit unit in unitVec)
                    {
                        unit.State = state;
                    }
                }
            }
        }

        private bool isChange = true;

        public bool IsChange
        {
            get { return isChange; }
            set {
                isChange = value;
                if (unitVec != null)
                {
                    foreach (BattleDamageNumUnit unit in unitVec)
                    {
                        unit.IsChange = isChange;
                    }
                }
            }
        }

		
		public BattleDamageNumGroup(int _drawIndex, int _index)
		{
			drawIndex = _drawIndex;
			index = _index;

			string indexStr = index.ToString();

			positionsStr = "positions" + indexStr;

			scaleFixStr = "scaleFix" + indexStr;

            IsChange = true;
		}

        public void init(string _str, int _color)
        {
			int i,m,n;
			n = 0;
            unitVec = new BattleDamageNumUnit[_str.Length];
            
			for(i = 0 ; i < _str.Length ; i++){

                unitVec[i] = bdm.GetDamageUnit(this);
				
				m = picStr.IndexOf(_str[i]);
				
				unitVec[i].uFix = m * BattleDamageNum.FONT_WIDTH / BattleDamageNum.ASSET_WIDTH;

                unitVec[i].vFix = -_color * BattleDamageNum.FONT_HEIGHT / BattleDamageNum.ASSET_HEIGHT;
				
				n++;
			}
			
			groupWidth = n * BattleDamageNum.FONT_WIDTH;
			
			for(i = 0 ; i < _str.Length ; i++){
				
				unitVec[i].xFix = -groupWidth * 0.5f + BattleDamageNum.FONT_WIDTH * 0.5f + i * BattleDamageNum.FONT_WIDTH;
			}
			
			if(_color == 2){
				
				SetScale(criticalScale);
				
			}else{

                SetScale(1);
			}
		}

        public void SetScale(float s)
        {
            s = s * 1.3f;
            nowScale = scale.x = scale.y = scale.z = s;
        }

		public void GetPositionsVec(Material _material)
		{
			if (Alpha > 0)
			{
				posVec.x = pos.x;
				posVec.y = pos.y;
				posVec.z = pos.z;
			}

			_material.SetVector(positionsStr,posVec);
		}

        public Vector4 GetPositionsVec()
        {
            if (Alpha > 0)
            {
				posVec.x = pos.x;
				posVec.y = pos.y;
				posVec.z = pos.z;
            }
			return posVec;
        }

        public Matrix4x4 GetMatrix()
        {
            matrix.SetTRS(nonePos, rotation, scale);
            return matrix;
        }

		public void GetScaleFix(Material _material)
		{
			scaleVec4.x = scaleVec4.y = scaleVec4.z = scaleVec4.w = nowScale;

			_material.SetVector(scaleFixStr,scaleVec4);
		}

        public Vector4 GetScaleFix()
        {
            scaleVec4.x = scaleVec4.y = scaleVec4.z = scaleVec4.w = nowScale;
            return scaleVec4;
        }

    }
}
