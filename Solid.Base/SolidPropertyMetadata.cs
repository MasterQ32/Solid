namespace Solid
{
	public delegate object SolidPropertyDefaultFactory(SolidObject target, SolidProperty property);

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

		/// <summary>
		/// Gets or sets if the property is inherited from parenting objects.
		/// </summary>
		/// <remarks>The object must implement IHierarchicalObject to declare the hierarchy.</remarks>
		public bool InheritFromHierarchy { get; set; } = false;

		/// <summary>
		/// Gets or sets the default value generator.
		/// </summary>
		/// <remarks>The default generator returns property.Metadata.DefaultValue. If the generator is changed, SolidPropertyMetadata.DefaultValue is ignored.</remarks>
		public SolidPropertyDefaultFactory DefaultGenerator { get; set; } = (obj, prop) => prop.Metadata.DefaultValue;
	}
}