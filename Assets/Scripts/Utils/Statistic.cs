using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistic : MonoBehaviour
{
    public GameObject foxes;
    public GameObject rabbits;
    public GameObject foods;

    public TextMeshProUGUI foxesText;
    public TextMeshProUGUI rabbitsText;
    public TextMeshProUGUI foodsText;
    public TextMeshProUGUI timerText;

    private float _time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _time = 0f;

        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        int foxCount = foxes.transform.childCount;
        foxesText.SetText("Fox: " + foxCount);

        int rabbitCount = rabbits.transform.childCount;
        rabbitsText.SetText("Rabbit: " + rabbitCount);

        int foodCount = foods.transform.childCount;
        foodsText.SetText("Food: " + foodCount);
    }

    private IEnumerator timer()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);

            _time++;

            int minutes = (int) _time / 60;
            float fMinutes = (float) minutes;
            float fSeconds = _time - minutes * 60f;
            int seconds = (int)fSeconds;

            timerText.SetText(minutes.ToString() + ":" + seconds.ToString());
        }
    }
}
