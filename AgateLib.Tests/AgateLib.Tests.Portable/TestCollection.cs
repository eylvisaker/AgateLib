using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests
{
	public static class TestCollection
	{
		static List<TestInfo> mTests = new List<TestInfo>();

		public static IList<TestInfo> Tests { get { return mTests; } }

		static TestCollection()
		{
			AddTests(typeof(TestCollection).GetTypeInfo().Assembly);
		}

		public static void AddTests(Assembly assembly)
		{
			foreach (var typeinfo in assembly.DefinedTypes)
			{
				if (typeinfo.ImplementedInterfaces.Contains(typeof(IAgateTest)) && 
					typeinfo.IsAbstract == false)
				{
					Add(typeinfo);
				}
			}

			mTests.Sort((x, y) =>
			{
				if (x.Category != y.Category)
					return x.Category.CompareTo(y.Category);
				else
					return x.Name.CompareTo(y.Name);
			});
		}

		private static void Add(TypeInfo typeinfo)
		{
			var type = typeinfo.AsType();

			if (mTests.Any(x => x.Class == type))
				return;

			IAgateTest obj = (IAgateTest)Activator.CreateInstance(type);

			mTests.Add(new TestInfo { Name = obj.Name, Category = obj.Category, Class = type });
		}
	}
}
