using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAnimalVision
{
    void OnVisionEnter(Collider other);
    void OnVisionExit(Collider other);
}

public class AnimalVision : MonoBehaviour
{
    private GameObject _animal;
    private IAnimalVision _vision;

    private void Start()
    {
        _animal = transform.parent.gameObject;
        _vision = _animal.GetComponent<IAnimalVision>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_vision != null)
        {
            _vision.OnVisionEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _vision.OnVisionExit(other);
    }
}
