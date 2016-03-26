using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Solid
{
	public class SolidObject : INotifyPropertyChanged
	{
		private Dictionary<SolidProperty, PropertyValue> properties = new Dictionary<SolidProperty, PropertyValue>();

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected internal void Set(SolidProperty property, object value)
		{
			GetValueHolder(property).Value = value;
		}

		protected internal object Get(SolidProperty property)
		{
			return GetValueHolder(property).Value;
		}

		private PropertyValue GetValueHolder(SolidProperty property)
		{
			lock (this.properties)
			{
				if (this.properties.ContainsKey(property))
					return this.properties[property];
				var value = new PropertyValue(property);
				value.ValueChanged += Value_ValueChanged;
				this.properties.Add(property, value);
				return value;
			}
		}

		private void Value_ValueChanged(object sender, EventArgs e)
		{
			var value = (PropertyValue)sender;
			this.OnPropertyChanged(value.Property.Name);
		}

		protected T Get<T>(SolidProperty property) => (T)Get(property);

		private class PropertyValue
		{
			private object value;
			private readonly SolidProperty property;

			public event EventHandler ValueChanged;

			public PropertyValue(SolidProperty property)
			{
				this.property = property;
				this.value = this.property.DefaultValue;
			}

			public object Value
			{
				get { return this.value; }
				set
				{
					if (this.property.PropertyType.IsAssignableFrom(value?.GetType()) == false)
						throw new InvalidOperationException($"Cannot assign {this.property.PropertyType.Name} from {value?.GetType()?.Name}.");
					if (this.value == value)
						return;
					var changed =
						((this.value == null) && (value != null)) ||
						((this.value != null) && (value == null)) ||
						((this.value != null) && (this.value.Equals(value) == false)) ||
						((value != null) && (value.Equals(this.value) == false));
					this.value = value;
					if (changed)
						this.ValueChanged.Invoke(this, EventArgs.Empty);
				}
			}

			public SolidProperty Property => this.property;
		}
	}
}
