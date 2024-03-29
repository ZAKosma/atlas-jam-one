using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject winCavas;
    public float impactThreshold = 10f;
    public float groundImpactDamage = 50;
    
    // This class handles the ending victory condition, the player reaches the goal and doesn't die (waiting 1 to 3 frames to check), they win!
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Call delay check win
            StartCoroutine(DelayCheckWin(other.gameObject));
        }
    }
    
    //Delay check
    IEnumerator DelayCheckWin(GameObject player)
    {
        //Check if the player is dead
        yield return new WaitForSeconds(0.1f);
        //If the player is not dead, they win
        if (!player.GetComponent<Health>().isDead)
        {
            Debug.Log("You win!");
            WinScreen();
        }
    }

    void WinScreen()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; // Free the cursor
        Cursor.visible = true; // Show the cursor
        winCavas.SetActive(true);
    }
}
    