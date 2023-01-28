using UnityEngine;
using TMPro;

/// <summary>
/// Displays a configurable health bar for any object with a Damageable as a parent
/// </summary>
public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI _food_text;

    Camera mainCamera;
    private int _food_max = 0;

    private void Start()
    {
        // Cache since Camera.main is super slow
        mainCamera = Camera.main;
    }

    private void Update()
    {
        alignCamera();
    }

    public void initParams(int food_max)
    {
        _food_max = food_max;
    }

    public void updateParams(int food)
    {
        _food_text.SetText("Food: " + food + "/" + _food_max);
    }

    private void alignCamera()
    {
        if (mainCamera != null)
        {
            var camXform = mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

}