using Microsoft.EntityFrameworkCore;
using Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Entityes
{
    public class NoteContext : DbContext
    {
        /// <summary>
        /// Return all notes from data base
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {

        }
    }
}
