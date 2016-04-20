using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroShadowUnit
    {
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(2, 2, 2);
        public Quaternion rotation = Quaternion.Euler(-15, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();

        private Vector4 stateInfoVec = new Vector4();

        public bool IsBattleHero = false;

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

        private GameObject go;

        public void Init(GameObject _go)
        {
            go = _go;
        }

        public Vector4 GetStateInfoVec()
        {
            
            stateInfoVec.x = Alpha;
            stateInfoVec.y = State;
            return stateInfoVec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (go != null)
            {
                if (IsBattleHero)
                {
                    pos.x = go.transform.position.x;
                    pos.y = go.transform.position.y + 0.1f;
                    pos.z = go.transform.position.z;



                    rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z);

                    scale.x = Math.Abs(go.transform.localScale.x);
                    scale.y = Math.Abs(go.transform.localScale.y);
                    scale.z = Math.Abs(go.transform.localScale.z);
                }
                else
                {
                    pos.x = go.transform.position.x;
                    pos.y = go.transform.position.y;
                    pos.z = go.transform.position.z;

                    scale = new Vector3(2, 2, 2);
                    rotation = Quaternion.Euler(-15, 0, 0);
                }

                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }
    }
}
