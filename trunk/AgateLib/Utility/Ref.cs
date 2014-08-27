//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
	public class RefCounter<T> : IDisposable where T : IDisposable
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

		public static bool operator==(Ref<T> r, object obj)
		{
			if (obj == null && r.v == null)
				return true;

			else 
				return false;
		}
		public static bool operator !=(Ref<T> r, object obj)
		{
			return !(r == obj);
		}
	}
}
