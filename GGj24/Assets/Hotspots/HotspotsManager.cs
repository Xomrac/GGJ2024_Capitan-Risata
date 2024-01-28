using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotsManager : MonoBehaviour
{
    public static HotspotsManager instance;
    public Camera displayCamera;

    public float transitionTime;
    public List<Hotspot> hotspots;
    public int currentIndex = 0;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        instance = this;
    }

    public void GoToNextHotspot()
    {
        currentIndex++;
        GoToHotspot(currentIndex);
    }

    public void GoToHotspot(int index)
    {
        moveCoroutine = StartCoroutine(MoveCamera(hotspots[index]));
    }



    private IEnumerator MoveCamera(Hotspot targetHotspot)
    {
        var finalPosition = targetHotspot.transform.position;
        var finalRotation = targetHotspot.transform.rotation;
        var currentPos = displayCamera.transform.position;
        var currentRotation = displayCamera.transform.rotation;
        var elapsedTime = 0f;
        while (elapsedTime<=transitionTime)
        {
            displayCamera.transform.position = Vector3.Lerp(currentPos, finalPosition, elapsedTime / transitionTime);
            displayCamera.transform.rotation = Quaternion.Slerp(currentRotation, finalRotation, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetHotspot.Trigger();
    }
}
