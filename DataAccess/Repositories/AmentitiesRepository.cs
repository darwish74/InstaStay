﻿using DataAccess.Data;
using Models.IRepositories;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Repositories
{
    public class AmentitiesRepository : BaseRepository<Amentities>,IAmentitiesRepository
    {
        public AmentitiesRepository(ApplicationDbContext _context) : base(_context)
        {

        }
    }
}
