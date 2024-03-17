using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    // Update is called once per frame

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 6.0f)
        {
            Destroy(gameObject);
        }
    }


}
