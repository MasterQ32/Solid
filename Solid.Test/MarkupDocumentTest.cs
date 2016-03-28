using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Solid.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Solid.Test
{
	[TestClass]
	public class MarkupDocumentTest
	{

		[TestMethod]
		public void TestMapper()
		{
			var doc = Parser.Parse(@"Root { Child(Name=""Harry""); Child(Name=""Maria""); }");

			var mapper = new TypeMapper();
			mapper.Add("Root", typeof(RootType));
			mapper.Add("Child", typeof(ChildType));

			var result = doc.Map(mapper);

			IsNotNull(result);
			AreSame(typeof(RootType), result.GetType());

			var root = result as RootType;
			AreEqual(2, root.children.Count);

			AreSame(typeof(ChildType), root.children[0].GetType());
			AreSame(typeof(ChildType), root.children[1].GetType());

			AreEqual("Harry", root.children[0].Name);
			AreEqual("Maria", root.children[1].Name);
		}

		[TestMethod]
		public void TestPropertyMapping()
		{
			var doc = Parser.Parse(@"Properties(
	Byte = 8, 
	SByte = -10, 
	Short = -20, 
	UShort = 30, 
	Int = -40, 
	UInt = 50,
	Long = -60, 
	ULong = 70,
	Float = 1.3,
	Double = 2.6, 
	Decimal = 3.8,
	Enum = Second,
	String = ""Hello World!"", 
	Boolean = true,
	DateTime = ""15. Oct 2003"",
	TimeSpan = ""10:20:30.123"",
	CultureInfo = ""de-DE"",
	Guid = ""{962C7B95-AA1F-4853-966E-1DE34CD12435}""
);");

			var mapper = new TypeMapper();
			mapper["Properties"] = typeof(PropertyTest);

			var properties = doc.Map(mapper) as PropertyTest;

			AreEqual<byte>(8, properties.Byte);
			AreEqual<sbyte>(-10, properties.SByte);
			AreEqual<short>(-20, properties.Short);
			AreEqual<ushort>(30, properties.UShort);
			AreEqual<int>(-40, properties.Int);
			AreEqual<uint>(50, properties.UInt);
			AreEqual<long>(-60, properties.Long);
			AreEqual<ulong>(70, properties.ULong);
			AreEqual<float>(1.3f, properties.Float);
			AreEqual<double>(2.6d, properties.Double);
			AreEqual<decimal>(3.8m, properties.Decimal);
			AreEqual<TestEnumeration>(TestEnumeration.Second, properties.Enum);
			AreEqual<string>("Hello World!", properties.String);
			AreEqual<bool>(true, properties.Boolean);

			AreEqual<DateTime>(new DateTime(2003, 10, 15), properties.DateTime);
			AreEqual<TimeSpan>(new TimeSpan(0, 10, 20, 30, 123), properties.TimeSpan);
			AreEqual<CultureInfo>(CultureInfo.GetCultureInfo("de-DE"), properties.CultureInfo);
			AreEqual<Guid>(new Guid("{962C7B95-AA1F-4853-966E-1DE34CD12435}"), properties.Guid);
		}

		[TestMethod]
		public void TestChildMapping()
		{
			var doc = Parser.Parse(@"Root { ChildA; ChildB(Name = ""Maria"", Age = 20); ChildC(Special = 30); }");

			var mapper = new TypeMapper();
			mapper.Add("Root", typeof(RootType));
			mapper.Add("ChildA", typeof(ChildType));
			mapper.Add("ChildB", typeof(ChildSubType));
			mapper.Add("ChildC", typeof(SpecialChildType));

			var result = doc.Map(mapper);

			IsNotNull(result);
			AreSame(typeof(RootType), result.GetType());

			var root = result as RootType;
			AreEqual(2, root.children.Count);
			AreEqual(1, root.specials.Count);

			AreSame(typeof(ChildType), root.children[0].GetType());
			AreSame(typeof(ChildSubType), root.children[1].GetType());

			AreEqual("Maria", root.children[1].Name);
			AreEqual(20, ((ChildSubType)root.children[1]).Age);

			AreSame(typeof(SpecialChildType), root.specials[0].GetType());
			AreEqual(30, root.specials[0].Special);
		}

		public void TestNamedMapping()
		{
			var types = new TypeMapper();
			types.Add("Container", typeof(ChildContainer));
			types.Add("NamedContainer", typeof(NamedChildContainer));

			var doc = Parser.Parse(
@"Container {
	a : Container;
	b : NamedContainer {
		c : Container;
		d : Container {
			e : Container;
		}
	}
}");
			var dict = new Dictionary<string, ChildContainer>();

			var mapper = new NativeMapper(types);
			mapper.EmitAllNamedNodes = true;
			mapper.NamedNodeEmitted += (s, e) => dict.Add(e.Name, (ChildContainer)e.Node);

			var root = (ChildContainer)mapper.Instantiate(doc);

			AreEqual(2, root.children.Count);
			AreEqual(0, root.children[0].children.Count);
			AreEqual(2, root.children[1].children.Count);

			IsInstanceOfType(root.children[1], typeof(NamedChildContainer));

			AreEqual(0, root.children[1].children[0].children.Count);
			AreEqual(1, root.children[1].children[1].children.Count);

			IsInstanceOfType(root.children[1].children[0], typeof(NamedChildContainer));

			AreEqual(1, root.children[1].children[1].children[0].children.Count);

			AreEqual(2, dict.Count);
			IsTrue(dict.ContainsKey("a"));
			IsTrue(dict.ContainsKey("e"));
			AreSame(root.children[0], dict["a"]);
			AreSame(root.children[1].children[1].children[0], dict["e"]);

			var b = (NamedChildContainer)root.children[1];
			AreEqual(2, b.namedChildren.Count);
			IsTrue(b.namedChildren.ContainsKey("b"));
			IsTrue(b.namedChildren.ContainsKey("c"));
			AreSame(b.children[0], b.namedChildren["b"]);
			AreSame(b.children[1], b.namedChildren["c"]);
		}

		class RootType
		{
			public List<ChildType> children = new List<ChildType>();
			public List<SpecialChildType> specials = new List<SpecialChildType>();

			public void Add(ChildType child)
			{
				this.children.Add(child);
			}
			public void Add(SpecialChildType child)
			{
				this.specials.Add(child);
			}
		}

		class ChildType
		{
			public string Name { get; set; }
		}

		class ChildSubType : ChildType
		{
			public int Age { get; set; }
		}

		class SpecialChildType : ChildType
		{
			public int Special { get; set; }
		}

		enum TestEnumeration
		{
			Default,
			First,
			Second,
		}

		class PropertyTest
		{
			public byte Byte { get; set; }
			public sbyte SByte { get; set; }
			public short Short { get; set; }
			public ushort UShort { get; set; }
			public int Int { get; set; }
			public uint UInt { get; set; }
			public long Long { get; set; }
			public ulong ULong { get; set; }
			public float Float { get; set; }
			public double Double { get; set; }
			public decimal Decimal { get; set; }
			public TestEnumeration Enum { get; set; }
			public string String { get; set; }
			public bool Boolean { get; set; }

			// Special types:
			public DateTime DateTime { get; set; }
			public TimeSpan TimeSpan { get; set; }
			public CultureInfo CultureInfo { get; set; }
			public Guid Guid { get; set; }
		}

		class ChildContainer
		{
			public List<ChildContainer> children;

			public void Add(ChildContainer child) => this.children.Add(child);
		}

		class NamedChildContainer : ChildContainer, INamedNodeContainer
		{
			public Dictionary<string, ChildContainer> namedChildren = new Dictionary<string, ChildContainer>();

			public void SetChildNodeName(object child, string name)
			{
				namedChildren.Add(name, (ChildContainer)child);
			}
			
		}
	}
}
