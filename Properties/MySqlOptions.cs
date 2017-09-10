//*--Added for secure DB use
//"Properties" directory in root, MySqlOptions.cs contained
//This file contains all 'models'
using System;
using System.ComponentModel.DataAnnotations;

namespace movie_API
{
    public class MySqlOptions
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }

    public class SearchModel
    {
    public string SearchVariable { set;get;}
    }

    public class APIKeyOptions
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}