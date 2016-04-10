using System.Collections;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

public class StaticData
{
    public static string path;

    public const string datName = "csv.dat";

	public static Dictionary<Type, IDictionary> dic = new Dictionary<Type, IDictionary>();

	private static Dictionary<Type, IList> dicList = new Dictionary<Type, IList>();

    public static T GetData<T>(int _id) where T : CsvBase
    {
        Dictionary<int, T> tmpDic = dic[typeof(T)] as Dictionary<int, T>;

        if (!tmpDic.ContainsKey(_id))
        {
            Console.WriteLine(typeof(T).Name + "表中未找到ID为:" + _id + "的行!");
        }

        return tmpDic[_id];
    }

    public static T GetDataOfKey<T>(string key, object keyValueParam) where T : CsvBase
    {
        Dictionary<int, T> dict = GetDic<T>();

		Type type = typeof(T);

		FieldInfo field = type.GetField(key);

        foreach (T item in dict.Values)
        {
            object keyValue = field.GetValue(item);

            if (keyValue.Equals(keyValueParam))
            {
                return item;
            }
        }

        return default(T);
    }

    public static Dictionary<int, T> GetDic<T>() where T : CsvBase
    {
		Type type = typeof(T);

		if (!dic.ContainsKey(type))
		{
            Console.WriteLine("not find: " + type);
		}
		
		return dic[type] as Dictionary<int, T>;
    }

    public static List<T> GetList<T>() where T : CsvBase
    {
		Type type = typeof(T);

		if(dicList.ContainsKey(type)){

			return dicList[type] as List<T>;

		}else{

	        Dictionary<int, T> dict = GetDic<T>();

	        List<T> list = new List<T>();

			Dictionary<int,T>.ValueCollection.Enumerator enumerator = dict.Values.GetEnumerator();

			while(enumerator.MoveNext()){

				list.Add(enumerator.Current);
			}

			dicList.Add(type,list);

	        return list;
		}
    }

	public static bool IsIDExists<T>(int _id)where T : CsvBase{

		Dictionary<int, T> dict = GetDic<T>();

		return dict.ContainsKey(_id);
	}
    
    public static void Dispose()
    {
        dic = new Dictionary<Type, IDictionary>();
    }

    public static void Load<T>(string _name) where T : CsvBase , new()
    {
        Type type = typeof(T);

        if (dic.ContainsKey(type))
        {
            return;
        }

        Dictionary<int, T> result = new Dictionary<int, T>();

		using (FileStream fs = new FileStream(path + "/" + _name + ".csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                int i = 0;

                string lineStr = reader.ReadLine();

                FieldInfo[] infoArr = null;

                while (lineStr != null)
                {
                    if (i == 2)
                    {
                        string[] dataArr = lineStr.Split(',');

                        infoArr = new FieldInfo[dataArr.Length];

                        for (int m = 1; m < dataArr.Length; m++)
                        {
                            infoArr[m] = type.GetField(dataArr[m]);
                        }
                    }
                    else if (i > 2)
                    {
                        string[] dataArr = lineStr.Split(',');

                        T csv = new T();

                        csv.ID = Int32.Parse(dataArr[0]);

                        for (int m = 1; m < infoArr.Length; m++)
                        {
                            FieldInfo info = infoArr[m];

                            if (info != null)
                            {
                                setData(info, csv, dataArr[m]);
                            }
                        }

						csv.Fix();

                        result.Add(csv.ID, csv);
                    }

                    i++;

                    lineStr = reader.ReadLine();
                }
            }
        }

        dic.Add(type, result);

        //        SuperDebug.Log("StaticData.Load" + Application.dataPath + path + _name + ".csv" + "  complete!!!");
    }

    private static void setData(FieldInfo _info, CsvBase _csv, string _data)
    {
        string str = "setData:" + _info.Name + "   " + _info.FieldType.Name + "   " + _data + "   " + _data.Length + Environment.NewLine;

        //SuperDebug.Log(str);
        try
        {
            switch (_info.FieldType.Name)
            {
                case "Int32":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, Int32.Parse(_data));
                    }

                    break;

                case "String":

                    _info.SetValue(_csv, _data);

                    break;

                case "Boolean":

                    _info.SetValue(_csv, _data == "1" ? true : false);

                    break;

                case "Single":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, float.Parse(_data));
                    }

                    break;

                case "Int32[]":

                    int[] intResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        intResult = new int[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            intResult[i] = Int32.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        intResult = new int[0];
                    }

                    _info.SetValue(_csv, intResult);

                    break;

                case "String[]":

                    string[] stringResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        stringResult = _data.Split('$');
                    }
                    else
                    {
                        stringResult = new string[0];
                    }

                    _info.SetValue(_csv, stringResult);

                    break;

                case "Boolean[]":

                    bool[] boolResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        boolResult = new bool[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            boolResult[i] = strArr[i] == "1" ? true : false;
                        }
                    }
                    else
                    {
                        boolResult = new bool[0];
                    }

                    _info.SetValue(_csv, boolResult);

                    break;

                default:

                    float[] floatResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        floatResult = new float[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            floatResult[i] = float.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        floatResult = new float[0];
                    }

                    _info.SetValue(_csv, floatResult);

                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(str + "   " + e.ToString());
        }
    }
}
