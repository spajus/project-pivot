using Microsoft.Xna.Framework;
using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Pathfinding {
    public class AStar {
        private Queue<Cell> path;

        public AStar(Queue<Cell> path) {
            if (path == null || !path.Any()) {
                Console.WriteLine("Created path with no cells");
            }
            this.path = path;
        }

        public AStar(Map map, Cell cellStart, Cell goal) {
            path = new Queue<Cell>();
            if (CellGraph.Current == null) {
                CellGraph.Current = new CellGraph(map);
            }
            Dictionary<Cell, Node<Cell>> nodes = CellGraph.Current.Nodes;
            if (nodes.ContainsKey(cellStart) == false) {
                Console.WriteLine("Starting cell is not in AStar CellGraph node list!");
                return;
            }
            if (nodes.ContainsKey(goal) == false) {
                Console.WriteLine("Goal cell is not in AStar CellGraph node list!");
                return;
            }

            Node<Cell> start = nodes[cellStart];
            Node<Cell> finish = nodes[goal];

            HashSet<Node<Cell>> closedSet = new HashSet<Node<Cell>>();
            PathfindingPriorityQueue<Node<Cell>> openSet = new PathfindingPriorityQueue<Node<Cell>>();

            Dictionary<Node<Cell>, Node<Cell>> cameFrom = new Dictionary<Node<Cell>, Node<Cell>>();
            Dictionary<Node<Cell>, float> gScore = new Dictionary<Node<Cell>, float>();
            gScore[start] = 0;
            Dictionary<Node<Cell>, float> fScore = new Dictionary<Node<Cell>, float>();
            fScore[start] = heuristicCostEstimate(start, finish);

            while (openSet.Count > 0) {
                Node<Cell> current = openSet.Dequeue();
                if (current.Data == goal) {
                    reconstructPath(cameFrom, current);
                    return;
                }

                closedSet.Add(current);

                foreach (Edge<Cell> edgeNeighbour in current.Edges) {
                    Node<Cell> neighbour = edgeNeighbour.Node;
                    if (closedSet.Contains(neighbour)) {
                        continue; //already completed
                    }
                    float pathfindingCostToNeighbour = neighbour.Data.PathfindingCost * distanceBetween(current, neighbour);
                    float tentativeGScore = gScore[current] + pathfindingCostToNeighbour;

                    if (openSet.Contains(neighbour) && tentativeGScore >= gScore[neighbour]) {
                        continue;
                    }

                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = gScore[neighbour] + heuristicCostEstimate(neighbour, finish);
                    openSet.EnqueueOrUpdate(neighbour, fScore[neighbour]);
                }
            }

            Console.WriteLine("Burned out through patfhinding without result");
        }

        public Cell Dequeue() {
            if (path == null) {
                Console.WriteLine("Trying to dequeue AStar from null queue");
                return null;
            }
            if (path.Count <= 0) {
                Console.WriteLine("Trying to dequeue AStar from empty queue");
                return null;
            }
            return path.Dequeue();
        }

        public int Length() {
            if (path == null) {
                return 0;
            }
            return path.Count;
        }

        public Cell EndCell() {
            if (path == null || path.Count == 0) {
                Console.WriteLine("Trying to get end cell from no path in AStar");
                return null;
            }
            return path.Last();
        }

        public List<Cell> List() {
            return path.ToList();
        }

        private void reconstructPath(Dictionary<Node<Cell>, Node<Cell>> cameFrom, Node<Cell> current) {
            // current is goal right now, we will walk backwards through cameFrom
            // end of it is starting node
            Queue<Cell> totalPath = new Queue<Cell>();
            totalPath.Enqueue(current.Data);
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                totalPath.Enqueue(current.Data);
            }

            path = new Queue<Cell>(totalPath.Reverse());
        }

        private float heuristicCostEstimate(Node<Cell> a, Node<Cell> b) {
            if (b == null) {
                return 0f;
            }
            return distanceBetween(a, b);
        }

        private float distanceBetween(Node<Cell> a, Node<Cell> b) {
            // horizontal / vertical neighbours, distance = 1
            if (Math.Abs(a.Data.MapX - b.Data.X) + Math.Abs(a.Data.Y - b.Data.Y) == 1) {
                return 1f;
            }
            // diagonal neighbours, precalculated value;
            if (Math.Abs(a.Data.MapX - b.Data.X) == 1 && Math.Abs(a.Data.Y - b.Data.Y) == 1) {
                return 1.41421356237f;
            }
            // not neighbours, let's calculate expensively
            return (float) Math.Sqrt(Math.Pow(a.Data.MapX - b.Data.MapX, 2) +
                             Math.Pow(a.Data.MapY - b.Data.MapY, 2));
        }
    }
}
