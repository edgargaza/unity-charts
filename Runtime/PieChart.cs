using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCharts
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasRenderer))]
    public class PieChart : Chart
    {
        private const int Segments = 720;

        [SerializeField, Range(0, 360)] private float angleDegrees = 360f;
        [SerializeField, Range(0, 1)] private float distanceFromCenterPercentage;

        [SerializeField, Min(0)] private float outlineThickness;
        [SerializeField] private Color32 outlineColor = Color.black;

        [SerializeField] private float inlineThickness;
        [SerializeField] private Color32 inlineColor = Color.black;

        [SerializeField] private bool isAnimated = true;

        [SerializeField] private List<DataNode> data = new List<DataNode>();

        private bool playAnimation;
        private float playAnimationTimestamp;
        private float fillAmount = 1f;

        protected override void Awake()
        {
            base.Awake();
            if (isAnimated) PlayAnimation();
        }

        private void Update()
        {
            if (!playAnimation) return;

            fillAmount += Mathf.Lerp(0f, 1f, Time.deltaTime);
            if (Time.time - playAnimationTimestamp > 1f)
            {
                playAnimation = false;
                fillAmount = 1f;
            }

            SetVerticesDirty();
        }

        private void PlayAnimation()
        {
            if (Application.isPlaying)
            {
                playAnimation = true;
                playAnimationTimestamp = Time.time;
                fillAmount = 0f;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var width = rectTransform.rect.width;

            var distanceFromCenter = -(distanceFromCenterPercentage * (width / 2 - 5f));

            var outer = -rectTransform.pivot.x * rectTransform.rect.width;
            var inner = -rectTransform.pivot.x * rectTransform.rect.width;

            vh.Clear();

            var prevX = Vector2.zero;
            var prevY = Vector2.zero;

            var f = fillAmount;
            var degrees = -angleDegrees / Segments;
            var fa = (int) ((Segments + 1) * f);

            var dataIndex = 0;
            var total = 0f;
            var currentValue = data[0].value;

            data.ForEach(s => total += s.value);

            var fillColor = data[0].tint;

            for (var i = 0; i < fa; i++)
            {
                var rad = Mathf.Deg2Rad * (i * degrees);
                var c = Mathf.Cos(rad);
                var s = Mathf.Sin(rad);

                var uv0 = new Vector2(0, 1);
                var uv1 = new Vector2(1, 1);
                var uv2 = new Vector2(1, 0);
                var uv3 = new Vector2(0, 0);

                var pos0 = prevX;
                var pos1 = new Vector2(outer * c, outer * s);

                var pos2 = new Vector2(inner * c, inner * s);
                var pos3 = prevY;

                if (i > currentValue / total * Segments)
                {
                    if (dataIndex < data.Count - 1)
                    {
                        dataIndex += 1;
                        currentValue += data[dataIndex].value;
                        fillColor = data[dataIndex % data.Count].tint;
                    }
                }

                // Draw fill.
                vh.AddUIVertexQuad(GenerateVertexes(new[]
                    {
                        pos0, pos1, pos2 * distanceFromCenter / inner,
                        pos3 * distanceFromCenter / inner
                    },
                    new[]
                    {
                        uv0, uv1, uv2, uv3
                    }, fillColor)
                );

                // Draw inline.
                vh.AddUIVertexQuad(GenerateVertexes(new[]
                    {
                        pos0 * (distanceFromCenterPercentage - distanceFromCenterPercentage / 10), 
                        pos1 * (distanceFromCenterPercentage - distanceFromCenterPercentage / 10), 
                        pos2 * (distanceFromCenterPercentage - distanceFromCenterPercentage / 10) * inlineThickness, 
                        pos3 * (distanceFromCenterPercentage - distanceFromCenterPercentage / 10) * inlineThickness
                    }, new[]
                    {
                        uv0, uv1, uv2, uv3
                    }, inlineColor)
                );

                // Draw outline.
                vh.AddUIVertexQuad(GenerateVertexes(new[]
                    {
                        pos0, pos1, pos2 * (1 + outlineThickness / 50), pos3 * (1 + outlineThickness / 50)
                    }, new[]
                    {
                        uv0, uv1, uv2, uv3
                    }, outlineColor)
                );

                /*
                // Draw outer circle.
                vh.AddUIVertexQuad(SetVbo(new[] {pos0, pos1, pos2, pos3}, new[] {uv0, uv1, uv2, uv3}, borderColor));

                // Draw inner circle.
                vh.AddUIVertexQuad(SetVbo(
                    new[] {pos0 * outer1 / outer, pos1 * outer1 / outer, pos2 * inner1 / inner, pos3 * inner1 / inner},
                    new[] {uv0, uv1, uv2, uv3}, borderColor));
                */

                prevX = pos1;
                prevY = pos2;
            }
        }

        [Serializable]
        public sealed class DataNode
        {
            public string text;
            public float value;
            public Color tint = Color.white;
        }
    }
}