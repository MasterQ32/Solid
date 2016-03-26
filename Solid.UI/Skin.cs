using Solid.Markup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using GDIRectangle = System.Drawing.Rectangle;

namespace Solid.UI
{
	public sealed class Skin : IDictionary<string, Texture>
	{
		private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

		private Skin()
		{

		}

		public Texture this[string key]
		{
			get
			{
				return textures[key];
			}

			set
			{
				textures[key] = value;
			}
		}

		public int Count
		{
			get
			{
				return textures.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IDictionary<string, Texture>)textures).IsReadOnly;
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				return ((IDictionary<string, Texture>)textures).Keys;
			}
		}

		public ICollection<Texture> Values
		{
			get
			{
				return ((IDictionary<string, Texture>)textures).Values;
			}
		}

		public static Skin Load(string fileName)
		{
			var document = Parser.Load(fileName);

			var typeMapper = new TypeMapper();
			typeMapper.Add("Skin", typeof(SkinRoot));
			typeMapper.Add("Component", typeof(SkinComponent));

			var mapper = new NativeMapper(typeMapper);
			mapper.NamedNodeEmitted += (s, e) =>
			{
				if (e.Node is SkinComponent)
				{
					((SkinComponent)e.Node).ID = e.Name;
				}
			};

			var root = (SkinRoot)mapper.Instantiate(document);

			var skin = new Skin();

			using (var source = new Bitmap(root.Source))
			{
				foreach(var component in root.components)
				{
					using (var bmp = new Bitmap(component.Value.Width, component.Value.Height))
					{
						using (var g = Graphics.FromImage(bmp))
						{
							g.DrawImage(
								source,
								new GDIRectangle(0, 0, bmp.Width, bmp.Height),
								new GDIRectangle(component.Value.X, component.Value.Y, bmp.Width, bmp.Height),
								GraphicsUnit.Pixel);
						}

							skin.Add(component.Key, new Texture(bmp));
					}
				}
			}
			return skin;
		}

		public void Add(KeyValuePair<string, Texture> item)
		{
			((IDictionary<string, Texture>)textures).Add(item);
		}

		public void Add(string key, Texture value)
		{
			textures.Add(key, value);
		}

		public void Clear()
		{
			textures.Clear();
		}

		public bool Contains(KeyValuePair<string, Texture> item)
		{
			return ((IDictionary<string, Texture>)textures).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return textures.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, Texture>[] array, int arrayIndex)
		{
			((IDictionary<string, Texture>)textures).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, Texture>> GetEnumerator()
		{
			return ((IDictionary<string, Texture>)textures).GetEnumerator();
		}

		public bool Remove(KeyValuePair<string, Texture> item)
		{
			return ((IDictionary<string, Texture>)textures).Remove(item);
		}

		public bool Remove(string key)
		{
			return textures.Remove(key);
		}

		public bool TryGetValue(string key, out Texture value)
		{
			return textures.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, Texture>)textures).GetEnumerator();
		}
	}

	internal abstract class SkinNodeBase
	{

	}

	internal sealed class SkinRoot : SkinNodeBase
	{
		public readonly Dictionary<string, SkinComponent> components = new Dictionary<string, SkinComponent>();

		public void Add(SkinComponent component)
		{
			if (component.ID == null)
				throw new InvalidOperationException("Each skin component must have an assigned name.");
			components.Add(component.ID, component);
		}

		public string Source { get; set; }
	}

	internal sealed class SkinComponent : SkinNodeBase
	{
		public string ID { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }
	}
}
