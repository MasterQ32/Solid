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
		private readonly State fallbackState;

		public Style()
		{
			this.fallbackState = new State() { style =this };
		}

		public State Default { get; set; }

		public State Hovered { get; set; }

		public State Active { get; set; }

		public State Disabled { get; set; }
		
		public State GetState(StyleKey key)
		{
			switch (key)
			{
				case StyleKey.Default: return this.Default ?? this.fallbackState; 
				case StyleKey.Hovered: return this.Hovered ?? this.Default ?? this.fallbackState; 
				case StyleKey.Active: return this.Active ?? this.Hovered ?? this.Default ?? this.fallbackState; 
				case StyleKey.Disabled: return this.Disabled ?? this.Default ?? this.fallbackState;
				default: throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the brush for the given style key. If no brush is available for this key, the default brush will be returned.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="foreground"></param>
		/// <returns></returns>
		public IBrush GetBrush(StyleKey key, bool foreground)
		{
			var state = this.GetState(key);
			if (state == null)
				return null;

			if (foreground)
				return state.Foreground;
			else
				return state.Background;
		}

		void INamedNodeContainer.SetChildNodeName(object child, string name)
		{
			var state = (State)child;
			state.style = this;
			switch(name)
			{
				case nameof(Default): this.Default = state; break;
				case nameof(Hovered): this.Hovered = state; break;
				case nameof(Active): this.Active = state; break;
				case nameof(Disabled): this.Disabled = state; break;
				default: throw new InvalidOperationException();
			}
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