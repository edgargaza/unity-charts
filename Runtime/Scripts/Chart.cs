using UnityEngine;
using UnityEngine.UI;

namespace UnityCharts
{
    public class Chart : MaskableGraphic
    {
        protected static UIVertex[] GenerateVertexes(Vector2[] vertices, Vector2[] uvs, Color32 tint)
        {
            var vbo = new UIVertex[4];
            for (var i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = tint;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }

            return vbo;
        }
    }
}