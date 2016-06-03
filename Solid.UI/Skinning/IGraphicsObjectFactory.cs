namespace Solid.UI.Skinning
{
	public interface IGraphicsObjectFactory
	{
		IBrush CreateBrush(string spec);

		IFont CreateFont(string spec);
	}
}