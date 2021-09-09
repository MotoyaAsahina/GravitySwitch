using UnityEngine;

public class Initializer : MonoBehaviour
{
    public float gravity;

    public GameObject player;
    public GameObject box;

    public GameObject boxBegin;
    public GameObject boxEnd;

    private int _beginPos;
    private int _endPos;

    public int normY = 10;
    public float speed;

    public int[] seed;
    public ValueNoise1dGenerator.NoiseParameter[] noiseParameters;

    public Color[] color;

    public GameObject coin;
    public float coinRatio;

    public GameObject titleCanvas;
    public GameObject scoreCanvas;

    private bool _isReady;

    private ParticleSystem _playerParticle;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, 0, 0);

        Values.Box = box;
        Values.Coin = coin;

        Values.NormY = normY;
        Values.Speed = speed;
        Values.Color = color;
        Values.CoinRatio = coinRatio;

        Values.IndexOfBox = 0;
        Values.Score = 0;

        _beginPos = (int) boxBegin.transform.position.x;
        _endPos = (int) boxEnd.transform.position.x + 1;

        Values.BeginX = _beginPos;
        Values.EndX = _endPos;

        if (seed.Length == 0)
        {
            int seed1 = Random.Range(-100000, 100000);
            int seed2 = Random.Range(-100000, 100000);
            seed = new[] {seed1, seed2};
        }
        else if (seed.Length == 1)
        {
            int seed1 = seed[0];
            seed = new[] {seed1, seed1};
        }

        Values.Noise1 = new ValueNoise1dGenerator(seed[0], true, noiseParameters);
        Values.Noise2 = new ValueNoise1dGenerator(seed[1], true, noiseParameters);

        _playerParticle = player.GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int x = _endPos; x <= _beginPos; x++)
        {
            var i = ++Values.IndexOfBox;
            var set = new GameObject();
            set.name = $"Box {i}";
            set.transform.position = new Vector3(x, 0, 0);
            set.AddComponent<MoveCourse>();

            var floor = Instantiate(box, new Vector3(x, -normY + 2, 0), Quaternion.identity);
            var ceil = Instantiate(box, new Vector3(x, normY - 2, 0), Quaternion.identity);

            floor.transform.parent = set.transform;
            ceil.transform.parent = set.transform;

            floor.GetComponent<Renderer>().material.color = color[i % 2];
            ceil.GetComponent<Renderer>().material.color = color[i % 2];

            if (x != _beginPos)
            {
                set.GetComponent<MoveCourse>().hasNext = true;
            }
        }

        titleCanvas.SetActive(true);
        _isReady = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Values.IsPlaying && Input.GetMouseButtonDown(0))
        {
            Physics.gravity *= -1;
        }

        if (_isReady && Input.GetMouseButtonDown(0))
        {
            titleCanvas.SetActive(false);
            scoreCanvas.SetActive(true);

            Physics.gravity = new Vector3(0, -gravity, 0);

            _playerParticle.Play();

            _isReady = false;
            Values.IsPlaying = true;
        }
    }
}