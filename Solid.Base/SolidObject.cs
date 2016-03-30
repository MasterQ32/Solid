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
	public class SolidObject : INotifyPropertyChanged
	{
		public static readonly SolidProperty BindingSourceProperty = SolidProperty.Register<SolidObject, object>(nameof(BindingSource), new SolidPropertyMetadata()
		{
			InheritFromHierarchy = true,
		});
		
		private Dictionary<SolidProperty, PropertyValue> properties = new Dictionary<SolidProperty, PropertyValue>();

		public SolidObject()
		{
			// Binding source is 
			{
				var value = new BindingSourceValue(this);
				value.ValueChanged += Value_ValueChanged;
				this.properties.Add(BindingSourceProperty, value);
			}
			foreach (var property in SolidProperty.GetProperties(this.GetType()))
			{
				if (property.Value == BindingSourceProperty)
					continue;
				var value = new PropertyValue(this, property.Value);
				value.ValueChanged += Value_ValueChanged;
				this.properties.Add(property.Value, value);
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
		/// Sends a PropertyChanged event.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Sets the value of a SolidProperty.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		protected internal void Set(SolidProperty property, object value)
		{
			GetValueHolder(property).Value = value;
		}

		/// <summary>
		/// Gets the value of a SolidProperty.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected internal object Get(SolidProperty property)
		{
			return GetValueHolder(property).Value;
		}

		internal void SetPropertyBinding(SolidProperty property, string targetPropertyName)
		{
			GetValueHolder(property).Binding = targetPropertyName;
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

		private void Value_ValueChanged(object sender, EventArgs e)
		{
			var value = (PropertyValue)sender;
			this.OnPropertyChanged(value.Property.Name);
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
			protected object value;
			protected readonly SolidProperty property;
			protected readonly SolidObject target;
			protected readonly IHierarchicalObject hierarchy;

			public event EventHandler ValueChanged;

			public PropertyValue(SolidObject target, SolidProperty property)
			{
				this.property = property;
				this.target = target;
				this.hierarchy = this.target as IHierarchicalObject;
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
						return this.value;
					if (this.property.Metadata.InheritFromHierarchy && (this.hierarchy != null))
					{
						var parent = this.hierarchy.Parent;
						while (parent != null)
						{
							var obj = (SolidObject)parent;
							if (obj.HasProperty(this.property))
							{
								return obj.Get(this.property);
							}
							parent = parent.Parent;
						}
					}
					return this.property.Metadata.DefaultGenerator(this.target, this.property);
				}
				set
				{
					if (this.property.PropertyType.IsAssignableFrom(value?.GetType()) == false)
						throw new InvalidOperationException($"Cannot assign {this.property.PropertyType.Name} from {value?.GetType()?.Name}.");

					if (this.IsBindingApplicable.Write)
					{
						var bindingSource = this.GetBindingSource();
						var property = bindingSource.GetType().GetProperty(this.Binding);
						property.SetValue(bindingSource, value);
						return;
					}

					this.HasValueAssigned = true;

					if (this.value == value)
						return;
					var changed =
						((this.value == null) && (value != null)) ||
						((this.value != null) && (value == null)) ||
						((this.value != null) && (this.value.Equals(value) == false)) ||
						((value != null) && (value.Equals(this.value) == false));
					this.value = value;
					if (changed && this.property.Metadata.EmitsChangedEvent)
						this.ValueChanged?.Invoke(this, EventArgs.Empty);
				}
			}

			public SolidProperty Property => this.property;

			/// <summary>
			/// Gets or sets the property name of the binding.
			/// </summary>
			public string Binding { get; set; }

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

					bool read = this.property.PropertyType.IsAssignableFrom(property.PropertyType);
					bool write = property.PropertyType.IsAssignableFrom(this.property.PropertyType);
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
				this.ValueChanged += (s, e) => System.Diagnostics.Debugger.Break();
			}

			protected override object GetBindingSource()
			{
				var t = this.target;
				if (t is IHierarchicalObject)
				{
					var h = (IHierarchicalObject)t;
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
