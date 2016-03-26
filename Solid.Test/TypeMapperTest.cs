using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Solid.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Solid.Test
{
	[TestClass]
	public class TypeMapperTest
	{
		[TestMethod]
		public void CheckAddAndExistWithAddMethod()
		{
			var mapper = new TypeMapper();
			mapper.Add("name", typeof(object));

			AreEqual(typeof(object), mapper["name"]);
		}

		[TestMethod]
		public void CheckAddAndExistWithIndexer()
		{
			var mapper = new TypeMapper();
			mapper["name"] = typeof(object);

			AreEqual(typeof(object), mapper["name"]);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CheckAddWithInvalidType()
		{
			var mapper = new TypeMapper();
			mapper.Add("name", typeof(Type));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CheckAddWithInvalidTypeViaIndexer()
		{
			var mapper = new TypeMapper();
			mapper["name"] = typeof(Type);
		}

		[TestMethod]
		public void CheckGetType()
		{
			var mapper = new TypeMapper();
			mapper["customType"] = typeof(CustomType);

			AreSame(typeof(object), mapper.Get("System.Object"), "System.Object");
			AreSame(typeof(ArrayList), mapper.Get("System.Collections.ArrayList"), "System.Collections.ArrayList");
			// TODO: Implement getting non-userdefined, but global available types
			// AreSame(typeof(TestNamespace.SomeClass), mapper.Get("TestNamespace.SomeClass"), "TestNamespace.SomeClass");
			AreSame(typeof(CustomType), mapper.Get("customType"), "customType");
		}

		private class CustomType
		{
		}
	}
}

namespace TestNamespace
{
	public class SomeClass
	{

	}
}
