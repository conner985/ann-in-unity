using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class MouseBest : MonoBehaviour
{

    private int iter;
    private bool start = false;

    private const int input_neurons = 3, output_neurons = 3;
    private double[,] sensors2inputs, inputs2outputs;
    private const int moves = 25;

    private List<RaycastHit2D> sensors;
    private RaycastHit2D front, right, left;


    void Update()
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

        }
        else if (start && iter == moves) Stop();

        CheckPosition();

    }

    private void Stop()
    {
        start = false;
    }

    private void CheckPosition()
    {
        int y = (int)(Math.Abs(transform.position.y) / 0.62f);
        if (y == 6) Stop();
    }

    private void UpdateSensors()
    {
        right = Physics2D.Raycast(transform.position, transform.right, 0.64f, 1 << LayerMask.NameToLayer("Wall"));
        left = Physics2D.Raycast(transform.position, -transform.right, 0.64f, 1 << LayerMask.NameToLayer("Wall"));
        front = Physics2D.Raycast(transform.position, transform.up, 0.64f, 1 << LayerMask.NameToLayer("Wall"));
        sensors = new List<RaycastHit2D>() { front, right, left };
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

    public void SetGenotype(double[] genotype)
    {

        Init();
        Create_2LayerPerceptron(genotype);
    }

}
