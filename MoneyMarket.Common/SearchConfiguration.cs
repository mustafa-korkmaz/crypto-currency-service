using System;
using System.Diagnostics;
using System.Text;
using Elasticsearch.Net;
using MoneyMarket.Common.Helper;
using Nest;

namespace MoneyMarket.Common
{
    public static class SearchConfiguration
    {
        public static ElasticClient GetClient<T>()
        {
            var timeOut = TimeSpan.FromSeconds(Statics.ElasticSearchTimeOutInSeconds);

            var pool = new SingleNodeConnectionPool(new Uri(Statics.ConnectionUrlToElasticServer));
            var indexName = string.Format("{0}index", typeof(T).Name.ToLower());

            var settings = new ConnectionSettings(pool)
                .DefaultIndex(indexName)
                .MapDefaultTypeIndices(m => m.Add(typeof(T), indexName)
            );

            settings.DisableDirectStreaming().OnRequestCompleted(details =>
            {
                Debug.WriteLine("### ES REQEUST ###");
                if (details.RequestBodyInBytes != null)
                {
                    Debug.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));

                }
                Debug.WriteLine("### ---------- ###");
            }).PrettyJson().ThrowExceptions().RequestTimeout(timeOut);

            return new ElasticClient(settings);
        }
    }
}
