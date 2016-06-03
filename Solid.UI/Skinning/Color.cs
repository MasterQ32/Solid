using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Skinning
{
	public partial struct Color
	{
		public Color(float r, float g, float b, float a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		public Color(float r, float g, float b) :
			this(r, g, b, 1.0f)
		{

		}

		public float R;
		public float G;
		public float B;
		public float A;

		public override string ToString() => string.Format("({0:F2} {1:D2} {2} {3})", R, G, B, A);
	}
}
