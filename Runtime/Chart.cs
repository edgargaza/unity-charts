using UnityEngine;
using UnityEngine.UI;

namespace UnityCharts
{
    public class Chart : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        public virtual void CalculateLayoutInputHorizontal()
        {
        }

        public virtual void CalculateLayoutInputVertical()
        {
        }

        public virtual float minWidth => 0;
        public virtual float preferredWidth => 0;

        public virtual float flexibleWidth => -1;
        public virtual float minHeight => 0;

        public virtual float preferredHeight => 0;
        public virtual float flexibleHeight => -1;

        public virtual int layoutPriority => 0;

        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) => true;

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