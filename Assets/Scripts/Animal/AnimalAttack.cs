using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAnimalAttack
{
    void OnAttackEnter(Collider other);
}

public class AnimalAttack : MonoBehaviour
{
    private GameObject _animal;
    private IAnimalAttack _vision;

    private void Awake()
    {
        _animal = transform.parent.gameObject;
        _vision = _animal.GetComponent<IAnimalAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _vision.OnAttackEnter(other);
    }
}
