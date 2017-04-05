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
            foreach (Cell c in map.HollowCells) {
                if (c != null) {
                    Node<Cell> n = new Node<Entities.Cell>();
                    n.Data = c;
                    Nodes.Add(c, n);
                }
            }

            for (int i = 0; i < Nodes.Count; i++) {
                GenerateEdgesByCell(Nodes.ElementAt(i).Key);
            }
        }

        private Node<Cell> getNodeFor(Cell cell) {
            if (!Nodes.ContainsKey(cell)) {
                Node<Cell> n = new Node<Cell>();
                n.Data = cell;
                Nodes.Add(cell, n);
            }
            return Nodes[cell];
        }

        private void GenerateEdgesByCell(Cell cell) {
            if (cell == null) { return; }

            Node<Cell> node = getNodeFor(cell);
            List<Edge<Cell>> edges = new List<Edge<Cell>>();

            Cell[] neighbours = cell.Neighbours(diagonalOk: true);

            for (int i = 0; i < neighbours.Length; i++) {
                if (neighbours[i] != null && neighbours[i].PathfindingCost > 0 
                    && !cell.IsClippingCorner(neighbours[i])) {
                    Edge<Cell> edge = new Edge<Cell>();
                    edge.Cost = neighbours[i].PathfindingCost;
                    edge.Node = getNodeFor(neighbours[i]);

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
