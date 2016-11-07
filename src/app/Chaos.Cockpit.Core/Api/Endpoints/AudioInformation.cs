using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
	public class AudioInformation : AExtension
	{
		public AudioInformation(IPortalApplication portalApplication) : base(portalApplication)
		{
		}

		public IEnumerable<AudioInformationResult> Search(string function, string arguments)
		{
			var xml = XDocument.Parse(QueryOctopus());
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

		private static string QueryOctopus()
		{
			var dummyData =
				"<Items>" +
				"	<Item>" +
				"		<Id>1</Id>" +
				"		<Index>1</Index>" +
				"		<Stimulus>" +
				"			<URI>rtpm://TBD_{INSERT_SESSION_GUID}</URI>" +
				"			<Type>audio/mp3</Type>" +
				"			<MD5>aa576ff5e76fe76ba587ab57d53</MD5>" +
				"			<Authentication>TODO WHAT IS NEEDED</Authentication>" +
				"			<StartOffset>00:00:30.23254353</StartOffset>" +
				"			<EndOffset>00:00:31.2325435</EndOffset>" +
				"			<Duration>00:00:01.2325435</Duration>" +
				"		</Stimulus>" +
				"		<Metadata SchemaId=\"ItemType1\">	<!-- Type defined above, this correponds to the out from a standard experiemnt-->" +
				"			<ProgrammeName   	  Editable=\"false\">Radio Avisen</ProgrammeName>" +
				"			<ChannelHeaderLabel   Editable=\"false\">DR-P3</ChannelHeaderLabel>" +
				"			<PublicationStartTime Editable=\"false\">00:00:30.232</PublicationStartTime>" +
				"			<PublicationEndTime   Editable=\"false\">00:00:32.232</PublicationEndTime>" +
				"		</Metadata>" +
				"		<Segments>" +
				"			<Segment>" +
				"				<CaterogyId>1</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.454322</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454222</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:01:25.454222</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>true</Editable>" +
				"				<Deleteable>true</Deleteable>" +
				"				<ColorGroup>1</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType1\">" +
				"					<TranscriptionAsString Editable=\"true\">Der er ikke fejet noget ind under gulvtæppet sagde Poul Schluter i dagens...</TranscriptionAsString>" +
				"				</Metadata>" +
				"			</Segment>" +
				"			<Segment>" +
				"				<CaterogyId>2</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.454111</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454111</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:01:25.454111</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>false</Editable>" +
				"				<Deleteable>false</Deleteable>" +
				"				<ColorGroup>1</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType2\">" +
				"					<TranscriptionAsWordListWithTimeline Editable=\"False\">{0,Der} {0.341,er} {0.76,ikke} {2.341,fejet} {4.141,noget} {5.341,ind} {6.341,under} {7.341,gulvtæppet} {8.341,sagde} {9.341,Poul} 	{10.341,Schluter} {0.341,i} {0.341,dagens}..." +
				"					</TranscriptionAsWordListWithTimeline>" +
				"				</Metadata>" +
				"			</Segment>" +
				"			<Segment>" +
				"				<CaterogyId>3</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.4532324</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454323</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:21:25.454323</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>false</Editable>" +
				"				<Deleteable>false</Deleteable>" +
				"				<ColorGroup>2</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType3\">" +
				"					<UserRankingAsNummericList Editable=\"true\"></UserRankingAsNummericList> <!-- Note no data entered apriori, expectred to be filled by User and returned by the component-->" +
				"					<CustomRankingAsDecimal    Editable=\"true\"></CustomRankingAsDecimal>" +
				"					<AudioQualityAsDropbown    Editable=\"true\"></AudioQualityAsDropbown>" +
				"					<CommentAsText			   Editable=\"true\"></CommentAsText>" +
				"				</Metadata>" +
				"			</Segment>" +
				"			<Segment>" +
				"				<CaterogyId>4</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.4532324</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454323</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:21:25.454323</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>false</Editable>" +
				"				<Deleteable>false</Deleteable>" +
				"				<ColorGroup>2</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType4\">" +
				"					<Word Editable=\"true\">Der</Word>" +
				"				</Metadata>" +
				"			</Segment>" +
				"			<Segment>" +
				"				<CaterogyId>4</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.4532324</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454323</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:21:25.454323</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>false</Editable>" +
				"				<Deleteable>false</Deleteable>" +
				"				<ColorGroup>2</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType4\">" +
				"					<Word Editable=\"true\">er</Word>" +
				"				</Metadata>" +
				"			</Segment>" +
				"			<Segment>" +
				"				<CaterogyId>4</CaterogyId>	  <!-- Category, used for visualizaiton-->" +
				"				<StartTime>00:21:21.4532324</StartTime> <!-- Relative to the Item (not the file -->" +
				"				<EndTime>00:21:25.454323</EndTime> <!-- Relative to the Item (not the file -->" +
				"				<Duration>00:21:25.454323</Duration> <!-- Relative to the Item (not the file -->" +
				"				<Index>1</Index>" +
				"				<Editable>false</Editable>" +
				"				<Deleteable>false</Deleteable>" +
				"				<ColorGroup>2</ColorGroup>  <!-- Color group used for visualizaiton, color -->" +
				"				<Metadata SchemaId=\"SegmentType4\">" +
				"					<Word Editable=\"true\">ikke</Word>" +
				"				</Metadata>" +
				"			</Segment>" +
				"		</Segments>" +
				"	</Item>" +
				"</Items>";
			return dummyData;
		}
	}
}