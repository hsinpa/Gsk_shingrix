using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;


public class SocketConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var manager = new SocketManager(new System.Uri("http://localhost:3000"));

        manager.Socket.On("connect", () => Debug.Log(manager.Handshake.Sid));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
