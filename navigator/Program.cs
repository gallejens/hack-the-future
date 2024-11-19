using Shared;

namespace navigator
{
    internal class Program
    {
        private static readonly string START_WAYPOINT = "You";
        private static readonly string END_WAYPOINT = "S42";

        static async Task Main(string[] args)
        {
            var response = await APICalls.Get<RequestDTO>("/api/challenges/navigator?isTest=false");

            NavigationType navType = response.navigationType.Equals("ShortestPath") ? NavigationType.ShortestPath : NavigationType.LongestPath;
            var dijkstra = new DijkstraAlgorithm(response.pointData.ToList(), navType);
            var result = dijkstra.FindPath(START_WAYPOINT, END_WAYPOINT);

            await APICalls.Post("/api/challenges/navigator", new ResponseDTO { answer = result.Path.ToArray() });
        }
    }
}
