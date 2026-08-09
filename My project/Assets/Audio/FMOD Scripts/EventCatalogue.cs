using UnityEngine;
using FMODUnity;
public class EventCatalogue : MonoBehaviour
{
    #region Event Catalogue

    /*ADD HEADERS AND EVENT REFERENCES FOR SOUNDS AS NEEDED!

    [field: Header("Example Track")]
    [field: SerializeField] public EventReference ExampleEvent { get; private set; }

    */

    [field: Header("Click")]
    [field: SerializeField] public EventReference ClickEvent { get; private set; }

    [field: Header("Timer")]
    [field: SerializeField] public EventReference TimerEvent { get; private set; }

    [field: Header("CounterUp")]
    [field: SerializeField] public EventReference CounterUpEvent { get; private set; }

    [field: Header("Score")]
    [field: SerializeField] public EventReference ScoreEvent { get; private set; }

    [field: Header("Highscore")]
    [field: SerializeField] public EventReference HighscoreEvent { get; private set; }

    [field: Header("MedusaFlash")]
    [field: SerializeField] public EventReference MedusaFlashEvent { get; private set; }

    [field: Header("MedusaSad")]
    [field: SerializeField] public EventReference MedusaSadEvent { get; private set; }

    [field: Header("Collide")]
    [field: SerializeField] public EventReference CollideEvent { get; private set; }

    [field: Header("Fall")]
    [field: SerializeField] public EventReference FallEvent { get; private set; }

    [field: Header("RockMusic")]
    [field: SerializeField] public EventReference RockMusicEvent { get; private set; }

    [field: Header("CrystalMusic")]
    [field: SerializeField] public EventReference CrystalMusicEvent { get; private set; }

    [field: Header("ObsidianMusic")]
    [field: SerializeField] public EventReference ObsidianMusicEvent { get; private set; }

    [field: Header("CameraMove")]
    [field: SerializeField] public EventReference CameraMoveEvent { get; private set; }

    [field: Header("Fail")]
    [field: SerializeField] public EventReference FailEvent { get; private set; }

    #endregion

    #region Initialisation
    public static EventCatalogue Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one audio manager in scene");
        }
        Instance = this;
    }
    #endregion

}