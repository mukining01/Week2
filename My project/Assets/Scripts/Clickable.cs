using UnityEngine;
using UnityEngine.UI;

public class Clickable : MonoBehaviour
{
    public Camera myCamera;
    bool clicking = false;

    Rigidbody myRigidbody;
    public LayerMask hit_layer = 1 << 7;
    public LayerMask hit_layer_body = 1 << 8;

    public Material ClickMaterial;
    Material CurrentMaterial;
    MeshRenderer myMeshRenderer;

    private void Awake()
    {
        myCamera = Camera.main;
        myRigidbody = GetComponent<Rigidbody>();

        myMeshRenderer = GetComponent<MeshRenderer>();
        CurrentMaterial = myMeshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            clicking = false;
            myMeshRenderer.material = CurrentMaterial;
        }
        else if (Input.GetMouseButtonDown(0)) OnClick();

        if (clicking) MovementBehaviour();
    }

    public void OnClick()
    {
        Vector3 mouse_position = Input.mousePosition;

        Ray myRay = myCamera.ScreenPointToRay(mouse_position);

        RaycastHit raycastHit;
        RaycastHit raycastHit_1;

        bool _weHiSomething_1 = Physics.Raycast(myRay, out  raycastHit, Mathf.Infinity, hit_layer);
        bool _weHiSomething_2 = Physics.Raycast(myRay, out raycastHit_1, Mathf.Infinity, hit_layer_body);

        if (_weHiSomething_1 || _weHiSomething_2)
        {
            if (!Check_Hit_Is_Child(raycastHit) && !Check_Hit_Is_Child(raycastHit_1)) return;

            clicking = true;
            myMeshRenderer.material = ClickMaterial;
        }
    }

    public void MovementBehaviour()
    {
        Vector3 mouse_position = Input.mousePosition;

        Vector3 new_pos = new Vector3(myCamera.ScreenToWorldPoint(mouse_position).x, myCamera.ScreenToWorldPoint(mouse_position).y, 0);
        // transform.position = Vector3.Lerp(transform.position, new_pos, 0.1f)
        float _distance = Vector3.Distance(transform.position, new_pos);
        Vector2 _force = MathH.Direction_Between_Two_Vectors(transform.position, new_pos);
        myRigidbody.AddForce(_force * _distance * 10);
    }

    public bool Check_Hit_Is_Child(RaycastHit hit)
    {
        if(hit.collider == null)
        {
            print("no col _ 1");
            return false;
        }

        if (hit.collider.gameObject == gameObject)
        {
            print("no col _ true");
            return true;
        }
        print("no col _ 2: " + hit.collider.gameObject);
        return false;

        //if (click == null)
        //{
        //    print("no col _ 2");
        //    return false;
        //}

        //if (click.Root_Parent != Root_Parent)
        //{
        //    print("no col _ 3");
        //    return false;
        //}
        //return true;
    }
}
