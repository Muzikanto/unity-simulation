using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using System.Drawing;
using static UnityEngine.ParticleSystem;

public class Rabbit : MonoBehaviour, IAnimalVision, IAnimalAttack
{
    public float speed = 5f;
    public float jump_force = 2f;

    public int food_max = 10;
    public int food = 0;

    private Transform _transform;
    private Rigidbody _rigidBody;

    private List<Collider> _food_list = new List<Collider>();

    public HealthBar healthBar;
    private AnimalController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
        _controller = GetComponent<AnimalController>();

        updateTarget();

        food = 5;
        healthBar.initParams(food_max);

        StartCoroutine(consumeFood());
    }

    // Update is called once per frame
    void Update()
    {
        updateTarget();
        updateParams();
    }

    public void updateParams()
    {
        healthBar.updateParams(food);
    }

    private void updateTarget()
    {
        if (_controller.target != null)
        {
            return;
        }

        if (food != food_max && _food_list.Count > 0)
        {
            //_food_list.Sort(delegate(Collider x, Collider y)
            //{
            //    return 1;
            //});

            _food_list.ForEach(x => { if (x == null) _food_list.Remove(x); });


            Collider first = _food_list.Find(x =>
            {
                Food foodGo = x.GetComponent<Food>();

                return foodGo.size > 2;
            });


            if (first)
            {
                _controller.updateTarget(first);
            }
        }
    }

    IEnumerator consumeFood()
    {
        while (food > 0)
        {
            //Wait for seconds
            yield return new WaitForSecondsRealtime(10);

            food--;
        }

        Destroy(gameObject);
    }

    // imlements

    public void OnVisionEnter(Collider other)
    {
        string entityTag = other.tag;

        if (entityTag == "Food")
        {
            _food_list.Add(other);
        }
    }

    public void OnVisionExit(Collider other)
    {
        _food_list.Remove(other);
    }

    public void OnAttackEnter(Collider other)
    {
        string entityTag = other.tag;

        if (entityTag == "Food")
        {
            Food foodGO = other.GetComponent<Food>();

            if (food < food_max)
            {
                if (food + foodGO.size >= food_max)
                {
                    food = food_max;
                }  else
                {
                    food = food + foodGO.size;
                }
            }

            foodGO.reset();
            _controller.updateTarget(null);
        }
    }
}
