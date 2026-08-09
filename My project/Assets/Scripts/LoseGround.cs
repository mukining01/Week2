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
        }
    }
}
