namespace Solid.UI.Skinning
{
	using System;
	using Solid.Markup;
	using Layout;
	public class Style : SolidObject, INamedNodeContainer
	{
		public static readonly SolidProperty FontProperty = SolidProperty.Register<Style, IFont>(nameof(Font));

		public static readonly SolidProperty FontSizeProperty = SolidProperty.Register<Style, int>(nameof(FontSize), 16);

		public static readonly SolidProperty FontColorProperty = SolidProperty.Register<Style, Color>(nameof(FontColor), new Color(0.0f, 0.0f, 0.0f));

		public static readonly SolidProperty MarginProperty = SolidProperty.Register<Style, Thickness>(nameof(Margin));

		public static readonly SolidProperty PaddingProperty = SolidProperty.Register<Style, Thickness>(nameof(Padding));

		public static readonly SolidProperty VerticalAlignmentProperty = SolidProperty.Register<Style, VerticalAlignment>(nameof(VerticalAlignment));

		public static readonly SolidProperty HorizontalAlignmentProperty = SolidProperty.Register<Style, HorizontalAlignment>(nameof(HorizontalAlignment));

		public static readonly SolidProperty SizeProperty = SolidProperty.Register<Style, Size>(nameof(Size));


		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<Style, IBrush>(nameof(Background));

		public static readonly SolidProperty BackgroundHoveredProperty = SolidProperty.Register<Style, IBrush>(nameof(BackgroundHovered));

		public IBrush Background
		{
			get { return Get<IBrush>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		public IBrush Foreground { get; set; }

		public IBrush BackgroundHovered
		{
			get { return Get<IBrush>(BackgroundHoveredProperty); }
			set { Set(BackgroundHoveredProperty, value); }
		}

		public IBrush BackgroundActive { get; set; }

		public IBrush BackgroundDisabled { get; set; }

		public IBrush ForegroundHovered { get; set; }

		public IBrush ForegroundActive { get; set; }

		public IBrush ForegroundDisabled { get; set; }

		/// <summary>
		/// Gets the brush for the given style key. If no brush is available for this key, the default brush will be returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="foreground"></param>
		/// <returns></returns>
		public IBrush GetBrush(StyleKey key, bool foreground)
		{
			if (foreground)
			{
				switch (key)
				{
					case StyleKey.Default: return this.Foreground;
					case StyleKey.Hovered: return this.ForegroundHovered ?? this.Foreground;
					case StyleKey.Active: return this.ForegroundActive ?? this.Foreground;
					case StyleKey.Disabled: return this.ForegroundDisabled ?? this.Foreground;
					default: throw new ArgumentException("The given key is not a valid style key.", nameof(key));
				}
			}
			else
			{
				switch (key)
				{
					case StyleKey.Default: return this.Background;
					case StyleKey.Hovered: return this.BackgroundHovered ?? this.Background;
					case StyleKey.Active: return this.BackgroundActive ?? this.Background;
					case StyleKey.Disabled: return this.BackgroundDisabled ?? this.Background;
					default: throw new ArgumentException("The given key is not a valid style key.", nameof(key));
				}
			}
		}

		void INamedNodeContainer.SetChildNodeName(object child, string name)
		{
			/*
			var brushDescriptor = (BrushDescriptor)child;
			switch (name)
			{
				case "Background":
				{
					this.Background = brushDescriptor.CreateBrush();
					break;
				}
				default: throw new NotSupportedException($"The {name} is not a valid style option.");
			}
			*/
		}

		public void SetChildNodeName(object child, string name)
		{
			throw new NotImplementedException();
		}

		public IFont Font
		{
			get { return Get<IFont>(FontProperty); }
			set { Set(FontProperty, value); }
		}

		public int FontSize
		{
			get { return Get<int>(FontSizeProperty); }
			set { Set(FontSizeProperty, value); }
		}

		public Color FontColor
		{
			get { return Get<Color>(FontColorProperty); }
			set { Set(FontColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the default vertical alignment.
		/// </summary>
		public VerticalAlignment VerticalAlignment
		{
			get { return Get<VerticalAlignment>(VerticalAlignmentProperty); }
			set { Set(VerticalAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the default horizontal alignment.
		/// </summary>
		public HorizontalAlignment HorizontalAlignment
		{
			get { return Get<HorizontalAlignment>(HorizontalAlignmentProperty); }
			set { Set(HorizontalAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the widgets default margins.
		/// </summary>
		public Thickness Margin
		{
			get { return Get<Thickness>(MarginProperty); }
			set { Set(MarginProperty, value); }
		}

		/// <summary>
		/// Gets or sets the widgets default paddings.
		/// </summary>
		public Thickness Padding
		{
			get { return Get<Thickness>(PaddingProperty); }
			set { Set(PaddingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the declared size of the widget.
		/// </summary>
		public Size Size
		{
			get { return Get<Size>(SizeProperty); }
			set { Set(SizeProperty, value); }
		}


		public float Width
		{
			get { return this.Size.Width; }
			set
			{
				var s = this.Size;
				s.Width = value;
				this.Size = s;
			}
		}

		public float Height
		{
			get { return this.Size.Height; }
			set
			{
				var s = this.Size;
				s.Height = value;
				this.Size = s;
			}
		}

	}
}