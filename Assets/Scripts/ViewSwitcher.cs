using UnityEngine;
using Cinemachine;

public class ViewSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineVirtualCamera thirdPersonCam;
    private bool isFirstPerson = true;

    // This method is now public and can be called by a UI button
    public void ToggleView()
    {
        isFirstPerson = !isFirstPerson;

        firstPersonCam.gameObject.SetActive(isFirstPerson);
        thirdPersonCam.gameObject.SetActive(!isFirstPerson);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleView();
        }
    }
}
