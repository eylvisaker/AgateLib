using AgateLib.Display;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public class InlineElementStyle : IRenderElementStyleProperties
    {
        private int? fontSize;

        public static bool Equals(InlineElementStyle a, InlineElementStyle b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.FontFace != b.FontFace) return false;
            if (a.TextColor != b.TextColor) return false;
            if (a.FontSize != b.FontSize) return false;
            if (a.FontStyle != b.FontStyle) return false;
            if (a.TextAlign != b.TextAlign) return false;
            if (a.Overflow != b.Overflow) return false;
            if (!BackgroundStyle.Equals(a.Background, b.Background)) return false;
            if (!BorderStyle.Equals(a.Border, b.Border)) return false;
            if (!AnimationStyle.Equals(a.Animation, b.Animation)) return false;
            if (!FlexStyle.Equals(a.Flex, b.Flex)) return false;
            if (!FlexItemStyle.Equals(a.FlexItem, b.FlexItem)) return false;
            if (!Nullable.Equals(a.Padding, b.Padding)) return false;
            if (!Nullable.Equals(a.Margin, b.Margin)) return false;
            if (!LayoutStyle.Equals(a.Layout, b.Layout)) return false;
            if (!SizeConstraints.Equals(a.Size, b.Size)) return false;

            return true;
        }

        public float Scaling { get; set; } = 1;

        public string FontFace { get; set; }

        public Color? TextColor { get; set; }

        public int? FontSize
        {
            get
            {
                int? result = fontSize;

                if (result != null)
                {
                    result = (int)(result.Value * Scaling);
                }

                return result;
            }
            set => fontSize = value;
        }

        public FontStyles? FontStyle { get; set; }

        public BackgroundStyle Background { get; set; }

        public BorderStyle Border { get; set; }

        public AnimationStyle Animation { get; set; }

        public FlexStyle Flex { get; set; }

        public FlexItemStyle FlexItem { get; set; }

        public LayoutStyle Layout { get; set; }

        public SizeConstraints Size { get; set; }
        ISizeConstraints IRenderElementStyleProperties.Size => Size;

        public LayoutBox? Padding { get; set; }

        public LayoutBox? Margin { get; set; }

        public TextAlign? TextAlign { get; set; }

        public Overflow? Overflow { get; set; }

        public int Specificity { get; set; }

        IReadOnlyCollection<string> IRenderElementStyleProperties.PseudoClasses => null;

        public override bool Equals(object obj)
        {
            if (!(obj is InlineElementStyle other))
                return false;

            return Equals(this, other);
        }

        public override string ToString() => $"Inline Style: {BuildStyleDescription()}";

        private string BuildStyleDescription()
        {
            List<string> results = new List<string>();

            BuildFontDescription(results);

            Describe(results, "background", Background);
            Describe(results, "border", Border);
            Describe(results, "animation", Animation);
            Describe(results, "flex", Flex);
            Describe(results, "flexitem", FlexItem);
            Describe(results, "layout", Layout);
            Describe(results, "padding", Padding);
            Describe(results, "margin", Margin);

            return string.Join(" ", results);
        }

        private void BuildFontDescription(List<string> results)
        {
            List<string> fontDesc = new List<string>();

            DescribeValue(fontDesc, FontFace);
            DescribeValue(fontDesc, TextColor);
            DescribeValue(fontDesc, FontSize);
            DescribeValue(fontDesc, FontStyle);

            if (fontDesc.Count > 0)
            {
                results.Add($"font: {{ {string.Join(" ", fontDesc)} }}");
            }
        }

        private void Describe<T>(List<string> results, string name, T value) where T : class
        {
            if (value != null)
            {
                results.Add($"{name} {{ {value} }}");
            }
        }

        private void Describe<T>(List<string> results, string name, T? value) where T : struct
        {
            if (value != null)
            {
                results.Add($"{name} {{ {value} }}");
            }
        }

        private void DescribeValue<T>(List<string> results, T value) where T : class
        {
            if (value != null)
            {
                results.Add(value.ToString());
            }
        }

        private void DescribeValue<T>(List<string> results, T? value) where T : struct
        {
            if (value != null)
            {
                results.Add(value.ToString());
            }
        }
    }
}
