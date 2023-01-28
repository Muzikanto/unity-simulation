using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using System.Drawing;
using static UnityEngine.ParticleSystem;
using System.Security.Cryptography;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;
using System.ComponentModel;

public class Rabbit : MonoBehaviour, IAnimalVision, IAnimalAttack
{
    public GameObject baby;

    public int years_for_breading = 3;

    public bool breeding_is_search = true;
    public float breeding_interval = 10f;
    public float breeding_duration = 3f;
    public int breeding_child_count = 2;
    public bool breading_is = false;
    public GameObject breading_target = null;

    public HealthBar healthBar;
    private Transform _transform = null;
    private Rigidbody _rigidBody = null;
    private AnimalController _controller = null;
    private AnimalConsumeFood _animalFood = null;
    private AnimalYear _year = null;

    private List<GameObject> _food_list = new List<GameObject>();
    private List<GameObject> _rabbit_list = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<AnimalController>();
        _animalFood = GetComponent<AnimalConsumeFood>();
        _year = GetComponent<AnimalYear>();

        updateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        updateTarget();
    }

    // methods

    private void updateTarget()
    {
        // stop if target exists
        if (_controller.target != null)
        {
            return;
        }

        // breading
        if (isStartBreading())
        {
            // if i see rabbits
            if (_rabbit_list.Count > 0)
            {
                // remove deleted
                _rabbit_list.RemoveAll(x => x == null);

                // get first breadable rabbit
                GameObject first = _rabbit_list.Find(el => {
                    Rabbit r = el.GetComponent<Rabbit>();
                    bool is_true = r.isStartBreading();

                    return is_true;
                });

                // update target
                if (first)
                {
                    Rabbit partnerRabbit = first.GetComponent<Rabbit>();

                    // stop partner searching
                    stopBreadingSearch();
                    partnerRabbit.stopBreadingSearch();

                    // setup partner
                    breading_target = first;
                    partnerRabbit.breading_target = gameObject;

                    // move to partner
                    _controller.updateTarget(first);
                    partnerRabbit._controller.updateTarget(gameObject);

                    return;
                }
            }
        }

        // find food
        if (!_animalFood.isFull())
        {
            // if i see food
            if (_food_list.Count > 0)
            {
                // remove deleted
                _food_list.RemoveAll(x => x == null);

                // get first available food
                GameObject first = _food_list.Find(x =>
                {
                    Food foodGo = x.GetComponent<Food>();

                    return foodGo.size > 2;
                });


                // update target
                if (first)
                {
                    _controller.updateTarget(first.gameObject);

                    return;
                }
            }
        }
    }

    private void breeding() {
        // stop dance for 2
        if (breading_target)
        {
            Rabbit rabbit2 = breading_target.GetComponent<Rabbit>();

            rabbit2._controller.stopDance();
            rabbit2._controller.updateTarget(null);

            rabbit2.breading_target = null;
        }

        // stop dance for 1
        _controller.stopDance();
        _controller.updateTarget(null);

        breading_target = null;

        // spawn babies
        GameObject container = transform.parent.gameObject;

        for(int i = 0; i < breeding_child_count; i++)
        {
            createBaby(container);
        }

        Invoke("startSearchBreeding", breeding_interval);
    }

    // imlements

    public void OnVisionEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Food":
                {
                    _food_list.Add(other.gameObject);
                    break;
                };

            case "Rabbit":
                {
                    _rabbit_list.Add(other.gameObject);
                    break;
                };
        }
    }

    public void OnVisionExit(Collider other)
    {
        switch (other.tag)
        {
            case "Food":
                {
                    _food_list.Remove(other.gameObject);
                    break;
                };

            case "Rabbit":
                {
                    _rabbit_list.Remove(other.gameObject);
                    break;
                };
        }
    }

    public void OnAttackEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Food":
                {
                    if (_animalFood.isFull() || breading_target)
                    {
                        return;
                    }

                    Food foodGO = other.GetComponent<Food>();

                    int food_taked = 0;

                    if (_animalFood.food + foodGO.size >= _animalFood.food_max)
                    {
                        food_taked = _animalFood.food_max - _animalFood.food;
                    }
                    else
                    {
                        food_taked = foodGO.size;
                    }

                    foodGO.decrement(food_taked);
                    _animalFood.increase(food_taked);

                    _controller.clearTarget();

                    break;
                };

            case "Rabbit":
                {
                    // skip trigger for ground
                    if (other.isTrigger)
                    {
                        return;
                    }

                    Rabbit partnerRabbit = other.GetComponent<Rabbit>();

                    if (!partnerRabbit || !isBreadable(partnerRabbit) || partnerRabbit.breading_is)
                    {
                        return;
                    }

                    breading_is = true;

                    // start dancing
                    partnerRabbit._controller.startDance();
                    _controller.startDance();

                    Debug.Log("Breading: " + other.name);

                    Invoke("breeding", breeding_duration);

                    break;
                }
        }
    }

    // utils

    private void createBaby(GameObject container)
    {
        GameObject obj = Instantiate(baby, transform.position, Quaternion.identity);
        obj.transform.SetParent(container.transform);
        obj.name = "Rabbit new";

        Rabbit rabbit = obj.GetComponent<Rabbit>();
        rabbit.Start();

        rabbit._year.Start();
        rabbit._year.updateYear(1);
    }

    public bool isBreadable(Rabbit partnerRabbit)
    {
        return !(!breading_target || !partnerRabbit.breading_target
                        || breading_target != partnerRabbit.gameObject
                        || partnerRabbit.breading_target != gameObject);
    }

    public bool isStartBreading()
    {
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
