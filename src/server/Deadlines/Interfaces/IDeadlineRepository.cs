using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Deadlines.Interfaces
{
    public interface IDeadlineRepository
    {
        Task<IEnumerable<Context.Models.Deadlines>> GetDeadlines();
        Task<Context.Models.Deadlines> GetDeadlineById(int id);

        Task<int> UpdateDeadline(int id, Context.Models.Deadlines deadlines);
        Task<Context.Models.Deadlines> InsertDeadline(Context.Models.Deadlines deadline);
        Task<Context.Models.Deadlines> DeleteDeadline(int id);

    }
}
