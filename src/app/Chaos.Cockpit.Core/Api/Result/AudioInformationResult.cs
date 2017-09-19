using System.Collections.Generic;
using Chaos.Portal.Core.Data.Model;
using CHAOS.Serialization;

namespace Chaos.Cockpit.Core.Api.Result
{
	public class AudioInformationResult : AResult
	{
		[Serialize("Id")]
		public string Id { get; set; }

		[Serialize]
		public string Index { get; set; }

		[Serialize]
		public StimulusResult Stimulus { get; set; }

		[Serialize]
		public MetadataResult Metadata { get; set; }

		[Serialize]
		public IList<SegmentResult> Segments { get; set; }

		public AudioInformationResult()
		{
			Stimulus = new StimulusResult();
			Metadata = new MetadataResult();
			Segments = new List<SegmentResult>();
		}

		public class SegmentResult
		{
			[Serialize]
			public string Id { get; set; }
			
			[Serialize]
			public string CaterogyId { get; set; }

			[Serialize]
			public string StartTime { get; set; }

			[Serialize]
			public string EndTime { get; set; }

			[Serialize]
			public string Duration { get; set; }

			[Serialize]
			public string Index { get; set; }

			[Serialize]
			public string Editable { get; set; }

			[Serialize]
			public string Deletable { get; set; }

			[Serialize]
			public string ColorGroup { get; set; }

			[Serialize]
			public SegmentMetadataResult Metadata { get; set; }

			public SegmentResult()
			{
				Metadata = new SegmentMetadataResult();
			}

			public class SegmentMetadataResult
			{
				[Serialize]
				public string SchemaId { get; set; }

				[Serialize]
				public Dictionary<string, EditableValueResult> Fields { get; set; }

				public SegmentMetadataResult()
				{
					Fields = new Dictionary<string, EditableValueResult>();
				}
			}
		}

		public class MetadataResult
		{
			[Serialize]
			public string SchemaId { get; set; }
			
			[Serialize]
			public Dictionary<string, EditableValueResult> Fields { get; set; }

			public MetadataResult()
			{
				Fields = new Dictionary<string, EditableValueResult>();
			}
		}

		public class StimulusResult
		{
			[Serialize]
			public string URI { get; set; }

			[Serialize]
			public string Type { get; set; }

			[Serialize]
			public string MD5 { get; set; }

			[Serialize]
			public string Authentication { get; set; }

			[Serialize]
			public string StartOffset { get; set; }

			[Serialize]
			public string EndOffset { get; set; }

			[Serialize]
			public string Duration { get; set; }
		}
	}

	public class EditableValueResult
	{
		[Serialize]
		public string Value { get; set; }

		[Serialize]
		public bool IsEditable { get; set; }

		public EditableValueResult(string value, bool isEditable)
		{
			Value = value;
			IsEditable = isEditable;
		}

		public EditableValueResult(string value) : this(value, false)
		{
		}
	}
}