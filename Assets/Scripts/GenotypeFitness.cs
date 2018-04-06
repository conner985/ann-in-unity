
public class GenotypeFitness
{
    private double[] genotype;
    private double fitness;

    public GenotypeFitness(double[] genotype, double fitness)
    {
        this.genotype = genotype;
        this.fitness = fitness;
    }

    public double[] GetGenotype()
    {
        return genotype;
    }

    public void SetGenotype(double[] genotype)
    {
        this.genotype = genotype;
    }

    public double GetFitness()
    {
        return fitness;
    }

    public void SetFitness(double fitness)
    {
        this.fitness = fitness;
    }

    public GenotypeFitness Clone()
    {
        return new GenotypeFitness((double[])genotype.Clone(), fitness);
    }
}