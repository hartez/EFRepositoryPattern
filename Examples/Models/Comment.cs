using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Examples.Models {
    public class Comment {
        // A property named ID is treated as a PK by default
        public int ID { get; set; }

        // Naming it PostID makes it a foreign key into the Posts table
        public int PostID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public string Author { get; set; }
    }
}
