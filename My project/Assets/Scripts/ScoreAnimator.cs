using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreAnimator : MonoBehaviour
{
    public TMP_Text current_score_text_p;
    static TMP_Text current_score_text;
    public TMP_Text current_high_score_text_p;
    static TMP_Text current_high_score_text;
    public GameObject NEWHIGHSCORE_p;
    static GameObject NEWHIGHSCORE;
    public GameObject other_HIGHSCORE_p;
    static GameObject other_HIGHSCORE;

    static Animator animator;

    static float score_counting_time = 3;
    static float current_score_counting_time = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        current_score_text = current_score_text_p;
        current_high_score_text = current_high_score_text_p;
        NEWHIGHSCORE = NEWHIGHSCORE_p;
        other_HIGHSCORE = other_HIGHSCORE_p;
    }

    static float current_highscore = 10;
    static float current_score = 0;

    static bool _set_Score = false;

    public static void Set_Score(float Score)
    {
        current_score = Score;
        current_score_counting_time = 0;

        animator.SetBool("DisplayScore", true);

        _set_Score = true;

        NEWHIGHSCORE.SetActive(false);
        other_HIGHSCORE.SetActive(false);

        counting_audio = true;
        if (current_score != 0) AudioManager.Instance.StopStartMusic(AudioManager.Instance.CounterUpEventInstance);
    }

    private void Update()
    {
        if (_set_Score) Count_Up_Score();
    }

    static bool counting_audio = false;

    public static void Count_Up_Score()
    {
        current_score_counting_time += Time.deltaTime;
        float _time = current_score_counting_time;

        if (_time >= score_counting_time)
        {
            _time = score_counting_time;
            if(counting_audio && current_score != 0)
            {
                AudioManager.Instance.StopStartMusic(AudioManager.Instance.CounterUpEventInstance);
                counting_audio = false;
            }
        }

            if (current_score == 0) current_score_counting_time = 50;

        float _meters = Mathf.Lerp(0, current_score, _time / score_counting_time);
        _meters = Mathf.Round(_meters);

        current_score_text.text = _meters.ToString() + " meters";

        if(current_score_counting_time > score_counting_time + 1)
        {
            Display_HighScore();
            _set_Score = false;
        }
    }

    public static void Display_HighScore()
    {
        if (current_highscore < current_score)
        {
            current_highscore = current_score;
            NEWHIGHSCORE.SetActive(true);

            AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.HighscoreEvent, current_high_score_text.transform.position);
        }
        else
        {
            current_high_score_text.text = current_highscore.ToString() + " meters";
            other_HIGHSCORE.SetActive(true);

            AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.ScoreEvent, current_high_score_text.transform.position);
        }
    }

    public static void Reset_ScoreUI()
    {
        animator.SetBool("DisplayScore", false);
    }
}
