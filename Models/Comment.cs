using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Comment
    {
        #region Keys
        public int Id { get; set; }

        public int PostId { get; set; }

        public string BlogUserId { get; set; }
        #endregion

        #region Comment Properties
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
        #endregion

        #region Navigation
        public virtual Post Post { get; set; }

        public  virtual BlogUser BlogUser { get; set; }
        #endregion

        public string FormatCreateDate()
        {
            return this.Created.ToString("dddd, dd MMMM yyyy");
        }
    }
}
