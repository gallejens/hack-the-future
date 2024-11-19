namespace navigator
{
    public class DijkstraAlgorithm
    {
        private readonly Dictionary<string, Dictionary<string, int>> graph;
        private readonly Dictionary<string, int> distances;
        private readonly Dictionary<string, string> predecessors;
        private readonly HashSet<string> visited;
        private readonly PriorityQueue<string> priorityQueue;

        public DijkstraAlgorithm(List<GraphNode> graphData, NavigationType navigationType)
        {
            graph = graphData.ToDictionary(
                node => node.label,
                node => node.connections.ToDictionary(
                    conn => conn.Key,
                    conn => navigationType == NavigationType.ShortestPath ? conn.Value : -conn.Value
                )
            );

            // Initialize data structures
            distances = new Dictionary<string, int>();
            predecessors = new Dictionary<string, string>();
            visited = new HashSet<string>();
            priorityQueue = new PriorityQueue<string>();
        }

        public DijkstraResult FindPath(string start, string end)
        {
            // Initialize distances
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
                    break;

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                foreach (var neighbor in graph[current])
                {
                    string nextNode = neighbor.Key;
                    int weight = neighbor.Value;

                    if (visited.Contains(nextNode))
                        continue;

                    int newDistance = distances[current] + weight;

                    if (newDistance < distances[nextNode])
                    {
                        distances[nextNode] = newDistance;
                        predecessors[nextNode] = current;
                        priorityQueue.Enqueue(nextNode, newDistance);
                    }
                }
            }

            // Reconstruct path
            List<string> path = new List<string>();
            string currentNode = end;

            while (currentNode != null)
            {
                path.Add(currentNode);
                predecessors.TryGetValue(currentNode, out currentNode);
            }
            path.Reverse();

            // Calculate actual distance (removing negation for longest path)
            int actualDistance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var current = path[i];
                var next = path[i + 1];
                actualDistance += Math.Abs(graph[current][next]);
            }

            return new DijkstraResult
            {
                Path = path,
                Distance = actualDistance
            };
        }
    }

}
