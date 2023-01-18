using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Ball : NetworkBehaviour
    {
        public Rigidbody2D rb2d;

        public override void OnStartServer()
        {
            base.OnStartServer();
            Invoke("GoBall", 1);
        }

        void GoBall() {
            float rand = Random.Range (0, 2);
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

        void RestartGame() {
            ResetBall ();
            Invoke ("GoBall", 1);
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
            if (col.collider.CompareTag ("Wall")) {
                Debug.Log("Ball hit wall");
            }
        }
    }
}
