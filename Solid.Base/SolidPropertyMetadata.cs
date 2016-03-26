namespace Solid
{
	public sealed class SolidPropertyMetadata
	{
		/// <summary>
		/// Gets or sets the properties default value.
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		/// Gets or sets if the property is accessable from external scripts or files.
		/// </summary>
		public bool IsExported { get; set; } = true;
	}
}