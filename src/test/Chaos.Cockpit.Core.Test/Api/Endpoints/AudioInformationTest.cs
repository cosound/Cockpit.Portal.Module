using System.Linq;
using System.Web;
using System.Xml.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using Chaos.Cockpit.Core.Core;
using Moq;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
	[TestFixture]
	public class AudioInformationTest : TestBase
	{
		[Test]
		public void Search()
		{
			var httpGet = new Mock<IHttpGet>();
			var aiExt = new AudioInformation(PortalApplication.Object, httpGet.Object);
			httpGet.Setup(m => m.Get(It.IsAny<string>())).Returns(QueryOctopus());

			var result = aiExt.Search("", "").First();

			Assert.That(result.Id, Is.EqualTo("1"));
			Assert.That(result.Index, Is.EqualTo("1"));
			Assert.That(result.Stimulus.URI, Is.EqualTo("rtpm://TBD_{INSERT_SESSION_GUID}"));
			Assert.That(result.Stimulus.Type, Is.EqualTo("audio/mp3"));
			Assert.That(result.Stimulus.MD5, Is.EqualTo("aa576ff5e76fe76ba587ab57d53"));
			Assert.That(result.Stimulus.Authentication, Is.EqualTo("TODO WHAT IS NEEDED"));
			Assert.That(result.Stimulus.StartOffset, Is.EqualTo("00:00:30.23254353"));
			Assert.That(result.Stimulus.EndOffset, Is.EqualTo("00:00:31.2325435"));
			Assert.That(result.Stimulus.Duration, Is.EqualTo("00:00:01.2325435"));
			Assert.That(result.Metadata.SchemaId, Is.EqualTo("ItemType1"));
			Assert.That(result.Metadata.ProgrammeName.Value, Is.EqualTo("Radio Avisen"));
			Assert.That(result.Metadata.ChannelHeaderLabel.Value, Is.EqualTo("DR-P3"));
			Assert.That(result.Metadata.PublicationStartTime.Value, Is.EqualTo("00:00:30.232"));
			Assert.That(result.Metadata.PublicationEndTime.Value, Is.EqualTo("00:00:32.232"));
			Assert.That(result.Segments.First().CaterogyId, Is.EqualTo("1"));
			Assert.That(result.Segments.First().StartTime, Is.EqualTo("00:21:21.454322"));
			Assert.That(result.Segments.First().EndTime, Is.EqualTo("00:21:25.454222"));
			Assert.That(result.Segments.First().Duration, Is.EqualTo("00:01:25.454222"));
			Assert.That(result.Segments.First().Index, Is.EqualTo("1"));
			Assert.That(result.Segments.First().Editable, Is.EqualTo("true"));
			Assert.That(result.Segments.First().Deleteable, Is.EqualTo("true"));
			Assert.That(result.Segments.First().ColorGroup, Is.EqualTo("1"));
			Assert.That(result.Segments.First().Metadata.SchemaId, Is.EqualTo("SegmentType1"));
			Assert.That(result.Segments.First().Metadata.Fields["TranscriptionAsString"].Value, Is.EqualTo("Der er ikke fejet noget ind under gulvtæppet sagde Poul Schluter i dagens..."));
		}

		private static string QueryOctopus()
		{
			var json =
			"{"+
				"\"Results\": ["+
				"{"+
				"\"id\": \"22122928-8ccd-4c37-a269-2cc12005ff61\","+
				"\"steps\": ["+
				"{"+
				"\"tasks\": ["+
				"{"+
				"\"taskId\": \"f7e755b8-9d42-41dd-92df-62d5835546b0\","+
				"\"pluginId\": \"com.chaos.octopus.agent.unit.TestPlugin, 1.0.0\","+
				"\"_State\": \"Committed\","+
				"\"progress\": 1.0,"+
				"\"output\": \"{OUTPUT_DATA}\"" +
				"}" +
				"]" +
				"}" +
				"]," +
				"\"status\": \"new\"" +
				"}" +
				"]" +
				"}";

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

			return json.Replace("{OUTPUT_DATA}", HttpUtility.HtmlEncode(XDocument.Parse(dummyData).ToString(SaveOptions.DisableFormatting)));
		}
	}
}