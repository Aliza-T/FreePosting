using FreePosting.Data;
using FreePosting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreePosting.Models;

namespace FreePosting.Controllers
{

    public class HomeController : Controller
    {
        PostDb postsdb = new PostDb(Properties.Settings.Default.ConStr);

        public ActionResult Index()
        {
            //HttpCookie PersonsId = Request.Cookies["PersonsId"];
            IEnumerable<Post> posting = postsdb.GetPosts();
            //List<string> Ids = new List<string>();
            List<PostsViewModel> posts = new List<PostsViewModel>();
            //if (PersonsId != null)
            //{
            //    Ids = PersonsId.Value.Split(',').ToList();
            //}

            foreach (Post p in posting)
            {
                var postViewModel = new PostsViewModel();
                postViewModel.Post = p;
                if (Session["CanDelete"] != null)
                {
                    List<int> check = (List<int>)Session["CanDelete"];
                    postViewModel.CanDelete = check.Contains(p.Id);
                }

                posts.Add(postViewModel);
            }

            return View(posts);

        }

        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPost(Post Post)
        {
            PostDb posts = new PostDb(Properties.Settings.Default.ConStr);
            int id = posts.InsertPost(Post);
            //string Ids = "";
            //HttpCookie cookie = Request.Cookies["PersonsId"];
            //if (cookie != null)
            //{
            //    Ids = $"{cookie.Value},";
            //}
            //Ids += Id;
            //Response.Cookies.Add(new HttpCookie("PersonsId", Ids));
            Session["CanDelete"] = new List<int>();
            if (Session["CanDelete"] == null)
            {
                Session["CanDelete"] = new List<int>();
            }
            List<int> ids = (List<int>)Session["CanDelete"];
            ids.Add(id);

            return Redirect("/home/Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            postsdb.Delete(id);
            return Redirect("/home/Index");
        }
    }
}