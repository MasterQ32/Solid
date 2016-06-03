using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Solid
{
	/// <summary>
	/// Provides a bindable property system with property notifications.
	/// </summary>	
	public class SolidObject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public static SolidProperty BindingSourceProperty { get; } = SolidProperty.Register<SolidObject, object>(nameof(BindingSource), new SolidPropertyMetadata()
		{
			InheritFromHierarchy = true,
		});

		private Dictionary<SolidProperty, PropertyValue> properties = new Dictionary<SolidProperty, PropertyValue>();

		public SolidObject()
		{
			// Binding source is 
			{
				this.properties.Add(BindingSourceProperty, new BindingSourceValue(this));
			}
			foreach (var property in SolidProperty.GetProperties(this.GetType()))
			{
				if (property.Value == BindingSourceProperty)
					continue;
				this.properties.Add(property.Value, new PropertyValue(this, property.Value));
			}
		}

		public bool HasProperty(SolidProperty property)
		{
			return this.properties.ContainsKey(property);
		}

		/// <summary>
		/// Fires when a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Fires when a property is changing.
		/// </summary>
		/// <remarks>The value of the property will be the old value when this event is fired.</remarks>
		public event PropertyChangingEventHandler PropertyChanging;

		/// <summary>
		/// Sends a PropertyChanged event.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Sends a PropertyChanging event.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanging(string propertyName)
		{
			this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
		}

		/// <summary>
		/// Sets the value of a SolidProperty.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		protected internal void Set(SolidProperty property, object value)
		{
			var vh = GetValueHolder(property);
			vh.Value = value;
		}

		/// <summary>
		/// Gets the value of a SolidProperty.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected internal object Get(SolidProperty property)
		{
			var vh = GetValueHolder(property);
			return vh.Value;
		}

		internal void SetPropertyBinding(SolidProperty property, string targetPropertyName)
		{
			var vh = GetValueHolder(property);
			vh.Binding = targetPropertyName;
		}

		/// <summary>
		/// Gets an object that stores and verifies a property value.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		private PropertyValue GetValueHolder(SolidProperty property)
		{
			lock (this.properties)
			{
				if (this.properties.ContainsKey(property))
					return this.properties[property];
				throw new NotSupportedException("Late binding of properties is not supported.");
				/*
				var value = new PropertyValue(this, property);
				value.ValueChanged += Value_ValueChanged;
				this.properties.Add(property, value);
				return value;
				*/
			}
		}

		public object BindingSource
		{
			get { return Get(BindingSourceProperty); }
			set { Set(BindingSourceProperty, value); }
		}

		/// <summary>
		/// Gets a value of a SolidProperty.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property"></param>
		/// <returns></returns>
		protected T Get<T>(SolidProperty property) => (T)Get(property);

		private class PropertyValue
		{
			protected object propertyValue;
			private string binding;
			protected readonly SolidProperty solidProperty;
			protected readonly SolidObject target;
			protected readonly IHierarchicalObject hierarchy;

			public PropertyValue(SolidObject target, SolidProperty property)
			{
				this.solidProperty = property;
				this.target = target;
				this.target.PropertyChanging += Target_PropertyChanging;
				this.target.PropertyChanged += Target_PropertyChanged;
				this.hierarchy = this.target as IHierarchicalObject;
			}

			protected virtual void Target_PropertyChanging(object sender, PropertyChangingEventArgs e)
			{
				this.target.PropertyChanging -= Target_PropertyChanging;
				if ((e.PropertyName == nameof(SolidObject.BindingSource)) && (this.Binding != null))
					this.target.OnPropertyChanging(this.solidProperty.Name);
				this.target.PropertyChanging += Target_PropertyChanging;
			}

			protected virtual void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				this.target.PropertyChanged -= Target_PropertyChanged;
				if ((e.PropertyName == nameof(SolidObject.BindingSource)) && (this.Binding != null))
					this.target.OnPropertyChanged(this.solidProperty.Name);
				this.target.PropertyChanged += Target_PropertyChanged;
			}

			public bool HasValueAssigned { get; private set; }

			public void ResetValue()
			{
				this.HasValueAssigned = false;
			}

			protected virtual object GetBindingSource()
			{
				return this.target.BindingSource;
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)")]
			public virtual object Value
			{
				get
				{
					if (this.IsBindingApplicable.Read)
					{
						var bindingSource = this.GetBindingSource();
						var property = bindingSource.GetType().GetProperty(this.Binding);
						return property.GetValue(bindingSource);
					}

					if (this.HasValueAssigned)
						return this.propertyValue;
					if (this.solidProperty.Metadata.InheritFromHierarchy && (this.hierarchy != null))
					{
						var parent = this.hierarchy.Parent;
						while (parent != null)
						{
							var obj = (SolidObject)parent;
							if (obj.HasProperty(this.solidProperty))
							{
								return obj.Get(this.solidProperty);
							}
							parent = parent.Parent;
						}
					}
					return this.solidProperty.Metadata.DefaultGenerator(this.target, this.solidProperty);
				}
				set
				{
					if (this.solidProperty.PropertyType.IsAssignableFrom(value?.GetType()) == false)
						throw new InvalidOperationException($"Cannot assign {this.solidProperty.PropertyType.Name} from {value?.GetType()?.Name}.");

					if (this.IsBindingApplicable.Write)
					{
						var bindingSource = this.GetBindingSource();
						var property = bindingSource.GetType().GetProperty(this.Binding);
						property.SetValue(bindingSource, value);
						return;
					}

					this.HasValueAssigned = true;

					if (this.propertyValue == value)
						return;
					var changed =
						((this.propertyValue == null) && (value != null)) ||
						((this.propertyValue != null) && (value == null)) ||
						((this.propertyValue != null) && (this.propertyValue.Equals(value) == false)) ||
						((value != null) && (value.Equals(this.propertyValue) == false));
					if (changed == false)
						return;
					if (this.solidProperty.Metadata.EmitsChangedEvent)
						this.target.OnPropertyChanging(this.solidProperty.Name);
					this.propertyValue = value;
					if (this.solidProperty.Metadata.EmitsChangedEvent)
						this.target.OnPropertyChanged(this.solidProperty.Name);
				}
			}

			/// <summary>
			/// Gets or sets the property name of the binding.
			/// </summary>
			public string Binding
			{
				get { return this.binding; }
				set
				{
					if (this.binding == value)
						return;
					this.target.OnPropertyChanging(this.solidProperty.Name);
					this.binding = value;
					this.target.OnPropertyChanged(this.solidProperty.Name);
				}
			}

			/// <summary>
			/// Gets if the binding source of the object is set and has the bound property.
			/// </summary>
			/// <returns></returns>
			protected virtual BindingApplicability IsBindingApplicable
			{
				get
				{
					if (this.Binding == null)
						return BindingApplicability.None;
					if (this.target.BindingSource == null)
						return BindingApplicability.None;
					var property = this.target.BindingSource.GetType().GetProperty(this.Binding);
					if (property == null)
						return BindingApplicability.None;

					bool read = this.solidProperty.PropertyType.IsAssignableFrom(property.PropertyType);
					bool write = property.PropertyType.IsAssignableFrom(this.solidProperty.PropertyType);
					return new BindingApplicability(read, write);
				}
			}

			protected struct BindingApplicability
			{
				public static readonly BindingApplicability None = new BindingApplicability(false, false);

				public static readonly BindingApplicability All = new BindingApplicability(true, true);

				private readonly bool read, write;

				public BindingApplicability(bool read, bool write)
				{
					this.read = read;
					this.write = write;
				}

				public bool Read => this.read;

				public bool Write => this.write;
			}
		}

		private class BindingSourceValue : PropertyValue
		{
			public BindingSourceValue(SolidObject solidObject) :
				base(solidObject, SolidObject.BindingSourceProperty)
			{
				if(this.target is IHierarchicalObject)
				{
					var hier = (IHierarchicalObject)this.target;
					hier.ParentChanged += Hier_ParentChanged;
				}
			}

			private void Hier_ParentChanged(object sender, ParentChangedEventArgs e)
			{
				var old = (SolidObject)e.OldParent;
				var @new = (SolidObject)e.NewParent;
				if (old != null)
				{
					old.PropertyChanging -= Parent_PropertyChanging;
					old.PropertyChanged -= Parent_PropertyChanged;
				}
				if (@new != null)
				{
					@new.PropertyChanging += Parent_PropertyChanging;
					@new.PropertyChanged += Parent_PropertyChanged;
				}
			}

			private void Parent_PropertyChanging(object sender, PropertyChangingEventArgs e)
			{
				this.target.OnPropertyChanging(this.solidProperty.Name);
			}

			private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				this.target.OnPropertyChanged(this.solidProperty.Name);
			}

			protected override void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				// base.Target_PropertyChanged(sender, e);
			}

			protected override void Target_PropertyChanging(object sender, PropertyChangingEventArgs e)
			{
				// base.Target_PropertyChanging(sender, e);
			}

			protected override object GetBindingSource()
			{
				var t = this.target;
				var h = t as IHierarchicalObject;
				if(h != null)
				{
					if (h.Parent is SolidObject)
						return ((SolidObject)h.Parent).BindingSource;
					return null;
				}
				return null;
			}

			protected override BindingApplicability IsBindingApplicable
			{
				get
				{
					if (this.Binding == null)
						return BindingApplicability.None;
					var src = this.GetBindingSource();
					if (src == null)
						return BindingApplicability.None;
					var property = src.GetType().GetProperty(this.Binding);
					if (property == null)
						return BindingApplicability.None;
					return BindingApplicability.All;
				}
			}
		}
	}
}
