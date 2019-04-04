using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deadlines.Context;
using Deadlines.Exceptions;
using Deadlines.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Deadlines.Repository
{
    public class DeadlineRepository : IDeadlineRepository
    {
        private readonly DeadlineDBContext _context;

        public DeadlineRepository(DeadlineDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Context.Models.Deadlines>> GetDeadlines()
        {
            return await _context.Deadlines.ToListAsync();
        }

        public async Task<Context.Models.Deadlines> GetDeadlineById(int id)
        {
            var deadlines = await _context.Deadlines.FindAsync(id);

            if (deadlines == null)
            {
                throw new NotFoundException();
            }

            return deadlines;
        }

        public async Task<int> UpdateDeadline(int id, Context.Models.Deadlines deadlines)
        {
            if (id != deadlines.Id)
            {
                throw new BadRequestException();
            }

            _context.Entry(deadlines).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeadlinesExists(id))
                {
                    throw new NotFoundException();
                }

                throw;
            }
        }

        public async Task<Context.Models.Deadlines> InsertDeadline(Context.Models.Deadlines deadlines)
        {
            _context.Deadlines.Add(deadlines);
            await _context.SaveChangesAsync();

            return deadlines;
        }

        public async Task<Context.Models.Deadlines> DeleteDeadline(int id)
        {
            var deadlines = await _context.Deadlines.FindAsync(id);
            if (deadlines == null)
            {
                throw new NotFoundException();
            }

            _context.Deadlines.Remove(deadlines);
            await _context.SaveChangesAsync();

            return deadlines;
        }

        private bool DeadlinesExists(int id)
        {
            return _context.Deadlines.Any(e => e.Id == id);
        }
    }
}
