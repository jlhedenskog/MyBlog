using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Utilities;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public PostsController(ApplicationDbContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Post.Include(p => p.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Blog)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .ThenInclude(c => c.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            //if (post.Image != null)
            //{
            //    var binary = Convert.ToBase64String(post.Image);
            //    var ext = Path.GetExtension(post.FileName);
            //    //var ext = post.FileName.Split('.')[1];
            //    string imageDataURl = $"data:image/{ext};base64,{binary}";

            //    ViewData["Image"] = imageDataURl;

            //var myImageHelper = new ImageHelper();
            if (post.Image != null)
            {
                ViewData["Image"] = ImageHelper.GetImage(post.Image, post.FileName);
            }
            else
            {
                ViewData["Image"] = "../../Images/default_post_img.jpg";
            }

            var personId = _userManager.GetUserId(User);

            ViewData["UserLiked"] = post.Likes.Any(l => l.BlogUserId == personId);

            return View(post);
        }

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Name");
                return View();
            }

            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var newpost = new Post { BlogId = (int)id };
            return View(newpost);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,Image,IsReadyForPublicViewing")] Post post, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;



                if (image != null)
                {
                    post.FileName = image.FileName;
                    post.Image = ImageHelper.PutImage(image);
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Name", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,Slug,Image,Created,Updated,IsReadyForPublicViewing,Created")] Post post, IFormFile image)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.Updated = DateTime.Now;

                    if (image != null)
                    {
                        post.Image = ImageHelper.PutImage(image);
                        post.FileName = image.FileName;
                    }

                    _context.Update(post);
                    if (image == null)
                    {
                        _context.Entry(post).Property(p => p.Image).IsModified = false;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blog, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        public async Task<IActionResult> BlogPosts(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            ViewData["BlogName"] = blog.Name;
            ViewData["BlogId"] = blog.Id;

            var blogPosts = await _context.Post.Where(p => p.BlogId == id).ToListAsync();
            return View(blogPosts);
        }

        [HttpPost]
        public async Task PostLike([FromForm] bool boolLike, int postId)
        {
            var personId = _userManager.GetUserId(User);

            if (boolLike)
            {
                _context.Like.Add(new Like { BlogUserId = personId, PostId = postId });
            }
            else
            {
                var likeToRemove = await _context.Like.FirstOrDefaultAsync(l => l.BlogUserId == personId && l.PostId == postId);
                _context.Like.Remove(likeToRemove);
            }

            await _context.SaveChangesAsync();
        }
    }
}
