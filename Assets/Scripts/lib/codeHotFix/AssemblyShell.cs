using UnityEngine;
using System.Collections;

public class AssemblyShell : MonoBehaviour {

	public static readonly string ASSEMBLY_FILE_NAME = "Script.bytes";

	// Use this for initialization
	void Start () {

#if PLATFORM_PC

		AssemblyManager.Instance.Init("file:///" + Application.persistentDataPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyUpdated);

#elif PLATFORM_ANDROID

		AssemblyManager.Instance.Init("file://" + Application.persistentDataPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyUpdated);

#elif PLATFORM_IOS

		AssemblyManager.Instance.Init("file:" + Application.persistentDataPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyUpdated);
#endif
	}

	private void GetAssemblyUpdated(bool _result){
		
		if(_result){

			SuperDebug.Log("找到了新的代码文件！！！");
			
		}else{

			SuperDebug.Log("没有找到新的代码文件！！！");
		}

		Application.LoadLevel("main");
	}
}
