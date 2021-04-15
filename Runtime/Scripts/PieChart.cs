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

        [SerializeField, Range(0, 1)] private float inlineThickness;
        [SerializeField] private Color32 inlineColor = Color.black;

        [SerializeField, Range(0, 1)] private float outlineThickness;
        [SerializeField] private Color32 outlineColor = Color.black;
        [SerializeField] private bool outlineMimicsInline;

        [SerializeField] private bool isAnimated = true;

        [SerializeField] private List<DataNode> data = new List<DataNode>();

        private bool _playAnimation;
        private float _playAnimationTimestamp;
        private float _fillAmount = 1f;

        protected override void Awake()
        {
            base.Awake();
            if (isAnimated) PlayAnimation();
        }

        private void Update()
        {
            if (!_playAnimation) return;

            _fillAmount += Mathf.Lerp(0f, 1f, Time.deltaTime);
            if (Time.time - _playAnimationTimestamp > 1f)
            {
                _playAnimation = false;
                _fillAmount = 1f;
            }

            SetVerticesDirty();
        }

        private void PlayAnimation()
        {
            if (Application.isPlaying)
            {
                _playAnimation = true;
                _playAnimationTimestamp = Time.time;
                _fillAmount = 0f;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (data.Count == 0) return;

            var width = rectTransform.rect.width;

            var distanceFromCenter = -(distanceFromCenterPercentage * (width / 2 - 5));

            var outer = -rectTransform.pivot.x * rectTransform.rect.width;
            var inner = -rectTransform.pivot.x * rectTransform.rect.width;

            vh.Clear();

            var prevX = Vector2.zero;
            var prevY = Vector2.zero;

            var f = _fillAmount;
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
                        pos0 * distanceFromCenterPercentage * ((width - 10) / width),
                        pos1 * distanceFromCenterPercentage * ((width - 10) / width),
                        pos2 * distanceFromCenterPercentage * ((width - 10) / width) * (1 - inlineThickness),
                        pos3 * distanceFromCenterPercentage * ((width - 10) / width) * (1 - inlineThickness)
                    }, new[]
                    {
                        uv0, uv1, uv2, uv3
                    }, inlineColor)
                );

                // Draw outline.
                if (outlineMimicsInline)
                {
                    vh.AddUIVertexQuad(GenerateVertexes(new[]
                        {
                            pos0,
                            pos1,
                            pos2 * (1 + inlineThickness * distanceFromCenterPercentage * outlineThickness),
                            pos3 * (1 + inlineThickness * distanceFromCenterPercentage * outlineThickness)
                        }, new[]
                        {
                            uv0, uv1, uv2, uv3
                        }, outlineColor)
                    );
                }
                else
                {
                    vh.AddUIVertexQuad(GenerateVertexes(new[]
                        {
                            pos0,
                            pos1,
                            pos2 * (1 + outlineThickness),
                            pos3 * (1 + outlineThickness)
                        }, new[]
                        {
                            uv0, uv1, uv2, uv3
                        }, outlineColor)
                    );
                }

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