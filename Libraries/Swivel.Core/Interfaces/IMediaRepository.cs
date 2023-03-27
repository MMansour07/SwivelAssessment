using Swivel.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Swivel.Core.Interfaces
{
    public interface IMediaRepository : IRepository<Media>
    {
        void UpdateMedia(int JobId, List<string> Titles, List<string> PublicIds);
    }
}
