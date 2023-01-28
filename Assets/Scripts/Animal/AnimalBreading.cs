using UnityEngine;
using System.Collections;

public class AnimalBreading : MonoBehaviour
{
    public GameObject baby;

    public int years_for_breading = 3;
    public float breeding_interval = 10f;
    public float breeding_duration = 3f;
    public int breeding_child_count = 2;

    //

    public bool breeding_is_search = true;
    public bool breading_is = false;
    public GameObject breading_target = null;

    //

    private AnimalController _controller = null;
    private AnimalYear _year = null;

    // Use this for initialization
    private void Start()
	{
        _controller = GetComponent<AnimalController>();
        _year = GetComponent<AnimalYear>();
    }


    // public methods

    public void startBreading(GameObject partnerGo)
    {
        AnimalBreading partnerBreeding = partnerGo.GetComponent<AnimalBreading>();
        AnimalController partnerController = partnerGo.GetComponent<AnimalController>();

        if (!isBreadable(partnerBreeding) || partnerBreeding.breading_is)
        {
            return;
        }

        breading_is = true;

        // start dancing
        partnerController.startDance();
        _controller.startDance();

        Invoke("breeding", breeding_duration);
        
    }

    // private methods

    private void breeding()
    {
        // stop dance for 2
        if (breading_target)
        {
            AnimalBreading partnerBreeding = breading_target.GetComponent<AnimalBreading>();
            AnimalController partnerController = breading_target.GetComponent<AnimalController>();

            partnerController.stopDance();
            partnerController.clearTarget();

            partnerBreeding.breading_target = null;
        }

        // stop dance for 1
        _controller.stopDance();
        _controller.clearTarget();

        breading_target = null;

        // spawn babies
        GameObject container = transform.parent.gameObject;

        for (int i = 0; i < breeding_child_count; i++)
        {
            createBaby(container);
        }

        Invoke("startSearchBreeding", breeding_interval);
    }

    // private utils

    private void createBaby(GameObject container)
    {
        GameObject newRabbit = Instantiate(baby, transform.position, Quaternion.identity);
        newRabbit.transform.SetParent(container.transform);
        newRabbit.name = baby.name;

        Rabbit rabbit = newRabbit.GetComponent<Rabbit>();
        //rabbit.Start();
        AnimalYear newRabbitYear = newRabbit.GetComponent<AnimalYear>();

        newRabbitYear.Start();
        newRabbitYear.updateYear(1);
    }

    // public utils

    public bool isBreadable(AnimalBreading breeding2)
    {
        return !(!breading_target || !breeding2.breading_target
                        || breading_target != breeding2.gameObject
                        || breeding2.breading_target != gameObject);
    }

    public bool isStartBreading()
    {
        if (!_year)
        {
            return false;
        }

        return breeding_is_search && _year.years >= years_for_breading;
    }

    public void stopBreadingSearch()
    {
        breeding_is_search = false;
    }

    public void startSearchBreeding()
    {
        breeding_is_search = true;
    }
}

