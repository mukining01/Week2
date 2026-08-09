using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Instructions
    // Three functions are provided for use in other scripts:
    // 1. PlayOneShot(EventReference sound, Vector3 worldPos) - Plays a one-shot sound at a given world position.
    // 2. StopStartSnapshot(EventInstance snapshotInstance) - Activates or stops a snapshot.
    // 3. StopStartMusic(EventInstance eventInstance) - Activates or stops a music event.
    //
    //Example usage:
    // 1. AudioManager.Instance.PlayOneShot(EventCatalogue.Instance.ClickEvent, transform.position);
    //    Event references can be found in the EventCatalogue script.
    //
    // 2. AudioManager.Instance.StopStartSnapshot(AudioManager.Instance.RockSnapshotInstance);
    //    Three options are provided for snapshots: RockSnapshotInstance, CrystalSnapshotInstance, and ObsidianSnapshotInstance.
    //
    // 3. AudioManager.Instance.StopStartMusic(AudioManager.Instance.RockMusicEventInstance);
    //    Three options are provided for music events: RockMusicEventInstance, CrystalMusicEventInstance, and ObsidianMusicEventInstance.
    //
    //    PLUS: Camera move sound loop counts as a music event and can be started/stopped with the same function: CameraMoveEventInstance.
    //    PLUS: Timer event counts as a music event and can be started/stopped with the same function: TimerEventInstance.
    //
    #endregion

    #region Volume
    [Header("Volume")]
    [Range(0f, 1.0f)]
    public float MasterVolume = 1.0f;
    [Range(0f, 1.0f)]
    public float MusicVolume = 1.0f;
    [Range(0f, 1.0f)]
    public float SFXVolume = 1.0f;

    //Bus references for volume control
    private Bus MasterBus;
    private Bus MusicBus;
    private Bus SFXBus;
    //add more buses as needed  
    #endregion

    #region Initialisation

    //event instances for music players etc. that need to be started/stopped/modified (+ camera move event)
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    public EventInstance RockMusicEventInstance;
    public EventInstance CrystalMusicEventInstance;
    public EventInstance ObsidianMusicEventInstance;
    public EventInstance CameraMoveEventInstance;
    public EventInstance TimerEventInstance;

    //snapshots for audio adjustments
    public EventInstance RockSnapshotInstance;
    public EventInstance CrystalSnapshotInstance;
    public EventInstance ObsidianSnapshotInstance;

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

        //Add to list for cleanup later
        eventInstances.Add(eventInstance);

        return eventInstance;

    }

    private void InitializeRockMusicEvent(EventReference exampleEventReference)
    {
        RockMusicEventInstance = CreateInstance(exampleEventReference);

    }
    private void InitializeCrystalMusicEvent(EventReference exampleEventReference)
    {
        CrystalMusicEventInstance = CreateInstance(exampleEventReference);

    }
    private void InitializeObsidianMusicEvent(EventReference exampleEventReference)
    {
        ObsidianMusicEventInstance = CreateInstance(exampleEventReference);
    }

    private void InitializeCameraMoveEvent(EventReference exampleEventReference)
    {
        CameraMoveEventInstance = CreateInstance(exampleEventReference);
    }

    private void InitializeTimerEvent(EventReference exampleEventReference)
    {
        TimerEventInstance = CreateInstance(exampleEventReference);
    }

    //Snapshots for audio adjustments
    private void InitializeRockSnapshot(string exampleSnapshotPath)
    {
        RockSnapshotInstance = RuntimeManager.CreateInstance(exampleSnapshotPath);
        eventInstances.Add(RockSnapshotInstance);
    }
    private void InitializeCrystalSnapshot(string exampleSnapshotPath)
    {
        CrystalSnapshotInstance = RuntimeManager.CreateInstance(exampleSnapshotPath);
        eventInstances.Add(CrystalSnapshotInstance);
    }
    private void InitializeObsidianSnapshot(string exampleSnapshotPath)
    {
        ObsidianSnapshotInstance = RuntimeManager.CreateInstance(exampleSnapshotPath);
        eventInstances.Add(ObsidianSnapshotInstance);
    }

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one audio manager in scene");
        }
        Instance = this;

        //Part of cleanup process
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start()
    {

        // assign busses for volume control
        MasterBus = RuntimeManager.GetBus("bus:/Master");
        MusicBus = RuntimeManager.GetBus("bus:/Master/Music");
        SFXBus = RuntimeManager.GetBus("bus:/Master/SFX");


        //Intialize music events for music players that need to be started/stopped/modified
        InitializeRockMusicEvent(EventCatalogue.Instance.RockMusicEvent);
        InitializeCrystalMusicEvent(EventCatalogue.Instance.CrystalMusicEvent);
        InitializeObsidianMusicEvent(EventCatalogue.Instance.ObsidianMusicEvent);
        InitializeCameraMoveEvent(EventCatalogue.Instance.CameraMoveEvent);
        InitializeTimerEvent(EventCatalogue.Instance.TimerEvent);

        //Initialise snapshots for audio adjustments
        InitializeRockSnapshot("snapshot:/Rock");
        InitializeCrystalSnapshot("snapshot:/Crystal");
        InitializeObsidianSnapshot("snapshot:/Obsidian");

    }

    private void Update()
    {
        ////Update volume levels
        MasterBus.setVolume(MasterVolume);
        MusicBus.setVolume(MusicVolume);
        SFXBus.setVolume(SFXVolume);
    }
      

    #endregion

    #region Sound Functions
    //This function will play a one shot sound at a given world position
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);

    }
   
   

    //This function will activate/stop a snapshot
    public void StopStartSnapshot(EventInstance snapshotInstance)
    {
        // Get playback state to only play if not already playing.
        PLAYBACK_STATE playbackState;
        snapshotInstance.getPlaybackState(out playbackState);
        if (playbackState == PLAYBACK_STATE.STOPPED)
        {
            snapshotInstance.start();
        } 
        else
        {
            snapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    //This function will activate/stop a music event
    public void StopStartMusic(EventInstance eventInstance)
    {
        // Get playback state to only play if not already playing.
        PLAYBACK_STATE playbackState;
        eventInstance.getPlaybackState(out playbackState);
        if (playbackState == PLAYBACK_STATE.STOPPED)
        {
            eventInstance.start();
        } 
        else
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

}
    
    #endregion

    #region Clean Up
    private void CleanUp()
    {
        //stop and release all event emitters
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        //stop all event emitters
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
    private void OnDestroy()
    {
        CleanUp();
    }

    #endregion

}
