using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Transform player;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = transform.position;

        temp.x = player.position.x;
        temp.x += offset;
        temp.y = player.position.y;
        temp.y += offset;

        transform.position = temp;

    }
}
