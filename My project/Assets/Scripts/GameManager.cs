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
    public GameObject height_tracker_p;
    static GameObject height_tracker;
    public GameObject TutorialText_p;
    static GameObject TutorialText;

    static Transform self;

   // static Animator animator;

    static float stone_timer = 2;
    static float current_timer = 0;
    static float current_stone_timer = 0;
    static int human_count = 0;
    static int human_count_max = 10;
    static int global_human_count = 0;

    static float[] times = new float[6] { 8, 7, 6, 5, 4, 3 };
    static int current_time = 0;
    static bool faster = false;

    static bool lose = false;

    static List<GameObject> humans = new List<GameObject>();

    static float current_max_height = 0;
    static float tracking_target_add = 15;

    static TurnToStone current_human = null;

    static int stone_type = 0;

    static bool reset = false;

    private void Awake()
    {
       // animator = GetComponent<Animator>();

        HumansToTurn = HumansToTurn_p;
        TrackingTarget = TrackingTarget_p;
        self = transform;
        height_tracker = height_tracker_p;

        text = text_p;
        TutorialText = TutorialText_p;
    }

    public void Start()
    {
        AudioManager.Instance.StopStartMusic(AudioManager.Instance.RockMusicEventInstance);

        XFailure.Reset_XFailure();

        Restart();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (lose) return;

        if(reset)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                reset = false;
                TutorialText.SetActive(false);
                text.gameObject.SetActive(true);
                AudioManager.Instance.StopStartMusic(AudioManager.Instance.TimerEventInstance);
                counting = true;

                AudioManager.Instance.StopStartMusic(AudioManager.Instance.RockMusicEventInstance);
                AudioManager.Instance.StopStartSnapshot(AudioManager.Instance.RockSnapshotInstance);
            }

            return;
        }

        if(current_timer > 0)
        {
            current_timer -= Time.deltaTime;

            float _time_normal = Mathf.Round(current_timer);

            text.text = _time_normal.ToString();
            if(_time_normal < 10) text.text = "0" + _time_normal.ToString();

            if(faster)
            {
                if (current_timer > times[current_time] - 1f) text.text = "FASTER!";
                else faster = false;
            }

            if (current_timer <= 0)
            {
                Turn_Human_To_Stone();
            }
        } else if (current_stone_timer > 0)
        {
            current_stone_timer -= Time.deltaTime; 

            if(current_stone_timer <= 0)
            {
                human_count++;
                global_human_count++;

                if (human_count >= human_count_max && current_time < times.Length - 1)
                {
                    faster = true;
                    current_time++;
                    human_count = 0;
                }

                if (global_human_count > 20)
                {
                    stone_type = 2;
                    AudioManager.Instance.StopStartMusic(AudioManager.Instance.ObsidianMusicEventInstance);
                    AudioManager.Instance.StopStartSnapshot(AudioManager.Instance.ObsidianSnapshotInstance);
                }
                else if (global_human_count > 10)
                {
                    stone_type = 1;
                    AudioManager.Instance.StopStartMusic(AudioManager.Instance.CrystalMusicEventInstance);
                    AudioManager.Instance.StopStartSnapshot(AudioManager.Instance.CrystalSnapshotInstance);
                }


                Reset_Timers(false);
            }
        }
    }

    public static void Reset_Timers(bool _starting_human)
    {
        current_timer = times[current_time];
        current_stone_timer = 5;

        Spawn_Human(_starting_human);
    }

    static bool counting = false;

    static float current_distance = 0f;
    static float new_distance_small = 0f;

    public static void Calculate_New_Height(float y_pos)
    {
        float _new_distance =Mathf.Floor( y_pos  * 2.5f);
        new_distance_small = y_pos;
        if (_new_distance > current_distance) current_distance = _new_distance;
        print("best ditance: " + current_distance);
    }

    public static void Turn_Human_To_Stone()
    {

        current_human.Stone(stone_type);

        text.text = "FREEZE!";
        current_stone_timer = stone_timer;

        counting = false;

        AudioManager.Instance.StopStartMusic(AudioManager.Instance.TimerEventInstance);
        AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.MedusaFlashEvent, self.position);
    }

    public static void Spawn_Human(bool _starting_human)
    {
        counting = true;
        AudioManager.Instance.StopStartMusic(AudioManager.Instance.TimerEventInstance);

        Vector3 _spawn_position = new Vector3(0.79f, height_tracker.transform.position.y + tracking_target_add + new_distance_small, 0);

        if(_starting_human) _spawn_position = new Vector3(0.79f, height_tracker.transform.position.y + 1, 0);

        var _human = Instantiate(HumansToTurn, self);
        _human.transform.position = _spawn_position;
        humans.Add(_human.gameObject);
        current_human = _human.GetComponent<TurnToStone>();
    }

    public static void Stop_Counting()
    {
        if(counting)
        {
            AudioManager.Instance.StopStartMusic(AudioManager.Instance.TimerEventInstance);
            counting = false;
        }
    }

    public static void Lose()
    {
        Stop_Counting();

        lose = true;
        text.text = "LOSE";
        // animator.SetBool("Lose", true);

        ScoreAnimator.Set_Score(current_distance);

        if (humans.Count > 0)
        {
            for (int i = 0; i < humans.Count; i++)
            {
                humans[i].GetComponent<TurnToStone>().Set_Kinematic();
            }
        }

        AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.MedusaSadEvent, self.position);
    }

    public static void Restart()
    {
        //Time.timeScale = 1;
        //animator.SetBool("Lose", false);

        lose = false;

        current_distance = 0;
        global_human_count = 0;

        XFailure.Reset_XFailure();
        ScoreAnimator.Reset_ScoreUI();

        TrackingTarget.transform.position = new Vector3(TrackingTarget.position.x, 2.5f, TrackingTarget.position.z);
        
        if (humans.Count > 0)
        {
            for (int i = 0; i < humans.Count; i++)
            {
                Destroy(humans[i]);
            }

            humans.Clear();
        }

        Reset_Timers(true);

        reset = true;
        TutorialText.SetActive(true);
        text.gameObject.SetActive(false);
    }
}

