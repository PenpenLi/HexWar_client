using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

[Serializable]
public class AddAtt{

	public string att;

	public int attType;

	public int dataInt;
	public string dataString;
	public bool dataBool;
	public float dataFloat;
	public UnityEngine.Object dataObj;

	public int[] dataInts;
	public string[] dataStrings;
	public bool[] dataBools;
	public float[] dataFloats;
	public UnityEngine.Object[] dataObjs;

	public Color32 dataColor32;
	public Color32[] dataColor32s;

	public Color dataColor;
	public Color[] dataColors;

	public Vector4 dataVector4;

	public LayerMask layerMask;

	public AddAtt(string _attName,object _target){

		att = _attName;

		GetData(_target);
	}

	private void GetData(object vv){

		if(vv is Int32){
			
			attType = 1;
			
			dataInt = (int)vv;
			
		}else if(vv is String){
			
			attType = 2;
			
			dataString = (string)vv;
			
		}else if(vv is Boolean){
			
			attType = 3;
			
			dataBool = (bool)vv;
			
		}else if(vv is Single){
			
			attType = 4;
			
			dataFloat = (float)vv;
			
		}else if(vv is UnityEngine.Object){
			
			attType = 5;
			
			dataObj = (UnityEngine.Object)vv;
			
		}else if(vv is Int32[]){
			
			attType = 6;
			
			dataInts = (int[])vv;
			
		}else if(vv is String[]){
			
			attType = 7;
			
			dataStrings = (string[])vv;
			
		}else if(vv is Boolean[]){
			
			attType = 8;
			
			dataBools = (bool[])vv;
			
		}else if(vv is Single[]){
			
			attType = 9;
			
			dataFloats = (float[])vv;
			
		}else if(vv is UnityEngine.Object[]){
			
			attType = 10;
			
			dataObjs = (UnityEngine.Object[])vv;

		}else if(vv is Color32){
			
			attType = 11;
			
			dataColor32 = (Color32)vv;
			
		}else if(vv is Color32[]){
			
			attType = 12;
			
			dataColor32s = (Color32[])vv;
			
		}else if(vv is Color){
			
			attType = 13;
			
			dataColor = (Color)vv;
			
		}else if(vv is Color[]){
			
			attType = 14;
			
			dataColors = (Color[])vv;
			
		}else if(vv is Vector4){
			
			attType = 15;
			
			dataVector4 = (Vector4)vv;
			
		}else if(vv is LayerMask){

			attType = 16;

			layerMask = (LayerMask)vv;

		}else{

			throw new Exception("AddAtt error! Type not found:" + vv.GetType());
		}
	}

	// Use this for initialization
	public void Init (Component component,Type type) {
	
//		SuperDebug.Log("SetAtt:" + att);

		FieldInfo fieldInfo = type.GetField(att,BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

		switch(attType){

		case 1:

			fieldInfo.SetValue(component,dataInt);

			break;

		case 2:
			
			fieldInfo.SetValue(component,dataString);
			
			break;

		case 3:
			
			fieldInfo.SetValue(component,dataBool);
			
			break;

		case 4:
			
			fieldInfo.SetValue(component,dataFloat);
			
			break;

		case 5:

			if(dataObj != null){

				fieldInfo.SetValue(component,dataObj);
			}
			
			break;

		case 6:
			
			fieldInfo.SetValue(component,dataInts);
			
			break;

		case 7:
			
			fieldInfo.SetValue(component,dataStrings);
			
			break;

		case 8:
			
			type.GetField(att).SetValue(component,dataBools);
			
			break;

		case 9:
			
			fieldInfo.SetValue(component,dataFloats);
			
			break;

		case 10:

			if(dataObjs != null){

				Type unitType = fieldInfo.FieldType.GetElementType();

				Array arr = Array.CreateInstance(unitType,dataObjs.Length);

				for(int i = 0 ; i < dataObjs.Length ; i++){

					arr.SetValue(dataObjs[i],i);
				}

				fieldInfo.SetValue(component,arr);
			}
			
			break;

		case 11:
			
			fieldInfo.SetValue(component,dataColor32);
			
			break;

		case 12:
			
			fieldInfo.SetValue(component,dataColor32s);
			
			break;

		case 13:
			
			fieldInfo.SetValue(component,dataColor);
			
			break;

		case 14:
			
			fieldInfo.SetValue(component,dataColors);
			
			break;

		case 15:
			
			fieldInfo.SetValue(component,dataVector4);
			
			break;

		case 16:

			fieldInfo.SetValue(component,layerMask);

			break;
		}
	}
}
