using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.Resources
{
	public static class BuiltinResources
	{
		static FontSurface mSans10;
		static FontSurface mSans14;
		static FontSurface mSans24;
		static FontSurface mSerif10;
		static FontSurface mSerif14;
		static FontSurface mSerif24;
		static FontSurface mMono10;

		private static FontSurface LazyCreateFont(ref FontSurface store, string name, int size)
		{
			if (store == null)
				store = new FontSurface(name, size);

			return store;
		}
		public static FontSurface AgateSans10 { get { return LazyCreateFont(ref mSans10, "Arial", 10); } }
		public static FontSurface AgateSans14 { get { return LazyCreateFont(ref mSans14, "Arial", 14); } }
		public static FontSurface AgateSans24 { get { return LazyCreateFont(ref mSans24, "Arial", 24); } }

		public static FontSurface AgateSerif10 { get { return LazyCreateFont(ref mSerif10, "Times New Roman", 10); } }
		public static FontSurface AgateSerif14 { get { return LazyCreateFont(ref mSerif14, "Times New Roman", 14); } }
		public static FontSurface AgateSerif24 { get { return LazyCreateFont(ref mSerif24, "Times New Roman", 24); } }

		public static FontSurface AgateMono10 { get { return LazyCreateFont(ref mMono10, "Consolas", 10); } }
	}
}
