using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.Entities
{
    public class Post
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Author { get; set; }
        public int Points { get; set; }
        public int Comments { get; set; }
        public int Rank { get; set; }
    }
}
