
using UnityEngine;

public class RotattorCamera : MonoBehaviour
{
    public float speed = 5f;
    private Transform _rotetor;

    private void Start()
    {
        _rotetor = GetComponent<Transform>();
    }

    private void Update()
    {
        _rotetor.Rotate(0,speed * Time.deltaTime,0);
    }
}
