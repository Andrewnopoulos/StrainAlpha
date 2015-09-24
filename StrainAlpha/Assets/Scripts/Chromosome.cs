using UnityEngine;
using System.Collections;

public enum GENE
{
    HEALTH = 0,
    DAMAGE = 1,
    RANGED = 2,
    SPEED = 3
}

public class Chromosome {

    private static int length = 4;
    public static float mutationRate = 0.1f;
    public static float mutationStrength = 0.3f;
    public static float randomInitValue = 0.04f;

    public static float additionScaling = 0.4f;

    private static float MaxValue = 1.0f;

    private float[] genes = new float[length];
    private GENE topIndex = GENE.HEALTH;

    public Chromosome(int input)
    {
        if (input == 4)
        {
            initGenes(randomInitValue);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == input)
                {
                    genes[i] = Random.Range(0.6f, 0.8f);
                }
                else
                {
                    genes[i] = Random.Range(0.0f, randomInitValue);
                }
            }
        }
    }

    //public Chromosome(float initVal)
    //{
    //    initGenes(initVal);
    //}

    public Chromosome()
    {
        initGenes(0);
    }

    public Chromosome(float[] initValues)
    {
        if (initValues.Length == length)
        {
            for (int i = 0; i < length; i++)
            {
                genes[i] = initValues[i];
            }
        }
        else
        {
            initGenes(randomInitValue);
        }
    }

    private void initGenes(float inputValue)
    {
        for (int i = 0; i < length; i++)
        {
            genes[i] = Random.Range(0.0f, inputValue);
        }
    }

    public float this[int i]
    {
        get { return genes[i]; }
        set { genes[i] = value; }
    }

    public static int ChromosomeLength()
    {
        return length;
    }

    public static Chromosome Crossover(Chromosome mum, Chromosome dad)
    {
        int crossoverPoint = Random.Range(0, length - 1);
        float[] output1 = new float[length];
        float[] output2 = new float[length];
        for (int i = 0; i < crossoverPoint; i++)
        {
            output1[i] = mum[i];
            output2[i] = dad[i];
        }
        for (int i = crossoverPoint; i < length; i++)
        {
            output2[i] = mum[i];
            output1[i] = dad[i];
        }

        Chromosome out1 = new Chromosome(output1);
        Chromosome out2 = new Chromosome(output2);

        float c1Strength = 0;
        float c2Strength = 0;

        for (int i = 0; i < length; i ++)
        {
            c1Strength += output1[i];
            c2Strength += output2[i];
        }

        return (c1Strength > c2Strength) ? out1 : out2;
    }

    public void Mutate()
    {
        for (int i = 0; i < length; i++)
        {
            if (Random.Range(0.0f, 1.0f) > mutationRate)
            {
                genes[i] += Random.Range(-0.5f * mutationStrength, mutationStrength);

                if (genes[i] > MaxValue)
                {
                    genes[i] = MaxValue;
                }else if (genes[i] < 0)
                {
                    genes[i] = 0;
                }
            }
        }

        Evaluate();
    }

    public float Evaluate()
    {
        float output = 0;
        int index = 0;

        for (int i = 0; i < length; i++)
        {
            if (genes[i] > output)
            {
                output = genes[i];
                index = i;
            }
        }

        topIndex = (GENE)index;

        return output;
    }

    public void AddChromosome(Chromosome inputChromosome)
    {
        for (int i = 0; i < length; i++)
        {
            genes[i] += inputChromosome[i] * additionScaling;

            if (genes[i] > MaxValue)
            {
                genes[i] = MaxValue;
            }
            else if (genes[i] < 0)
            {
                genes[i] = 0;
            }
        }
    }

    public float[] GetGenes()
    {
        return genes;
    }
	
	// Update is called once per frame
	void Update () {

	}

}
