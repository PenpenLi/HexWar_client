using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = default(T);
    private static object _lock = new object();
    private static bool _appIsQuit = false;


    public static T Instance {
        get {

            if (_appIsQuit) {
                SuperDebug.LogWarning("Singleton instance["+typeof(T)+"] is destoryed on application quite,won't create again,return null");
                return null;
            }

            lock (_lock) {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1) {
                        SuperDebug.LogError("the Sigleton [" + typeof(T).ToString() + "] numbers is more than one");
                        return _instance;
                    }

                    if (_instance == null) {

                        GameObject singleTon = new GameObject();
                        singleTon.name = typeof(T).ToString();
                        _instance = singleTon.AddComponent<T>();
                        DontDestroyOnLoad(singleTon);
                    }
                }
            }
            return _instance;
        }
    }

    public void OnDestory() {
        _appIsQuit = true;
    }


}
