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

public class Fox : MonoBehaviour, IAnimalVision, IAnimalAttack
{
    private Transform _transform = null;
    private Rigidbody _rigidBody = null;

    public HealthBar healthBar;
    public AnimalController _controller = null;
    public AnimalConsumeFood _food = null;
    public AnimalYear _year = null;
    public AnimalBreading _breeding = null;

    private List<GameObject> _fox_list = new List<GameObject>();
    private List<GameObject> _rabbit_list = new List<GameObject>();

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
            if (_fox_list.Count > 0)
            {
                // remove deleted
                _fox_list.RemoveAll(x => x == null);

                // get first breadable rabbit
                GameObject first = _fox_list.Find(el =>
                {
                    AnimalBreading b = el.GetComponent<AnimalBreading>();
                    bool is_true = b.isStartBreading();

                    return is_true;
                });

                // update target
                if (first)
                {
                    Fox partnerRabbit = first.GetComponent<Fox>();

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
            if (_rabbit_list.Count > 0)
            {
                // remove deleted
                _rabbit_list.RemoveAll(x => x == null);

                // get first available food
                GameObject first = _rabbit_list[0];


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
        switch (other.tag)
        {
            case "Rabbit":
                {
                    _rabbit_list.Add(other.gameObject);
                    break;
                };

            case "Fox":
                {
                    _fox_list.Add(other.gameObject);
                    break;
                };
        }
    }

    public void OnVisionExit(Collider other)
    {
        switch (other.tag)
        {
            case "Rabbit":
                {
                    _rabbit_list.Remove(other.gameObject);
                    break;
                };

            case "Fox":
                {
                    _fox_list.Remove(other.gameObject);
                    break;
                };
        }
    }

    public void OnAttackEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Rabbit":
                {
                    if (_food.isFull() || _breeding.breading_target)
                    {
                        return;
                    }

                    int food_size = 2;
                    int food_taked = 0;

                    if (_food.food + food_size >= _food.food_max)
                    {
                        food_taked = _food.food_max - _food.food;
                    }
                    else
                    {
                        food_taked = food_size;
                    }

                    Destroy(other.gameObject);
                    _food.increase(food_taked);

                    _controller.clearTarget();

                    break;
                };

            case "Fox":
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

        // utils
    }
}
