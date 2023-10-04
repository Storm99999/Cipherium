using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Cipherium.Core.Webhook
{
    internal class Webhook
    {
        public static WebClient shitClient { get; } = new WebClient();

        public static void Send(string url, string message, string username)
        {
            HttpPost(url, new NameValueCollection()
            {
                { "username", username},
                { "content", message}
            });
        }

        public static void SendWithFile(string url, string message, string username, string filePath, string fileName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                var file_bytes = File.ReadAllBytes(filePath);
                form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "Document", fileName);
                httpClient.PostAsync(url, form).Wait();
                httpClient.Dispose();
            }
        }

        public static byte[] HttpPost(string uri, NameValueCollection pairs)
        {
            return shitClient.UploadValues(uri, pairs);
        }
    }
}
