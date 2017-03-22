using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Pathfinding {
    public class Node<T> {
        public T Data;
        public Edge<T>[] Edges;
    }
}
