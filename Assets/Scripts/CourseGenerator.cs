using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CourseGenerator : MonoBehaviour
{
    public float gravity;

    public GameObject player;
    public GameObject box;

    public int width;
    public int genPositionX;
    public int genPositionY;
    public float genSpan = 1f;
    private float lifeTime;

    private GameObject newestBox;
    private Queue<GameObject> deleteQueue;

    public int[] seed;
    public ValueNoise1dGenerator.NoiseParameter[] parameters;

    private ValueNoise1dGenerator noise1, noise2;

    public Color[] color;

    public GameObject coin;
    public float coinRatio;

    public GameObject titleCanvas;
    public GameObject scoreCanvas;

    private bool isReady = false;

    private ParticleSystem playerParticle;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, 0, 0);

        Values.Score = 0;

        Values.GenSpan = genSpan;
        lifeTime = 40 * genSpan;

        deleteQueue = new Queue<GameObject>();

        if (seed.Length == 0)
        {
            int seed1 = Random.Range(0, 100000);
            int seed2 = Random.Range(0, 100000);
            seed = new[] {seed1, seed2};
        }
        else if (seed.Length == 1)
        {
            int seed1 = seed[0];
            seed = new[] {seed1, seed1};
        }

        noise1 = new ValueNoise1dGenerator(seed[0], true, parameters);
        noise2 = new ValueNoise1dGenerator(seed[1], true, parameters);

        playerParticle = player.GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (Values.IsPlaying && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Physics.gravity *= -1;
        }

        if (isReady && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            scoreCanvas.SetActive(true);
            titleCanvas.SetActive(false);
            GameStart();
        }
    }

    private void GameStart()
    {
        isReady = false;
        Values.IsPlaying = true;

        for (int x = -30; x < 0; x++)
        {
            GameObject floorBox =
                Instantiate(box, new Vector3(genPositionX + x, -genPositionY + 2, 0), Quaternion.identity);
            GameObject ceilBox =
                Instantiate(box, new Vector3(genPositionX + x, genPositionY - 2, 0), Quaternion.identity);

            floorBox.GetComponent<Renderer>().material.color = color[(x + 40) % 2];
            ceilBox.GetComponent<Renderer>().material.color = color[(x + 40) % 2];

            newestBox = floorBox;
            deleteQueue.Enqueue(floorBox);
            deleteQueue.Enqueue(ceilBox);
        }

        playerParticle.Play();

        Physics.gravity = new Vector3(0, -gravity, 0);
        StartCoroutine("Gen");
    }

    private void GameSetting()
    {
        // for (int x = -30; x < 0; x++)
        // {
        //     GameObject floorBox =
        //         Instantiate(box, new Vector3(genPositionX + x, -genPositionY + 2, 0), Quaternion.identity);
        //     GameObject ceilBox =
        //         Instantiate(box, new Vector3(genPositionX + x, genPositionY - 2, 0), Quaternion.identity);
        //
        //     floorBox.GetComponent<Renderer>().material.color = color[(x + 40) % 2];
        //     ceilBox.GetComponent<Renderer>().material.color = color[(x + 40) % 2];
        //
        //     newestBox = floorBox;
        //     deleteQueue.Enqueue(floorBox);
        //     deleteQueue.Enqueue(ceilBox);
        //
        //     // Destroy(floorBox, lifeTime);
        //     // Destroy(ceilBox, lifeTime);
        // }

        isReady = true;
    }

    IEnumerator Gen()
    {
        for (int numOfBlocks = 0; Values.IsPlaying; numOfBlocks++)
        {
            int floor = noise1.eval(numOfBlocks);
            int ceil = noise2.eval(numOfBlocks);

            Vector3 position = newestBox.transform.position;
            GameObject floorBox =
                Instantiate(box, new Vector3(position.x + 1, -genPositionY + floor, 0), Quaternion.identity);
            GameObject ceilBox =
                Instantiate(box, new Vector3(position.x + 1, genPositionY - ceil, 0), Quaternion.identity);

            floorBox.GetComponent<Renderer>().material.color = color[numOfBlocks % 2];
            ceilBox.GetComponent<Renderer>().material.color = color[numOfBlocks % 2];

            if (Random.Range(0f, 1f) < coinRatio)
            {
                GameObject genCoin = Instantiate(
                    coin,
                    new Vector3(position.x + 1, (float) (-genPositionY + floor + 5.5), 0),
                    Quaternion.identity);
                Destroy(genCoin, lifeTime);
            }

            if (Random.Range(0f, 1f) < coinRatio)
            {
                GameObject genCoin = Instantiate(
                    coin,
                    new Vector3(position.x + 1, (float) (genPositionY - ceil - 5.5), 0),
                    Quaternion.identity);
                Destroy(genCoin, lifeTime);
            }

            newestBox = floorBox;
            deleteQueue.Enqueue(floorBox);
            deleteQueue.Enqueue(ceilBox);

            if (numOfBlocks % 60 != 0)
            {
                Destroy(deleteQueue.Dequeue());
                Destroy(deleteQueue.Dequeue());
            }

            yield return new WaitForSeconds(genSpan);
        }
    }
}