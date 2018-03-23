using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Edge<TVertex, TEdge>
    {
        public Vertex<TVertex, TEdge> firstVertex { get; set; }
        public Vertex<TVertex, TEdge> secondVertex { get; set; }
        public double Weight { get; set; } = 0;

        public TEdge EdgeType { get; set; }

        public Edge(Vertex<TVertex, TEdge> firstVertex, Vertex<TVertex, TEdge> secondVertex, TEdge edgeType, double weight = 0)
        {
            this.firstVertex = firstVertex;
            this.secondVertex = secondVertex;
            Weight = weight;
            EdgeType = edgeType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (Edge<TVertex, TEdge>)obj == this;
        }

        public static bool operator ==(Edge<TVertex, TEdge> lhs, Edge<TVertex, TEdge> rhs)
        {
            if(lhs.firstVertex == rhs.firstVertex && lhs.secondVertex == rhs.secondVertex && lhs.Weight == rhs.Weight)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Edge<TVertex, TEdge> lhs, Edge<TVertex, TEdge> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
