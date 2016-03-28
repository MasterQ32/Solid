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
		private Dictionary<SolidProperty, PropertyValue> properties = new Dictionary<SolidProperty, PropertyValue>();

		public SolidObject()
		{
			foreach (var property in SolidProperty.GetProperties(this.GetType()))
			{
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

		/// <summary>
		/// Gets a value of a SolidProperty.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property"></param>
		/// <returns></returns>
		protected T Get<T>(SolidProperty property) => (T)Get(property);

		private class PropertyValue
		{
			private object value;
			private readonly SolidProperty property;
			private readonly SolidObject target;
			private readonly IHierarchicalObject hierarchy;

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

			public object Value
			{
				get
				{
					if(this.HasValueAssigned)
						return this.value;
					if(this.property.Metadata.InheritFromHierarchy && (this.hierarchy != null))
					{
						var parent = this.hierarchy.Parent;
						while(parent != null)
						{
							var obj = (SolidObject)parent;
							if(obj.HasProperty(this.property))
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
						this.ValueChanged.Invoke(this, EventArgs.Empty);
				}
			}

			public SolidProperty Property => this.property;
		}
	}
}
