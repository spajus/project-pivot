using ProjectPivot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Pathfinding {
    public class CellGraph {
        public Dictionary<Cell, Node<Cell>> Nodes;
        public static CellGraph Current;

        public CellGraph(Map map) {
            Nodes = new Dictionary<Cell, Node<Cell>>();
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    Cell c = map.CellAt(x, y);

                    Node<Cell> n = new Node<Entities.Cell>();
                    n.Data = c;
                    Nodes.Add(c, n);
                }
            }

            foreach (Cell c in Nodes.Keys) {
                GenerateEdgesByCell(c);
            }
        }

        private void GenerateEdgesByCell(Cell cell) {
            if (cell == null) { return; }

            Node<Cell> node = Nodes[cell];
            List<Edge<Cell>> edges = new List<Edge<Cell>>();

            Cell[] neighbours = cell.Neighbours(diagonalOk: true);

            for (int i = 0; i < neighbours.Length; i++) {
                if (neighbours[i] != null && neighbours[i].PathfindingCost > 0 
                    && !cell.IsClippingCorner(neighbours[i])) {
                    Edge<Cell> edge = new Edge<Cell>();
                    edge.Cost = neighbours[i].PathfindingCost;
                    edge.Node = Nodes[neighbours[i]];

                    edges.Add(edge);
                }
            }

            node.Edges = edges.ToArray();

        }

        public void RegenerateGraphAtCell(Cell changedCell) {
            if (changedCell == null) { return; }
            GenerateEdgesByCell(changedCell);
            foreach (Cell cell in changedCell.Neighbours(true)) {
                GenerateEdgesByCell(cell);
            }
        }
    }
}
