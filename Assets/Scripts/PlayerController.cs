using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float scoreTime;
    public Text scoreText;

    public GameObject gameOverCanvas;
    public Text resultScoreText;
    public Text highScoreText;

    public GameObject explosion;

    private bool isGameOvered = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private float timeCounter;

    // Update is called once per frame
    void Update()
    {
        if (!Values.isPlaying)
        {
            if (isGameOvered)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene("Main");
                }
            }

            return;
        }

        timeCounter += Time.deltaTime;
        if (timeCounter >= scoreTime)
        {
            timeCounter = 0;
            Values.score++;
            scoreText.text = Values.score.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Side"))
        {
            isGameOvered = true;
            Debug.Log("GameOver");

            Values.isPlaying = false;
            Physics.gravity = Vector3.zero;

            GetComponent<ParticleSystem>().Stop();
            GetComponent<MeshRenderer>().enabled = false;

            GameObject once = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(once, 1);

            if (Values.highScore < Values.score)
            {
                Values.highScore = Values.score;
            }

            gameOverCanvas.SetActive(true);
            resultScoreText.text = $"Score: {Values.score}";
            highScoreText.text = $"HighScore: {Values.highScore}";
        }
    }
}