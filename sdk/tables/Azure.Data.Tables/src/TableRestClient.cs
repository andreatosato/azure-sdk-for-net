// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Data.Tables.Models;

namespace Azure.Data.Tables
{
    internal partial class TableRestClient
    {
        internal string endpoint => url;
        internal string clientVersion => version;

        internal HttpMessage CreateBatchRequest(MultipartContent content, string requestId, ResponseFormat? responsePreference)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.AppendRaw(url, false);
            uri.AppendPath("/$batch", false);
            request.Uri = uri;
            request.Headers.Add("x-ms-version", version);
            if (requestId != null)
            {
                request.Headers.Add("x-ms-client-request-id", requestId);
            }
            request.Headers.Add("DataServiceVersion", "3.0");
            if (responsePreference != null)
            {
                request.Headers.Add("Prefer", responsePreference.Value.ToString());
            }

            request.Content = content;
            content.ApplyToRequest(request);
            return message;
        }

        internal static MultipartContent CreateBatchContent(Guid batchGuid)
        {
            var guid = batchGuid == default ? Guid.NewGuid() : batchGuid;
            return new MultipartContent("mixed", $"batch_{guid}");
        }

        /// <summary> Submits a batch operation to a table. </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="message"/> is null. </exception>
        public async Task<Response<IReadOnlyList<Response>>> SendBatchRequestAsync(HttpMessage message, CancellationToken cancellationToken = default)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        var responses = await Multipart.ParseAsync(
                                message.Response.ContentStream,
                                message.Response.Headers.ContentType,
                                false,
                                true,
                                cancellationToken)
                            .ConfigureAwait(false);

                        var failedSubResponse = responses.FirstOrDefault(r => r.Status >= 400);
                        if (failedSubResponse == null)
                        {
                            return Response.FromValue(responses.ToList() as IReadOnlyList<Response>, message.Response);
                        }

                        RequestFailedException rfex = await _clientDiagnostics.CreateRequestFailedExceptionAsync(failedSubResponse).ConfigureAwait(false);

                        var ex = new TableTransactionFailedException(rfex);
                        throw ex;
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Submits a batch operation to a table. </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="message"/> is null. </exception>
        public Response<IReadOnlyList<Response>> SendBatchRequest(HttpMessage message, CancellationToken cancellationToken = default)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 202:
                    {
                        var responses = Multipart.ParseAsync(
                                message.Response.ContentStream,
                                message.Response.Headers.ContentType,
                                false,
                                false,
                                cancellationToken)
                            .EnsureCompleted();

                        var failedSubResponse = responses.FirstOrDefault(r => r.Status >= 400);
                        if (failedSubResponse == null)
                        {
                            return Response.FromValue(responses.ToList() as IReadOnlyList<Response>, message.Response);
                        }

                        RequestFailedException rfex = _clientDiagnostics.CreateRequestFailedException(responses[0]);
                        var ex = new TableTransactionFailedException(rfex);
                        throw ex;
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
