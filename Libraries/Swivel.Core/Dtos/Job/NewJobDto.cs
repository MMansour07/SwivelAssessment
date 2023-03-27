using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace Swivel.Core.Dtos.Job
{
    public class NewJobDto
    {
        [Required(ErrorMessage = "Required Field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public double Salary { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Experience { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int Vacancies { get; set; }
        public string UserId { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
        public List<string> Titles { get; set; }

        public Swivel.Core.Model.Job ToJob(NewJobDto src, Swivel.Core.Model.Job dest, ICollection<Swivel.Core.Model.Media> lst)
        {
            dest.Medias = lst;
            int x = 0;
            // worest case scenario iterate 3 times
            foreach (var item in dest.Medias)
            {
                item.Title = src.Titles[x];
                x++;
            }

            return dest;
        }
    }
}