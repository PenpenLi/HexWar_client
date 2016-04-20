using UnityEngine;
using System.Collections;
using System.IO;
using System;

using xy3d.tstd.lib.wwwManager;
using System.Xml;
using System.Collections.Generic;
using xy3d.tstd.lib.publicTools;
using DamienG.Security.Cryptography;
using xy3d.tstd.lib.systemIO;

namespace xy3d.tstd.lib.versionControl{

	struct UpdateFileInfo{

		public int version;
		public int size;
		public string path;
		public UInt32 crc;
	}

	public class VersionControl{

#if PLATFORM_PC || PLATFORM_ANDROID

		private const string CODE_BYTES_NAME = "Script.bytes";

		private byte[] codeBytes;

#endif

		private const string FILE_NAME = "version.dat";

		private static VersionControl _Instance;

		public static VersionControl Instance{

			get{

				if(_Instance == null){

					_Instance = new VersionControl();
				}

				return _Instance;
			}
		}

		private VersionData data;

		public void Init(int _localVersion,int _remoteVersion,Func<int,string> _fixFun,Func<string,string> _fixFun2,Action _callBack,Action<string> _setTextCallBack,Action<float> _setPercentCallBack,int _updateWarningSize,Action<string,Action> _showWarningCallBack){

			if(File.Exists(Application.persistentDataPath + "/" + FILE_NAME)){

				data = VersionData.LoadFromFile(Application.persistentDataPath + "/" + FILE_NAME);

				if(_localVersion > data.version){//说明残留的version.dat是老版本的  必须立即清除掉

					SuperDebug.Log("发现残留的version.dat 删除掉!");

					data = new VersionData();
					
					data.version = _localVersion;

					VersionData.SaveToFile(Application.persistentDataPath + "/" + FILE_NAME,data);
				}					

			}else{

				SuperDebug.Log("这是第一次进游戏  生成新的version.dat");

				data = new VersionData();

				data.version = _localVersion;

				VersionData.SaveToFile(Application.persistentDataPath + "/" + FILE_NAME,data);
			}

			SuperDebug.Log("客户端资源版本号:" + data.version + "  服务器资源版本号:" + _remoteVersion);

			if(data.version < _remoteVersion){

				_setTextCallBack("读取热更新列表");

				Dictionary<string,UpdateFileInfo> dic = new Dictionary<string, UpdateFileInfo>();

				LoadUpdateXML(0,dic,_remoteVersion,_remoteVersion,_fixFun,_fixFun2,_callBack,_setTextCallBack,_setPercentCallBack,_updateWarningSize,_showWarningCallBack);

			}else{

				_callBack();
			}
		}

		private void LoadUpdateXML(int _allFileSize,Dictionary<string,UpdateFileInfo> _dic,int _remoteVersion,int _version,Func<int,string> _fixFun,Func<string,string> _fixFun2,Action _callBack,Action<string> _setTextCallBack,Action<float> _setPercentCallBack,int _updateWarningSize,Action<string,Action> _showWarningCallBack){

			if(_version > data.version){

				string url = _fixFun(_version);

				Action<WWW> callBack = delegate(WWW obj) {

					if(obj.error != null){

						_setTextCallBack("热更新列表加载失败");

						SuperDebug.Log("文件热更新失败 文件名:" + obj.url);
						
						return;
					}

					XmlDocument xmlDoc = new XmlDocument ();
					
					xmlDoc.LoadXml(PublicTools.XmlFix(obj.text));
					
					XmlNodeList hhh = xmlDoc.ChildNodes[0].ChildNodes;
					
					foreach(XmlNode node in hhh){

						string nodeUrl = node.InnerText;

						if(!_dic.ContainsKey(nodeUrl)){

							int fileSize = int.Parse(node.Attributes["size"].Value);

							if(fileSize == -1){

								if(data.dic.ContainsKey(nodeUrl)){

									data.dic.Remove(nodeUrl);
								}

							}else{

								_allFileSize += fileSize;

								UpdateFileInfo fileInfo = new UpdateFileInfo();

								fileInfo.version = _version;

								fileInfo.size = fileSize;

								fileInfo.path = node.Attributes["path"].Value;

								fileInfo.crc = Convert.ToUInt32(node.Attributes["crc"].Value,16);

								_dic.Add(nodeUrl,fileInfo);
							}
						}
					}

					LoadUpdateXML(_allFileSize,_dic,_remoteVersion,_version - 1,_fixFun,_fixFun2,_callBack,_setTextCallBack,_setPercentCallBack,_updateWarningSize,_showWarningCallBack);
				};

				WWWManager.Instance.LoadRemote(url,callBack);

			}else{

				if(_allFileSize >= _updateWarningSize && _showWarningCallBack != null){
					
					Action callBack = delegate() {
						
						LoadUpdateXMLOK(_dic,_remoteVersion,_fixFun2,_callBack,_setTextCallBack,_setPercentCallBack,_allFileSize);
					};

					float tempnum = (float)Math.Round(((float)_allFileSize) / 1024 / 1024, 2);
					
					string msg = "需要更新" + tempnum.ToString() + "MB大小的新文件，确认更新吗？";

					_showWarningCallBack(msg,callBack);

				}else{

					LoadUpdateXMLOK(_dic,_remoteVersion,_fixFun2,_callBack,_setTextCallBack,_setPercentCallBack,_allFileSize);
				}
			}
		}

		private void LoadUpdateXMLOK(Dictionary<string,UpdateFileInfo> _dic,int _remoteVersion,Func<string,string> _fixFun,Action _callBack,Action<string> _setTextCallBack,Action<float> _setPercentCallBack,int _allFileSize){

			int loadNum = _dic.Count;

			int loadSize = 0;

			foreach(KeyValuePair<string,UpdateFileInfo> pair in _dic){

				string path = pair.Key;

				UpdateFileInfo info = pair.Value;

				string url = _fixFun(info.path + path);

				Action<WWW> callBack = delegate(WWW obj) {

					if(obj.error != null){

						_setTextCallBack("文件热更新失败 文件名:" + obj.url);

						SuperDebug.Log("文件热更新失败 文件名:" + obj.url);

						return;
					}

					UInt32 crc = CRC32.Compute(obj.bytes);

					if(crc != info.crc){

						_setTextCallBack("文件热更新CRC比对错误 文件名:" + obj.url);

						SuperDebug.Log("文件热更新CRC比对错误 文件名:" + obj.url);
						
						return;
					}

#if PLATFORM_PC || PLATFORM_ANDROID

					if(path.Equals(CODE_BYTES_NAME)){

						codeBytes = obj.bytes;

					}else{

						SystemIO.SaveFile(Application.persistentDataPath + "/" + path,obj.bytes);
					}

#else

					SystemIO.SaveFile(Application.persistentDataPath + "/" + path,obj.bytes);

#endif

					if(data.dic.ContainsKey(path)){

						data.dic[path] = info.version;

					}else{

						data.dic.Add(path,info.version);
					}

					loadNum--;

					//_setTextCallBack("正在更新:" + (_dic.Count - loadNum) + "/" + _dic.Count);
					
					loadSize += info.size;

                    _setTextCallBack("正在更新:" + Math.Round((float)loadSize / (float)_allFileSize * 100) + "%（"+Math.Round((float)loadSize/1024/1024,1)+ "/" +Math.Round((float)_allFileSize/1024/1024,1) +"）" );

					_setPercentCallBack((float)loadSize / (float)_allFileSize);

					if(loadNum == 0){

						UpdateOver(_remoteVersion,_callBack);
					}
				};

				WWWManager.Instance.LoadRemote(url,callBack);
			}
		}

		private void UpdateOver(int _remoteVersion,Action _callBack){

			data.version = _remoteVersion;

			VersionData.SaveToFile(Application.persistentDataPath + "/" + FILE_NAME,data);

#if PLATFORM_PC || PLATFORM_ANDROID
			
			if(codeBytes != null){
				
				SystemIO.SaveFile(Application.persistentDataPath + "/" + CODE_BYTES_NAME,codeBytes);
				
				codeBytes = null;

				UseFulUtil.Instance.ShowDialogOne("更新已完成，请重启游戏。","",QuitApplication);

			}else{

				_callBack();
			}

#else
			_callBack();

#endif
		}

		private void QuitApplication(){

			Application.Quit();
		}

		public bool FixUrl(ref string _path){

			if(data != null){

				if(data.dic.ContainsKey(_path)){

#if PLATFORM_PC

					_path = "file:///" + Application.persistentDataPath + "/" + _path;

#elif PLATFORM_ANDROID

					_path = "file:///" + Application.persistentDataPath + "/" + _path;

#else
					_path = "file:///" + Application.persistentDataPath + "/" + _path;

#endif

					return true;

				}else{

					return false;
				}

			}else{

				return false;
			}
		}
	}
}