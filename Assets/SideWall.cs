using System.Collections;
using UnityEngine;
using Mirror.Examples.Pong;

public class SideWall : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D hitInfo) {
        if (hitInfo.name == "Ball" || hitInfo.CompareTag("Ball"))
        {
            string wallName = transform.name;
            NetworkManagerPong.Score(wallName);
            hitInfo.gameObject.SendMessage("RestartGame", 1.0f, SendMessageOptions.RequireReceiver);
        }
    }
}