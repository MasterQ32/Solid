namespace Solid.Layout
{
	using Markup;
	using System;
	using System.Collections.Specialized;
	using ItemsCollection = System.Collections.ObjectModel.ObservableCollection<object>;

	public abstract class Panel : Widget
	{
		public static readonly SolidProperty ItemsProperty = SolidProperty.Register<Panel, ItemsCollection>(nameof(Items));
		public static readonly SolidProperty TemplateProperty = SolidProperty.Register<Panel, MarkupDocument>(nameof(Template));

		protected Panel()
		{
			this.PropertyChanging += Panel_PropertyChanging;
			this.PropertyChanged += Panel_PropertyChanged;
		}

		private void Panel_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Items):
				{
					if (this.Items == null)
						break;
					this.Items.CollectionChanged -= Items_CollectionChanged;
					break;
				}
			}
		}

		private void Panel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Console.WriteLine("{0} changed!", e.PropertyName);
			switch (e.PropertyName)
			{
				case nameof(Items):
				{
					if (this.Items != null)
					{
						this.Items.CollectionChanged += Items_CollectionChanged;
					}
					this.ResetChildren(this.Items != null);
					break;
				}
			}
		}

		Widget CreateWidgetForChild(object obj)
		{
			var child = this.document.TemplateMapper.Map(this.document, this.Template.Root);
			child.BindingSource = obj;
			return child;
		}

		private void ResetChildren(bool lockAfterwards)
		{
			this.childrenAreLocked = false;
			this.Children.Clear();
			if (this.Items != null)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Children.Add(CreateWidgetForChild(this.Items[i]));
				}
			}
			this.childrenAreLocked = lockAfterwards;
		}

		private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.childrenAreLocked = false;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						this.Children.Insert(e.NewStartingIndex + i, CreateWidgetForChild(e.NewItems[i]));
					}
					break;
				}
				case NotifyCollectionChangedAction.Remove:
				{
					for (int i = 0; i < e.OldItems.Count; i++)
					{
						this.Children.RemoveAt(e.OldStartingIndex);
					}
					break;
				}
				case NotifyCollectionChangedAction.Replace:
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						this.Children[e.NewStartingIndex + i].BindingSource = e.NewItems[i];
					}
					break;
				}
				case NotifyCollectionChangedAction.Move:
				{
					throw new NotSupportedException();
				}
				case NotifyCollectionChangedAction.Reset:
				{
					this.ResetChildren(true);
					break;
				}
			}
			this.childrenAreLocked = true;
		}

		public ItemsCollection Items
		{
			get { return Get<ItemsCollection>(ItemsProperty); }
			set { Set(ItemsProperty, value); }
		}

		public MarkupDocument Template
		{
			get { return Get<MarkupDocument>(TemplateProperty); }
			set { Set(TemplateProperty, value); }
		}
	}
}