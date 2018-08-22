using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AgateLib.UserInterface.Styling.Themes
{
    public class CssSelectorTests
    {
        [Fact]
        public void MatchesWithComma()
        {
            var tree = Element(
                children: new[]
                {
                    Element(cls: "menu"),
                    Element(cls: "window"),
                    Element(),
                    Element(),
                });

            var selector = ".menu, .window";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            Matches(tree, selector, tree.Children.First());
            Matches(tree, selector, tree.Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.Skip(2).First());
            DoesNotMatch(tree, selector, tree.Children.Skip(3).First());
        }

        [Fact]
        public void MatchesFirstChildWildCard()
        {
            var tree = Element(cls: "workspace",
                children: new[]
                {
                    Element(children: new [] { Element() }),
                    Element(),
                    Element(),
                    Element(),
                });

            var selector = ".workspace > *";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            Matches(tree, selector, tree.Children.First());
            Matches(tree, selector, tree.Children.Skip(1).First());
            Matches(tree, selector, tree.Children.Skip(2).First());
            Matches(tree, selector, tree.Children.Skip(3).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
        }

        [Fact]
        public void SingleWildCard()
        {
            var tree = Element(cls: "workspace",
                children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new [] { Element() }),
                        Element(),
                    }),
                    Element(),
                });

            var selector = "*";

            // Top level
            Matches(tree, selector, tree);

            // First level
            Matches(tree, selector, tree.Children.First());
            Matches(tree, selector, tree.Children.Skip(1).First());

            // Second level
            Matches(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesAllDescendentsWildCard()
        {
            var tree = Element(cls: "workspace",
                children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new [] { Element() }),
                        Element(),
                    }),
                    Element(),
                });

            var selector = ".menu *";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            Matches(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void PsuedoClassMatches()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(typeId:"menu", children: new []
                    {
                        Element(children: new [] { Element() }),
                        Element(),
                    }),
                    Element(),
                });

            var selector = "menu:something";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            Matches(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(1).First());

            // Third level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesDescendents()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new []
                        {
                            Element(cls:"label")
                        }),
                        Element(cls:"label"),
                        Element(cls:"notlabel"),
                    }),
                    Element(cls: "label"),
                });

            var selector = ".menu .label";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesDescendentsWithTypeId()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new []
                        {
                            Element(typeId:"label")
                        }),
                        Element(typeId:"label"),
                        Element(typeId:"notlabel"),
                    }),
                    Element(typeId: "label"),
                });

            var selector = ".menu label";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesDescendentsWithId()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new []
                        {
                            Element(name:"label")
                        }),
                        Element(name:"label"),
                        Element(name:"notlabel"),
                    }),
                    Element(name: "label"),
                });

            var selector = ".menu #label";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesInteriorWildcard()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new []
                        {
                            Element(cls:"label")
                        }),
                        Element(cls:"label"),
                        Element(cls:"notlabel"),
                    }),
                    Element(cls: "label"),
                });

            var selector = ".workspace * .label";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            Matches(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesChildrenWithId()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(children: new []
                        {
                            Element(name:"label")
                        }),
                        Element(name:"label"),
                        Element(name:"notlabel"),
                    }),
                    Element(name: "label"),
                });

            var selector = ".menu > #label";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesClassWithDashes()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(cls:"menu-item", children: new []
                        {
                            Element(name:"label")
                        }),
                        Element(cls:"menu-item"),
                        Element(cls:"menu-item"),
                    }),
                    Element(name: "label"),
                });

            var selector = ".menu-item";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            Matches(tree, selector, tree.Children.First().Children.First());
            Matches(tree, selector, tree.Children.First().Children.Skip(1).First());
            Matches(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        [Fact]
        public void MatchesTypeAndClass()
        {
            var tree = Element(cls: "workspace", children: new[]
                {
                    Element(cls:"menu", children: new []
                    {
                        Element(typeId:"menuitem", cls:"first", children: new []
                        {
                            Element(name:"label")
                        }),
                        Element(typeId:"menuitem"),
                        Element(typeId:"menuitem", cls:"first"),
                    }),
                    Element(name: "label"),
                });

            var selector = "menuitem.first";

            // Top level
            DoesNotMatch(tree, selector, tree);

            // First level
            DoesNotMatch(tree, selector, tree.Children.First());
            DoesNotMatch(tree, selector, tree.Children.Skip(1).First());

            // Second level
            Matches(tree, selector, tree.Children.First().Children.First());
            DoesNotMatch(tree, selector, tree.Children.First().Children.Skip(1).First());
            Matches(tree, selector, tree.Children.First().Children.Skip(2).First());

            // Third level
            DoesNotMatch(tree, selector, tree.Children.First().Children.First().Children.First());
        }

        private void DoesNotMatch(IRenderElement tree, string selector, IRenderElement renderElement)
        {
            Matches(tree, selector, renderElement, false);
        }

        private void Matches(IRenderElement tree, string selector,
            IRenderElement renderElement, bool expectedResult = true)
        {
            var ex = new CssWidgetSelector(selector);
            var result = ex.FindMatch(renderElement, GetStack(tree, renderElement));

            if (!expectedResult)
            {
                result.Should().Be(null);
                return;
            }

            result.Should().NotBe(null);
        }

        private RenderElementStack GetStack(IRenderElement tree, IRenderElement renderElement)
        {
            RenderElementStack temp = new RenderElementStack();

            bool Search(IRenderElement node)
            {
                if (node == renderElement)
                    return true;

                if (node.Children == null)
                    return false;

                foreach (var item in node.Children)
                {
                    if (Search(item))
                    {
                        temp.PushParent(node);
                        return true;
                    }
                }

                return false;
            }

            Search(tree).Should().BeTrue("Could not find element in tree.");

            // We need to reverse the stack
            RenderElementStack result = new RenderElementStack();

            while (temp.Count > 0)
                result.PushParent(temp.PopParent());

            return result;
        }

        IRenderElement Element(
            string typeId = "",
            string cls = "",
            string name = "",
            IRenderElement[] children = null)
        {
            Mock<IRenderElement> result = new Mock<IRenderElement>();

            result.SetupGet(x => x.StyleTypeId).Returns(typeId);
            result.SetupGet(x => x.StyleClass).Returns(cls);
            result.SetupGet(x => x.Name).Returns(name);
            result.SetupGet(x => x.Children).Returns(children);

            return result.Object;
        }

    }
}
