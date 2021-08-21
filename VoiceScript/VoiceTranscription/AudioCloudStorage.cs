using System;
using System.IO;
using Google.Cloud.Storage.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// To be used for cloud storage of audio files longer than 1 minute.
    /// </summary>
    class AudioCloudStorage
    {
        readonly string projectId = "xxx";
        readonly string filePath;
        readonly string contentType;
        readonly string bucketName;
        readonly StorageClient client;

        public AudioCloudStorage(string filename, string fileContentType)
        {
            filePath = filename;
            contentType = fileContentType;

            #region InitializeProjectBucket
            client = StorageClient.Create();
            bucketName = Guid.NewGuid().ToString();
            client.CreateBucket(projectId, bucketName);
            #endregion
        }

        public string BucketName => bucketName;
        public string FileStorageUri => $"gs://{bucketName}/{filePath}";

        public void Upload(byte[] content)
        {
            client.UploadObject(bucketName, filePath, contentType, new MemoryStream(content));
        }

        public void Download(string objectDownloadName = null)
        {
            objectDownloadName ??= filePath;

            using var stream = File.OpenWrite(objectDownloadName);
            client.DownloadObject(bucketName, filePath, stream);
        }
    }
}
