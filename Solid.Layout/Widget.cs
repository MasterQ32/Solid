using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;


namespace Solid.Layout
{
	public class Widget : SolidObject, IHierarchicalObject
	{
		private readonly WidgetChildCollection children;
		private Widget parent = null;

		Point actualPosition;
		Size actualSize;

		public Widget()
		{
			this.children = new WidgetChildCollection(this);
		}

		public Widget Parent
		{
			get { return this.parent; }
			set
			{
				if (this.parent == value)
					return;
				this.parent?.Children.Remove(this);
				if (value != null)
				{
					// this.parent = value; <- implicit in next line
					value.Children.Add(this);
				}
				else
				{
					this.parent = null;
				}
			}
		}

		public IList<Widget> Children => this.children;

		protected internal virtual void AfterLayout()
		{
			// Just do nothing
		}

		protected virtual void OnLayout()
		{
			foreach (var child in this.Children)
			{
				child.ApplyAlignment(this.ClientPosition, this.ClientSize);
				child.SetupLayout();
			}
		}

		/// <summary>
		/// Returns the size the object wants to have.
		/// Override this method to create custom sized widgets.
		/// </summary>
		public virtual Size SizeHint
		{
			get
			{
				Size size = this.DeclaredSize;
				bool modX = (size.Width == 0);
				bool modY = (size.Height == 0);
				foreach (var child in this.Children)
				{
					var csize = child.WidgetSize;
					if (modX) size.Width = Max(size.Width, csize.Width);
					if (modY) size.Height = Max(size.Height, csize.Height);
				}
				if (modX) size.Width += this.Padding.Horizontal;
				if (modY) size.Height += this.Padding.Vertical;
				return size;
			}
		}

		/// <summary>
		/// Gets the position of the child area in local space.
		/// </summary>
		public Point ClientPosition => new Point(
			this.Padding.Left,
			this.Padding.Top);

		/// <summary>
		/// Gets the size of the child area.
		/// </summary>
		public Size ClientSize => new Size(
			this.Size.Width - this.Padding.Left - this.Padding.Right,
			this.Size.Height - this.Padding.Top - this.Padding.Bottom);


		/// <summary>
		/// Gets the size hint with an applied margin.
		/// </summary>
		public Size WidgetSize
		{
			get
			{
				Size size = this.SizeHint;
				size.Width += this.Margin.Left + this.Margin.Right;
				size.Height += this.Margin.Top + this.Margin.Bottom;
				return size;
			}
		}

		internal void SetupLayout()
		{
			if (this.IsVisibleInLayout == false)
			{
				return;
			}
			this.OnLayout();
			foreach (var child in this.Children)
			{
				child.AfterLayout();
			}
		}

		/// <summary>
		///  Gets the layouted local position.
		/// </summary>
		/// <remarks>This value is only valid after at least on @ref solid::layout::Layout::layout() layout() call has been made.</remarks>
		public Point LocalPosition
		{
			get { return this.actualPosition; }
		}

		/// <summary>
		/// Gets the layouted global position.
		/// </summary>
		/// <remarks>This value is only valid after at least on @ref solid::layout::Layout::layout() layout() call has been made.</remarks>
		public Point Position
		{
			get
			{
				if (this.Parent != null)
				{
					return this.Parent.Position + this.LocalPosition;
				}
				else {
					return this.LocalPosition;
				}
			}
		}

		/// <summary>
		/// Gets the layouted size.
		/// </summary>
		/// <remarks>This value is only valid after at least on @ref solid::layout::Layout::layout() layout() call has been made.</remarks>
		public Size Size
		{
			get { return this.actualSize; }
		}

		/// <summary>
		/// Gets if the element is visible in the layout.
		/// </summary>
		/// If the area of the element is 0, the element will be
		/// invisible. This property returns if the size of the element
		/// is valid and the element needs to be layouted.
		public bool IsVisibleInLayout
		{
			get
			{
				bool isVisible = (this.actualSize.Width > 0) && (this.actualSize.Height > 0);
				if (isVisible && (this.Parent != null))
				{
					isVisible &= this.Parent.IsVisibleInLayout;
				}
				return isVisible;
			}
		}


		/// <summary>
		/// Sets the actual position of the element considering the elements alignment.
		/// </summary>
		/// This method moves and resizes this element so that it fits
		/// into the given rectangle.The vertical and horizontal
		/// alignment determine how the element is located in the given
		/// area.
		/// 
		/// <param name="offset">Position of the alignment area.</param>
		/// <param name="area">Size of the alignment area.</param>
		public void ApplyAlignment(Point offset, Size area)
		{
			Point actualPosition = new Point(0, 0);
			Size actualSize = this.WidgetSize;

			if (this.CanOverlap == false)
			{
				// Cap X
				if (actualSize.Width > area.Width)
				{
					actualSize.Width = area.Width;
				}
				// Cap Y
				if (actualSize.Height > area.Height)
				{
					actualSize.Height = area.Height;
				}
			}

			switch (this.HorizontalAlignment)
			{
				case HorizontalAlignment.Stretch:
				{
					actualPosition.X = offset.X;
					actualSize.Width = area.Width;
					break;
				}
				case HorizontalAlignment.Left:
				{
					actualPosition.X = offset.X;
					break;
				}
				case HorizontalAlignment.Right:
				{
					actualPosition.X = offset.X + area.Width - actualSize.Width;
					break;
				}
				case HorizontalAlignment.Middle:
				{
					actualPosition.X = offset.X + (area.Width - actualSize.Width) / 2;
					break;
				}
				default: throw new NotSupportedException();
			}

			switch (this.VerticalAlignment)
			{
				case VerticalAlignment.Stretch:
				{
					actualPosition.Y = offset.Y;
					actualSize.Height = area.Height;
					break;
				}
				case VerticalAlignment.Top:
				{
					actualPosition.Y = offset.Y;
					break;
				}
				case VerticalAlignment.Bottom:
				{
					actualPosition.Y = offset.Y + area.Height - actualSize.Height;
					break;
				}
				case VerticalAlignment.Middle:
				{
					actualPosition.Y = offset.Y + (area.Height - actualSize.Height) / 2;
					break;
				}
				default: throw new NotSupportedException();
			}

			Thickness margin = this.Margin;
			actualPosition.X += margin.Left;
			actualPosition.Y += margin.Top;

			actualSize.Width -= margin.Left + margin.Right;
			actualSize.Height -= margin.Top + margin.Bottom;

			if (this.CanOverlap == false)
			{
				// Cap X
				if (actualSize.Width > area.Width)
				{
					actualSize.Width = area.Width;
				}
				// Cap Y
				if (actualSize.Height > area.Height)
				{
					actualSize.Height = area.Height;
				}
			}

			if (this.CanOverlap == false)
			{
				// Cap X
				if (actualPosition.X < 0)
				{
					actualPosition.X = 0;
				}
				// Cap Y
				if (actualPosition.Y < 0)
				{
					actualPosition.Y = 0;
				}
			}

			// Clamp space to zero
			if (actualSize.Width < 0)
			{
				actualSize.Width = 0;
			}
			if (actualSize.Height < 0)
			{
				actualSize.Height = 0;
			}

			this.actualPosition = actualPosition;
			this.actualSize = actualSize;
		}

		public float Width
		{
			get { return this.DeclaredSize.Width; }
			set
			{
				var s = this.DeclaredSize;
				s.Width = value;
				this.DeclaredSize = s;
			}
		}

		public float Height
		{
			get { return this.DeclaredSize.Height; }
			set
			{
				var s = this.DeclaredSize;
				s.Height = value;
				this.DeclaredSize = s;
			}
		}


		#region Solid Properties

		public static readonly SolidProperty DeclaredSizeProperty = SolidProperty.Register<Widget, Size>(nameof(DeclaredSize));

		public static readonly SolidProperty MarginProperty = SolidProperty.Register<Widget, Thickness>(nameof(Margin));

		public static readonly SolidProperty PaddingProperty = SolidProperty.Register<Widget, Thickness>(nameof(Padding));

		public static readonly SolidProperty VerticalAlignmentProperty = SolidProperty.Register<Widget, VerticalAlignment>(nameof(VerticalAlignment));

		public static readonly SolidProperty HorizontalAlignmentProperty = SolidProperty.Register<Widget, HorizontalAlignment>(nameof(HorizontalAlignment));

		public static readonly SolidProperty CanOverlapProperty = SolidProperty.Register<Widget, bool>(nameof(CanOverlap));

		/// <summary>
		/// Get or sets if the widget is allowed to overlap its determined size or not.
		/// </summary>
		public bool CanOverlap
		{
			get { return Get<bool>(CanOverlapProperty); }
			set { Set(CanOverlapProperty, value); }
		}

		/// <summary>
		/// Gets or sets the vertical alignment.
		/// </summary>
		public VerticalAlignment VerticalAlignment
		{
			get { return Get<VerticalAlignment>(VerticalAlignmentProperty); }
			set { Set(VerticalAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the horizontal alignment.
		/// </summary>
		public HorizontalAlignment HorizontalAlignment
		{
			get { return Get<HorizontalAlignment>(HorizontalAlignmentProperty); }
			set { Set(HorizontalAlignmentProperty, value); }
		}

		/// <summary>
		/// Gets or sets the declared size of the widget.
		/// </summary>
		public Size DeclaredSize
		{
			get { return Get<Size>(DeclaredSizeProperty); }
			set { Set(DeclaredSizeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the widgets margins.
		/// </summary>
		public Thickness Margin
		{
			get { return Get<Thickness>(MarginProperty); }
			set { Set(MarginProperty, value); }
		}

		/// <summary>
		/// Gets or sets the widgets paddings.
		/// </summary>
		public Thickness Padding
		{
			get { return Get<Thickness>(PaddingProperty); }
			set { Set(PaddingProperty, value); }
		}

		#endregion

		#region WidgetChildCollection

		private class WidgetChildCollection : IList<Widget>
		{
			private readonly List<Widget> children = new List<Widget>();
			private Widget widget;

			public WidgetChildCollection(Widget widget)
			{
				this.widget = widget;
			}

			private bool IsValidChild(Widget child)
			{
				var it = this.widget;
				while (it != null)
				{
					if (it == child)
						return false;
					it = it.parent;
				}
				return true;
			}

			private void CheckValidChild(Widget child)
			{
				if (child == null)
					throw new ArgumentNullException();
				if (IsValidChild(child) == false)
					throw new InvalidOperationException("Cannot assign value, a circular reference would be created.");
			}

			public Widget this[int index]
			{
				get { return children[index]; }

				set
				{
					if (children[index] == value)
						return;
					CheckValidChild(value);

					children[index].parent = null;
					children[index] = value;
					children[index].parent = this.widget;
				}
			}

			public int Count => children.Count;

			public bool IsReadOnly => ((IList<Widget>)children).IsReadOnly;

			public void Add(Widget item)
			{
				CheckValidChild(item);
				if (item.parent == this.widget)
					throw new InvalidOperationException("The widget is already a child.");

				if (item.parent != null)
					item.parent.Children.Remove(item);
				item.parent = this.widget;
				children.Add(item);
			}

			public void Clear()
			{
				foreach (var widget in this.children)
					widget.parent = null;
				children.Clear();
			}

			public bool Contains(Widget item) => children.Contains(item);

			public void CopyTo(Widget[] array, int arrayIndex) => children.CopyTo(array, arrayIndex);

			public IEnumerator<Widget> GetEnumerator() => children.GetEnumerator();

			public int IndexOf(Widget item) => children.IndexOf(item);

			public void Insert(int index, Widget item)
			{
				if (item.parent == this.widget)
					throw new InvalidOperationException("The widget is already a child.");
				item.parent?.children?.Remove(item);
				item.parent = this.widget;
				children.Insert(index, item);
			}

			public bool Remove(Widget item)
			{
				if (item.parent != this.widget)
					throw new InvalidOperationException("The widget is no child.");
				item.parent = null;
				return children.Remove(item);
			}

			public void RemoveAt(int index)
			{
				var child = this.children[index];
				child.parent = null;
				children.RemoveAt(index);
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		#endregion

		#region IHierarchicalObject

		IHierarchicalObject IHierarchicalObject.Parent => this.parent;

		IEnumerable<IHierarchicalObject> IHierarchicalObject.Children => this.children;

		#endregion
	}
}
