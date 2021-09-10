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
    private bool endExplosion;

    private float timeCounter;

    // Update is called once per frame
    void Update()
    {
        if (!Values.IsPlaying && endExplosion)
        {
            if (isGameOvered)
            {
                if (Input.GetMouseButtonDown(0))
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
            Values.Score++;
            scoreText.text = Values.Score.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Side"))
        {
            isGameOvered = true;

            Values.IsPlaying = false;
            Physics.gravity = Vector3.zero;

            GetComponent<ParticleSystem>().Stop();
            GetComponent<MeshRenderer>().enabled = false;

            GameObject once = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(once, 1);
            Invoke("ChangeFlag", 1);

            if (Values.HighScore < Values.Score)
            {
                Values.HighScore = Values.Score;
            }

            gameOverCanvas.SetActive(true);
            resultScoreText.text = $"Score: {Values.Score}";
            highScoreText.text = $"HighScore: {Values.HighScore}";
        }
    }

    private void ChangeFlag()
    {
        endExplosion = true;
    }
}