using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int size = 1;
    public int size_max = 5;
    public int grow_sec = 10;

    private float _scale_initial = 0.2f;
    private float _scale_per_size = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        updateSize(Random.Range(1, 2));
        StartCoroutine(grow());
    }

    // 

    public void reset()
    {
        updateSize(0);
    }

    //

    IEnumerator grow()
    {
        while (true)
        {
            //Wait for seconds
            yield return new WaitForSecondsRealtime(grow_sec);

            if (size >= size_max)
            {
                continue;
            }

            updateSize(size + 1);
        }
    }

    //

    private void updateSize(int new_size)
    {
        size = new_size;

        float scale_value = _scale_initial + _scale_per_size * size;
        Vector3 scale = new Vector3(scale_value, scale_value, scale_value);

        transform.localScale = scale;
    }
}
