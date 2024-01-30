using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XomracLabs;
using Random = UnityEngine.Random;

public class MouthMover : MonoBehaviour
{
    [SerializeField] private Vector2 yMovement;
    [SerializeField] private Vector2 xMovement;
    [SerializeField] private Vector2 movingSpeed = new Vector2(0.1f,0.3f);


    [SerializeField] private AudioBanks gibberish;

    
    private Vector3 neutralPosition;
    [SerializeField]private RectTransform rectTransform;

    private void Awake()
    {
        neutralPosition = rectTransform.localPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(MoveMouthCoroutine());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        rectTransform.localPosition = neutralPosition;
    }

    private void OnDisable()
    {
        StopMoving();
    }

    private IEnumerator MoveMouthCoroutine()
    {
        AudioManager.instance.PlayClipRandomPitched(gibberish?.GetClip());
        var newPosition = neutralPosition + new Vector3(Random.Range(xMovement.x, xMovement.y), Random.Range(yMovement.x, yMovement.y), 0);
        float elapsedTime = 0;
        var randomTime = Random.Range(movingSpeed.x, movingSpeed.y);
        while (elapsedTime<=randomTime)
        {
            rectTransform.localPosition = Vector3.Lerp(neutralPosition, newPosition, elapsedTime / randomTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(MoveToNeutralPos());
    }

    private IEnumerator MoveToNeutralPos()
    {
        // AudioManager.instance.PlayClipRandomPitched(gibberish?.GetClip());

        var startPos = rectTransform.localPosition;
        float elapsedTime = 0;
        var randomTime = Random.Range(movingSpeed.x, movingSpeed.y);
        while (elapsedTime<=randomTime)
        {
            rectTransform.localPosition = Vector3.Lerp(startPos, neutralPosition, elapsedTime / randomTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(MoveMouthCoroutine());
    }
}
