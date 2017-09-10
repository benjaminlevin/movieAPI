using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace movie_API
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;
        private readonly APIKeyOptions _APIKey;

        public HomeController(DbConnector connect, IOptions<APIKeyOptions> APIKey)
        {
            _dbConnector = connect;
            _APIKey = APIKey.Value;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/load")]
        public JsonResult Load(){
            List<Dictionary<string, object>> movieDB = _dbConnector.Query("SELECT * FROM movies ORDER BY id DESC");
            int i = 0;
            while(i < movieDB.Count)
            {
                DateTime rawDate = (DateTime)movieDB[i]["release_date"];
                string formatDate = rawDate.ToString("MMMM dd, yyyy");
                movieDB[i]["release_date"] = formatDate;
                i++;
            }
            ViewBag.Movies=(List<Dictionary<string,object>>)movieDB;
            return Json(movieDB);    
        }

        [HttpPost]
        [Route("/search")]
        public IActionResult Search(SearchModel model){
            var Movie = new Dictionary<string, object>();
            if(model.SearchVariable.Length == 0)
            {
                Console.WriteLine("empty search"); //this isn't working, maybe not necessary? --no impact on site functionality
            }
            else
            {
                WebRequest.GetMovieDataAsync(model.SearchVariable, _APIKey.Key, MovieInfo =>
                {
                    Movie["title"] = (string)MovieInfo["results"][0]["title"];
                    Movie["average_rating"] = (string)MovieInfo["results"][0]["vote_average"];
                    Movie["release_date"] = (string)MovieInfo["results"][0]["release_date"];
                }
                ).Wait();
                //Variables for convenience
                var title = Movie["title"];
                var rating = Movie["average_rating"];
                var release = Movie["release_date"];
                //Preventing Duplicate DB Entry
                List<Dictionary<string, object>> duplicateMovieCheck = _dbConnector.Query($"SELECT * FROM movies WHERE title='{title}' AND vote_average='{rating}';");
                if(duplicateMovieCheck.Count == 0)
                {
                    string query = $"INSERT INTO movies (title, vote_average, release_date) VALUES('{title}', '{rating}', '{release}');";
                    _dbConnector.Execute(query);
                }
                else
                    {
                        Console.WriteLine("already searched, not saved to db");
                    }
            }
            return new EmptyResult();
        }
    }
}