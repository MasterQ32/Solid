namespace Solid.Markup
{
	public interface INamedNodeContainer<T> 
		where T : class
	{
		void SetChildNodeName(T child, string name);
	}
}