using UnityEngine;
using System.Collections;
using System;

public class SuperDebug{

	public static void Log(object _str){

		Debug.Log(_str);

		RecordLog.Write(_str as String);
	}

	public static void Log(object _str,UnityEngine.Object _context){

		Debug.Log(_str,_context);
	}

	public static void LogError(object _str){

		Debug.LogError(_str);
	}
	
	public static void LogError(object _str,UnityEngine.Object _context){

		Debug.LogError(_str,_context);
	}

	public static void LogWarning(object _str){

		Debug.LogWarning(_str);
	}
	
	public static void LogWarning(object _str,UnityEngine.Object _context){

		Debug.LogWarning(_str,_context);
	}

	public static void LogWarningFormat(string format, params object[] args){

		Debug.LogWarningFormat(format,args);
	}

	public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args){

		Debug.LogWarningFormat(context,format,args);
	}

	public static void LogException(Exception _str){

		Debug.LogException(_str);
	}
	
	public static void LogException(Exception _str,UnityEngine.Object _context){

		Debug.LogException(_str,_context);
	}

	public static void LogErrorFormat(string format, params object[] args){

		Debug.LogErrorFormat(format,args);
	}

	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args){

		Debug.LogErrorFormat(context,format,args);
	}
}
