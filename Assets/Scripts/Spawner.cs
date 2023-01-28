using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entity;
    public GameObject entities;
    public int initialCount = 5;

    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();

        for(var i = 0; i < initialCount; i++)
        {
            spawnEntity();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnEntity()
    {
        Vector3 areaCenter = _transform.position;
        Vector3 areaSize = _transform.localScale;

        Vector3 pos = new Vector3(0, areaCenter.y, 0);
        pos.x = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
        pos.z = Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2);

        GameObject obj = Instantiate(entity, pos, Quaternion.identity);
        obj.transform.SetParent(entities.transform);
      }
}
