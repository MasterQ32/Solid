using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Solid.Markup
{
	public sealed class Parser
	{
		private readonly MarkupDocument document = new MarkupDocument();

		public MarkupDocument Document => this.document;

		private Parser()
		{

		}

		public static MarkupDocument Load(string fileName)
		{
			using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
			{
				return Parse(fs);
			}
		}

		public static MarkupDocument Parse(string source) => Parse(new StringReader(source));

		public static MarkupDocument Parse(Stream source) => Parse(source, Encoding.UTF8);

		public static MarkupDocument Parse(Stream source, Encoding encoding) => Parse(new StreamReader(source, encoding));

		public static MarkupDocument Parse(TextReader reader)
		{
			var instance = new Parser();

			instance.Read(reader);

			return instance.Document;
		}


		public void Read(TextReader reader)
		{
			this.document.Root = ReadNode(reader);
		}

		private MarkupNode ReadNode(TextReader reader)
		{
			SkipThroughWhitespace(reader);
			string nodeName = null;
			string nodeType = ReadNodeIdentifier(reader);
			SkipThroughWhitespace(reader);

			if (reader.Peek() == ':')
			{
				if (reader.Read() != ':')
					throw new InvalidDataException();
				SkipThroughWhitespace(reader);

				nodeName = nodeType;
				nodeType = ReadNodeIdentifier(reader);

				SkipThroughWhitespace(reader);
			}

			var node = new MarkupNode()
			{
				Type = nodeType,
				ID = nodeName,
			};

			if (nodeName != null)
			{
				if (this.document[nodeName] != null)
					throw new InvalidDataException("Duplicated node name.");
				this.document[nodeName] = node;
			}

			var c = reader.Peek();
			if (c == '(')
			{
				// Read attribute list
				ReadNodeAttributes(node, reader);
				SkipThroughWhitespace(reader);
				c = reader.Peek();
			}

			if (c == ';')
			{
				// consume the 
				reader.Read();
				return node;
			}
			else if (c == '{')
			{
				// read children list.
				ReadNodeChildren(node, reader);

				return node;
			}
			else
			{
				throw new InvalidDataException("Expected '{' or ';'");
			}
		}

		private void ReadNodeChildren(MarkupNode node, TextReader reader)
		{
			SkipThroughWhitespace(reader);
			if (reader.Read() != '{')
				throw new InvalidDataException("Expected a child list.");
			SkipThroughWhitespace(reader);

			while (true)
			{
				if (reader.Peek() == '}')
					break;

				var child = ReadNode(reader);
				node.Children.Add(child);

				SkipThroughWhitespace(reader);
			}
			if (reader.Read() != '}')
				throw new InvalidDataException("Expected '}'");
		}

		private void ReadNodeAttributes(MarkupNode node, TextReader reader)
		{
			SkipThroughWhitespace(reader);
			if (reader.Read() != '(')
				throw new InvalidDataException("Expected an argument list.");

			while (true)
			{
				SkipThroughWhitespace(reader);

				if (reader.Peek() == ')')
					return;

				var id = ReadAttributeIdentifier(reader);

				SkipThroughWhitespace(reader);

				if (reader.Read() != '=')
					throw new InvalidDataException("Expected '='");

				SkipThroughWhitespace(reader);

				MarkupPropertyType type;
				var value = ReadAttributeValue(reader, out type);

				if (node.Attributes.ContainsKey(id))
					throw new InvalidDataException("Duplicated attribute declaration.");

				node.Attributes.Add(id, new MarkupProperty(id, type, value));

				SkipThroughWhitespace(reader);

				var r = reader.Peek();
				if (r == -1)
					throw new EndOfStreamException();

				if (r == ')')
					break;
				if (reader.Read() != ',')
					throw new InvalidDataException("Expected ','");
			}
			if (reader.Read() != ')')
				throw new InvalidDataException("Expected ')'");
		}

		private string ReadAttributeValue(TextReader reader, out MarkupPropertyType type)
		{
			var r = reader.Peek();
			if (r == -1)
				throw new EndOfStreamException();

			if (char.IsDigit((char)r) || (r == '-'))
			{
				type = MarkupPropertyType.Number;
				return ReadNumber(reader);
			}
			else if (r == '\"')
			{
				type = MarkupPropertyType.String;
				return ReadString(reader);
			}
			else if (r == '[')
			{
				if (reader.Read() != '[')
					throw new InvalidDataException("Expected '['");
				type = MarkupPropertyType.Binding;
				var result = ReadAttributeIdentifier(reader);
				if (reader.Read() != ']')
					throw new InvalidDataException("Expected ']'");
				return result;
			}
			else {
				type = MarkupPropertyType.Enumeration;
				return ReadAttributeIdentifier(reader);
			}
		}

		private string ReadString(TextReader reader)
		{
			if (reader.Read() != '\"')
				throw new InvalidDataException("Expected '\"'");

			var result = new StringBuilder();

			while (true)
			{
				var r = reader.Peek();
				if (r == -1)
					throw new EndOfStreamException();
				var c = (char)r;
				if (c == '\"')
					break;
				c = (char)reader.Read();
				switch (c)
				{
					case '\\':
					{
						r = reader.Read();
						if (r == -1)
							throw new EndOfStreamException();
						switch (r)
						{
							case 'r': result.Append("\r"); break;
							case 'n': result.Append("\n"); break;
							case 't': result.Append("\t"); break;
							case '\'': result.Append("\'"); break;
							case '\"': result.Append("\""); break;
							default: throw new InvalidDataException($"Unknown escape sequence: '\\{(char)r}.'");
						}
						break;
					}
					default:
					{
						result.Append(c);
						break;
					}
				}
			}

			if (reader.Read() != '\"')
				throw new InvalidDataException("Expected '\"'");

			return result.ToString();
		}

		private string ReadNumber(TextReader reader)
		{
			var result = new StringBuilder();

			var r = reader.Peek();
			if (r == -1)
				throw new EndOfStreamException();
			if (r == '-')
			{
				reader.Read();
				result.Append("-");
				r = reader.Peek();
				if (r == -1)
					throw new EndOfStreamException();
			}

			if (char.IsDigit((char)r) == false)
				throw new InvalidDataException("Expected a digit.");

			while (true)
			{
				r = reader.Peek();
				if (r == '.')
					break;
				if (char.IsDigit((char)r) == false)
					return result.ToString();
				result.Append((char)reader.Read());
			}
			if (reader.Read() != '.')
				throw new InvalidDataException("Expected '.'");
			result.Append(".");

			while (true)
			{
				r = reader.Peek();
				if (char.IsDigit((char)r) == false)
					return result.ToString();
				result.Append((char)reader.Read());
			}
		}

		private string ReadNodeIdentifier(TextReader reader) => ReadWithPredicate(reader, (c) => char.IsLetter(c) || (c == '.'));

		private string ReadAttributeIdentifier(TextReader reader) => ReadWithPredicate(reader, (c) => char.IsLetter(c) || (c == '.') || (c == '_'));

		private string ReadWithPredicate(TextReader reader, Predicate<char> predicate)
		{
			var result = new StringBuilder();

			while (true)
			{
				var r = reader.Peek();
				if (r == -1)
					break;
				var c = (char)r;

				if (predicate(c))
					result.Append((char)reader.Read());
				else
					break;
			}
			if (result.Length == 0)
				throw new InvalidDataException("Expected an identifier.");

			return result.ToString();
		}

		private void SkipThroughWhitespace(TextReader reader)
		{
			while (true)
			{
				var r = reader.Peek();
				if (r == -1)
					return;
				var c = (char)r;
				if (char.IsWhiteSpace(c))
					reader.Read();
				else
					return;
			}
		}
	}
}
