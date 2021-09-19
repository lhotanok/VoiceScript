using System;
using System.IO;
using Google.Cloud.Storage.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Designed for cloud storage management of long audio files.
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

        /// <summary>
        /// Name of the bucket in which the file is stored in Google Cloud Storage.
        /// </summary>
        public string BucketName => bucketName;

        /// <summary>
        /// Uri address of the stored file.
        /// </summary>
        public string FileStorageUri => $"gs://{bucketName}/{filePath}";

        /// <summary>
        /// Uploads file to the Google Cloud Storage into default bucket.
        /// </summary>
        /// <param name="content">Uploaded file content</param>
        public void Upload(byte[] content)
        {
            client.UploadObject(bucketName, filePath, contentType, new MemoryStream(content));
        }

        /// <summary>
        /// Downloads file from Google Cloud Storage.
        /// File is stored in default bucket.
        /// </summary>
        /// <param name="objectDownloadName">Optional download name.
        /// If not specified, file's original name is used.</param>
        public void Download(string objectDownloadName = null)
        {
            objectDownloadName ??= filePath;

            using var stream = File.OpenWrite(objectDownloadName);
            client.DownloadObject(bucketName, filePath, stream);
        }
    }
}
