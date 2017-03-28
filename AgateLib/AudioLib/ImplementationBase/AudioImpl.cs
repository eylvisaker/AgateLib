//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using AgateLib.Drivers;
using AgateLib.IO;
using System.IO;

namespace AgateLib.AudioLib.ImplementationBase
{
	/// <summary>
	/// Implements Audio class factory.
	/// </summary>
	public abstract class AudioImpl : IDriverCore
	{

		/// <summary>
		/// This function is called once a frame to allow the Audio driver to update
		/// information.  There is no need to call base.Update() if overriding this
		/// function.
		/// </summary>
		public virtual void Update()
		{
		}


		/// <summary>
		/// This function is called when a Caps property is inspected.
		/// It should return false for any unknown value.
		/// </summary>
		/// <param name="audioBoolCaps"></param>
		/// <returns></returns>
		protected internal abstract bool CapsBool(AudioBoolCaps audioBoolCaps);

		/// <summary>
		/// Destroys the AudioImpl.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Override to dispose of local resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{ }

		/// <summary>
		/// Override to provide initialization after the connection to the Audio class is made.
		/// </summary>
		public abstract void Initialize();
	}

}
