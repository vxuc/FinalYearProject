using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
   public void Activate(GameObject UiPointer)
    {
        if (UiPointer.activeSelf)
            UiPointer.SetActive(false);
        else
            UiPointer.SetActive(true);
    }
}
