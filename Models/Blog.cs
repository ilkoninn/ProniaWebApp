
using Newtonsoft.Json.Linq;

namespace ProniaWebApp.Models
{
    public class Blog : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public Category Category { get; set; }
        public ICollection<BlogTag>? Tags { get; set; }
        public List<BlogImage>? BlogImage { get; set; }
        [NotMapped]
        public string FormatTime
        {
            get
            {
                TimeSpan timeDifference = DateTime.Now - CreatedDate;

                if (timeDifference.TotalSeconds < 60)
                {
                    return "Just Now";
                }
                else if (timeDifference.TotalMinutes < 60)
                {
                    int minutes = (int)timeDifference.TotalMinutes;
                    return $"{minutes} {(minutes == 1 ? "minute" : "minutes")} ago";
                }
                else if (timeDifference.TotalHours < 24)
                {
                    int hours = (int)timeDifference.TotalHours;
                    return $"{hours} {(hours == 1 ? "hour" : "hours")} ago";
                }
                else if (timeDifference.TotalDays < 7)
                {
                    int days = (int)timeDifference.TotalDays;
                    return $"{days} {(days == 1 ? "day" : "days")} ago";
                }
                else
                {
                    int weeks = (int)(timeDifference.TotalDays / 7);
                    return $"{weeks} {(weeks == 1 ? "week" : "weeks")} ago";
                }
            }
        }

    }
}
