using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishingStates
{
    nothing,
    Casting,
    Waiting,
    OnHook,
    Reeling,
    Victory
}

public class BusyFishing : MonoBehaviour
{


    //STUFF FOR FISHING
    public int HitNotes;
    public int MissedNotes;
    public int[] NeededNotes;
    private int extraNotes;

    public int SongID;

    public FishingStates FishingState;
    private Inventory inventory;
    private AudioManager audioManager;



    void Start()
    {
        inventory = GetComponent<Inventory>();
        audioManager = FindObjectOfType<AudioManager>();
        //Time.timeScale = 3f;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && FishingState == FishingStates.Reeling)
        {
            FinaliseFishing();
        }

        if (MissedNotes > inventory.AllowedMisses)
        {
            //END MINIGAME
        }
    }

    private void FinaliseFishing()
    {
        if (HitNotes >= NeededNotes[SongID] * 2)
        {
            Debug.Log("Double Catch!");
        }

        else if (HitNotes >= NeededNotes[SongID])
        {
            Debug.Log("Catch!");
        }

        else if (HitNotes < NeededNotes[SongID] || MissedNotes > inventory.AllowedMisses)
        {
            Debug.Log("Reeled in to soon");
        }

        StartCoroutine(audioManager.EndSong());
    }
}
