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

namespace AgateLib.Utility
{
	/// <summary>
	/// Class which implements a reference counter around a Disposable object.
	/// The object is disposed when the reference count drops to zero.  This class
	/// should not be used directly, instead use the Ref&lt;T&gt; structure.
	/// </summary>
	/// <remarks>
	/// [Experimental - This class may disappear in the future.]
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public sealed class RefCounter<T> : IDisposable where T : IDisposable
	{
		int mRefs = 0;
		T v;
		bool mIsDisposed = false;

		/// <summary>
		/// Constructs a RefCounter object around the specified object.
		/// </summary>
		/// <param name="obj"></param>
		public RefCounter(T obj)
		{
			v = obj;
			mRefs = 1;
		}
		/// <summary>
		/// Increases the reference count.
		/// </summary>
		public void AddRef()
		{
			mRefs++;
		}
		/// <summary>
		/// Decreases the reference count, and if it hits zero Dispose is
		/// called on the object.
		/// </summary>
		public void Dispose()
		{
			mRefs--;

			if (mRefs == 0)
			{
				v.Dispose();
				mIsDisposed = true;
			}
			else if (mRefs < 0)
			{
				System.Diagnostics.Debug.Assert(false, "Object dereferenced too many times!");
			}
		}
		/// <summary>
		/// Returns the number of references.
		/// This is meant for debugging purposes only, do not write code which 
		/// calls Dispose until GetRefCount drops to zero.
		/// </summary>
		/// <returns></returns>
		public int GetRefCount()
		{
			return mRefs;
		}

		/// <summary>
		/// Gets the object this is wrapped around.
		/// </summary>
		public T Value
		{
			get { return v; }
		}
		/// <summary>
		/// Returns true if the object has had its Dispose member called, and thus no
		/// more calls on that object should be made.
		/// </summary>
		public bool IsDisposed
		{
			get { return mIsDisposed; }
		}

	}

	/// <summary>
	/// A structure which is used for copying RefCounter&lt;T&gt; around.
	/// In order to make sure reference counters are updated correctly, never
	/// use the equals operator with this object.  Always create a new one and
	/// pass the old one to the constructor.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct Ref<T> : IDisposable where T : IDisposable
	{
		RefCounter<T> v;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public Ref(T obj)
		{
			if (obj == null)
				throw new NullReferenceException();

			v = new RefCounter<T>(obj);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="copyfrom"></param>
		public Ref(Ref<T> copyfrom)
		{
			v = copyfrom.v;
			v.AddRef();
		}
		/// <summary>
		/// Returns true if this reference has been disposed.
		/// This does not indicate whether the object being referenced has been disposed,
		/// but it may have been if this was the last reference.  In any case, if this
		/// is true, you shouldn't access the object anyway.
		/// </summary>
		public bool IsDisposed
		{
			get { return v == null; }
		}
		/// <summary>
		/// Returns the object being wrapped around.
		/// </summary>
		public T Value
		{
			get { return v.Value; }
		}
		/// <summary>
		/// The RefCounter&lt;T&gt; object this wraps.
		/// </summary>
		public RefCounter<T> Counter
		{
			get { return v; }
			set
			{
				if (v != null) v.Dispose();
				v = value;
				v.AddRef();
			}
		}
		/// <summary>
		/// Releases this reference to the object.
		/// </summary>
		public void Dispose()
		{
			v.Dispose();
			v = null;
		}

		/// <summary>
		/// Returns a hash code for this object.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			if (Value == null)
				return 0;

			return
				Value.GetHashCode();
		}

		/// <summary>
		/// Compares two object instances.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Ref<T>)
				return Equals((Ref<T>)obj);
			if (obj is T)
				return this == (T)obj;

			return false;
		}

		/// <summary>
		/// Compares two object instances.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(Ref<T> obj)
		{
			return this == obj.Value;
		}

		/// <summary>
		/// Compares two object instances
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Equals(T obj)
		{
			return this == obj;
		}
		/// <summary>
		/// Compares two object instances.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool operator ==(Ref<T> r, T obj)
		{
			if (obj == null)
			{
				if (r.v == null)
					return true;
				else
					return false;
			}

			if (r.v == null)
				return false;
			if (r.v.Value == null)
				return false;

			return r.v.Value.Equals(obj);
		}

		/// <summary>
		/// Compares two object instances.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool operator !=(Ref<T> r, T obj)
		{
			return !(r == obj);
		}
	}
}
