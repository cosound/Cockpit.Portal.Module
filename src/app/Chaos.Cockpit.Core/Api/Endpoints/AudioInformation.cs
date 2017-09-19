using System;
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
			if (string.IsNullOrEmpty(function)) function = "00001_cosound/01000_custom/01010_speechtranscription/system/740_plugin/source/asrindexquery/published/31a13888-bc05-4a6f-aec5-36dab32ea576/query.sh";
			if (string.IsNullOrEmpty(arguments)) arguments = "def";
			
			if(function.Contains(" ") && function.Contains("^")) throw new ArgumentException("Function, cannot contain whitespaces or ^");
			
			var url = string.Format(
				//"{{\"steps\": [{{\"tasks\": [{{\"pluginId\": \"com.chaos.octopus.CommandLinePlugin, 1.0.0\",\"properties\": {{\"commandline\": \"/home/ubuntu/wp0x-store/{0} \\\"{1}\\\"\"}} }} ] }} ]}}",
				"{{\"steps\": [{{\"tasks\": [{{\"pluginId\": \"com.chaos.octopus.CommandLinePlugin, 1.0.0\",\"properties\": {{\"commandline\": \"/home/ubuntu/wp0x-store/{0} {1}\"}} }} ] }} ]}}",
				function, arguments);
			
			var job = HttpUtility.UrlEncode(url);
			var result = HttpGet.Get("http://54.72.165.54:40000/Job/Enqueue?wait=true&job=" + job);
			
			var deserializeObject = JObject.Parse(result) as dynamic;
			var output = HttpUtility.HtmlDecode(deserializeObject.Results[0].steps[0].tasks[0].output.ToString() as string);
			
			var xml = XDocument.Parse(output);
			var aiResult = Parse(xml).ToList();

			return aiResult;
		}

		private IEnumerable<AudioInformationResult> Parse(XDocument xml)
		{
			foreach (var item in xml.Root.Element("Items").Elements("Item"))
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
				
				foreach (var fieldElement in item.Element("Metadata").Elements())
				{
					var value = fieldElement.Value;
					var isEditable = fieldElement.Attribute("Editable").Value.ToLower() == "true";
					aiResult.Metadata.Fields.Add(fieldElement.Name.LocalName, new EditableValueResult(value, isEditable));
				}
				
				foreach (var segment in item.XPathSelectElements("Segments/Segment"))
				{
					var segmentResult = new AudioInformationResult.SegmentResult();
					segmentResult.Id = segment.Element("Id").Value;
					segmentResult.CaterogyId = segment.Element("CaterogyId").Value;
					segmentResult.StartTime = segment.Element("StartTime").Value;
					segmentResult.EndTime = segment.Element("EndTime").Value;
					segmentResult.Index = segment.Element("Index").Value;
					segmentResult.Editable = segment.Element("Editable").Value;
					segmentResult.Deletable = segment.Element("Deletable").Value;
					segmentResult.ColorGroup = segment.Element("ColorGroup").Value;
					segmentResult.Metadata.SchemaId = segment.Element("Metadata").Attribute("SchemaId").Value;

					foreach (var fieldElement in segment.Element("Metadata").Elements())
					{
						var value = fieldElement.Value;
						var isEditable = fieldElement.Attribute("Editable").Value == "true";
						segmentResult.Metadata.Fields.Add(fieldElement.Name.LocalName, new EditableValueResult(value, isEditable));
					}

					aiResult.Segments.Add(segmentResult);
				}

				yield return aiResult;
			}
		}
	}
}