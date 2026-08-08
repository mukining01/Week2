using UnityEngine;

public class TurnToStone : MonoBehaviour
{
    public Rigidbody[] RigidBodies;
    public Clickable[] Clickables;

    public void Stone()
    {
        for (var i = 0;  i < RigidBodies.Length; i++)
        {
            RigidBodies[i].isKinematic = true;
        }

        for (var i = 0; i < Clickables.Length; i++)
        {
            Clickables[i].enabled = false;
        }
    }
}
