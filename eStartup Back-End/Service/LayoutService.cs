using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStartup_Back_End.DAL;
using eStartup_Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace eStartup_Back_End.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Setting>> GetDatas()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return settings;
        }
    }
}
