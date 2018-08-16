using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("================Sync Get call==========================");
            GetPost();
            Console.WriteLine("================Sync Get call(one Record)==========================");
            GetPostById();
            Console.WriteLine("================Sync post call==========================");
            PostData();
            Console.ReadLine();
        }

        public static HttpClient GetHttpClinet()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:3000");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public static void GetPost()
        {
            try
            {
                using (var httpClinet = GetHttpClinet())
                {
                    var result = httpClinet.GetAsync("/posts");
                    var response = result.Result.Content.ReadAsStringAsync().Result;
                    var posts = JsonConvert.DeserializeObject<List<Post>>(response);
                    foreach (var item in posts)
                    {
                        Console.WriteLine($"Id: {item.ID} Title: {item.Title} Author: {item.Author}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);

            }
        }

        public static void GetPostById()
        {
            using(var httpClinet = GetHttpClinet())
            {
                var response = httpClinet.GetAsync("/posts/1");
                var item = response.Result.Content.ReadAsAsync<Post>().Result;
                Console.WriteLine($"Id: {item.ID} Title: {item.Title} Author: {item.Author}");
            }
        }

        public static void PostData()
        {
            using (var httpClient = GetHttpClinet())
            {
                var post = new Post()
                {
                    Title = "WebApi sync call sample",
                    Author = "Naresh Vadala"
                };

                var response = httpClient.PostAsJsonAsync<Post>("/posts", post);
                Console.WriteLine(response.Status);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
        }
    }
}