using System;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Cockpit.Core.Core.Exceptions;
using Chaos.Mcm.Configuration;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Exceptions;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
	public class UploadExtension : AExtension
	{
		public AwsConfiguration Aws { get; set; }

		public UploadExtension(IPortalApplication portalApplication, AwsConfiguration aws) : base(portalApplication)
		{
			Aws = aws;
		}

		public StringResult Upload(string key)
		{
			if (Request.IsAnonymousUser)
				throw new InsufficientPermissionsException("Upload requires a authenticated used");

			var file = Request.Files.FirstOrDefault();

			if (file == null)
				throw new FileParameterMissingException("No image file found");

			if (string.IsNullOrEmpty(key))
				key = file.FileName;

			using (var client = AWSClientFactory.CreateAmazonS3Client(Aws.AccessKey, Aws.SecretKey, RegionEndpoint.EUWest1))
			{
				file.InputStream.Position = 0;

				var request = new PutObjectRequest
				{
					InputStream = file.InputStream,
					BucketName = "cockpit-upload",
					CannedACL = S3CannedACL.PublicRead,
					StorageClass = S3StorageClass.StandardInfrequentAccess,
					Key = key
				};

				client.PutObject(request);

				return new StringResult("https://s3-eu-west-1.amazonaws.com/cockpit-upload/" + key);
			}
		}
	}
}