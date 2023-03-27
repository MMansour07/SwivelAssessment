using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Swivel.Data.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Swivel.Data.Repositories
{
    public class MediaRepository : Repository<Media>, IMediaRepository
    {
        private readonly DataContext _context;
        public MediaRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        
        public void UpdateMedia(int JobId, List<string> Titles, List<string> PublicIds)
        {
            var MediaList = _context.Medias.Where(x => x.JobId == JobId).AsNoTracking();
            int i = 0;
            if(MediaList != null && MediaList.Count() > 0)
            {
                //worest case 3 times 
                //_context.Medias.update(item);
                foreach (var item in MediaList)
                {
                    if(PublicIds == null || !PublicIds.Contains(item.PublicId))
                    {
                        item.Title = Titles[i];

                        _context.Medias.Attach(item);
                        _context.Entry(item).State = EntityState.Modified;

                        i++;
                    }
                }
            }
        }
    }
}
