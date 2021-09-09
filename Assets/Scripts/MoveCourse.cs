using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCourse : MonoBehaviour
{
    private float genSpan;

    // Start is called before the first frame update
    void Start()
    {
        genSpan = Values.genSpan;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Values.isPlaying)
        {
            return;
        }

        gameObject.transform.position += new Vector3((float) (-1 / genSpan * 0.95), 0, 0) * Time.deltaTime;
    }
}