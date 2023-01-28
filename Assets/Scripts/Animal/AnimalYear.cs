using UnityEngine;
using System.Collections;

public class AnimalYear : MonoBehaviour
{
    public int years = 3;
    public int years_max = 3;

    private AnimalController _controller = null;
    private AnimalConsumeFood _animalFood = null;

    // Use this for initialization
    public void Start()
	{
        _controller = GetComponent<AnimalController>();
        _animalFood = GetComponent<AnimalConsumeFood>();

        StartCoroutine(enumeratorYear());
    }

    // Update is called once per frame
    void Update()
	{
			
	}

    // ienumerations

    private IEnumerator enumeratorYear()
    {
        while (years < years_max)
        {
            //Wait for seconds
            yield return new WaitForSecondsRealtime(10);

            updateYear(years + 1);
        }
    }

    // utils

    public void updateYear(int new_year)
    {
        years = new_year;

        float mult = 0.3f + (years * 0.1f);
        Vector3 scale = new Vector3(mult, mult, mult);
        transform.localScale = scale;

        _controller.updateYear(years);
        _animalFood.updateYear(years);
    }
}

