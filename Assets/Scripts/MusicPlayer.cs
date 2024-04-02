using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    
    public AudioClip[] songs; // Array of songs to shuffle and play.
    private AudioSource audioSource;
    private List<int> songIndexes; // To keep track of which songs have been played.
    private int currentSongIndex = 0; // Start with the main menu song.

    private void Awake()
    {
        if (Instance != this && Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene.

        // Initialize and shuffle song indexes, except the first one (main menu song).
        InitializeSongIndexes();

        yield return new WaitForSeconds(3f);
        // Start with the main menu song.
        PlaySong(currentSongIndex);
    }

    void Update()
    {
        // Check if the song has finished playing.
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    private void InitializeSongIndexes()
    {
        songIndexes = new List<int>();
        for (int i = 1; i < songs.Length; i++) // Start from 1 to exclude the main menu song.
        {
            songIndexes.Add(i);
        }
        Shuffle(songIndexes);
    }

    private void PlaySong(int index)
    {
        audioSource.clip = songs[index];
        audioSource.Play();
    }

    private void PlayNextSong()
    {
        // Remove the currently playing song's index from the list.
        if (songIndexes.Contains(currentSongIndex))
        {
            songIndexes.Remove(currentSongIndex);
        }

        // If all songs have been played, reinitialize the list to shuffle and play again.
        if (songIndexes.Count == 0)
        {
            InitializeSongIndexes();
        }

        // Pick the next song randomly from the remaining songs.
        int nextSongIndex = songIndexes[Random.Range(0, songIndexes.Count)];
        currentSongIndex = nextSongIndex;
        PlaySong(currentSongIndex);
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
