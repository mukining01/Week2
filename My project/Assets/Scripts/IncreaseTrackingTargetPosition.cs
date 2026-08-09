using UnityEngine;

public class IncreaseTrackingTargetPosition : MonoBehaviour
{
    float y_pos = 0;
    Vector3 current_pos;

    float change = 0.2f;
    float current_change = 0;

    public LayerMask hit_layer;
    public LayerMask hit_layer_body;

    private void Awake()
    {
        y_pos = transform.position.y;
    }

    bool changing = false;
    bool currently_changing = false;

    // Update is called once per frame
    void Update()
    {
        current_change = 0;
        if (Input.GetKey(KeyCode.UpArrow)) current_change = change;
        if (Input.GetKey(KeyCode.DownArrow)) current_change = -change;

        y_pos = Mathf.Lerp(y_pos, y_pos + current_change, 0.05f);

        y_pos = Mathf.Clamp(y_pos, 0, 50);

        transform.position = new Vector3(transform.position.x, y_pos, transform.position.z);

        if(current_change != 0 && changing == false)
        {
            changing = true;
            AudioManager.Instance.StopStartMusic(AudioManager.Instance.CameraMoveEventInstance);
        } else if (current_change == 0 && changing == true)
        {
            changing = false;
            AudioManager.Instance.StopStartMusic(AudioManager.Instance.CameraMoveEventInstance);
        }
    }
}
