using UnityEngine;

public class Fox : MonoBehaviour
{
    public float speed = 10f;
    public float vision = 3f;

    public float need_food = 10f;
    public float food = 0f;

    private Transform _transform;
    private Rigidbody _rigidBody;
    private GameObject _vision;
    private SphereCollider _visionCollider;

    private Vector3 _move_to = Vector3.zero;
    private float _move_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody>();

        _vision = _transform.GetChild(0).gameObject;
        _visionCollider = _vision.GetComponent<SphereCollider>();

        randomMove();

        food = need_food;
    }

    // Update is called once per frame
    void Update()
    {
        refresh();
        move();
    }

    // imlements
   
    public void onVision(Collider other)
    {
        string entityTag = other.tag;

        if (entityTag == "Rabbit")
        {
            // Vector3 dir = other.transform.position - transform.position;
            // Debug.DrawRay(transform.position, dir, Color.red, 5, false);

            Destroy(other.gameObject);
        }
    }

    // utils

    private void move()
    {
        _move_time += Time.deltaTime;

        if (_move_time >= 5)
        {
            randomMove();
            _move_time = 0;
        }


        Vector3 move = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * speed;
        _rigidBody.velocity = _move_to;
    }

    private void randomMove()
    {
        if (Random.value > 0.5f)
        {
            _move_to.x = -1;
        }
        else
        {
            _move_to.x = 1;
        }

        if (Random.value > 0.5f)
        {
            _move_to.z = -1;
        }
        else
        {
            _move_to.z = 1;
        }
      
    }

    private void refresh()
    {
        // settings
        _visionCollider.radius = vision;
    }
}
