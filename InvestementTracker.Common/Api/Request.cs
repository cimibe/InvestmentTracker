using System.Globalization;
using System.Text;

using Microsoft.Extensions.ObjectPool;

namespace InvestmentTracker.Common.Api
{
    /// <summary>
    /// Provides a relatively simply mechanism to build up request URIs to send to a particular API.
    /// </summary>
    public class Request : IDisposable
    {
        // Haven't done any testing, so this may be premature optimization. The intuition here is that we're likely
        // to be requesting StringBuilder instances with similar resulting sizes, so let's just pool them to avoid
        // the StringBuilders needing to be repeatedly resized to fit.
        private static ObjectPool<StringBuilder> s_pool =
            new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());

        private StringBuilder _builder = s_pool.Get();
        private bool _areInGetParameters = false;
        private bool _disposed = false;

        /// <summary>
        /// Constructs a basic instance with no base URI.
        /// </summary>
        public Request() { }

        /// <summary>
        /// Constructs an instance with a base URI which subsequent "Add" calls will append to.
        /// </summary>
        /// <param name="baseUrl">Base URI for the service</param>
        public Request(string baseUrl)
        {
            _builder.Append(baseUrl);
        }

        private void AssertNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Request));
            }
        }

        private void AssertNotInGetParameters()
        {
            if (_areInGetParameters)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Adds a get parameter (e.g. ?name=value).
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddGetParameter(string key, string value)
        {
            AssertNotDisposed();

            if (_areInGetParameters)
            {
                _builder.Append('&');
            }
            else
            {
                _areInGetParameters = true;

                _builder.Append('?');
            }

            _builder.Append($"{key}={value}");
            return this;
        }

        /// <summary>
        /// Adds a get parameter (e.g. ?name=value). This method handles a value of unknown type with the provided
        /// format/format-provider.
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="formatProvider">Format provider</param>
        /// <param name="format">Format string</param>
        /// <param name="value">Parameter value</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddGetParameter(string key, IFormatProvider formatProvider, string format, object? value)
        {
            AssertNotDisposed();

            if (_areInGetParameters)
            {
                _builder.Append('&');
            }
            else
            {
                _areInGetParameters = true;

                _builder.Append('?');
            }

            _builder.Append($"{key}=");
            _builder.AppendFormat(formatProvider, format, value);
            return this;
        }

        /// <summary>
        /// Adds a get parameter (e.g. ?name=value). This method handles a value of unknown type with the provided
        /// format and an invariant-culture format provider.
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="format">Format string</param>
        /// <param name="value">Parameter value</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddGetParameter(string key, string format, object? value)
        {
            return AddGetParameter(key, CultureInfo.InvariantCulture, format, value);
        }

        /// <summary>
        /// Adds a segment to the URI path (e.g. baseUri/segment).
        /// </summary>
        /// <param name="segment">Segment to append</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddToPath(string segment)
        {
            AssertNotDisposed();
            AssertNotInGetParameters();

            _builder.Append($"/{segment}");
            return this;
        }

        /// <summary>
        /// Adds a segment to the URI path (e.g. baseUri/segment). This version of the method allows segments of
        /// unknown types to be appended with the provided format provider and format string.
        /// </summary>
        /// <param name="formatProvider">Format provider</param>
        /// <param name="format">Format string</param>
        /// <param name="segment">Segment to append</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddToPath(IFormatProvider formatProvider, string format, object? segment)
        {
            AssertNotDisposed();
            AssertNotInGetParameters();

            _builder.Append('/');
            _builder.AppendFormat(formatProvider, format, segment);
            return this;
        }

        /// <summary>
        /// Adds a segment to the URI path (e.g. baseUri/segment). This version of the method allows segments of
        /// unknown types to be appended with the invariant format provider and the provided format string.
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="segment">Segment to append</param>
        /// <returns>The current Request so that calls can be easily chained</returns>
        public Request AddToPath(string format, object? segment)
        {
            return AddToPath(CultureInfo.InvariantCulture, format, segment);
        }

        /// <summary>
        /// Disposes of internal memory resources to constructing the URI.
        /// </summary>
        public void Dispose()
        {
            AssertNotDisposed();

            s_pool.Return(_builder);
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object? obj)
        {
            AssertNotDisposed();

            return Equals(obj as Request);
        }

        public bool Equals(Request? other)
        {
            AssertNotDisposed();

            return other != null
                && _areInGetParameters == other._areInGetParameters
                && _builder.Equals(other._builder);
        }

        public override int GetHashCode()
        {
            AssertNotDisposed();

            return _builder.GetHashCode();
        }

        public override string ToString()
        {
            AssertNotDisposed();

            return _builder.ToString();
        }

        public Uri ToUri()
        {
            AssertNotDisposed();

            return new Uri(_builder.ToString());
        }
    }
}