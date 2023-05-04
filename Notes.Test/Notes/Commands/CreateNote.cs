using Application.Controllers;
using Microsoft.Extensions.Logging;
using Notes.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Test.Notes.Commands
{
    public class CreateNote : TestCommandBase
    {
        [Fact]
        public async Task CreateNewNote_Success()
        {
            //Arrange
            var context = NotesContextFactory.Create();
            var controller = new NoteController(context);
            int noteId = 5;
            Note note = new Note() { Author = "Author", Text = "Text", Id = noteId, Time = DateTime.Now };

            //Act
            await controller.AddNote(note);

            //Assert
            Assert.NotNull(context.Notes.Where(note => note.Id == noteId).FirstOrDefault());
        }
    }
}
