using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class NewsHeader
    {
        [Key]
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDesc { get; set; }
        public DateTime NewsCreateDate { get; set; }
        public DateTime NewsPublishDate { get; set; }
        public Boolean IsPublished { get; set; }
#nullable enable
        //News can be typeof Article or as Changelog
        public int? NewsContentID { get; set; }
        public virtual NewsContent? NewsContent { get; set; }
        public int? NewsUpdateID { get; set; }
        public virtual NewsUpdate? NewsUpdate { get; set; }
#nullable disable
    }
}
