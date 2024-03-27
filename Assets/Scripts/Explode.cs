using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Explode : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public string nextSceneName = "NextScene"; 

    void Start()
    {
        // Ensure the video doesn't play immediately
        videoPlayer.playOnAwake = false;
        // videoPlayer.loopPointReached += EndReached; // Subscribe to the event
    }

    // Method to be called by the UI button to start video playback
    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    // // Load the next scene after the video finishes playing
    // private void EndReached(VideoPlayer vp)
    // {
    //     SceneManager.LoadScene(nextSceneName);
    // }
}
