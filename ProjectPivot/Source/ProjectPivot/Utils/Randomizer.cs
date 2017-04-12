using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPivot.Utils {
    public static class Randomizer {
        public static Random Random = new Random(Guid.NewGuid().GetHashCode());
    }
}
