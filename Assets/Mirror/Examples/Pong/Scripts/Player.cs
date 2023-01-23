using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public KeyCode moveUp = KeyCode.W;
        public KeyCode moveDown = KeyCode.S;
        public float speed = 10.0f;
        public float boundY = 10f;
        public Rigidbody2D rigidbody2d;

        static IPAddress mcastAddress;
        static int mcastPort;
        static Socket mcastSocket;

        // need to use FixedUpdate for rigidbody
        void FixedUpdate()
        {
            // only let the local player control the racket.
            // don't control other player's rackets
            if (isLocalPlayer) {
                var vel = rigidbody2d.velocity;
                if (Input.GetKey(moveUp)) {
                    vel.y = speed;
                }
                else if (Input.GetKey(moveDown)) {
                    vel.y = -speed;
                }
                else {
                    vel.y = 0;
                }
                rigidbody2d.velocity = vel;

                // mcastAddress = IPAddress.Parse("230.0.0.1");
                // mcastPort = 11000;
                // IPEndPoint endPoint;

                // try
                // {
                //     mcastSocket = new Socket(AddressFamily.InterNetwork,
                //                 SocketType.Dgram,
                //                 ProtocolType.Udp);

                //     //Send multicast packets to the listener.
                //     endPoint = new IPEndPoint(mcastAddress, mcastPort);
                //     // Debug.Log("Multicast data sent.....");

                // }
                // catch (Exception e)
                // {
                //     Console.WriteLine("\n" + e.ToString());
                // }
                // mcastSocket.Close();

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
    }
}
