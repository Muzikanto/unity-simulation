using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float speed = 30f;

    private Transform _transform;
    private Rigidbody _rigidBody;

    private bool _run = false;
    private bool _walk = false;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _walk = !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl);
        _run = Input.GetKey(KeyCode.LeftShift);

        
        if (_walk)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
            _rigidBody.velocity = move;
        }
        if (_run)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * 1.5f;
            _rigidBody.velocity = move;
        }
    }
}
