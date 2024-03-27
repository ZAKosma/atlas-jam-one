using UnityEngine;
//^My Directives

public class ButtonFLASH : MonoBehaviour //My blueprint
{
    public float buttonPulser = 1.5f; //Rate at which the Button Text pulses
    public float maxScale = 1.1f; //MAX scale of the obj during Button Pulse
    public float minScale = 1.0f; //MIN scale of the obj during Button Pulse
    private RectTransform rectTransform; //reference to the obj RectTransform

    void Awake() //my script instnace is being loaded
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() //updating every frameee
    {
        // the scale of the object between minScale and maxScale at a rate of Button Pulser !
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * buttonPulser, 1));
        rectTransform.localScale = new Vector3(scale, scale, 1f); //applying the caculated scale to the obj
    }
}