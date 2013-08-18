using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class InputNode
    {
        public int Value { get; set; }
        public int ParentValue { get; set; }
    }

    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Parent { get; set; }
    }

    public class Query
    {
        public string Type { get; set; }
        public int FirstNum { get; set; }
        public int SecondNum { get; set; }
    }

    public class Program2
    {
        private static string GetNextLine()
        {
            return Console.ReadLine();
        }

        private static Dictionary<int, TreeNode> BuildTree(IEnumerable<InputNode> inputNodes)
        {
            var treeByNode = new Dictionary<int, TreeNode>();
            var parents = new Dictionary<int, int>();

            // set up tree nodes
            foreach (var node in inputNodes)
            {
                treeByNode[node.Value] = new TreeNode {Value = node.Value};
                parents[node.Value] = node.ParentValue;
            }

            // set parent for all non-root nodes
            foreach (var node in treeByNode.Values.Where(x => parents[x.Value] > 0))
            {
                node.Parent = treeByNode[parents[node.Value]];
            }

            return treeByNode;
        }

        private static int RunQuery(int nodeStart, int numParents, Dictionary<int, TreeNode> treeByNode)
        {
            if (!treeByNode.ContainsKey(nodeStart))
            {
                return 0; // node doesn't exist
            }

            var node = treeByNode[nodeStart];

            for (var i = 0; i < numParents; i++)
            {
                node = node.Parent;

                if (node == null)
                {
                    return 0; // kth parent doesnt exist
                }
            }

            return node.Value;
        }
    
        private static void RunQuery(Query query, Dictionary<int, TreeNode> treeByNode, int outputId)
        {
            if (query.Type == "0")
            {
                treeByNode[query.SecondNum] = new TreeNode
                {
                    Value = query.SecondNum,
                    Parent = treeByNode[query.FirstNum],
                };
            }
            else if (query.Type == "1")
            {
                treeByNode.Remove(query.FirstNum);
            }
            else if (query.Type == "2")
            {
                var output = RunQuery(query.FirstNum, query.SecondNum, treeByNode);
                if (!Outputs.ContainsKey(outputId))
                {
                    Outputs[outputId] = output + "\n";
                }
                else
                {
                    Outputs[outputId] += output + "\n";
                }
            }
        }

        private static readonly Dictionary<int, string> Outputs = new Dictionary<int, string>();

        public static void Main(string[] args)
        {
            var numTestCases = int.Parse(GetNextLine());

            var toDo = new List<Tuple<List<InputNode>, List<Query>>>();

            for (var i = 0; i < numTestCases; i++)
            {
                var numNodes = int.Parse(GetNextLine());

                var inputNodes = new List<InputNode>(numNodes);

                for (var j = 0; j < numNodes; j++)
                {
                    var nodeParts = GetNextLine().Split(' ');

                    inputNodes.Add(new InputNode
                    {
                        Value = int.Parse(nodeParts[0]),
                        ParentValue = int.Parse(nodeParts[1]),
                    });
                }

                var numQueries = int.Parse(GetNextLine());

                var queries = new List<Query>();

                for (var j = 0; j < numQueries; j++)
                {
                    var queryLine = GetNextLine().Split(' ');

                    var query = new Query
                    {
                        Type = queryLine[0],
                        FirstNum = int.Parse(queryLine[1]),
                        SecondNum = queryLine.Length > 2 ? int.Parse(queryLine[2]) : 0,
                    };

                    queries.Add(query);
                }

                toDo.Add(Tuple.Create(inputNodes, queries));
            }
            
            var tasks = new Dictionary<int, Task>();

            for (var i = 0; i < numTestCases; i++)
            {
                var i1 = i;
                tasks[i1] = Task.Factory.StartNew(() => {
                    var treeByNode = BuildTree(toDo[i1].Item1);
                    foreach (var query in toDo[i1].Item2)
                    {
                        RunQuery(query, treeByNode, i1);
                    }
                });
            }

            foreach (var task in tasks.Values)
            {
                task.Wait();
            }

            foreach (var output in Outputs.OrderBy(x => x.Key).Select(x => x.Value))
            {
                Console.Write(output);
            }
        }
    }
}