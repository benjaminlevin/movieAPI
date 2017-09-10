using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
 
namespace movie_API
{
    public class WebRequest
    {
        public static async Task GetMovieDataAsync(string MovieSearch, string Key, Action<JObject> Callback) //key received from controller
        {
            // Create a temporary HttpClient connection.
            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri($"https://api.themoviedb.org/3/search/movie?api_key={Key}&query={MovieSearch}"); //modified to secure key
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                    // Then parse the result into JSON and convert to a dictionary that we can use.
                    // DeserializeObject will only parse the top level object, depending on the API we may need to dig deeper and continue deserializing
                    JObject rss = JObject.Parse(StringResponse);
                     
                    // Finally, execute our callback, passing it the response we got.
                    Callback(rss);
                }
                catch (HttpRequestException e)
                {
                    // If something went wrong, display the error.
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }
    }
}