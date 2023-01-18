using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class paddle1 : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public float speed = 10.0f;
    public float boundY = 10f;
    private Rigidbody2D rb2d;
    
    static IPAddress mcastAddress;
    static int mcastPort;
    static Socket mcastSocket;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        if (Input.GetKey(moveUp)) {
            vel.y = speed;
        }
        else if (Input.GetKey(moveDown)) {
            vel.y = -speed;
        }
        else {
            vel.y = 0;
        }
        rb2d.velocity = vel;

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

        var position = transform.position;
        if (Input.anyKeyDown) {
            String msg = gameObject.name + " - Position: x = " + position.x + ", y = " + position.y;
            mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(msg), endPoint);
        }

            // Debug.Log("Multicast data sent.....");

        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.ToString());
        }

                    mcastSocket.Close();


        var pos = transform.position;
        if (pos.y > boundY) {
            Debug.Log("posY:" + pos.y + ", boundY:" + boundY);
            pos.y = boundY;
        }
        else if (pos.y < -boundY) {
            Debug.Log("posY:" + pos.y + ", boundY:" + boundY);
            pos.y = -boundY;
        }
        transform.position = pos;

    }
}
