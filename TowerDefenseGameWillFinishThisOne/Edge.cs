using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Edge<T>
    {
        public Vertex<T> firstVertex { get; set; }
        public Vertex<T> secondVertex { get; set; }
        public double Weight { get; set; }

        public Edge(Vertex<T> firstVertex, Vertex<T> secondVertex, double weight)
        {
            this.firstVertex = firstVertex;
            this.secondVertex = secondVertex;
            Weight = weight;
        }
    }
}
