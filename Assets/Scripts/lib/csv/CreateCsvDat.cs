using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using xy3d.tstd.lib.wwwManager;
using System.Reflection;
using System;
using System.Collections.Generic;

public class CreateCsvDat
{
	public static void Start(string _path0,string _path1){

		FileInfo fib = new FileInfo(Application.streamingAssetsPath + "/csv.dat");
		
		if(fib.Exists){
			
			fib.Delete();
		}
		
		FileStream fs = fib.Create();
		
		BinaryWriter bw = new BinaryWriter(fs);
		
		FileInfo fix = new FileInfo(Application.dataPath + _path0 + "LoadCsv.cs");
		
		if(fix.Exists){
			
			fix.Delete();
		}
		
		StreamWriter sww = fix.CreateText();
		
		sww.WriteLine("using System.IO;");
		
		sww.WriteLine("using System.Collections;");
		
		sww.WriteLine("using System.Collections.Generic;");
		
		sww.WriteLine("using System;");
		
		sww.WriteLine("public class LoadCsv {");
		
		sww.WriteLine("    public static Dictionary<Type,IDictionary> Init(byte[] _bytes) {");
		
		sww.WriteLine("        MemoryStream ms = new MemoryStream(_bytes);");
		
		sww.WriteLine("        BinaryReader br = new BinaryReader(ms);");
		
		sww.WriteLine("        Dictionary<Type,IDictionary> dic = new Dictionary<Type,IDictionary>();");
		
		List<Type> typeList = new List<Type>();
		
		foreach(Type type in StaticData.dic.Keys){
			
			typeList.Add(type);
		}
		
		typeList.Sort(SortType);
		
		foreach(Type type in typeList){
			
			IDictionary unitDic = StaticData.dic[type];
			
			string fileName = type.Name;
			
			FileInfo fii = new FileInfo(Application.dataPath + _path1 + fileName + "_c.cs");
			
			if(fii.Exists){
				
				fii.Delete();
			}
			
			StreamWriter sw = fii.CreateText();
			
			sw.WriteLine("using System.IO;");
			
			sw.WriteLine("public class " + fileName + "_c {");
			
			sw.WriteLine("    public static void Init(" + fileName + " _csv, BinaryReader _br){");
			
			FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			
			List<FieldInfo> fisList = new List<FieldInfo>();
			
			foreach(FieldInfo fi in fis){
				
				fisList.Add(fi);
			}
			
			fisList.Sort(SortFieldInfo);
			
			foreach(FieldInfo fi in fisList){
				
				switch (fi.FieldType.Name)
				{
				case "Int32":
					
					sw.WriteLine("        _csv." + fi.Name + " = _br.ReadInt32();");
					
					break;
					
				case "String":
					
					sw.WriteLine("        _csv." + fi.Name + " = _br.ReadString();");
					
					break;
					
				case "Boolean":
					
					sw.WriteLine("        _csv." + fi.Name + " = _br.ReadBoolean();");
					
					break;
					
				case "Single":
					
					sw.WriteLine("        _csv." + fi.Name + " = _br.ReadSingle();");
					
					break;
					
				case "Int32[]":
					
					sw.WriteLine("        int length" + fi.Name + " = _br.ReadInt32();");
					
					sw.WriteLine("        _csv." + fi.Name + " = new int[length" + fi.Name + "];");
					
					sw.WriteLine("        for(int i = 0 ; i < length" + fi.Name + " ; i++){");
					
					sw.WriteLine("            _csv." + fi.Name + "[i] = _br.ReadInt32();");
					
					sw.WriteLine("        }");
					
					break;
					
				case "String[]":
					
					sw.WriteLine("        int length" + fi.Name + " = _br.ReadInt32();");
					
					sw.WriteLine("        _csv." + fi.Name + " = new string[length" + fi.Name + "];");
					
					sw.WriteLine("        for(int i = 0 ; i < length" + fi.Name + " ; i++){");
					
					sw.WriteLine("            _csv." + fi.Name + "[i] = _br.ReadString();");
					
					sw.WriteLine("        }");
					
					break;
					
				case "Boolean[]":
					
					sw.WriteLine("        int length" + fi.Name + " = _br.ReadInt32();");
					
					sw.WriteLine("        _csv." + fi.Name + " = new bool[length" + fi.Name + "];");
					
					sw.WriteLine("        for(int i = 0 ; i < length" + fi.Name + " ; i++){");
					
					sw.WriteLine("            _csv." + fi.Name + "[i] = _br.ReadBoolean();");
					
					sw.WriteLine("        }");
					
					break;
					
				case "Single[]":
					
					sw.WriteLine("        int length" + fi.Name + " = _br.ReadInt32();");
					
					sw.WriteLine("        _csv." + fi.Name + " = new float[length" + fi.Name + "];");
					
					sw.WriteLine("        for(int i = 0 ; i < length" + fi.Name + " ; i++){");
					
					sw.WriteLine("            _csv." + fi.Name + "[i] = _br.ReadSingle();");
					
					sw.WriteLine("        }");
					
					break;
					
				default:
					
					Debug.LogError("error1  class:" + fileName + "   field:" + fi.Name);
					
					break;
				}
			}
			
			sw.WriteLine("    }");
			
			sw.WriteLine("}");
			
			sw.Flush();
			
			sw.Close();
			
			bw.Write(unitDic.Count);
			
			sww.WriteLine("        Dictionary<int," + fileName + "> " + fileName + "Dic = new Dictionary<int," + fileName + ">();");
			
			sww.WriteLine("        int length" + fileName + " = br.ReadInt32();");
			
			sww.WriteLine("        for(int i = 0 ; i < length" + fileName + " ; i++){");
			
			sww.WriteLine("            " + fileName + " unit = new " + fileName + "();");
			
			sww.WriteLine("            " + fileName + "_c.Init(unit,br);");
			
			sww.WriteLine("            " + fileName + "Dic.Add(unit.ID,unit);");
			
			sww.WriteLine("        }");
			
			foreach(object obj in unitDic.Values){
				
				foreach(FieldInfo fi in fisList){
					
					switch (fi.FieldType.Name)
					{
					case "Int32":
						
						bw.Write((int)fi.GetValue(obj));
						
						break;
						
					case "String":
						
						string str = (string)fi.GetValue(obj);
						
						bw.Write(string.IsNullOrEmpty(str) ? "" : str);
						
						break;
						
					case "Boolean":
						
						bw.Write((bool)fi.GetValue(obj));
						
						break;
						
					case "Single":
						
						bw.Write((float)fi.GetValue(obj));
						
						break;
						
					case "Int32[]":
						
						int[] t = fi.GetValue(obj) as Int32[];
						
						if(t == null){
							
							bw.Write(0);
							
						}else{
							
							bw.Write(t.Length);
							
							foreach(int ll in t){
								
								bw.Write(ll);
							}
						}
						
						break;
						
					case "String[]":
						
						string[] t1 = fi.GetValue(obj) as string[];
						
						if(t1 == null){
							
							bw.Write(0);
							
						}else{
							
							bw.Write(t1.Length);
							
							foreach(string ll in t1){
								
								bw.Write(string.IsNullOrEmpty(ll) ? "" : ll);
							}
						}
						
						break;
						
					case "Boolean[]":
						
						bool[] t2 = fi.GetValue(obj) as Boolean[];
						
						if(t2 == null){
							
							bw.Write(0);
							
						}else{
							
							bw.Write(t2.Length);
							
							foreach(bool ll in t2){
								
								bw.Write(ll);
							}
						}
						
						break;
						
					case "Single[]":
						
						float[] t3 = fi.GetValue(obj) as float[];
						
						if(t3 == null){
							
							bw.Write(0);
							
						}else{
							
							bw.Write(t3.Length);
							
							foreach(float ll in t3){
								
								bw.Write(ll);
							}
						}
						
						break;
						
					default:
						
						//						Debug.LogError("error2  class:" + fileName + "   field:" + fi.Name);
						
						break;
					}
				}
			}
			
			sww.WriteLine("        dic.Add(typeof(" + fileName + ")," + fileName + "Dic);");
		}
		
		sww.WriteLine("        br.Close();");
		
		sww.WriteLine("        ms.Close();");
		
		sww.WriteLine("        ms.Dispose();");
		
		sww.WriteLine("        return dic;");
		
		sww.WriteLine("    }");
		
		sww.WriteLine("}");
		
		sww.Flush();
		
		sww.Close();
		
		fs.Flush();
		
		fs.Close();
		
		Debug.Log("csv.dat生成完毕!");
	}
	
	private static int SortType(Type _a,Type _b){
		
		return _a.ToString().CompareTo(_b.ToString());
	}
	
	private static int SortFieldInfo(FieldInfo _a,FieldInfo _b){
		
		return _a.ToString().CompareTo(_b.ToString());
	}
}
