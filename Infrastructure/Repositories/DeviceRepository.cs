using Domain.Entities;
using Domain.Queries;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class DeviceRepository(ApplicationDbContext _context)
    {
        public async Task<Device?> GetByIdAsync(Guid id)
            => await _context.Devices.FindAsync(id);

        public async Task<List<Device>> GetAsync(DeviceFilter filter)
        {
            var query = _context.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Brand))
                query = query.Where(d =>  d.Brand == filter.Brand);

            if (filter.State is not null)
                query = query.Where(d =>  d.State == filter.State.Value);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Device device)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
    }
}
