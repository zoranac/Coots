using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager i;

    public AudioSource source;
    public TMP_Text CurrentlyPlayingText;
    public GameObject MusicButtons;
    public GameObject MusicDisplay;
    public Slider VolumeSlider;
    public bool Loop;
    public bool PlayBattleMusic = true;
    
    public List<AudioClip> BaseAudioClips;

    int currentTrack = 0;

    public AudioClip BattleMusic;

    public Transform ShowTransform;
    public Transform HideTransform;

    bool showing = true;

    private void Awake()
    {
        if (i != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            i = this;
            DontDestroyOnLoad(this.gameObject);
            source = GetComponent<AudioSource>();
            StartMusic();
            VolumeSlider.value = source.volume;
        }
    }

    private void Update()
    {
        if (!source.isPlaying && source.isActiveAndEnabled && source.clip != BattleMusic)
        {
            NextTrack();
        }
    }

    public void OnVolumeSlider()
    {
        source.volume = VolumeSlider.value;
    }

    public void ToggleLoop()
    {
        Loop = !Loop;
        source.loop = Loop;
    }

    public void StartBattle()
    {
        if (PlayBattleMusic)
        {
            CurrentlyPlayingText.text = BattleMusic.name;
            source.clip = BattleMusic;
            source.loop = true;
            source.Play();
            MusicButtons.SetActive(false);
        }
    }

    public void StartMusic()
    {
        MusicButtons.SetActive(true);
        CurrentlyPlayingText.text = BaseAudioClips[currentTrack].name;
        source.clip = BaseAudioClips[currentTrack];
        source.loop = Loop;
        source.Play();
    }

    public void NextTrack()
    {
        if (currentTrack >= BaseAudioClips.Count - 1)
            currentTrack = 0;
        else
            currentTrack++;

        StartMusic();
    }

    public void PreviousTrack()
    {
        if (currentTrack <= 0)
            currentTrack = BaseAudioClips.Count - 1;
        else
            currentTrack--;

        StartMusic();
    }

    public void ToggleShowPlayer()
    {
        StopAllCoroutines();

        if (showing)
        {
            StartCoroutine(moveToPos(HideTransform.position));
        }
        else
        {
            StartCoroutine(moveToPos(ShowTransform.position));
        }

        showing = !showing;
    }

    IEnumerator moveToPos(Vector3 pos)
    {
        while (Vector3.Distance(MusicDisplay.transform.position, pos) > .15f)
        {
            MusicDisplay.transform.position = Vector3.MoveTowards(MusicDisplay.transform.position, pos, .35f);
            yield return null;
        }
    }

}
