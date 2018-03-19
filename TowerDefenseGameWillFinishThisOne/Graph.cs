using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Graph<T>
    {
        private List<Vertex<T>> Vertices = new List<Vertex<T>>();
        private List<Edge<T>> Edges = new List<Edge<T>>();

        public void AddEdge(Vertex<T> firstVertex, Vertex<T> secondVertex, int weight)
        {
            if (!DoesHaveVertex(firstVertex) || !DoesHaveVertex(secondVertex))
            {
                throw new InvalidOperationException("Vertex doesn't exist inside the Verticies list");
            }

            Edge<T> edge = new Edge<T>(firstVertex, secondVertex, weight);
            Edge<T> edge1 = new Edge<T>(secondVertex, firstVertex, weight);

            if (DoesHaveEdge(edge) || DoesHaveEdge(edge1))
            {
                throw new InvalidOperationException("Edge already exists inside of the Edges list");
            }

            firstVertex.Edges.Add(edge);
            secondVertex.Edges.Add(edge1);

            Edges.Add(edge);
            Edges.Add(edge1);
        }

        public void AddVertex(Vertex<T> vertex)
        {
            if (DoesHaveVertex(vertex))
            {
                //Vertex already exists inside of the Verticies list
                throw new InvalidOperationException("Vertex already exists inside the Verticies list");
            }
            Vertices.Add(vertex);
        }

        public bool DoesHaveVertex(Vertex<T> vertex)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Value.Equals(vertex.Value))
                {
                    return true;
                }
            }
            return false;
        }
        public bool DoesHaveEdge(Edge<T> edge)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                if (Edges[i].firstVertex == edge.firstVertex && Edges[i].secondVertex == edge.secondVertex && Edges[i].Weight == edge.Weight)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddVertex(T value)
        {
            Vertices.Add(new Vertex<T>(value));
        }
    }

    public class TileMap : Graph<Tile>
    {
        
    }
}
