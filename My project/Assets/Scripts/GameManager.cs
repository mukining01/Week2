using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject HumansToTurn_p;
    static GameObject HumansToTurn;
    public Transform TrackingTarget_p;
    static Transform TrackingTarget;
    public TMP_Text text_p;
    static TMP_Text text;

    static Transform self;

   // static Animator animator;

    static float stone_timer = 2;
    static float current_timer = 0;
    static float current_stone_timer = 0;
    static int human_count = 0;
    static int human_count_max = 3;

    static float[] times = new float[6] { 8, 7, 6, 5, 4, 3 };
    static int current_time = 0;
    static bool faster = false;

    static bool lose = false;

    static List<GameObject> humans = new List<GameObject>();

    static float current_max_height = 0;
    static float tracking_target_add = 15;

    static TurnToStone current_human = null;

    private void Awake()
    {
       // animator = GetComponent<Animator>();

        HumansToTurn = HumansToTurn_p;
        TrackingTarget = TrackingTarget_p;
        self = transform;

        text = text_p;

        Reset_Timers();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (lose)
        {
            Time.timeScale = 0;
            return;
        }

        if(current_timer > 0)
        {
            current_timer -= Time.deltaTime;

            float _time_normal = Mathf.FloorToInt(current_timer);

            text.text = _time_normal.ToString();
            if(_time_normal < 10) text.text = "0" + _time_normal.ToString();

            if(faster)
            {
                if (current_timer > times[current_time] - 1f) text.text = "FASTER!";
                else faster = false;
            }

            if (current_timer <= 0)
            {
                Turn_Huamn_To_Stone();
            }
        } else if (current_stone_timer > 0)
        {
            current_stone_timer -= Time.deltaTime; 

            if(current_stone_timer <= 0)
            {
                human_count++;

                if (human_count >= human_count_max && current_time < times.Length - 1)
                {
                    faster = true;
                    current_time++;
                    human_count = 0;
                }

                Reset_Timers();
            }
        }
    }

    public static void Reset_Timers()
    {
        current_timer = times[current_time];
        current_stone_timer = 0;

        Spawn_Human();
    }

    public static void Turn_Huamn_To_Stone()
    {
        current_human.Stone();

        text.text = "STONE!";
        current_stone_timer = stone_timer;
    }

    public static void Spawn_Human()
    {
        Vector3 _spawn_position = new Vector3(0.79f, TrackingTarget.position.y + tracking_target_add, 0);

        var _human = Instantiate(HumansToTurn, self);
        _human.transform.position = _spawn_position;
        humans.Add(_human.gameObject);
        current_human = _human.GetComponent<TurnToStone>();
    }

    public static void Lose()
    {
        lose = true;
        text.text = "LOSE";
       // animator.SetBool("Lose", true);
    }

    public static void Restart()
    {
        Time.timeScale = 1;
        //animator.SetBool("Lose", false);

        TrackingTarget.transform.position = new Vector3(TrackingTarget.position.x, 2.5f, TrackingTarget.position.z);
        
        if (humans.Count > 0)
        {
            for (int i = 0; i < humans.Count; i++)
            {
                Destroy(humans[i]);
            }

            humans.Clear();
        }

        Reset_Timers();
    }
}

