using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace EFRepositoryPattern.Tests.Models {
    public class Post {
        // A property named ID is treated as a PK by default
        public int ID { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime PublishDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
