using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Solid.Markup;
using System.IO;
using System.Linq;

namespace Solid.Test
{
	[TestClass]
	public class SoildMarkup
	{

		[TestMethod]
		[ExpectedException(typeof(InvalidDataException))]
		public void TestParsingEmpty()
		{
			Parser.Parse("");
		}

		[TestMethod]
		public void TestParsingOnlyRootWithoutAttributes()
		{
			var node = Parser.Parse("Root;").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleNumberAttribute()
		{
			var node = Parser.Parse("Root(a=10);").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
			AreEqual(1, node.Attributes.Count);
			IsTrue(node.Attributes.ContainsKey("a"));
			AreEqual("10", node.Attributes["a"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleNegativeNumberAttribute()
		{
			var node = Parser.Parse("Root(a=-10);").Root;
			IsNotNull(node, "Node was null");
			AreEqual(node.Type, "Root");
			AreEqual(node.Children.Count, 0);
			AreEqual(node.Attributes.Count, 1);
			IsTrue(node.Attributes.ContainsKey("a"));
			AreEqual("-10", node.Attributes["a"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleDecimalNumberAttribute()
		{
			var node = Parser.Parse("Root(a=10.0123);").Root;
			IsNotNull(node, "Node was null");
			AreEqual(node.Type, "Root");
			AreEqual(node.Children.Count, 0);
			AreEqual(node.Attributes.Count, 1);
			IsTrue(node.Attributes.ContainsKey("a"));
			AreEqual("10.0123", node.Attributes["a"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleNegativeDecimalNumberAttribute()
		{
			var node = Parser.Parse("Root(a=-10.123);").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
			AreEqual(1, node.Attributes.Count);
			IsTrue(node.Attributes.ContainsKey("a"));
			AreEqual("-10.123", node.Attributes["a"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleEnumerationAttribute()
		{
			var node = Parser.Parse("Root(enum=enumeration_key);").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
			AreEqual(1, node.Attributes.Count);
			IsTrue(node.Attributes.ContainsKey("enum"));
			AreEqual("enumeration_key", node.Attributes["enum"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithSingleStringAttribute()
		{
			var node = Parser.Parse("Root(SOME_text=\"some simple \\\"escaped\\\" string\");").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
			AreEqual(1, node.Attributes.Count);
			IsTrue(node.Attributes.ContainsKey("SOME_text"));
			AreEqual("some simple \"escaped\" string", node.Attributes["SOME_text"].Value);
		}

		[TestMethod]
		public void TestParsingOnlyRootWithMultipleAttributes()
		{
			var node = Parser.Parse("Root(a=10, b=20, c = 30);").Root;
			IsNotNull(node, "Node was null");
			AreEqual("Root", node.Type);
			AreEqual(0, node.Children.Count);
			AreEqual(3, node.Attributes.Count);
			IsTrue(node.Attributes.ContainsKey("a"));
			IsTrue(node.Attributes.ContainsKey("b"));
			IsTrue(node.Attributes.ContainsKey("c"));
			AreEqual("10", node.Attributes["a"].Value);
			AreEqual("20", node.Attributes["b"].Value);
			AreEqual("30", node.Attributes["c"].Value);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidDataException))]
		public void TestParsingOnlyRootWithDuplicatedAttributes()
		{
			Parser.Parse("Root(a=10, a=20);");
		}

		[TestMethod]
		public void TestParsingSingleChild()
		{
			var node = Parser.Parse("Root { Child; }").Root;
			IsNotNull(node);
			AreEqual("Root", node.Type);
			AreEqual(1, node.Children.Count);
			AreEqual(0, node.Attributes.Count);

			var child = node.Children[0];
			IsNotNull(child);
			AreEqual("Child", child.Type);
			AreEqual(0, child.Children.Count);
			AreEqual(0, child.Attributes.Count);
		}

		[TestMethod]
		public void TestParsingMultipleChildren()
		{
			var node = Parser.Parse("Root { ChildA; ChildB; ChildC; }").Root;
			IsNotNull(node);
			AreEqual("Root", node.Type);
			AreEqual(3, node.Children.Count);
			AreEqual(0, node.Attributes.Count);

			var child0 = node.Children[0];
			IsNotNull(child0);
			AreEqual("ChildA", child0.Type);
			AreEqual(0, child0.Children.Count);
			AreEqual(0, child0.Attributes.Count);

			var child1 = node.Children[1];
			IsNotNull(child1);
			AreEqual("ChildB", child1.Type);
			AreEqual(0, child1.Children.Count);
			AreEqual(0, child1.Attributes.Count);

			var child2 = node.Children[2];
			IsNotNull(child2);
			AreEqual("ChildC", child2.Type);
			AreEqual(0, child2.Children.Count);
			AreEqual(0, child2.Attributes.Count);
		}

		[TestMethod]
		public void TestParsingNestedChildren()
		{
			var node = Parser.Parse("Root { ChildA { ChildB; } ChildC; }").Root;
			IsNotNull(node);
			AreEqual("Root", node.Type);
			AreEqual(2, node.Children.Count);
			AreEqual(0, node.Attributes.Count);

			var child0 = node.Children[0];
			IsNotNull(child0);
			AreEqual("ChildA", child0.Type);
			AreEqual(1, child0.Children.Count);
			AreEqual(0, child0.Attributes.Count);

			var child1 = child0.Children[0];
			IsNotNull(child1);
			AreEqual("ChildB", child1.Type);
			AreEqual(0, child1.Children.Count);
			AreEqual(0, child1.Attributes.Count);

			var child2 = node.Children[1];
			IsNotNull(child2);
			AreEqual("ChildC", child2.Type);
			AreEqual(0, child2.Children.Count);
			AreEqual(0, child2.Attributes.Count);
		}


		[TestMethod]
		public void TestParsingNestedChildrenWithAttributes()
		{
			var node = Parser.Parse("Root { ChildA { ChildB(a=10); } ChildC(b=20); }").Root;
			IsNotNull(node);
			AreEqual("Root", node.Type);
			AreEqual(2, node.Children.Count);
			AreEqual(0, node.Attributes.Count);

			var child0 = node.Children[0];
			IsNotNull(child0);
			AreEqual("ChildA", child0.Type);
			AreEqual(1, child0.Children.Count);
			AreEqual(0, child0.Attributes.Count);

			var child1 = child0.Children[0];
			IsNotNull(child1);
			AreEqual("ChildB", child1.Type);
			AreEqual(0, child1.Children.Count);
			AreEqual(1, child1.Attributes.Count);
			IsTrue(child1.Attributes.ContainsKey("a"));
			AreEqual("10", child1.Attributes["a"].Value);

			var child2 = node.Children[1];
			IsNotNull(child2);
			AreEqual("ChildC", child2.Type);
			AreEqual(0, child2.Children.Count);
			AreEqual(1, child2.Attributes.Count);
			IsTrue(child2.Attributes.ContainsKey("b"));
			AreEqual("20", child2.Attributes["b"].Value);
		}

		static readonly string simpleFullDocument =
@"Widget(image=""fractale-06-scenery-antenna.jpg"")
{
	Widget(image= ""crosshair.png"", verticalAlignment= center, horizontalAlignment= center, size= ""96;96"");
	Widget(image= ""minimap.png"", verticalAlignment= top, horizontalAlignment= right, size= ""200;200"");
	StackLayout(verticalAlignment= bottom, horizontalAlignment= left, orientation= vertical)
	{
		Widget(image = ""endurance.png"", size = ""200;64"");
		Widget(image = [example], size = ""200;64"");
		Widget(image = ""healthbar.png"", size = ""200;64"");
	}
}";

		static readonly MarkupNode simpleFullDocumentReference = CreateNode(
			"Widget",
			new[] { Tuple.Create("image", "fractale-06-scenery-antenna.jpg", MarkupPropertyType.String), },
			new[]
			{
				CreateNode("Widget", new[] {
					Tuple.Create("image", "crosshair.png", MarkupPropertyType.String),
					Tuple.Create("verticalAlignment", "center", MarkupPropertyType.Enumeration),
					Tuple.Create("horizontalAlignment", "center", MarkupPropertyType.Enumeration),
					Tuple.Create("size", "96;96", MarkupPropertyType.String)}),

				CreateNode("Widget", new[] {
					Tuple.Create("image", "minimap.png", MarkupPropertyType.String),
					Tuple.Create("verticalAlignment", "top", MarkupPropertyType.Enumeration),
					Tuple.Create("horizontalAlignment", "right", MarkupPropertyType.Enumeration),
					Tuple.Create("size", "200;200", MarkupPropertyType.String)}),

				CreateNode("StackLayout", new [] {
					Tuple.Create("verticalAlignment", "bottom", MarkupPropertyType.Enumeration),
					Tuple.Create("horizontalAlignment", "left", MarkupPropertyType.Enumeration),
					Tuple.Create("orientation", "vertical", MarkupPropertyType.Enumeration),
					},
					new [] {
						CreateNode("Widget", new [] {
							Tuple.Create("image", "endurance.png", MarkupPropertyType.String),
							Tuple.Create("size", "200;64", MarkupPropertyType.String)
						}),
						CreateNode("Widget", new [] {
							Tuple.Create("image", "example", MarkupPropertyType.Binding),
							Tuple.Create("size", "200;64", MarkupPropertyType.String)
						}),
						CreateNode("Widget", new [] {
							Tuple.Create("image", "healthbar.png", MarkupPropertyType.String),
							Tuple.Create("size", "200;64", MarkupPropertyType.String)
						}),
					}),
			});

		private void AssertNodeEquals(MarkupNode a, MarkupNode b)
		{
			AreEqual(a.Type, b.Type);
			foreach (var attrib in a.Attributes)
			{
				AreEqual(a.Attributes[attrib.Key], b.Attributes[attrib.Key]);
			}
			AreEqual(0, b.Attributes.Count(x => !a.Attributes.ContainsKey(x.Key)));
			AreEqual(a.Children.Count, b.Children.Count);
			for (int i = 0; i < a.Children.Count; i++)
			{
				AssertNodeEquals(a.Children[i], b.Children[i]);
			}
		}

		private static MarkupNode CreateNode(string type, Tuple<string, string, MarkupPropertyType>[] attributes, MarkupNode[] children = null)
		{
			var node = new MarkupNode();
			node.Type = type;
			foreach (var attribute in attributes)
			 	node.Attributes[attribute.Item1] = new MarkupProperty(attribute.Item1, attribute.Item3, attribute.Item2);
			foreach (var child in children ?? new MarkupNode[0])
				node.Children.Add(child);
			return node;
		}

		[TestMethod]
		public void TestSimpleFullDocument()
		{
			var doc = Parser.Parse(simpleFullDocument);

			AssertNodeEquals(simpleFullDocumentReference, doc.Root);
		}

		[TestMethod]
		public void TestNamedNodes()
		{
			var doc = Parser.Parse("root : Root { child : Child; }");
			IsNotNull(doc.Root);
			var root = doc.Root;

			IsNotNull(doc["root"]);
			IsNotNull(doc["child"]);

			AreEqual(doc.Root, doc["root"]);
			AreEqual(doc.Root.Children[0], doc["child"]);
		}
	}
}
