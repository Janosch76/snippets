namespace Js.Snippets.CSharp.StreamUtils
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for <see cref="Stream"/> instances
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Asynchronously reads the bytes from the current stream and writes them to another stream, using the 
        /// specified buffer size, cancellation token, and progress reporter.
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. The  default size is 81920.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="progress">The progress reporter, receiving the total number of bytes read.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize = 81920, CancellationToken cancellationToken = default(CancellationToken), IProgress<long> progress = null)
        {
            if (progress == null)
            {
                // use default copy to async when no progress reporting is required
                await source.CopyToAsync(destination, bufferSize, cancellationToken);
                return;
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.CanRead)
            {
                throw new ArgumentException("Has to be readable", nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (!destination.CanWrite)
            {
                throw new ArgumentException("Has to be writable", nameof(destination));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress.Report(totalBytesRead);
            }
        }
    }
}