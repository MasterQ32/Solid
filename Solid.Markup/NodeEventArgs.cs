using System;

namespace Solid.Markup
{
	public sealed class NodeEventArgs : EventArgs
	{
		public object Node { get; set; }

		public string Name { get; set; }
	}
}