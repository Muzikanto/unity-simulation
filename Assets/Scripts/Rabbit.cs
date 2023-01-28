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
    private Transform _transform = null;
    private Rigidbody _rigidBody = null;

    public HealthBar healthBar;
    public AnimalController _controller = null;
    public AnimalConsumeFood _food = null;
    public AnimalYear _year = null;
    public AnimalBreading _breeding = null;

    private List<GameObject> _food_list = new List<GameObject>();
    private List<GameObject> _rabbit_list = new List<GameObject>();

    public void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<AnimalController>();
        _food = GetComponent<AnimalConsumeFood>();
        _year = GetComponent<AnimalYear>();
        _breeding = GetComponent<AnimalBreading>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<AnimalController>();
        _food = GetComponent<AnimalConsumeFood>();
        _year = GetComponent<AnimalYear>();
        _breeding = GetComponent<AnimalBreading>();

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
        if (_breeding.isStartBreading())
        {
            // if i see rabbits
            if (_rabbit_list.Count > 0)
            {
                // remove deleted
                _rabbit_list.RemoveAll(x => x == null);

                // get first breadable rabbit
                GameObject first = _rabbit_list.Find(el => {
                    AnimalBreading b = el.GetComponent<AnimalBreading>();
                    bool is_true = b.isStartBreading();

                    return is_true;
                });

                // update target
                if (first)
                {
                    Rabbit partnerRabbit = first.GetComponent<Rabbit>();

                    // stop partner searching
                    _breeding.stopBreadingSearch();
                    partnerRabbit._breeding.stopBreadingSearch();

                    // setup partner
                    _breeding.breading_target = first;
                    partnerRabbit._breeding.breading_target = gameObject;

                    // move to partner
                    _controller.updateTarget(first);
                    partnerRabbit._controller.updateTarget(gameObject);

                    return;
                }
            }
        }

        // find food
        if (!_food.isFull())
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
                    if (_food.isFull() || _breeding.breading_target)
                    {
                        return;
                    }

                    Food foodGO = other.GetComponent<Food>();

                    int food_taked = 0;

                    if (_food.food + foodGO.size >= _food.food_max)
                    {
                        food_taked = _food.food_max - _food.food;
                    }
                    else
                    {
                        food_taked = foodGO.size;
                    }

                    _food.increase(food_taked);

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

                    if (other.gameObject)
                    {
                        _breeding.startBreading(other.gameObject);
                    }

                    break;
                }
        }
    }

    // utils
}
