using MoneyMarket.Common.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using MoneyMarket.Common;
using MoneyMarket.Common.Helper;
using Newtonsoft.Json;

namespace MoneyMarket.Business.HttpClient
{
    /// <summary>
    /// invokes api for client requests
    /// </summary>
    public class SlackApiClient
    {
        #region singleton definition
        static readonly SlackApiClient _instance = new SlackApiClient();

        public static SlackApiClient Instance => _instance;

        private SlackApiClient()
        {
            //initialize
            _apiBaseUrl = Statics.GetSlackApiUrl();
        }

        #endregion singleton definition

        private readonly string _apiBaseUrl;

        /// <summary>
        ///  for  requests which has a T typed object parameter
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="actionUrl">action url except base api url</param>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<HttpClientResponse<TRes>> InvokeApi<TReq, TRes>(string actionUrl, TReq requestObject)
        {
            Uri url = new Uri(new Uri(_apiBaseUrl), actionUrl);
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var reqParams = GetRequestParameters(requestObject);

                var content = new FormUrlEncodedContent(reqParams);
             
                var apiResponse = new HttpClientResponse<TRes>();

                using (var response = await client.PostAsync(url, content))
                {
                    apiResponse.HttpStatusCode = response.StatusCode;

                    if (apiResponse.HttpStatusCode != HttpStatusCode.OK)
                    {
                        return apiResponse;
                    }

                    var data = await response.Content.ReadAsStringAsync();

                    var json = GetParsedData<TRes>(data);

                    apiResponse.ResponseData = json;

                    return apiResponse;
                }
            }
        }

        /// <summary>
        ///  for parameterless requests
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="actionUrl">action url except base api url</param>
        /// <param name="methodType">request method type</param>
        /// <returns></returns>
        public async Task<HttpClientResponse<TRes>> InvokeApi<TRes>(string actionUrl, WebMethodType methodType = WebMethodType.Post)
        {
            Uri url = new Uri(new Uri(_apiBaseUrl), actionUrl);

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                switch (methodType)
                {
                    case WebMethodType.Get:
                        using (var response = await client.GetAsync(url))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                return new HttpClientResponse<TRes> { HttpStatusCode = response.StatusCode };
                            }

                            var data = await response.Content.ReadAsStringAsync();
                            var json = GetParsedData<TRes>(data); //JObject.Parse(data);

                            return new HttpClientResponse<TRes> { HttpStatusCode = response.StatusCode, ResponseData = json };
                        }
                    case WebMethodType.Post:
                        using (var response = await client.PostAsync(url, null))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                return new HttpClientResponse<TRes> { HttpStatusCode = response.StatusCode };
                            }

                            var data = await response.Content.ReadAsStringAsync();
                            var json = GetParsedData<TRes>(data); //JObject.Parse(data);

                            return new HttpClientResponse<TRes> { HttpStatusCode = response.StatusCode, ResponseData = json };
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(methodType), methodType, null);
                }
            }
        }

        /// <summary>
        ///  for parameterless requests and no need to get any  response content
        /// </summary>
        /// <param name="actionUrl"></param>
        /// <param name="methodType">request method type</param>
        /// <returns></returns>
        public async Task<HttpClientResponse<ApiResponse>> InvokeApi(string actionUrl, WebMethodType methodType = WebMethodType.Post)
        {
            Uri url = new Uri(new Uri(_apiBaseUrl), actionUrl);

            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Get Token
                using (var response = await client.PostAsync(url, null))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return new HttpClientResponse<ApiResponse> { HttpStatusCode = response.StatusCode };
                    }

                    var data = await response.Content.ReadAsStringAsync();

                    var json = GetParsedData<ApiResponse>(data); //JObject.Parse(data);

                    return new HttpClientResponse<ApiResponse> { HttpStatusCode = response.StatusCode, ResponseData = json };
                }
            }
        }

        /// <summary>
        /// returns generic type from gven json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private T GetParsedData<T>(string data)
        {
            #region sample json string

            /* 
            {
             "$d":
                [
                 {
                   "Text": "Foo",
                   "Value": "foo"
                 },
                 {
                   "Text": "Bar",
                   "Value": "bar"
                 }
               ]
             }
           */

            #endregion sample json string

            if (data[0] != '{') // primitive type returned  like int,string
            {
                return (T)Convert.ChangeType(data, typeof(T));
            }

            var parsedObject = JsonConvert.DeserializeObject<T>(data);

            //var rawJson = JObject.Parse(data);

            //string arraySignature = "$d";

            //if (data.Substring(2, 2) == arraySignature)  // is this data is actually an array?
            //{
            //    var json = rawJson.GetValue("$d").ToString(); // first remove the top element named '$d'

            //    JArray jsonArray = JArray.Parse(json);

            //    parsedObject = jsonArray.ToObject<T>();

            //    return parsedObject;
            //}

            //parsedObject = rawJson.ToObject<T>();

            return parsedObject;
        }


        private Dictionary<string, string> GetRequestParameters<T>(T requestObject)
        {
            Dictionary<string, string> reqDic = new Dictionary<string, string>();
            foreach (PropertyInfo pi in requestObject.GetType().GetProperties())
            {
                var objVal = pi.GetValue(requestObject, null);

                if (objVal != null)
                {
                    reqDic.Add(pi.Name, GetJsonValueWithValidatedChars(objVal));
                }
            }

            return reqDic;
        }

        /// <summary>
        /// web api do not accept chars=> requestPathInvalidCharacters="<,>,*,%,:,&"
        /// this method replaces invalid chars with the valid ones
        /// </summary>
        /// <param name="apiParameter">parameter we pass to web api</param>
        /// <returns></returns>
        private string GetJsonValueWithValidatedChars(object apiParameter)
        {
            // var enumarableVariable = apiParameter as IEnumerable;
            if (apiParameter is ICollection)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(apiParameter);
            }

            string validatedText = apiParameter.ToString().Replace('<', '(').Replace('>', ')').Replace('*', '-').Replace('%', '-').Replace('&', '-');

            return validatedText;
        }
    }

    public class SlackMessage
    {
        public string token { get; set; }
        public string channel { get; set; }
        public string text { get; set; }
    }
}
