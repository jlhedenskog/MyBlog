using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Tag
    {
        #region Keys
        public int Id { get; set; }

        public int PostId { get; set; }
        #endregion

        #region Tag Properties
        public string Name { get; set; }
        #endregion

        #region Navigation
        public virtual Post Post { get; set; }
        #endregion
    }
}
