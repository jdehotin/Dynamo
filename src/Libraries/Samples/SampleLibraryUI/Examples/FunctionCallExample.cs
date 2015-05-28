using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;
using SampleLibraryZeroTouch;

namespace SamplesLibraryUI.Examples
{
    [NodeName("Optimo_GA")]
    [NodeCategory("OptimoTest")]
    [NodeDescription("Single and Muli-objective Optimization using Genetic Algorithm (NSGA_II)", typeof(string))]
    [IsDesignScriptCompatible]
    [NodeSearchTags(new[] { "optimo", "optimization", "ga", "genetic", "algorithm" })]
    public class OptimoNSGA : NodeModel
    {
        private double popSize = 10;
        private double mutation = 0.01;
        private double crossover = 0.9;

        public double PopSize
        {
            get { return popSize; }
            set
            {
                popSize = value;
                RaisePropertyChanged("PopSize");

                OnNodeModified();
            }
        }

        public double Mutation
        {
            get { return mutation; }
            set
            {
                mutation = value;
                RaisePropertyChanged("Mutation");

                OnNodeModified();
            }
        }

        public double Crossover
        {
            get { return crossover; }
            set
            {
                crossover = value;
                RaisePropertyChanged("Crossover");

                OnNodeModified();
            }
        }

        [IsVisibleInDynamoLibrary(false)]
        public OptimoNSGA()
        {
            InPortData.Add(new PortData("Num Objectives", "Number of Objectives"));
            InPortData.Add(new PortData("LL", "Lower Limits"));
            InPortData.Add(new PortData("UL", "Upper Limits"));
            InPortData.Add(new PortData("Iteration", "Iteration Number"));
            InPortData.Add(new PortData("fitness func", "Fitness functions list"));

            OutPortData.Add(new PortData("Initial Pop", "Optimal solution results and variables"));
            OutPortData.Add(new PortData("Initial Func", "Optimal solution results and variables"));
            OutPortData.Add(new PortData("Initial SolutionList", "Optimal solution results and variables"));

            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Disabled;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            //Creates a list of list of doubles, in a container class (List<List<double>> inputs to functions break Dynamo)
            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, List<double>, List<double>, double, double, DummyContainer>(DummyContainer.InitialSolutionList),
                    new List<AssociativeNode>() 
                    { 
                        AstFactory.BuildDoubleNode(PopSize),
                        inputAstNodes[0],
                        inputAstNodes[1],
                        inputAstNodes[2],
                        AstFactory.BuildDoubleNode(Mutation),
                        AstFactory.BuildDoubleNode(Crossover)
                    });

            //Changing the Container to a List<List<double>>
            var functionCall2 =
                 AstFactory.BuildFunctionCall(
                     new Func<DummyContainer, List<List<double>>>(DummyContainer.CreateListofLists),
                     new List<AssociativeNode>
                    { 
                        functionCall
                    });

            //Applying functions to the initial list
            var externalFunction =
                AstFactory.BuildFunctionCall(
                    "__ApplyList",
                    new List<AssociativeNode>
                    {
                        inputAstNodes[4],
                        functionCall2
                     });

            //Combining the two lists (from externalFunction and functionCall)
            var functionCall3 =
                 AstFactory.BuildFunctionCall(
                     new Func<DummyContainer, List<List<double>>, DummyContainer>(DummyContainer.AssignFitnessFunctionResults),
                     new List<AssociativeNode>
                    { 
                        functionCall, externalFunction
                    });

            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0), functionCall),

                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(1), functionCall2),

                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(2), functionCall3)
            };
        }
    }
}
