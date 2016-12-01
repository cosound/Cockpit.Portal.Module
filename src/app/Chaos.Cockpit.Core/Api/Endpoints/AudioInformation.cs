using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;
using Newtonsoft.Json.Linq;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
	public class AudioInformation : AExtension
	{
		public IHttpGet HttpGet { get; set; }

		public AudioInformation(IPortalApplication portalApplication, IHttpGet httpGet) : base(portalApplication)
		{
			HttpGet = httpGet;
		}

		public IEnumerable<AudioInformationResult> Search(string function, string arguments)
		{
			var result = HttpGet.Get("http://52.212.183.101:40000/Job/Enqueue?wait=false&job={}");
			var deserializeObject = JObject.Parse(result) as dynamic;
			var output = HttpUtility.HtmlDecode(deserializeObject.Results[0].steps[0].tasks[0].output.ToString() as string);

			var xml = XDocument.Parse(output);
			var aiResult = Parse(xml).ToList();

			return aiResult;
		}

		private IEnumerable<AudioInformationResult> Parse(XDocument xml)
		{
			foreach (var item in xml.Root.Elements("Item"))
			{
				var aiResult = new AudioInformationResult();
				aiResult.Id = item.Element("Id").Value;
				aiResult.Index = item.Element("Index").Value;

				aiResult.Stimulus.URI = item.XPathSelectElement("Stimulus/URI").Value;
				aiResult.Stimulus.Type = item.XPathSelectElement("Stimulus/Type").Value;
				aiResult.Stimulus.MD5 = item.XPathSelectElement("Stimulus/MD5").Value;
				aiResult.Stimulus.Authentication = item.XPathSelectElement("Stimulus/Authentication").Value;
				aiResult.Stimulus.StartOffset = item.XPathSelectElement("Stimulus/StartOffset").Value;
				aiResult.Stimulus.EndOffset = item.XPathSelectElement("Stimulus/EndOffset").Value;
				aiResult.Stimulus.Duration = item.XPathSelectElement("Stimulus/Duration").Value;

				aiResult.Metadata.SchemaId = item.Element("Metadata").Attribute("SchemaId").Value;
				aiResult.Metadata.ProgrammeName = new EditableValueResult(item.XPathSelectElement("Metadata/ProgrammeName").Value);
				aiResult.Metadata.ChannelHeaderLabel = new EditableValueResult(item.XPathSelectElement("Metadata/ChannelHeaderLabel").Value);
				aiResult.Metadata.PublicationStartTime = new EditableValueResult(item.XPathSelectElement("Metadata/PublicationStartTime").Value);
				aiResult.Metadata.PublicationEndTime = new EditableValueResult(item.XPathSelectElement("Metadata/PublicationEndTime").Value);

				foreach (var segment in item.XPathSelectElements("Segments/Segment"))
				{
					var segmentResult = new AudioInformationResult.SegmentResult();
					segmentResult.CaterogyId = segment.Element("CaterogyId").Value;
					segmentResult.StartTime = segment.Element("StartTime").Value;
					segmentResult.EndTime = segment.Element("EndTime").Value;
					segmentResult.Duration = segment.Element("Duration").Value;
					segmentResult.Index = segment.Element("Index").Value;
					segmentResult.Editable = segment.Element("Editable").Value;
					segmentResult.Deleteable = segment.Element("Deleteable").Value;
					segmentResult.ColorGroup = segment.Element("ColorGroup").Value;
					segmentResult.Metadata.SchemaId = segment.Element("Metadata").Attribute("SchemaId").Value;


					foreach (var fieldElement in segment.Element("Metadata").Elements())
					{
						segmentResult.Metadata.Fields.Add(fieldElement.Name.LocalName, new EditableValueResult(fieldElement.Value));
					}

					aiResult.Segments.Add(segmentResult);
				}

				yield return aiResult;
			}
		}
	}
}