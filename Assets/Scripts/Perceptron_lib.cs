using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Perceptron_lib
    {

        public static List<int> GetFiredOutputs (List<RaycastHit2D> sensors, double[,] sensors2inputs, double[,] inputs2outputs)
        {
            List<int> activated_inputs = new List<int>();
            List<int> activated_outputs = new List<int>();

            int input_neurons = sensors2inputs.GetLength(1);
            int output_neurons = inputs2outputs.GetLength(1);

            int i, j;
            //input
            for (j = 0; j < input_neurons; j++)
            {
                double weight_sum = 0;
                for (i = 0; i < sensors.Count; i++)
                {
                    if (sensors[i]) weight_sum += sensors2inputs[i, j];
                }
                if (weight_sum >= 1) activated_inputs.Add(j);
            }

            //output
            for (j = 0; j < output_neurons; j++)
            {
                double weight_sum = 0;
                foreach (int h in activated_inputs)
                {
                    weight_sum += inputs2outputs[h, j];
                }
                if (weight_sum >= 1) activated_outputs.Add(j);
            }


            return activated_outputs;

        } 

        public static double[] GetWeights(double[,] sensors2inputs, double[,] inputs2outputs)
        {

            int sensors = sensors2inputs.GetLength(0);
            int input_neurons = sensors2inputs.GetLength(1);
            int output_neurons = inputs2outputs.GetLength(1);

            double[] genotype = new double[(sensors * input_neurons) + (input_neurons * output_neurons)];

            int i, j, g = 0;
            for (i = 0; i < sensors; i++)
            {
                for (j = 0; j < input_neurons; j++)
                {
                    genotype[g] = sensors2inputs[i, j];
                    g++;
                }
            }

            for (i = 0; i < input_neurons; i++)
            {
                for (j = 0; j < output_neurons; j++)
                {
                    genotype[g] = inputs2outputs[i, j];
                    g++;
                }
            }
            return genotype;

        }

        public static List<double[,]> Create_2LayerPerceptron(double[] weights, int sensors, int input_neurons, int output_neurons)
        {
            double[,] sensors2inputs = new double[sensors, input_neurons];
            double[,] inputs2outputs = new double[input_neurons, output_neurons];

            int i, j, w = 0;
            for (i = 0; i < sensors; i++)
            {
                for (j = 0; j < input_neurons; j++)
                {
                    sensors2inputs[i, j] = weights[w];
                    w++;
                }
            }

            for (i = 0; i < input_neurons; i++)
            {
                for (j = 0; j < output_neurons; j++)
                {
                    inputs2outputs[i, j] = weights[w];
                    w++;
                }
            }

            return new List<double[,]>() { sensors2inputs, inputs2outputs };
        }
    }
}
