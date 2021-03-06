﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Post
    {
        #region Keys
        public int Id { get; set; }
        [Display(Name = "Blog")]
        public int BlogId { get; set; }
        #endregion

        #region Post Properties
        [StringLength(100, MinimumLength = 6)]
        public string Title { get; set; }

        [StringLength(300, MinimumLength = 6)]
        public string Abstract { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        //Incorporate the code to turn this data into a useable image right inside the Get();
        public byte[] Image { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsReadyForPublicViewing { get; set; }
        #endregion

        #region Navigation
        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        #endregion

        //public string SearchString { get; set; }

        public Post()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
            Likes = new HashSet<Like>();
        }

        public string FormatCreateDate()
        {
            return this.Created.ToString("dddd, dd MMMM yyyy");
        }

    }
}
