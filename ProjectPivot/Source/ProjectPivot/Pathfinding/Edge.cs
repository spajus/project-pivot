using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Pathfinding {
    public class Edge<T> {
        public CellGraph Cell;
        public float Cost;
        public Node<T> Node;
    }
}
