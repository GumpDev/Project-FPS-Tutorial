using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerManager : MonoBehaviour {

    public Camera cam;
    public GameObject player;

	public void CreateServer(string name)
    {
        Network.InitializeServer(10, 25565);
        MasterServer.RegisterHost("FPS2SessonMP",name);
        cam.gameObject.SetActive(false);
        Network.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    public void Connect(string ip)
    {
        Network.Connect(ip, 25565);
    }

    private void OnConnectedToServer()
    {
        cam.gameObject.SetActive(false);
        Network.Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
