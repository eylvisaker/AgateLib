using AgateLib.Display;
using AgateLib.Quality;
using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
    public static class UserInterfaceFunctions
    {
        public static void InitializeStyles(this IRenderElement root, 
                                            IUserInterfaceRenderContext renderContext)
        {
            Require.ArgumentNotNull(root, nameof(root), "Root element cannot be null.");
            Require.ArgumentNotNull(renderContext, nameof(renderContext), 
                "Render Context should not be null.");
            Require.That(renderContext.Fonts != null,
                "The render context's font collection should not be null.");

            root.InitializeStyles(renderContext.Fonts.Default);
        }

        public static void InitializeStyles(this IRenderElement root,
                                            Font defaultFont)
        {
            Require.ArgumentNotNull(root, nameof(root), "Root element cannot be null.");
            Require.ArgumentNotNull(defaultFont, nameof(defaultFont),
                "Default font should not be null.");

            UpdateStyles(root);
        }

        public static void UpdateStyles(IRenderElement root)
        {
            root.Style.Update();

            if (root.Children == null)
                return;

            foreach(var child in root.Children)
            {
                UpdateStyles(child);
            }
        }
    }
}
