using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Vertex<TVertex, TEdge>
    {
        public List<Edge<TVertex, TEdge>> Edges = new List<Edge<TVertex, TEdge>>();
        public TVertex Value { get; set; }

        public Vertex(TVertex value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return (Vertex<TVertex, TEdge>)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vertex<TVertex, TEdge> lhs, Vertex<TVertex, TEdge> rhs)
        {
            if (lhs is null || rhs is null)
            {
                return false;
            }

            if (lhs.Value.Equals(rhs.Value))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Vertex<TVertex, TEdge> lhs, Vertex<TVertex, TEdge> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
