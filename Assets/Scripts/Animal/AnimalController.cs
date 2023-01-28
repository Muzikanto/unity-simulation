using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class AnimalController : MonoBehaviour
{

    public float speed_initial = 2f;
    public float speed_by_year = 0.3f;

    public float jump_force_initial = 1.5f;
    public float jump_force_by_year = 0.25f;

    public float jump_force = 2f;

    private float speed = 3f;

    //

    private Rigidbody _rigidBody;

    public GameObject target = null;
    //
    private float _random_interval = 0;
    private Vector3 _random_position = Vector3.zero;

    public bool is_ground = false;
    public bool is_jump = false;
    public bool is_dance = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        updateRandomPosition();

        is_jump = false;
        is_dance = false;
        is_ground = false;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    //

    public void updateTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void updateYear(int new_year)
    {
        speed = speed_initial + (new_year * speed_by_year);
        jump_force = jump_force_initial + (new_year * jump_force_by_year);
    }

    //

    private void move()
    {
        if (!is_ground || is_jump)
        {
            fixedRotation();
            return;
        }
        if (is_dance)
        {
            fixedRotation();

            is_jump = true;

            _rigidBody.AddForce(Vector3.up * jump_force, ForceMode.Impulse);

            Invoke("resetJump", 1);

            return;
        }


        is_jump = true;

        if (target)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized * speed;
            dir.y = 1 * jump_force;

            _rigidBody.AddForce(dir, ForceMode.Impulse);
            rotate(target.transform.position);
        }
        else
        {
            _random_interval += Time.deltaTime;

            if (_random_interval >= 0.02)
            {
                updateRandomPosition();
                _random_interval = 0;
            }

            Vector3 dir = (_random_position - transform.position).normalized * speed;
            dir.y = 1 * jump_force;

            _rigidBody.AddForce(dir, ForceMode.Impulse);
            rotate(_random_position);
        }

        Invoke("resetJump", 1);
    }

    private void rotate(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            return;
        }

        Quaternion _lookRotation = Quaternion.LookRotation(dir);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 1);
        rotation.x = 0;
        rotation.z = 0;

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = rotation;
    }

    private void fixedRotation()
    {
        Quaternion rotation = transform.rotation;
        rotation.x = 0;
        rotation.z = 0;

        transform.rotation = rotation;
    }

    private void resetJump()
    {
        is_jump = false;
    }

    // dance

    public void startDance()
    {
        is_dance = true;
    }

    public void stopDance()
    {
        is_dance = false;
    }

    //

    private void updateRandomPosition()
    {
        _random_position.x = Random.value - 0.5f;
        _random_position.z = Random.value - 0.5f;

        _random_position.x *= 1_000;
        _random_position.z *= 1_000;
    }

    // implements

    void OnTriggerStay(Collider col)
    {               //если в тригере что то есть и у обьекта тег "ground"
        if (col.tag == "Ground") is_ground = true;      //то включаем переменную "на земле"

    }
    void OnTriggerExit(Collider col)
    {              //если из триггера что то вышло и у обьекта тег "ground"
        if (col.tag == "Ground") is_ground = false;     //то вЫключаем переменную "на земле"
    }

    // utils

    public void clearTarget()
    {
        target = null;
    }
}
