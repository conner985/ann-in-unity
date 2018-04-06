using System;
using System.Collections.Generic;

public class GA_lib {

    public static List<GenotypeFitness> Evaluation(List<GenotypeFitness> genotype_fitness)
    {
        List<GenotypeFitness> g_f = new List<GenotypeFitness>();

        Comparison<GenotypeFitness> c = (x, y) => x.GetFitness().CompareTo(y.GetFitness());
        genotype_fitness.Sort(c);

        int i;
        for(i = 0; i<genotype_fitness.Count; i++)
        {
            double rank = (i + 1f) / (genotype_fitness.Count);
            genotype_fitness[i].SetFitness(rank);
        } 

        double partial_sum = 0;
        for (i = 0; i < genotype_fitness.Count; i++)
        {
            partial_sum += genotype_fitness[i].GetFitness();
            genotype_fitness[i].SetFitness(partial_sum);
        }

        foreach (GenotypeFitness g in genotype_fitness)
        {
            g_f.Add(g.Clone());
        }


        return g_f;
        
    }

    public static List<double[]> Selection(List<GenotypeFitness> genotype_fitness)
    {
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

        List<double[]> selected_pop = new List<double[]>();
        double min = genotype_fitness[0].GetFitness(); 
        double max = genotype_fitness[genotype_fitness.Count - 1].GetFitness(); 

        int i;
        for (i = 0; i < genotype_fitness.Count; i++)
        {
            double extraction = (rnd.NextDouble() * (max-min))+min;
            int j;
            for (j = 0; j < genotype_fitness.Count; j++)
            {
                if (extraction <= genotype_fitness[j].GetFitness()) 
                {
                    selected_pop.Add((double[])(genotype_fitness[j].GetGenotype().Clone()));
                    break;
                }
            }
        }

        return selected_pop;
    }

    public static List<double[]> Crossover(List<double[]> selected_pop, double crossover_prob)
    {
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

        List<double[]> new_pop = new List<double[]>();
        int i;
        for (i = 0; i < selected_pop.Count; i+=2)
        {
            double[] mother_genotype = (double[])selected_pop[i].Clone();
            double[] father_genotype = (double[])selected_pop[i+1].Clone();

            double extraction = rnd.NextDouble();
            if (extraction <= crossover_prob)
            {
                int cutpoint = rnd.Next(0, selected_pop[0].Length);
                int j;
                for (j = 0; j < selected_pop[0].Length; j++)
                {
                    if (cutpoint <= j)
                    {
                        double temp = mother_genotype[j];
                        mother_genotype[j] = father_genotype[j];
                        father_genotype[j] = temp;
                    }
                }
            }

            new_pop.Add(mother_genotype);
            new_pop.Add(father_genotype);
        }

        return new_pop;
    }

    public static List<double[]> Mutation(List<double[]> new_pop, double mutation_prob, double mutation_dev)
    {
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

        List<double[]> final_pop = new List<double[]>();

        int i;
        for(i = 0; i < new_pop.Count-1; i++)
        {
            int j;
            for (j = 0; j < new_pop[0].Length; j++)
            {
                double extraction = rnd.NextDouble();
                if (extraction <= mutation_prob)
                {
                    new_pop[i][j] += mutation_dev*(rnd.NextDouble()*2 - 1);
                }
            }
            final_pop.Add((double[])new_pop[i].Clone());
        }
        return final_pop;
    }
}
