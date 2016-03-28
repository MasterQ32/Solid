namespace Solid.UI.Skinning
{
	using System;
	using Solid.Markup;

	public class Style : SolidObject, INamedNodeContainer
	{
		public Brush Default { get; set; }

		public Brush Hovered { get; set; }

		public Brush Active { get; set; }

		public Brush Disabled { get; set; }

		/// <summary>
		/// Gets the brush for the given style key. If no brush is available for this key, the default brush will be returned.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Brush GetBrush(StyleKey key)
		{
			switch(key)
			{
				case StyleKey.Default: return this.Default;
				case StyleKey.Hovered: return this.Hovered ?? this.Default;
				case StyleKey.Active: return this.Active ?? this.Default;
				case StyleKey.Disabled: return this.Disabled ?? this.Default;
				default: throw new ArgumentException("The given key is not a valid style key.", nameof(key));
			}
		}

		void INamedNodeContainer.SetChildNodeName(object child, string name)
		{
			var brushDescriptor = (BrushDescriptor)child;
			switch (name)
			{
				case nameof(Default):
				{
					this.Default = brushDescriptor.CreateBrush();
					break;
				}
				case nameof(Hovered):
				{
					this.Hovered = brushDescriptor.CreateBrush();
					break;
				}
				case nameof(Active):
				{
					this.Active = brushDescriptor.CreateBrush();
					break;
				}
				case nameof(Disabled):
				{
					this.Disabled = brushDescriptor.CreateBrush();
					break;
				}
				default: throw new NotSupportedException($"The {name} is not a valid style option.");
			}
		}

		public void SetChildNodeName(object child, string name)
		{
			throw new NotImplementedException();
		}
	}

	public enum StyleKey
	{
		Default,
		Hovered,
		Active,
		Disabled,
	}
}