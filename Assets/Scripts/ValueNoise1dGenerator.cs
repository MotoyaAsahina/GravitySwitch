using System;

public class ValueNoise1dGenerator
{
    [Serializable]
    public struct NoiseParameter
    {
        public int freq;
        public int amp;

        public NoiseParameter(int freq, int amp)
        {
            this.freq = freq;
            this.amp = amp;
        }
    }

    private bool positive;
    private int octaves;
    private NoiseParameter[] parameters;

    private double[][] rand;

    public ValueNoise1dGenerator(int seed, bool positive, NoiseParameter[] parameters)
    {
        this.positive = positive;
        this.parameters = parameters;
        octaves = parameters.Length;

        Random random = new Random(seed);
        this.rand = new double[octaves][];

        for (int i = 0; i < octaves; i++)
        {
            // 長さ256の乱数配列を octaves 個作成
            rand[i] = new double[256];
            for (int j = 0; j < 256; j++)
            {
                rand[i][j] = random.NextDouble();
            }
        }
    }

    public int eval(int x)
    {
        int res = 0;
        for (int i = 0; i < octaves; i++)
        {
            res += (int) lerp(i, x);
        }

        return res;
    }

    private double lerp(int octave, int x)
    {
        int freq = this.parameters[octave].freq;
        int lx = (int) Math.Floor((double) x / freq);
        int rx = lx + 1;

        double t = (double) x / freq - lx;

        double lo = rand[octave][lx & 255];
        double hi = rand[octave][rx & 255];

        // Debug
//        System.out.println("octave = " + octave + ", x = " + x + ", freq = " + freq);
//        System.out.println("lx = " + lx + ", rx = " + rx + ", t = " + t);
//        System.out.println("lo = " + lo + ", hi = " + hi);
//        System.out.println((lo * (freq - t) + hi * t) / freq);

        t = remap(t);

        if (positive) return (lo * (1 - t) + hi * t) * parameters[octave].amp;
        return (lo * (1 - t) + hi * t) * parameters[octave].amp; // not working
    }

    private double remap(double t)
    {
        return t * t * t * (6 * t * t - 15 * t + 10);
    }
}