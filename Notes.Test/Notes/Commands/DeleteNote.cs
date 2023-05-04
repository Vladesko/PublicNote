using Application.Controllers;
using Notes.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Test.Notes.Commands
{
    public class DeleteNote : TestCommandBase
    {
        [Fact]
        public async Task DeleteNote_Success()
        {
            //Arrange
            var context = NotesContextFactory.Create();
            var controller = new NoteController(context);
            Note note = context.Notes.Where(note => note.Id == NotesContextFactory.NoteIdForDelete).FirstOrDefault();

            //Act
            await controller.RemoveNote(note);

            //Assert
            Assert.Null(context.Notes.Where(note => note.Id == NotesContextFactory.NoteIdForDelete).FirstOrDefault());
        }
        [Fact]
        public async Task DeleteNoteWithNotFoundNote_WrongId()
        {
            //Arrange
            var context = NotesContextFactory.Create();
            var controller = new NoteController(context);
            int notFoundId = 10;
            Note note = context.Notes.Where(note => note.Id == notFoundId).FirstOrDefault();

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await controller.RemoveNote(note));
        }
    }
}
