using FreePosting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreePosting.Models
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public bool CanDelete { get; set; }
    }
}