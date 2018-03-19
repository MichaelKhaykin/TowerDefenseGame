using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Vertex<T>
    {
        public List<Edge<T>> Edges = new List<Edge<T>>();
        public T Value { get; set; }

        public Vertex(T value)
        {
            Value = value;
        }
    }
}
