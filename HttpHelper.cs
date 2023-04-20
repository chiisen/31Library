using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace _31Library
{
    /// <summary>
    /// Http 小幫手
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 避免使用 HttpClient 產生太多連線
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// 預設的 Timeout 時間，單位秒
        /// </summary>
        private readonly int _defaultTimeout = 14;

        /// <summary>
        ///  建構 HttpHelper 強制必須帶入 IHttpClientFactory
        /// </summary>
        public HttpHelper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<(string body, HttpStatusCode statusCode)> Post(string url, object? postData, Dictionary<string, string> headers = null)
        {
            using (var request = _httpClientFactory.CreateClient("log"))
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                request.Timeout = TimeSpan.FromSeconds(_defaultTimeout);
                var response = await request.PostAsync(url, new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return (body, response.StatusCode);
            }
        }
    }
}