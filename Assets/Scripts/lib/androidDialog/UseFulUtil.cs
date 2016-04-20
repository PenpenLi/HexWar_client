using UnityEngine;
using System.Collections;
using System;

public class UseFulUtil : MonoBehaviour {

    private static UseFulUtil _instance;

    public static UseFulUtil Instance{
        get {
            if (_instance == null) {
                GameObject go = new GameObject("ThirdUtil");
                GameObject.DontDestroyOnLoad(go);
                _instance = go.AddComponent<UseFulUtil>();
            }
            return _instance;
        }   
    }


    private Action _confirmCallBack = null;
    private Action _cancelCallBack = null;

    /// <summary>
    /// 这个函数用来调用android原生的对话框，对话框只有一个确定的按钮
    /// </summary>
    /// <param name="msg">对话框显示的内容</param>
    /// <param name="title">对话框的标题</param>
    /// <param name="confirmCallback">对话框确定按钮的回调函数，不想传的话设置传null</param>
   
    public void ShowDialogOne(string msg,string title,Action confirmCallback) {
#if PLATFORM_ANDROID
        _confirmCallBack = confirmCallback;
        using (AndroidJavaClass ajc = new AndroidJavaClass("mobi.xy3d.tstd.DialogUtil")) {
            ajc.CallStatic("ShowDialogConfirm", new object[] { msg, title });
        }
#endif
    }

    /// <summary>
    /// 这个函数用来调用android原生的对话框，对话框有确定和取消的按钮
    /// </summary>
    /// <param name="msg">对话框显示描述内容</param>
    /// <param name="title">对话框的标题</param>
    /// <param name="comfirmCallBack">确定之后的回调</param>
    /// <param name="cancelCallBack">取消之后的回调</param>
    /// 
    public void ShowDialogTwo(string msg,string title,Action comfirmCallBack,Action cancelCallBack) {

#if PLATFORM_ANDROID
        _confirmCallBack = comfirmCallBack;
        _cancelCallBack = cancelCallBack;
        using (AndroidJavaClass ajc = new AndroidJavaClass("mobi.xy3d.tstd.DialogUtil"))
        {
            ajc.CallStatic("ShowDialogConfirmCancel", new object[] { msg, title });    
        }
#endif

    }

    public void Confirm(string data) {
        if (_confirmCallBack != null) {
            Action temp = _confirmCallBack;
            _confirmCallBack = null;
            temp();
        }
    }

    public void Cancel(string data) {
        if (_cancelCallBack != null) {
            Action temp = _cancelCallBack;
            _cancelCallBack = null;
            temp();
        }
    }


}
