using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistic : MonoBehaviour
{
    public GameObject foxes;
    public GameObject rabbits;
    public GameObject foods;

    private TextMeshProUGUI _foxesText;
    private TextMeshProUGUI _rabbitsText;
    private TextMeshProUGUI _foodsText;

    // Start is called before the first frame update
    void Start()
    {
        _foxesText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _rabbitsText = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _foodsText = gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int foxCount = foxes.transform.childCount;
        _foxesText.SetText("Fox: " + foxCount);

        int rabbitCount = rabbits.transform.childCount;
        _rabbitsText.SetText("Rabbit: " + rabbitCount);

        int foodCount = foods.transform.childCount;
        _foodsText.SetText("Food: " + foodCount);
    }
}
