using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib
{
	/// <summary>
	/// Used by AgateLib.AgateApp class's error reporting functions
	/// to indicate how severe an error is.
	/// </summary>
	public enum ErrorLevel
	{
		/// <summary>
		/// Indicates an message is just a comment, and safe to ignore.
		/// </summary>
		Comment,
		/// <summary>
		/// Indicates that the error message is not severe, and the program may
		/// continue.  However, unexpected behavior may occur due to the result of
		/// this error.
		/// </summary>
		Warning,
		/// <summary>
		/// Indicates that the error condition is too severe and the program 
		/// may not continue.
		/// </summary>
		Fatal,


		/// <summary>
		/// Indicates the error condition indicates some assumption
		/// has not held that should have.  This should only be used
		/// if the condition is caused by a bug in the code.
		/// </summary>
		Bug,
	}

	/// <summary>
	/// Enum used to inidicate the level of cross-platform debugging that should occur.
	/// </summary>
	public enum CrossPlatformDebugLevel
	{
		/// <summary>
		/// Ignores any issues related to cross platform deployment.
		/// </summary>
		None,

		/// <summary>
		/// Outputs comments using AgateApp.Report with a comment level.
		/// </summary>
		Comment,

		/// <summary>
		/// Throws exceptions on issues that may cause problems when operating on another platform.
		/// </summary>
		Exception,
	}

}
