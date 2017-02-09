using hackernews.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.Interfaces
{
    public interface IScraper
    {
        List<Post> parse();
        List<Post> readDocument(int page, int numberOfPostRemain);
        List<Post> parse(int _nuberOfPostRequested);
    }
}
