using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using NetworkAPI;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public KeyCode moveUp = KeyCode.W;
        public KeyCode moveDown = KeyCode.S;
        public float speed = 10.0f;
        public float boundY = 10f;
        public Rigidbody2D rigidbody2d;
        NetworkComm networkComm;

        void Start()
        {
            networkComm = new NetworkComm();
            networkComm.MsgReceived += new NetworkComm.MsgHandler(processMsg);
            (new Thread(new ThreadStart(networkComm.ReceiveMessages))).Start();
        }

        private void processMsg(String message)
        {
            // Debug.Log("From Delegate:  " + message);
        }

        void Update()
        {
            var position = transform.position;
            if(Input.anyKeyDown){
                String msg = gameObject.name + " - Position: x = " + position.x + ", y = " + position.y;
                networkComm.sendMessage(msg);
            }
        }

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
