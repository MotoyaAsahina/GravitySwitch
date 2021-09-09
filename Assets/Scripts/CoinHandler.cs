using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    public float[] rotation = new float[3];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotation[0], rotation[1], rotation[2]) * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Values.score += 5;
            other.gameObject.GetComponent<PlayerController>().scoreText.text = Values.score.ToString();
        }
    }
}