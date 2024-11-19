namespace navigator
{
    public class DijkstraAlgorithm
    {
        private readonly Dictionary<string, Dictionary<string, int>> graph;
        private readonly Dictionary<string, int> distances = [];
        private readonly Dictionary<string, string> predecessors = [];
        private readonly HashSet<string> visited = new();
        private readonly PriorityQueue<string> priorityQueue = new();

        public DijkstraAlgorithm(List<GraphNode> graphData, NavigationType navigationType)
        {
            graph = graphData.ToDictionary(
                node => node.label,
                node => node.connections.ToDictionary(
                    conn => conn.Key,
                    conn => navigationType == NavigationType.ShortestPath ? conn.Value : -conn.Value
                )
            );
        }

        public List<string> FindPath(string start, string end)
        {
            foreach (var node in graph.Keys)
            {
                distances[node] = int.MaxValue;
            }
            distances[start] = 0;

            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                string current = priorityQueue.Dequeue();

                if (current == end)
                {
                    break;
                }

                if (visited.Contains(current))
                {
                    continue;
                }

                visited.Add(current);

                foreach (var connection in graph[current])
                {
                    string label = connection.Key;
                    int weight = connection.Value;

                    if (visited.Contains(label))
                        continue;

                    int newDistance = distances[current] + weight;

                    if (newDistance < distances[label])
                    {
                        distances[label] = newDistance;
                        predecessors[label] = current;
                        priorityQueue.Enqueue(label, newDistance);
                    }
                }
            }

            List<string> path = new List<string>();
            string currentNode = end;

            while (currentNode != null)
            {
                path.Add(currentNode);
                predecessors.TryGetValue(currentNode, out currentNode);
            }
            path.Reverse();

            //int actualDistance = 0;
            //for (int i = 0; i < path.Count - 1; i++)
            //{
            //    var current = path[i];
            //    var next = path[i + 1];
            //    actualDistance += Math.Abs(graph[current][next]);
            //}

            return path;
        }
    }

}
