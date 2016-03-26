namespace Solid.Markup
{
	public interface IMarkupDocument<T>
		where T : class
	{
		/// <summary>
		/// This method sets the root node of the document.
		/// </summary>
		/// <param name="root"></param>
		void SetRoot(T root);

		/// <summary>
		/// This method sets the name of a given node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="name"></param>
		void SetNodeName(T node, string name);

		/// <summary>
		/// This method handles the initialization of created nodes.
		/// </summary>
		/// <param name="node"></param>
		void NotifyCreateNode(T node);
	}
}