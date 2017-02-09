using hackernews.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.Interfaces
{
    public interface ILauncher
    {
        List<Post> getPosts(int nuberOfPostRequested);
    }
}
