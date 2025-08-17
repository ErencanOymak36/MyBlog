using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsPublished { get; set; }



        private BlogPost() { }

        public BlogPost(string title,string content,string authorName)
        {

            // İş kuralları burada
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Blog başlığı boş olamaz", nameof(title));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Blog içeriği boş olamaz", nameof(content));
            if (string.IsNullOrWhiteSpace(authorName))
                throw new ArgumentException("Yazar adı boş olamaz", nameof(authorName));
            if (title.Length > 200)
                throw new ArgumentException("Blog başlığı 200 karakterden uzun olamaz", nameof(title));

            Title = title;
            Content = content;
            AuthorName = authorName;
            CreatedAt = DateTime.Now;
            UpdateAt = DateTime.Now;
            IsPublished = false;
        }

        public void Update(string title,string content)
        {

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Blog başlığı boş olamaz", nameof(title));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Blog içeriği boş olamaz", nameof(content));
            if (title.Length > 200)
                throw new ArgumentException("Blog başlığı 200 karakterden uzun olamaz", nameof(title));


            Title = title;
            Content = content;
            UpdateAt= DateTime.Now;
        }

        public void Publish()
        {
            IsPublished = true;
            UpdateAt=DateTime.Now;
        }
        
        public void UnPublish()
        {
            IsPublished=false;
            UpdateAt=DateTime.Now;
        }

        public string GetTimeAgo()
        {
            var timeSpan = DateTime.UtcNow - CreatedAt;
            if (timeSpan.Days > 0)
                return $"{timeSpan.Days} gün önce";
            else if (timeSpan.Hours > 0)
                return $"{timeSpan.Hours} saat önce";
            else if (timeSpan.Minutes > 0)
                return $"{timeSpan.Minutes} dakika önce";
            else
                return "Az önce";
        }
    }
    

}
