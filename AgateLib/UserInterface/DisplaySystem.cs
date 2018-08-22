//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.UserInterface.Rendering.Animations;
using AgateLib.UserInterface;

namespace AgateLib.UserInterface
{
    public interface IDisplaySystem
    {
        IRenderElement Focus { get; }

        IFontProvider Fonts { get; }

        IInstructions Instructions { get; }

        IRenderElement ParentOf(IRenderElement element);

        void SetFocus(IRenderElement newFocus);

        /// <summary>
        /// Adds a workspace to the desktop.
        /// </summary>
        /// <param name="newWorkspace"></param>
        void PushWorkspace(Workspace newWorkspace);

        /// <summary>
        /// Removes the active workspace from the desktop.
        /// </summary>
        void PopWorkspace();

        void PlaySound(object originator, UserInterfaceSound sound);
    }

    public static class DisplaySystemExtensions
    {
        public static void SetFocus(this IDisplaySystem displaySystem, ElementReference reference)
        {
            displaySystem.SetFocus(reference.Current);
        }
    }
}
