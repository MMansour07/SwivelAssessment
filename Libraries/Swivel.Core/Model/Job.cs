using System.Collections.Generic;

namespace Swivel.Core.Model
{
    public class Job : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public string Experience { get; set; }
        public int Vacancies { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Media> Medias { get; set; }
    }
}
