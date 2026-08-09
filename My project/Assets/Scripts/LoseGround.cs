using UnityEngine;
using UnityEngine.UI;

public class LoseGround : MonoBehaviour
{
    public LayerMask hit_layer;

    void OnCollisionEnter(Collision c)
    {
        if ((hit_layer.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            print("lose");
            GameManager.Lose();

            Vector2 collision_point = Vector2.zero;

            for(var i = 0 ; i < c.contacts.Length; i++)
            {
                collision_point = new Vector2(collision_point.x + c.contacts[i].point.x, collision_point.y + c.contacts[i].point.y);
            }

            collision_point = new Vector2(collision_point.x / c.contacts.Length, collision_point.y / c.contacts.Length);
            XFailure.Position(collision_point);
        }
    }
}
