using UnityEngine;

public class XFailure : MonoBehaviour
{
    static Transform x_failure = null;

    private void Awake()
    {
        x_failure = transform.GetChild(0);
        x_failure.gameObject.SetActive(false);
    }

    public static void Position(Vector2 position)
    {
        x_failure.gameObject.SetActive(true);
        x_failure.transform.position = new Vector3(position.x, position.y, -13);
    }

    public static void Reset_XFailure()
    {
        x_failure.gameObject.SetActive(false);
    }
}
