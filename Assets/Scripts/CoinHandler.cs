using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    public float[] rotation = new float[3];

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotation[0], rotation[1], rotation[2]) * Time.deltaTime, Space.World);

        if (!Values.IsPlaying) return;

        transform.position += new Vector3(-Values.Speed, 0, 0) * Time.deltaTime;

        if (gameObject.transform.position.x < Values.EndX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Values.IsPlaying)
        {
            Destroy(gameObject);
            Values.Score += 5;
            other.gameObject.GetComponent<PlayerController>().scoreText.text = Values.Score.ToString();
        }
    }
}