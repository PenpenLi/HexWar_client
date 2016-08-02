using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class Connection : MonoBehaviour {

	private static Connection _Instance;

	public static Connection Instance{

		get{

			if(_Instance == null){

				GameObject go = new GameObject("Connection");

				_Instance = go.AddComponent<Connection>();
			}

			return _Instance;
		}
	}

	private const int HEAD_LENGTH = 2;
	
	private ushort bodyLength = 0;
	private ushort headLength = 0;
	private byte[] headBuffer = new byte[HEAD_LENGTH];
	private byte[] bodyBuffer = new byte[ushort.MaxValue];
	private int headOffset = 0;
	private int bodyOffset = 0;

	private Socket socket;

	private Action<byte[]> receiveCallBack;

	private bool isReceivingHead = true;

	private bool isConnect = false;

	public void Init(string _ip, int _port, Action<byte[]> _receiveCallBack){

		receiveCallBack = _receiveCallBack;

		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(_ip), _port);
		
		socket.Connect(ipe);

		isConnect = true;
	}

	void Update(){

		if (!isConnect) {

			return;
		}

		if (isReceivingHead) {

			ReceiveHead ();

		} else {

			ReceiveBody ();
		}
	}

	private void ReceiveHead(){

		if (socket.Available >= HEAD_LENGTH)
		{
			socket.Receive(headBuffer, HEAD_LENGTH, SocketFlags.None);
			
			isReceivingHead = false;
			
			bodyLength = BitConverter.ToUInt16(headBuffer, 0);
			
			ReceiveBody();
		}
	}

	private void ReceiveBody(){

		if(socket.Available >= bodyLength)
		{
			socket.Receive(bodyBuffer, bodyLength, SocketFlags.None);
			
			isReceivingHead = true;
			
			byte[] result = new byte[bodyLength];

			Array.Copy(bodyBuffer,result,bodyLength);

			receiveCallBack(result);
		}
	}

	public void Send(MemoryStream _ms){

		int length = HEAD_LENGTH + (int)_ms.Length;
		
		byte[] bytes = new byte[length];
		
		Array.Copy(BitConverter.GetBytes((ushort)_ms.Length), bytes, HEAD_LENGTH);
		
		Array.Copy(_ms.GetBuffer(), 0, bytes, HEAD_LENGTH, _ms.Length);
		
		socket.BeginSend(bytes, 0, length, SocketFlags.None, SendCallBack, null);
	}

	private void SendCallBack(IAsyncResult result)
	{
		
	}
}
