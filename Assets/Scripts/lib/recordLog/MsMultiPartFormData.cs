using System.IO;
using System.Text;
public class MsMultiPartFormData  
{ 
	public string Boundary= "---------------------------7db1851cd1158"; 
		
	private string fieldName="Content-Disposition: form-data; name=\"XXXX\""; 
		
	private string fieldValue= "XXXX"; 
		
	private string fileField="Content-Disposition: form-data; name=\"XXXX\"; filename=\"XXXXXXXX\""; 
		
	private string fileContentType= "Content-Type: XXXX"; 
		
	private Stream formData;  
		
	/** 
		* ... 
		* @author qwliang 
		*/ 
	public MsMultiPartFormData (Stream s) 
	{ 
		formData=s; 
	} 
		
	/** 
		* 添加一个字段数据到From的数据包中 
		* @author qwliang 
		*/ 
	public void AddFormField(string FieldName,  string FieldValue) 
	{ 
		string newFieldName=fieldName; 
		string newFieldValue=fieldValue; 
			
		newFieldName=newFieldName.Replace("XXXX",FieldName); 
		newFieldValue=newFieldValue.Replace("XXXX",FieldValue); 
			
        WriteStringToStream("--" + Boundary + "\r\n");

        WriteStringToStream(newFieldName + "\r\n\r\n");

        WriteStringToStream(newFieldValue + "\r\n");
	} 
		
		
	/** 
		* 添加一个文件二进流数据到Form的数据包中，并指定二进流数据的类型 
		* @author qwliang 
		*/ 
	public void AddFile(string FieldName, string FileName,byte[] FileContent, string ContentType) 
	{ 
		string newFileField=fileField; 
		string newFileContentType=fileContentType; 
			
		newFileField=newFileField.Replace("XXXX",FieldName); 
		newFileField=newFileField.Replace("XXXXXXXX",FileName); 
			
		newFileContentType=newFileContentType.Replace("XXXX",ContentType);

        WriteStringToStream("--" + Boundary + "\r\n");
        WriteStringToStream(newFileField + "\r\n");
        WriteStringToStream(newFileContentType + "\r\n");
        WriteStringToStream("Content-Transfer-Encoding:binary\r\n\r\n");

        formData.Write(FileContent, 0, FileContent.Length);

        WriteStringToStream("\r\n"); 
	} 
		
	/** 
		* 添加一个文件二进流数据到Form的数据包中 
		* @author qwliang 
		*/ 
	public void AddStreamFile(string FieldName, string FileName,byte[] FileContent) 
	{ 
		AddFile( FieldName, FileName, FileContent,"application/octet-stream"); 
	} 
		
	/** 
		* 把Form中所有的字段与二进制流数据打包成一个完整的From数据包 
		* @author qwliang 
		*/ 
	public void PrepareFormData() 
	{
        WriteStringToStream("--" + Boundary + "--"); 
	}

    public void WriteStringToStream(string str)
    {
        byte[] strBytes = Encoding.UTF8.GetBytes(str);
        formData.Write(strBytes, 0, strBytes.Length);
    }
		
	/** 
		* 获得From的完整数据 
		* @author qwliang 
		*/ 
	public Stream GetFormData() 
	{ 
		return formData; 
	} 
}