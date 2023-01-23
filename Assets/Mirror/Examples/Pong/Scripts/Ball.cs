using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using NetworkAPI;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public Rigidbody2D rb2d;

        public GUISkin layout;

        public NetworkComm networkComm;

        [SyncVar]
        public int PlayerScore1 = 0;
        [SyncVar]
        public int PlayerScore2 = 0;


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

        public override void OnStartServer()
        {
            base.OnStartServer();
            Invoke("GoBall", 1);
        }

        void Update(){
            var position = transform.position;
            String msg = gameObject.name + " - Position: x = " + position.x + ", y = " + position.y;
            networkComm.sendMessage(msg);
        }

        void GoBall() {
            float rand = UnityEngine.Random.Range (0, 2);
            if (rand < 1) {
                rb2d.AddForce(new Vector2 (20, -15));
            } else {
                rb2d.AddForce(new Vector2 (-20, -15));
            }
        }

        void ResetBall() {
            rb2d.velocity = new Vector2 (0, 0);
            transform.position = Vector2.zero;
        }

        public void RestartGame() {
            ResetBall ();
            Invoke ("GoBall", 1);
        }

        [ServerCallback]
        void RestartRound() {
            if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
            {
                PlayerScore1 = 0;
                PlayerScore2 = 0;
                this.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
            }
        }

        // only call this on server
        [ServerCallback]
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.CompareTag ("Player")) {
                Vector2 vel;
                vel.x = rb2d.velocity.x;
                vel.y = (rb2d.velocity.y / 2.0f) + (col.collider.attachedRigidbody.velocity.y / 3.0f);
                rb2d.velocity = vel;
            }
            Score(col.collider.name.ToString());
        }

        public void Score (string wallID) {
            if (wallID == "RightWall")
            {
                PlayerScore2++;
                this.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
            } else if (wallID == "LeftWall")
            {
                PlayerScore1++;
                this.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
            }
        }
    
    void OnGUI () {
        GUI.skin = layout;
        GUI.skin.label.fontSize = Screen.width / Screen.height * 15;
        GUI.Label(new Rect(Screen.width / 2 - 150 - 15, 15, 100, 100), "Player 1");
        GUI.Label(new Rect(Screen.width / 2 - 150 - 15, 30, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 15, 15, 100, 100), "Player 2");
        GUI.Label(new Rect(Screen.width / 2 + 150 + 15, 30, 100, 100), "" + PlayerScore2);

        if (PlayerScore1 == 5)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            this.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
            this.SendMessage("RestartRound", null, SendMessageOptions.RequireReceiver);
        } else if (PlayerScore2 == 5)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            this.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
            this.SendMessage("RestartRound", null, SendMessageOptions.RequireReceiver);
        }
    }
    }
}
