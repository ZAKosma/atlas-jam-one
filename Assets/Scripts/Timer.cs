using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = string.Format("{0:0}:{1:00}.{2:00}", timer / 60, timer % 60, timer * 100 % 100);
    }
}
