using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BookOrganizer
{
    public class API_Data
    {
        [JsonProperty("books")]
        public Books[] Books { get; set; }        
    }

    public class Books
    {
        [JsonProperty("book")]
        public Book Book { get; set; }
    }

    public class Book
    {
        [JsonProperty("authors_list")]
        public string Author { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("publish_year")]
        public string Year { get; set; }

        [JsonProperty("pages_count")]
        public string Pages { get; set; }

        [JsonProperty("annotation")]
        public string Annotation { get; set; }

    }
}


