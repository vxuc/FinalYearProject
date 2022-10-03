using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchScript : MonoBehaviour
{
    public GameObject contentHolder;

    public GameObject[] Element;

    public GameObject searchBar;

    public TMP_Dropdown dropdown;

    public int totalElements;

    // Start is called before the first frame update
    void Start()
    {
        totalElements = contentHolder.transform.childCount;

        Element = new GameObject[totalElements];

        for (int i = 0; i < totalElements; i++)
            Element[i] = contentHolder.transform.GetChild(i).gameObject;

        Init();
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
                    if (ele.GetComponent<AircraftInfoPanel>().type == GetCurrentType() || GetCurrentType() == "All")
                        ele.SetActive(true);
                }
                else
                    ele.SetActive(false);
            }
        }
    }

    public void Init()
    {
        List<string> types = new List<string>();
        types.Add("All");
        foreach (GameObject ele in Element)
        {
            if (!types.Contains(ele.GetComponent<AircraftInfoPanel>().type))
                types.Add(ele.GetComponent<AircraftInfoPanel>().type);
        }

        dropdown.AddOptions(types);
        dropdown.onValueChanged.AddListener(delegate { Select(); });

        Select();
    }

    public void Select()
    {
        int index = dropdown.value;
        string type = dropdown.options[index].text;

        foreach (GameObject ele in Element)
        {
            if (type == "All")
            {
                ele.SetActive(true);
                continue;
            }
            if (ele.GetComponent<AircraftInfoPanel>().type == type)
            {
                ele.SetActive(true);
            }
            else
                ele.SetActive(false);
        }
    }

    public string GetCurrentType()
    {
        return dropdown.options[dropdown.value].text;
    }
}
