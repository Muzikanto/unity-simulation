using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalConsumeFood : MonoBehaviour
{
    public int food_max = 10;
    public int food_interval = 10;
    public int food = 5;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.initParams(food_max);

        updateFood(food);

        StartCoroutine(enumeratorFood());
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    //

    private IEnumerator enumeratorFood()
    {
        while (food > 0)
        {
            //Wait for seconds
            yield return new WaitForSecondsRealtime(food_interval);

            updateFood(food - 1);
        }

        Destroy(gameObject);
    }

    // public

    public bool isFull()
    {
        return food == food_max;
    }

    public void updateFood(int new_food)
    {
        food = new_food;
        healthBar.updateParams(new_food);
    }

    public void increaseFood(int add_food)
    {
        updateFood(food + add_food);
    }

    public void updateYear(int year)
    {
        if (year <= 2)
        {
            food_max = 5;
            food_interval = 5;
        } else
        {
            food_max = 10;
            food_interval = 10;
        }

        updateFood(food_max);
    }

    // utils
}
