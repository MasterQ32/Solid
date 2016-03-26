namespace Solid
{
	/// <summary>
	/// Defines metadata for properties.
	/// </summary>
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

		/// <summary>
		/// Gets or sets if the property should emit a PropertyChanged event when the value has changed.
		/// </summary>
		public bool EmitsChangedEvent { get; set; } = true;
	}
}