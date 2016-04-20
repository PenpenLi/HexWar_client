using System;
using UnityEngine;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.textureFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroBuff
    {
        public const float ASSET_WIDTH = 1024f / 256f;
        public const float ASSET_HEIGHT = 512f / 256f;

        public const float FONT_WIDTH = 104f / 256f;
        public const float FONT_HEIGHT = 104f / 256f;

        public const int unitNum = 50;

        private BattleHeroBuffUnit[] unitVec;

        private Material mat;

        private MeshRenderer mr;
        private Mesh mesh;

        private GameObject buffGO;
        private GameObject container;

        private static BattleHeroBuff _Instance;

        public static BattleHeroBuff Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleHeroBuff();
                }

                return _Instance;
            }
        }

        public BattleHeroBuff()
        {
        }

        public void Init(GameObject con, Action call)
        {

            container = con;

            if (buffGO)
            {
                buffGO.SetActive(true);
                if (call != null)
                {
                    call();
                }
                return;
            }

            unitVec = new BattleHeroBuffUnit[unitNum];


            for (int i = 0; i < unitNum; i++)
            {

                unitVec[i] = new BattleHeroBuffUnit(i);
            }

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
                buffGO = _go;
                buffGO.transform.SetParent(container.transform, false);
                mr = _go.GetComponent<MeshRenderer>();
                mat = mr.material;

                mesh = _go.GetComponent<MeshFilter>().mesh;

                if (call != null)
                {
                    call();
                }
            };

            GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/BuffIcon.prefab", loadGameObject, true);
        }

        public void Update()
        {
            if (mr != null)
            {

                for (int i = 0; i < unitNum; i++)
                {
                    BattleHeroBuffUnit unit = unitVec[i];

                    if (unit.State == 1 || unit.IsChange)
                    {
                        if (unit.IsChange)
                        {

                            unit.IsChange = false;
                        }

                        unit.GetPositionsVec(mr.material);

                        unit.GetFixVec(mr.material);

                        unit.GetScaleFix(mr.material);

                    }
                }
            }
        }

        public BattleHeroBuffUnit GetBuffIcon(string _name, string _addOrSub, float _height, GameObject _go)
        {

            for (int i = 0; i < unitNum; i++)
            {
                if (unitVec[i].State == 0)
                {
                    BattleHeroBuffUnit unit = unitVec[i];
                    unit.Init(_height, _go);

                    unit.alpha = 1;
                    unit.State = 1;

                    int tempindex = i;

                    Action<Sprite> callBack = delegate(Sprite _texture)
                    {
                        mat.mainTexture = _texture.texture;

                        Vector2[] uvs = mesh.uv;

                        Vector2 tempuvfix = new Vector2(_texture.textureRect.x / _texture.texture.width, _texture.textureRect.y / _texture.texture.height);


                        uvs[tempindex * 2 * 4] = new Vector2(0, 0) + tempuvfix;
                        uvs[tempindex * 2 * 4 + 1] = new Vector2(BattleHeroBuff.FONT_WIDTH / (_texture.texture.width * 2 / 256f), BattleHeroBuff.FONT_HEIGHT / (_texture.texture.height * 2 / 256f)) + tempuvfix;
                        uvs[tempindex * 2 * 4 + 2] = new Vector2(BattleHeroBuff.FONT_WIDTH / (_texture.texture.width * 2 / 256f), 0) + tempuvfix;
                        uvs[tempindex * 2 * 4 + 3] = new Vector2(0, BattleHeroBuff.FONT_HEIGHT / (_texture.texture.height * 2 / 256f)) + tempuvfix;

                        mesh.uv = uvs;

                        unit.uFix = _texture.textureRect.x / _texture.texture.width;
                        unit.vFix = _texture.textureRect.y / _texture.texture.height;
                        unit.alpha = 1;
                    };

                    TextureFactory.Instance.GetTexture("Assets/Arts/ui/skillIcon/" + _name + ".png", callBack, true);

                    if (!string.IsNullOrEmpty(_addOrSub))
                    {
                        Action<Sprite> call2Back = delegate(Sprite _texture)
                        {
                            mat.mainTexture = _texture.texture;

                            Vector2[] uvs = mesh.uv;

                            Vector2 tempuvfix = new Vector2(_texture.textureRect.x / _texture.texture.width, _texture.textureRect.y / _texture.texture.height);


                            uvs[tempindex * 2 * 4 + 4] = new Vector2(0, 0) + tempuvfix;
                            uvs[tempindex * 2 * 4 + 5] = new Vector2(BattleHeroBuff.FONT_WIDTH / (_texture.texture.width * 2 / 256f), BattleHeroBuff.FONT_HEIGHT / (_texture.texture.height * 2 / 256f)) + tempuvfix;
                            uvs[tempindex * 2 * 4 + 6] = new Vector2(BattleHeroBuff.FONT_WIDTH / (_texture.texture.width * 2 / 256f), 0) + tempuvfix;
                            uvs[tempindex * 2 * 4 + 7] = new Vector2(0, BattleHeroBuff.FONT_HEIGHT / (_texture.texture.height * 2 / 256f)) + tempuvfix;

                            mesh.uv = uvs;

                            //unit.uFix = _texture.textureRect.x / _texture.texture.width;
                            //unit.vFix = _texture.textureRect.y / _texture.texture.height;

                            //unit.alpha = 1;
                        };

                        TextureFactory.Instance.GetTexture("Assets/Arts/ui/skillIcon/" + _addOrSub + ".png", call2Back, true);
                    }
                    else
                    {
                        Vector2[] uvs = mesh.uv;
                        uvs[i * 4 + 4] = Vector2.zero;
                        uvs[i * 4 + 5] = Vector2.zero;
                        uvs[i * 4 + 6] = Vector2.zero;
                        uvs[i * 4 + 7] = Vector2.zero;
                        mesh.uv = uvs;
                    }
                    return unit;
                }


            }

            throw new Exception("BuffIcon is out of use!!!");
        }

        public void DelBuffIcon(BattleHeroBuffUnit _unit)
        {

            _unit.alpha = 0;
            _unit.State = 0;
            _unit.IsChange = true;

        }

        public void ClearAll()
        {
            foreach (BattleHeroBuffUnit unit in unitVec)
            {
                DelBuffIcon(unit);
            }
        }

        public void Dispose()
        {
            buffGO.SetActive(false);
        }
    }
}
