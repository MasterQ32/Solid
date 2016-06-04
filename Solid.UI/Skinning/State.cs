using Solid.Layout;
using System;

namespace Solid.UI.Skinning
{
	public sealed class State : SolidObject
	{
		internal Style style;

		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<State, IBrush>(
			nameof(Background));

		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<State, IBrush>(
			nameof(Foreground));


		public static readonly SolidProperty FontProperty = SolidProperty.Register<State, IFont>(
			nameof(Font));

		public static readonly SolidProperty FontSizeProperty = SolidProperty.Register<State, int>(
			nameof(FontSize), 16);

		public static readonly SolidProperty FontColorProperty = SolidProperty.Register<State, Color>(
			nameof(FontColor), new Color(0.0f, 0.0f, 0.0f));

		public static readonly SolidProperty MarginProperty = SolidProperty.Register<State, Thickness>(
			nameof(Margin));

		public static readonly SolidProperty PaddingProperty = SolidProperty.Register<State, Thickness>(
			nameof(Padding));

		public static readonly SolidProperty VerticalAlignmentProperty = SolidProperty.Register<State, VerticalAlignment>(
			nameof(VerticalAlignment));

		public static readonly SolidProperty HorizontalAlignmentProperty = SolidProperty.Register<State, HorizontalAlignment>(
			nameof(HorizontalAlignment));

		public static readonly SolidProperty SizeProperty = SolidProperty.Register<State, Size>(
			nameof(Size));


		private static void ExtendDefaultGen(SolidProperty property, Func<Style, object> getValue)
		{
			// Overrides the property defaults of some properties so the style can define the default value.
			var prevGen = property.Metadata.DefaultGenerator;
			property.Metadata.DefaultGenerator = (obj, prop) =>
			{
				if (obj is State)
				{
					var state = (State)obj;
					return getValue(state.style);
				}
				return prevGen(obj, prop);
			};
		}

		static State()
		{
			ExtendDefaultGen(FontProperty, (style) => style.Font);
			ExtendDefaultGen(FontColorProperty, (style) => style.FontColor);
			ExtendDefaultGen(MarginProperty, (style) => style.Margin);
			ExtendDefaultGen(PaddingProperty, (style) => style.Padding);
			ExtendDefaultGen(HorizontalAlignmentProperty, (style) => style.HorizontalAlignment);
			ExtendDefaultGen(VerticalAlignmentProperty, (style) => style.VerticalAlignment);
			ExtendDefaultGen(SizeProperty, (style) => style.Size);
		}



		public IBrush Background
		{
			get { return Get<IBrush>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		public IBrush Foreground
		{
			get { return Get<IBrush>(ForegroundProperty); }
			set { Set(ForegroundProperty, value); }
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