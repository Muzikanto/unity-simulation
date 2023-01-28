using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalConsumeFood : MonoBehaviour
{
    public int food_max = 10;
    public int food_consume_interval = 15;
    public int food = 5;

    public HealthBar healthBar;

    //
    private float move_time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.initParams(food_max);

        updateFood(food);

        //StartCoroutine(enumeratorFood());
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    //

    //private IEnumerator enumeratorFood()
    //{
    //    while (food > 0)
    //    {
    //        //Wait for seconds
    //        yield return new WaitForSecondsRealtime(food_consume_interval);

    //        updateFood(food - 1);
    //    }

    //    Destroy(gameObject);
    //}

    // public

    public bool isFull()
    {
        return food == food_max;
    }

    public void updateMoveTime(float time)
    {
        move_time += time;

        if (move_time >= food_consume_interval)
        {
            move_time -= food_consume_interval;
            decrease(1);
        }
    }

    public void updateYear(int year)
    {
        if (year <= 2)
        {
            food_max = 5;
            food_consume_interval = 5;
        } else
        {
            food_max = 10;
            food_consume_interval = 10;
        }

        updateFood(food_max);
    }

    public void increase(int value)
    {
        updateFood(food + value);
    }

    public void decrease(int value)
    {
        updateFood(food - value);
    }

    // utils

    private void updateFood(int new_food)
    {
        food = new_food;
        healthBar.updateParams(new_food);

        if (food == 0)
        {
            Destroy(gameObject);
        }
    }
}
