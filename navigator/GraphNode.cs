namespace navigator
{
    public class GraphNode
    {
        public string label { get; set; }
        public Dictionary<string, int> connections { get; set; }
    }
}
