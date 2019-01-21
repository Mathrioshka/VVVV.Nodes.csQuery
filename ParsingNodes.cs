using System.Linq;
using CsQuery;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes.csQuery
{
	[PluginInfo(Name = "Select", Category = "csQuery", Author = "alg", Help = "Perform jQuery select on HTML")]
	public class SelectNode : IPluginEvaluate
	{
		[Input("HTML")]
		protected IDiffSpread<string> HTMLIn;

		[Input("jQuery Selector")]
		private ISpread<string> FSelectorIn;

		[Output("Selection Result")] 
		private ISpread<string> FSelectionOut;

		[Output("DOM Object")] 
		private ISpread<ISpread<IDomObject>> FDOMObjectOut;
		
		[Output("Status")]
		private ISpread<string> FStatusOut;

		private readonly Spread<CQ> FDom = new Spread<CQ>();

		public void Evaluate(int spreadMax)
		{
			FSelectionOut.SliceCount = FDOMObjectOut.SliceCount = FStatusOut.SliceCount = FDom.SliceCount = spreadMax;

			for (var i = 0; i < spreadMax; i++)
			{
				if (HTMLIn.IsChanged)
				{
					FDom[i] = CQ.Create(HTMLIn[i]);
					FStatusOut[i] = "DOM Created";
				}

				var result = FDom[i].Select(FSelectorIn[i]);
				var count = FDOMObjectOut[i].SliceCount = result.Count();

				for (var j = 0; j < count; j++)
				{
					FDOMObjectOut[i][j] = result[j];
				}

				FSelectionOut[i] = result.RenderSelection();
			}
		}
	}

	[PluginInfo(Name = "GetAttribute", Category = "csQuery", Author = "alg", Help = "Get element attribute value")]
	public class GetAttributeNode : IPluginEvaluate
	{
		[Input("DOM Object")]
		private ISpread<IDomObject> FDomObjectIn;

		[Input("Attribute")]
		private ISpread<string> FAttributeIn;

		[Output("Value")]
		private ISpread<string> FValueOut;
		
		public void Evaluate(int spreadMax)
		{
			FValueOut.SliceCount = spreadMax;

			for (var i = 0; i < spreadMax; i++)
			{
				FValueOut[i] = FDomObjectIn[i].GetAttribute(FAttributeIn[i]);
			}
		}
	}
}
