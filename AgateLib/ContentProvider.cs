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
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AgateLib
{
    /// <summary>
    /// Provides an interface for accessing game content.
    /// </summary>
    public interface IContentProvider
    {
        /// <summary>
        /// Loads a piece of content that was processed with the content pipeline.
        /// Essentially equivalent to using a ContentManager.Load<typeparamref name="T"/> method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        T Load<T>(string assetName);

        /// <summary>
        /// Opens a file for reading. The file is searched for relative to the same location as
        /// content is accessed from the Load method.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Stream Open(string filename);
    }

    /// <summary>
    /// Implements IContentProvider.
    /// </summary>
    public class ContentProvider : IContentProvider
    {
        private readonly ContentManager content;

        public ContentProvider(ContentManager content)
        {
            this.content = content;
        }

        public T Load<T>(string assetName)
        {
            return content.Load<T>(assetName);
        }

        [DebuggerStepThrough]
        public Stream Open(string filename)
        {
            return TitleContainer.OpenStream(content.RootDirectory + "/" + filename);
        }
    }

    public static class ContentProviderExtensions
    {
        public static string ReadAllText(this IContentProvider content, string filename)
        {
            using (var s = new StreamReader(content.Open(filename)))
            {
                return s.ReadToEnd();
            }
        }

        /// <summary>
        /// Opens a file if it exists, or returns null.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Stream OpenOrNull(this IContentProvider content, string filename)
        {
            try
            {
                return content.Open(filename);
            }
            catch(FileNotFoundException)
            {
                return null;
            }
        }

        public static string ReadAllTextOrNull(this IContentProvider content, string filename)
        {
            var stream = content.OpenOrNull(filename);
            if (stream == null)
                return null;

            using (var s = new StreamReader(stream))
            {
                return s.ReadToEnd();
            }
        }

    }
}
