using UnityEngine;
using System.Collections;
using System.IO;
using xy3d.tstd.lib.publicTools;
using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

public class RecordLog{

    private const string ERROR_PATH = "/error";

	private const string LOG_FILE_NAME = "tstd_log.dat";

    private static string[] filterError = new string[] { "EGL" };

	private static StreamWriter sw;

	private static bool isStart = false;

//	[RuntimeInitializeOnLoadMethod]
	public static void Start(){

		Application.logMessageReceived += UnityLogCallback;

		string path = Application.persistentDataPath + "/" + LOG_FILE_NAME;

		if(File.Exists(path)){

			File.Delete(path);
		}

		sw = new StreamWriter(path);

		isStart = true;
	}

    private static List<string> errorPathList;

    public static void SendErrorFile()
    {
        if (Directory.Exists(Application.persistentDataPath + ERROR_PATH))
        {
            string[] errorPaths = Directory.GetFiles(Application.persistentDataPath + ERROR_PATH);

            if (errorPaths.Length > 0)
            {
                errorPathList = new List<string>(errorPaths);

                beginGetRequestStream();
            }
           
        }
    }


    private static void beginGetRequestStream()
    {
        if (errorPathList != null && errorPathList.Count > 0)
        {
            HttpWebRequest request = WebRequest.Create(@"http://up.qiniu.com/") as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = "---------------------------7db1851cd1158";
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
        }
        
    }

    private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
    {
        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

        string path = errorPathList[0];
        int pos = path.LastIndexOf("\\");
        string fileName = path.Substring(pos + 1);
        ////请求头部信息 
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bArr = new byte[fs.Length];
        fs.Read(bArr, 0, bArr.Length);
        fs.Close();
        Stream postStream = request.EndGetRequestStream(asynchronousResult);

        MsMultiPartFormData form = new MsMultiPartFormData(postStream);
        //form.AddFormField("token", "UmyhgwyTCXazlqnBh6sypTbxuCrH0VUQ8vjNoPzT:lf4Kcq7RrRrg508o25YxdusZyTU=:eyJzY29wZSI6InRzdGRkdW1wIiwiZGVhZGxpbmUiOjE0NTQzNTMyNTZ9");
        form.AddFormField("token", "UmyhgwyTCXazlqnBh6sypTbxuCrH0VUQ8vjNoPzT:nMp6tOdlV9jz4fsGvju-Kv2vvik=:eyJzY29wZSI6InRzdGRkdW1wIiwiZGVhZGxpbmUiOjE3NzExNDY5NjN9");
        
        form.AddFormField("key", path);

        form.AddStreamFile("file", fileName, bArr);
        form.PrepareFormData();

        postStream.Close();

        request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
    }

    private static void GetResponseCallback(IAsyncResult asynchronousResult)
    {
        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

        // End the operation
        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
        Stream streamResponse = response.GetResponseStream();
        StreamReader streamRead = new StreamReader(streamResponse);
        string responseString = streamRead.ReadToEnd();
        // Close the stream object
        streamResponse.Close();
        streamRead.Close();

        // Release the HttpWebResponse
        response.Close();

        File.Delete(errorPathList[0]);

        errorPathList.RemoveAt(0);

        beginGetRequestStream();
    }

	private static void UnityLogCallback(string condition, string stackTrace, LogType type)
	{
		string str = "";

		switch (type) {
			
		case LogType.Assert:
		case LogType.Error:
		case LogType.Exception:
		case LogType.Warning:
			str = string.Format("{0}, {1}",condition,stackTrace);
			break;

		case LogType.Log:
//			str = string.Format("{0}",condition);
			break;
			
		}

        Write(str);

        bool tempbool = true;

        foreach (string f in filterError)
        {
            if (condition.IndexOf(f) != -1)
            {
                tempbool = false;
                break;
            }
        }
        if (tempbool)
        {
            string errorStr;

            switch (type)
            {

                case LogType.Error:
                case LogType.Exception:

                    try
                    {
                        if (!Directory.Exists(Application.persistentDataPath + ERROR_PATH))
                        {
                            Directory.CreateDirectory(Application.persistentDataPath + ERROR_PATH);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally { }

					Stop();

					FileInfo fi = new FileInfo(Application.persistentDataPath + "/" + LOG_FILE_NAME);

					fi.CopyTo(Application.persistentDataPath + ERROR_PATH + "/" + (DateTime.Now.ToFileTime().ToString()) + ".error");

//                    errorStr = string.Format("{0}, {1}", condition, stackTrace);
//                    StreamWriter errorSW = new StreamWriter(Application.persistentDataPath + ERROR_PATH + "/" + (DateTime.Now.ToFileTime().ToString()) + ".error");
//                    errorSW.WriteLine(errorStr);
//
//                    errorSW.Flush();
//                    errorSW.Close();

                    Action quitCall = delegate()
                    {
                        Application.Quit();
                    };
#if PLATFORM_ANDROID
        UseFulUtil.Instance.ShowDialogOne("游戏出错,请退出游戏再进入!!!", "出错", quitCall);
#endif
                    break;

            }

        }
	}

	public static void Write(string _str){

		if(isStart){

			sw.WriteLine(_str);
			
			sw.Flush();
		}
	}

	public static void Stop(){

		isStart = false;

		sw.Close();
	}
}
