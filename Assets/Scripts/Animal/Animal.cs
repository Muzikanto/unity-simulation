using UnityEngine;

public class Animal : MonoBehaviour
{
    public float speed = 10f;

    private Transform _transform;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(1, 0, 0) * speed;
        _rigidBody.velocity = move;
    }
}
