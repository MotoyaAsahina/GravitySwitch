using UnityEngine;

public class MoveCourse : MonoBehaviour
{
    public bool hasNext;

    // Update is called once per frame
    void Update()
    {
        if (!Values.IsPlaying) return;

        gameObject.transform.position += new Vector3(-Values.Speed, 0, 0) * Time.deltaTime;

        if (gameObject.transform.position.x < Values.BeginX && !hasNext)
        {
            GenerateNextBox();
            hasNext = true;
        }

        if (gameObject.transform.position.x < Values.EndX)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void GenerateNextBox()
    {
        var i = ++Values.IndexOfBox;
        var x = gameObject.transform.position.x + 1;
        var yFloor = -Values.NormY + Values.Noise1.eval(i);
        var yCeil = Values.NormY - Values.Noise2.eval(i);

        var set = new GameObject();
        set.name = $"Box {i}";
        set.transform.position = new Vector3(x, 0, 0);
        set.AddComponent<MoveCourse>();

        var floor = Instantiate(Values.Box, new Vector3(x, yFloor, 0), Quaternion.identity);
        var ceil = Instantiate(Values.Box, new Vector3(x, yCeil, 0), Quaternion.identity);

        floor.transform.parent = set.transform;
        ceil.transform.parent = set.transform;

        floor.GetComponent<Renderer>().material.color = Values.Color[i % 2];
        ceil.GetComponent<Renderer>().material.color = Values.Color[i % 2];

        if (Random.Range(0f, 1f) < Values.CoinRatio)
        {
            Instantiate(
                Values.Coin,
                new Vector3(x, (float) (yFloor + 5.5), 0),
                Quaternion.identity);
        }

        if (Random.Range(0f, 1f) < Values.CoinRatio)
        {
            Instantiate(
                Values.Coin,
                new Vector3(x, (float) (yCeil - 5.5), 0),
                Quaternion.identity);
        }
    }
}