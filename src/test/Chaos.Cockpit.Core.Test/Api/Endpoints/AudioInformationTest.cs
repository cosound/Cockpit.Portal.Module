using System.Linq;
using Chaos.Cockpit.Core.Api.Endpoints;
using NUnit.Framework;

namespace Chaos.Cockpit.Core.Test.Api.Endpoints
{
	[TestFixture]
	public class AudioInformationTest : TestBase
	{
		[Test]
		public void Search()
		{
			var aiExt = new AudioInformation(PortalApplication.Object);

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
	}
}