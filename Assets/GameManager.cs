using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public GUISkin layout;
    GameObject theBall;

    static IPAddress mcastAddress;
    static int mcastPort;
    static Socket mcastSocket;

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
    }
    
    public static void Score (string wallID) {
        mcastAddress = IPAddress.Parse("230.0.0.1");
        mcastPort = 11000;
        IPEndPoint endPoint;

        try
        {
            mcastSocket = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram,
                           ProtocolType.Udp);

            //Send multicast packets to the listener.
            endPoint = new IPEndPoint(mcastAddress, mcastPort);
            if (wallID == "RightWall")
            {
                PlayerScore2++;
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Player 2 Scored"), endPoint);
            } else
            {
                PlayerScore1++;
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Player 1 Scored"), endPoint);
            }
            Debug.Log("Multicast data sent.....");
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.ToString());
        }

        mcastSocket.Close();


    }
    
    void OnGUI () {
        GUI.skin = layout;
        GUI.skin.label.fontSize = Screen.width / Screen.height * 15;
        GUI.Label(new Rect(Screen.width / 2 - 150 - 15, 15, 100, 100), "Player 1");
        GUI.Label(new Rect(Screen.width / 2 - 150 - 15, 30, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 15, 15, 100, 100), "Player 2");
        GUI.Label(new Rect(Screen.width / 2 + 150 + 15, 30, 100, 100), "" + PlayerScore2);

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theBall.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (PlayerScore1 == 5)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        } else if (PlayerScore2 == 5)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
    }
    // Update is called once per frame
}
