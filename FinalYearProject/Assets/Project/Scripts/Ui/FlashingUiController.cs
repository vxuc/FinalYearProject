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

    public void StartFlash(float secondsForOneFlash)
    {
        if(currentFlashRoutine != null)
            StopCoroutine(currentFlashRoutine);
        currentFlashRoutine = StartCoroutine(Flash(secondsForOneFlash));
    }

    IEnumerator Flash(float secondsForOneFlash)
    {
        //Flash start
        float flashInDuration = secondsForOneFlash / 2;
        for (float t = 0; t <= flashInDuration; t += Time.fixedDeltaTime)
        {
            Color flashColor = image.color;
            flashColor.a = Mathf.Lerp(1, 0, t / flashInDuration);
            image.color = flashColor;

            //Wait
            yield return null;
        }
    }
}
