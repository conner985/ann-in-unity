using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    public Sprite mouse;
    public Sprite wall;
    public Sprite floor;
    public Material red, green;
    public int pop_size = 10;
    public double mutation_prob = 0.1;
    public double crossover_prob = 0.7;
    public double mutation_dev = 0.2;

    private List<GenotypeFitness> genotype_fitness;

    private double[] best_genotype;
    private double best_fitness = 0;
    private bool new_best = false;
    
    private int act_mouse = 0;
    private int generation = 0;
    private bool complete = false;

    void Start () {

        Application.runInBackground = true;

        MazeCreator.CreateMaze(wall,floor,red,green);

        genotype_fitness = new List<GenotypeFitness>();
        
        NewRandomGeneration();
	}

    private void NewRandomGeneration()
    {
        GameObject.Find("Generation").GetComponent<Text>().text = "Generation: " + (++generation);
        int i;
        for (i = 0; i<pop_size; i++) SpawnRandomMouse();
    }
    
    private void SpawnRandomMouse()
    {
        GameObject mouse_obj = MouseCreator.CreateMouse(mouse, "Mouse", "Mouse");
        mouse_obj.GetComponent<MouseScript>().SetRandomGenotype();

    }


    private void NewGeneration(List<double[]> pop)
    { 
        if (new_best) SpawnBestMouse();
        genotype_fitness.Clear();
        
        GameObject.Find("Generation").GetComponent<Text>().text = "Generation: " + (++generation);

        foreach (double[] el in pop) SpawnMouse(el);
    }

    private void SpawnMouse(double[] genotype)
    {
        GameObject mouse_obj = MouseCreator.CreateMouse(mouse, "Mouse", "Mouse");
        mouse_obj.GetComponent<MouseScript>().SetGenotype(genotype);

    }

    public void Stop(MouseScript mouse)
    {

        if (!complete)
        {
            double mouse_fitness = mouse.GetGenotype_Fitness().GetFitness();
            double[] mouse_genotype = (double[])mouse.GetGenotype_Fitness().GetGenotype().Clone();

            genotype_fitness.Add(mouse.GetGenotype_Fitness().Clone());
            Destroy(mouse.gameObject);

            if (mouse_fitness > best_fitness)
            {
                new_best = true;
                best_fitness = mouse_fitness;
                best_genotype = mouse_genotype;
            }

            act_mouse++;
            if (act_mouse == pop_size)
            {
                act_mouse = 0;
                GA();
            }
        }
        else
        {
            Destroy(mouse.gameObject);
        }

    }

    public void Complete(MouseScript mouse)
    {
        complete = true;
        Debug.Log("Complete");
        best_genotype = (double[])mouse.GetGenotype_Fitness().GetGenotype().Clone();
        Destroy(mouse.gameObject);
        SpawnBestMouse();
    }

    private void GA()
    {
        genotype_fitness.Add(new GenotypeFitness((double[])best_genotype.Clone(), best_fitness));
        List<GenotypeFitness> g_f = GA_lib.Evaluation(genotype_fitness);
        List<double[]> selected_pop = GA_lib.Selection(g_f);
        List<double[]> new_pop = GA_lib.Crossover(selected_pop, crossover_prob);
        List<double[]> final_pop = GA_lib.Mutation(new_pop, mutation_prob, mutation_dev);

        NewGeneration(final_pop);

    }

    private void SpawnBestMouse()
    {
        new_best = false;
        GameObject mouse_obj = MouseCreator.CreateMouse(mouse,"Best_Mouse","Best_Mouse");
        mouse_obj.transform.position += new Vector3(0, 0, -1);
        Destroy(mouse_obj.GetComponent<MouseScript>());
        mouse_obj.AddComponent<MouseBest>();
        mouse_obj.GetComponent<SpriteRenderer>().color = green.color;
        mouse_obj.GetComponent<MouseBest>().SetGenotype((double[])best_genotype.Clone());
    }
}
