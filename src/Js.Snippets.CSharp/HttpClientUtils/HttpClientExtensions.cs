namespace Js.Snippets.CSharp.HttpClientUtils
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Js.Snippets.CSharp.StreamUtils;

    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync(this HttpClient client, string requestUri, string path, CancellationToken cancellationToken = default(CancellationToken), IProgress<float> progress = null)
        {
            await client.DownloadAsync(new Uri(requestUri), path, cancellationToken, progress);
        }

        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, string path, CancellationToken cancellationToken = default(CancellationToken), IProgress<float> progress = null)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await client.DownloadAsync(requestUri, fs, cancellationToken, progress);
            }
        }

        public static async Task DownloadAsync(this HttpClient client, Uri requestUri, Stream destination, CancellationToken cancellationToken = default(CancellationToken), IProgress<float> progress = null)
        {
            // Get the http headers first to examine the content length
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var contentLength = response.Content.Headers.ContentLength;

                using (var download = await response.Content.ReadAsStreamAsync())
                {

                    // Ignore progress reporting when no progress reporter was 
                    // passed or when the content length is unknown
                    IProgress<long> relativeProgress = null;
                    if (progress != null && contentLength.HasValue)
                    {
                        // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                        relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value * 100));
                    }

                    // Use extension method to report progress while downloading
                    progress?.Report(0);
                    await download.CopyToAsync(destination, 81920, cancellationToken, relativeProgress);
                    progress?.Report(100);
                }
            }
        }
    }
}
