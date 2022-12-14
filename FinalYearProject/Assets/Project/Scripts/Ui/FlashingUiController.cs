using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingUiController : MonoBehaviour
{
    Image image;
    Coroutine currentFlashRoutine;
    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StartFlash(float secondsForOneFlash,float delay)
    {
        if(currentFlashRoutine != null)
            StopCoroutine(currentFlashRoutine);
        currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash, delay));
    }

    IEnumerator Flash(float secondsForOneFlash,float delay)
    {
        //Flash start
        float flashInDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= delay; t += Time.fixedDeltaTime)
        {
            Time.timeScale = 0f;
            Color flashColor = image.color;
            flashColor.a = 0.5f;
            image.color = flashColor;
            
            //Wait
            yield return null;
        }
        for (float t = 0; t <= flashInDuration; t += Time.fixedDeltaTime)
        {
            Time.timeScale = 1f;
            Color flashColor = image.color;
            flashColor.a = Mathf.Lerp(1f, 0, t / flashInDuration);
            image.color = flashColor;

            //Wait
            yield return null;
        }
    }
}
