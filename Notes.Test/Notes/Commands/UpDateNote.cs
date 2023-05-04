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
    public class UpDateNote : TestCommandBase
    {
        [Fact]
        public async Task UpdateNote_Success()
        {
            //Arrange
            var context = NotesContextFactory.Create();
            var controller = new NoteController(context);
            Note note = context.Notes.Where(note => note.Id == NotesContextFactory.NoteIdForUpdate).FirstOrDefault();
            string newText = "New Text";
            note.Text = newText;

            //Act
            await controller.ChangeNote(note);

            //Assert
            Assert.NotNull(context.Notes.Where(note => note.Id == NotesContextFactory.NoteIdForUpdate && note.Text == newText).FirstOrDefault());
        }
    }
}
