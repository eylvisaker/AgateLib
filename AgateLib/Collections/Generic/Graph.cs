using AgateLib.Algorithms.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Collections.Generic
{
    public interface IGraph<TNode, TEdge> where TNode : IGraphNode where TEdge : IGraphEdge
    {
        IReadOnlyDictionary<int, TNode> Nodes { get; }
        IReadOnlyDictionary<int, TEdge> Edges { get; }
    }

    public interface IGraphNode
    {
        int Id { get; }
    }

    public interface IGraphEdge
    {
        int Id { get; }

        int Node1 { get; }
        int Node2 { get; }
    }

    public static class GraphExtensions
    {
        /// <summary>
        /// Uses the A* algorithm to find a path between any two
        /// nodes in a connected graph. Returns null if no path exists.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stepCost"></param>
        /// <param name="heuristic"></param>
        /// <returns></returns>
        public static List<TNode> FindPath<TNode, TEdge>(
                        this IGraph<TNode, TEdge> graph,
                        TNode start,
                        IEnumerable<TNode> end,
                        Func<TNode, TEdge, int> stepCost,
                        Func<TNode, TNode, int> heuristic,
                        double heuristicWeight = 0)
            where TNode : IGraphNode where TEdge : IGraphEdge
        {
            var astar = new AStar<TNode>(new GraphAStarMap<TNode, TEdge>(
                graph,
                stepCost,
                heuristic));

            astar.Start = start;
            astar.EndPoints.AddRange(end);
            astar.HeuristicWeight = heuristicWeight;

            astar.FindPath();

            if (astar.FoundPath)
            {
                return astar.Path;
            }

            return null;
        }

        public static IEnumerable<TEdge> EdgesFrom<TNode, TEdge>(
                        this IGraph<TNode, TEdge> graph,
                        IGraphNode start)
            where TNode : IGraphNode where TEdge : IGraphEdge
            => graph.Edges.Values.Where(x => x.Connects(start.Id));

        /// <summary>
        /// Searches the graph for an edge that connects the two
        /// nodes and returns it. Returns null if no edge connects
        /// the two nodes.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="graph"></param>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public static TEdge FindEdge<TNode, TEdge>(
                        this IGraph<TNode, TEdge> graph,
                        IGraphNode nodeA,
                        IGraphNode nodeB)
            where TNode : IGraphNode where TEdge : IGraphEdge
        {
            return graph.Edges.Values.FirstOrDefault(x =>
                x.Connects(nodeA, nodeB));
        }
    }

    public static class GraphEdgeExtensions
    {
        /// <summary>
        /// Returns true if node1 or node2 are nodeA.
        /// </summary>
        /// <param name="nodeA"></param>
        /// <returns></returns>
        public static bool Connects(this IGraphEdge edge, int nodeA)
        {
            if (edge.Node1 == nodeA) return true;
            if (edge.Node2 == nodeA) return true;

            return false;
        }

        /// <summary>
        /// Returns true if node1 and node2 are nodeA and nodeB. This method is symmetric,
        /// meaning that Connects(x,y) == Connects(y,x).
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public static bool Connects(this IGraphEdge edge, int nodeA, int nodeB)
        {
            if (edge.Node1 == nodeA && edge.Node2 == nodeB) return true;
            if (edge.Node1 == nodeB && edge.Node2 == nodeA) return true;

            return false;
        }

        /// <summary>
        /// Returns true if node1 and node2 are nodeA and nodeB. This method is symmetric,
        /// meaning that Connects(x,y) == Connects(y,x).
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public static bool Connects(this IGraphEdge edge, IGraphNode nodeA, IGraphNode nodeB)
            => Connects(edge, nodeA.Id, nodeB.Id);

        /// <summary>
        /// If the passed id is one of the nodes for this path, this method returns
        /// the other node id. Otherwise it throws an ArgumentException.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static int OtherNodeTo(this IGraphEdge edge, int nodeId)
        {
            if (nodeId == edge.Node1) return edge.Node2;
            if (nodeId == edge.Node2) return edge.Node1;

            throw new ArgumentException($"Node {nodeId} does not belong to this edge.");
        }
    }

    public class GraphAStarMap<TNode, TEdge>
        : IAStarMap<TNode>
        where TNode : IGraphNode where TEdge : IGraphEdge
    {
        private readonly IGraph<TNode, TEdge> graph;
        private readonly Func<TNode, TEdge, int> stepCost;
        private readonly Func<TNode, TNode, int> heuristic;

        public GraphAStarMap(IGraph<TNode, TEdge> graph,
                             Func<TNode, TEdge, int> stepCost,
                             Func<TNode, TNode, int> heuristic)
        {
            this.graph = graph;
            this.stepCost = stepCost;
            this.heuristic = heuristic;
        }

        public IEnumerable<TNode> AvailableStepsAt(AStarNode<TNode> node)
        {
            return graph.EdgesFrom(node.Location)
                .Select(x => x.OtherNodeTo(node.Location.Id))
                .Select(x => graph.Nodes[x]);
        }

        public int CalcHeuristic(AStarNode<TNode> node, TNode destination)
        {
            return heuristic(node.Location, destination);
        }

        public int StepCost(AStarNode<TNode> node, TNode target)
        {
            var edge = graph.FindEdge(node.Location, target);

            return stepCost(node.Location, edge);
        }
    }
}
