using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Skinning
{
	public interface IFont
	{
		Size Measure(string text, float? maxWidth = null);
	}
}
