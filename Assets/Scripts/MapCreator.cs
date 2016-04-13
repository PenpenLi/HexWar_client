using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class MapCreator : MonoBehaviour {


	[SerializeField]
	private GameObject choosePanel;

	[SerializeField]
	private GameObject inputPanel;

	[SerializeField]
	private InputField widthField;

	[SerializeField]
	private InputField heightField;

	[SerializeField]
	private GameObject mapPanel;

	[SerializeField]
	private Transform mapContainer;

	private float unitWidth = 0.6f;
	private static readonly float sqrt3 = Mathf.Sqrt (3);

	private MapData mapData;

	void Awake(){

	}

	public void CreateMap(){

		choosePanel.SetActive (false);

		inputPanel.SetActive (true);
	}

	public void LoadMap(){

		string path = EditorUtility.OpenFilePanel ("a", "a", "map");

		if (!string.IsNullOrEmpty (path)) {

			using(FileStream fs = new FileStream(path,FileMode.Open)){

				using(BinaryReader br = new BinaryReader(fs)){

					mapData = new MapData();

					mapData.GetData(br);

					choosePanel.SetActive (false);

					mapPanel.SetActive (true);

					CreateMapPanel ();
				}
			}
		}
	}

	public void ConfirmMapSize(){

		if (string.IsNullOrEmpty (widthField.text) || string.IsNullOrEmpty (heightField.text)) {

			return;
		}

		inputPanel.SetActive (false);

		mapPanel.SetActive (true);

		InitMapData (int.Parse (widthField.text),int.Parse (heightField.text));

		CreateMapPanel ();
	}

	private void InitMapData(int _mapWidth,int _mapHeight){
		
		mapData = new MapData (_mapWidth,_mapHeight);
		
		for (int i = 0; i < mapData.size; i++) {
			
			mapData.dic.Add(i,true);
		}
	}

	public void CreateMapPanel(){

		int index = 0;

		for (int i = 0; i < mapData.mapHeight; i++) {

			for(int m = 0 ; m < mapData.mapWidth ; m++){

				if(i % 2 == 1 && m == mapData.mapWidth - 1){

					continue;
				}

				GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("MapUnit"));

				go.transform.SetParent(mapContainer,false);

				go.transform.localPosition = new Vector3(m * unitWidth * sqrt3 * 2 + ((i % 2 == 1) ? unitWidth * Mathf.Sqrt(3) : 0),-i * unitWidth * 3,0);

				MapUnit unit = go.GetComponent<MapUnit>();

				unit.index = index;
				
				unit.SetOffVisible(false);

				if(mapData.dic.ContainsKey(index)){

					if(mapData.dic[index]){

						unit.SetMainColor(Color.red);

					}else{

						unit.SetMainColor(Color.green);
					}

				}else{

					unit.SetMainColor(new Color(0,0,0,0));
				}

				index++;
			}
		}

		mapContainer.transform.localPosition = new Vector3 (-0.5f * (mapData.mapWidth * unitWidth * sqrt3 * 2) + unitWidth * sqrt3, 0.5f * (mapData.mapHeight * unitWidth * 3 + unitWidth) - unitWidth * 2, 0);
	}

	public void SaveMap(){

		string path = EditorUtility.SaveFilePanel("a","a","","map");

		if (!string.IsNullOrEmpty (path)) {

			using(FileStream fs = new FileStream (path, FileMode.CreateNew)){

				using(BinaryWriter bw = new BinaryWriter (fs)){

					mapData.SetData (bw);
				}
			}
		}
	}

	public void MapUnitUpAsButton(MapUnit _unit){

		if (mapData.dic.ContainsKey (_unit.index)) {

			if (mapData.dic [_unit.index]) {

				_unit.SetMainColor (Color.green);

				mapData.dic [_unit.index] = false;

			} else {

				_unit.SetMainColor (new Color (0, 0, 0, 0));

				mapData.dic.Remove (_unit.index);
			}

		} else {

			mapData.dic.Add(_unit.index,true);

			_unit.SetMainColor (Color.red);
		}
	}
}
