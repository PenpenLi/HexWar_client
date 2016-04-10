using System.Xml;
using System.IO;
using System.Reflection;
using System;

public class Config
{
    public void LoadLocalConfig(string _path)
    {
        using (StreamReader sr = new StreamReader(_path))
        {
            string str = sr.ReadToEnd();

            sr.Close();

            SetData(str);
        }
    }

    public virtual void SetData(string _str)
    {
        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(XmlFix(_str));

        XmlNodeList hhh = xmlDoc.DocumentElement.ChildNodes;

        foreach (XmlNode node in hhh)
        {
            if (node.Name != "#comment")
            {
                FieldInfo field = this.GetType().GetField(node.Name);

                if (field != null)
                {
                    if (field.FieldType.IsGenericType)
                    {
                        object list = Activator.CreateInstance(field.FieldType);

                        field.SetValue(this, list);

                        Type type = field.FieldType.GetGenericArguments()[0];

                        foreach (XmlNode n in node.ChildNodes)
                        {
                            field.FieldType.GetMethod("Add").Invoke(list, new object[] { SetOneData(type, n.InnerText) });
                        }
                    }
                    else if (field.FieldType.IsArray)
                    {
                        Type type = field.FieldType.GetElementType();

                        Array arr = Array.CreateInstance(type, node.ChildNodes.Count);

                        field.SetValue(this, arr);

                        int i = 0;

                        foreach (XmlNode n in node.ChildNodes)
                        {
                            arr.SetValue(SetOneData(type, n.InnerText), i);

                            i++;
                        }
                    }
                    else
                    {
                        SetOneData(field, node.InnerText);
                    }
                }
                else
                {
                    PropertyInfo property = this.GetType().GetProperty(node.Name);

                    if (property != null)
                    {
                        SetOneData(property, node.InnerText);
                    }
                }
            }
        }
    }

    private static string XmlFix(string _str)
    {
        int index = _str.IndexOf("<");

        if (index == -1)
        {
            return "";
        }
        else
        {
            return _str.Substring(index, _str.Length - index);
        }
    }

    private object SetOneData(Type _type, string _data)
    {
        switch (_type.Name)
        {
            case "Int32":

                return int.Parse(_data);

            case "Boolean":

                return _data.Equals("1") ? true : false;

            case "String":

                return _data;

            case "Single":

                return float.Parse(_data);

            case "Double":

                return double.Parse(_data);
        }

        return null;
    }

    private void SetOneData(FieldInfo _field, string _data)
    {
        switch (_field.FieldType.Name)
        {
            case "Int32":

                _field.SetValue(this, int.Parse(_data));

                break;

            case "Boolean":

                _field.SetValue(this, _data.Equals("1") ? true : false);

                break;

            case "String":

                _field.SetValue(this, _data);

                break;

            case "Single":

                _field.SetValue(this, float.Parse(_data));

                break;

            case "Double":

                _field.SetValue(this, double.Parse(_data));

                break;
        }
    }

    private void SetOneData(PropertyInfo _property, string _data)
    {
        switch (_property.PropertyType.Name)
        {
            case "Int32":

                _property.SetValue(this, int.Parse(_data), null);

                break;

            case "Boolean":

                _property.SetValue(this, _data.Equals("1") ? true : false, null);

                break;

            case "String":

                _property.SetValue(this, _data, null);

                break;

            case "Single":

                _property.SetValue(this, float.Parse(_data), null);

                break;

            case "Double":

                _property.SetValue(this, double.Parse(_data), null);

                break;
        }
    }
}
