using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchScript : MonoBehaviour
{
    public GameObject contentHolder;

    public GameObject[] Element;

    public GameObject searchBar;

    public int totalElements;

    // Start is called before the first frame update
    void Start()
    {
        totalElements = contentHolder.transform.childCount;

        Element = new GameObject[totalElements];

        for (int i = 0; i < totalElements; i++)
            Element[i] = contentHolder.transform.GetChild(i).gameObject;
    }

    public void Search()
    {
        string searchText = searchBar.GetComponent<TMP_InputField>().text;
        int searchTextLength = searchText.Length;

        int searchedElements = 0;

        foreach (GameObject ele in Element)
        {
            searchedElements += 1;

            if (ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length >= searchTextLength)
            {
                if (ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToLower().Contains(searchText.ToLower()))
                {
                    ele.SetActive(true);
                }
                else
                    ele.SetActive(false);
            }
        }
    }
}
