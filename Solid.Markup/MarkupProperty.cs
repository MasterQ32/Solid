using System;

namespace Solid.Markup
{
	public class MarkupProperty : IEquatable<MarkupProperty>
	{
		public MarkupProperty(string name, MarkupPropertyType type, string value)
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;
		}

		public string Name { get; private set; }

		public string Value { get; set; }

		public MarkupPropertyType Type { get; private set; }

		public bool IsBinding => (this.Type == MarkupPropertyType.Binding);

		public bool Equals(MarkupProperty other)
		{
			if (other == null)
				return false;
			return
				(other.Name == this.Name) &&
				(other.Type == this.Type) &&
				(other.Value == this.Value);
		}

		public override bool Equals(object obj) => Equals(obj as MarkupProperty);

		public override int GetHashCode() => Name.GetHashCode() ^ Type.GetHashCode() ^ Value.GetHashCode();

		public override string ToString()
		{
			switch (Type)
			{
				case MarkupPropertyType.Binding:
					return $"{Name} = [Value]";
				case MarkupPropertyType.String:
					return $"{Name} = \"{Value}\"";
				default:
					return $"{Name} = {Value}";
			}
        }

		public static bool operator ==(MarkupProperty lhs, MarkupProperty rhs) => ((object)lhs != null) ? lhs.Equals(rhs) : ((object)rhs == null);
		public static bool operator !=(MarkupProperty lhs, MarkupProperty rhs) => !(lhs == rhs);
	}

	public enum MarkupPropertyType
	{
		String,
		Enumeration,
		Number,
		Binding,
	}
}