using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class MouseScript : MonoBehaviour {

    private int iter;
    private bool start = false;

    private const int input_neurons = 3, output_neurons = 3;
    private double[,] sensors2inputs, inputs2outputs;
    private const int moves = 25;

    private List<RaycastHit2D> sensors;
    private RaycastHit2D front, right, left;

    private GenotypeFitness genotype_fitness;
    
    
    void Update ()
    {

        if (start && iter < moves)
        {
            iter++;

            UpdateSensors();

            List<int> firedOutputs = Perceptron_lib.GetFiredOutputs(sensors, sensors2inputs, inputs2outputs);
            foreach (int o in firedOutputs) 
            { 
                switch (o)
                {
                    case 0: //rot left
                        transform.Rotate(0, 0, 90);
                        break;
                    case 1: //rot right
                        transform.Rotate(0, 0, -90);
                        break;
                    case 2: //go forward
                        if (!front) transform.position += transform.up * 0.64f; 
                        break;
                }
                UpdateSensors();
            }

        } else if (start && iter == moves) Stop();

        CheckPosition();

    }

    private void Stop()
    {
        start = false;

        double[] genotype = Perceptron_lib.GetWeights(sensors2inputs, inputs2outputs);
        double fitness = GetFitness();
        genotype_fitness = new GenotypeFitness(genotype,fitness);

        GameObject.Find("Master").SendMessage("Stop", this);

    }

    private void CheckPosition()
    {
        RaycastHit2D exit = Physics2D.Raycast(transform.position, transform.up, 0.1f, 1 << LayerMask.NameToLayer("Exit"));
        if(exit)
        {
            start = false;
            double[] genotype = Perceptron_lib.GetWeights(sensors2inputs, inputs2outputs);
            double fitness = GetFitness();

            genotype_fitness = new GenotypeFitness(genotype, fitness);
            GameObject.Find("Master").SendMessage("Complete", this);
        } 
            
    }

    private void UpdateSensors()
    {
        right = Physics2D.Raycast(transform.position,  transform.right, 0.64f, 1 << LayerMask.NameToLayer("Wall"));
        left  = Physics2D.Raycast(transform.position, -transform.right, 0.64f, 1 << LayerMask.NameToLayer("Wall"));
        front = Physics2D.Raycast(transform.position,  transform.up,    0.64f, 1 << LayerMask.NameToLayer("Wall"));
        sensors = new List<RaycastHit2D>() { front, right, left };
    }

    private double GetFitness()
    {
        double fitness = 0;
        int x = (int)(transform.position.x / 0.62f);
        int y = (int)(Math.Abs(transform.position.y) / 0.62f);

        switch (y)
        {
            case 1:
                fitness = 11 + Math.Abs(x - 5);
                break;
            case 2:
                fitness = 10;
                break;
            case 3:
                fitness = 6 + Math.Abs(x - 2);
                break;
            case 4:
                fitness = 5;
                break;
            case 5:
                fitness = 1 + Math.Abs(x - 5);
                break;
            case 6:
                fitness = 1;
                break;
        }

        return 1/fitness;
    }

    private void Init()
    {
        iter = 0;
        start = true;
        UpdateSensors();
    }

    private void Create_2LayerPerceptron(double[] weights)
    {
        List<double[,]> perceptron_layers = Perceptron_lib.Create_2LayerPerceptron(weights, sensors.Count, input_neurons, output_neurons);
        sensors2inputs = perceptron_layers[0];
        inputs2outputs = perceptron_layers[1];
    }


    public GenotypeFitness GetGenotype_Fitness()
    {
        return genotype_fitness;
    }

    public void SetGenotype(double[] genotype)
    {

        Init();
        Create_2LayerPerceptron(genotype);
    }

    public void SetRandomGenotype()
    {

        Init();

        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);

        double[] genotype = new double[(sensors.Count * input_neurons) + (input_neurons * output_neurons)];

        int i;
        for (i = 0; i < genotype.Length; i++) genotype[i] = rnd.NextDouble();

        Create_2LayerPerceptron(genotype);

    }

}
