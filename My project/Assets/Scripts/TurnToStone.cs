using UnityEngine;

public class TurnToStone : MonoBehaviour
{
    public LayerMask mask;

    public Rigidbody[] RigidBodies;
    public Clickable[] Clickables;

    bool stone = false;
    bool turned_already = false;

    Vector3 past_position = Vector3.zero;
    Vector3 new_position = Vector3.zero;


    public Material[] all_stone_materials = new Material[3];

    float _check_time = 0;

    public void FixedUpdate()
    {
        if(stone) _check_time -= Time.fixedDeltaTime;

        if (stone && _check_time <= 0)
        {
            _check_time = 0.1f;

            if (new_position == past_position && past_position != Vector3.zero)
            {
                GameManager.Calculate_New_Height(transform.localPosition.y);
                print("pos: " + new_position + ", old pos: " + past_position);
                stone = false;
            }
        }

        Set_Positions();
    }

    public void Stone(int freezeType)
    {

        _check_time = 0.1f;

        Set_Positions();
        stone = true;
        turned_already = true;

        for (var i = 0;  i < RigidBodies.Length; i++)
        {
            RigidBodies[i].constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            RigidBodies[i].constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            MeshRenderer mesh = RigidBodies[i].gameObject.GetComponent<MeshRenderer>();

            if (mesh != null) mesh.material = all_stone_materials[freezeType];
        }

        for (var i = 0; i < Clickables.Length; i++)
        {
            Clickables[i].enabled = false;
        }
    }

    public void Set_Positions()
    {
        past_position = new_position;
        new_position = new Vector3(Mathf.Floor(transform.position.x * 100) / 100, Mathf.Floor(transform.position.y * 100) / 100, Mathf.Floor(transform.position.z * 100) / 100);
    }

    public void Set_Kinematic()
    {
        for (var i = 0; i < RigidBodies.Length; i++)
        {
            RigidBodies[i].isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (turned_already == true) return;
        if ((mask.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) return;
            AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.CollideEvent, transform.position);
    }
}
