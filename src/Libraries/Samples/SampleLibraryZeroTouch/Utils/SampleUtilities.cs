using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;

namespace SampleLibraryZeroTouch
{
    /// <summary>
    /// A utility library containing methods that can be called 
    /// from NodeModel nodes, or used as nodes in Dynamo.
    /// </summary>
    public static class SampleUtilities
    {
        [IsVisibleInDynamoLibrary(false)]
        public static double MultiplyInputByNumber(double input)
        {
            return input * 42;
        }
    }

    public class DummyContainer
    {
        [IsVisibleInDynamoLibrary(false)]
        public static DummyContainer InitialSolutionList(double a, double b, List<double> c, List<double> d, double e, double f)
        {
            return new DummyContainer();
        }

        public DummyContainer()
        {
            
        }

        [IsVisibleInDynamoLibrary(false)]
        public static List<List<double>> CreateListofLists(DummyContainer container)
        {
            return Enumerable.Repeat(Enumerable.Repeat(42.0, 5).ToList(), 5).ToList();
        }

        [IsVisibleInDynamoLibrary(false)]
        public static DummyContainer AssignFitnessFunctionResults(DummyContainer container, List<List<double>> list)
        {
            return new DummyContainer();
        }
    }
}
