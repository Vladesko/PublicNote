using Application.Entityes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Test.Common
{
    public static class NotesContextFactory
    {
        public static int UserAId = 1;
        public static int UserBId = 2;

        public static int NoteIdForDelete = 3;
        public static int NoteIdForUpdate = 4;

        public static NoteContext Create()
        {
            var options = new DbContextOptionsBuilder<NoteContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new NoteContext(options);
            context.Database.EnsureCreated();

            context.Notes.AddRange(
                new Note 
                {
                    Author = "Author1",
                    Text = "Text1",
                    Time = DateTime.Now,
                    Id = UserAId
                },
                new Note
                {
                    Author = "Author2",
                    Text = "Text2",
                    Time = DateTime.Now,
                    Id = UserBId
                },
                new Note
                {
                    Author = "Author3",
                    Text = "Text3",
                    Time = DateTime.Now,
                    Id = NoteIdForUpdate
                },
                 new Note
                 {
                     Author = "Author4",
                     Text = "Text4",
                     Time = DateTime.Now,
                     Id = NoteIdForDelete
                 }
                );
            context.SaveChanges();
            return context;
        }
        public static void Destroy(NoteContext context)
        {
            context.Database.EnsureCreated();
            context.Dispose();
        }
    }
}
