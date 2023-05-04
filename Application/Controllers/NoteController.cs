using Application.Entityes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteContext context;
        public NoteController(NoteContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Get list with all notes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNotes")]
        public List<Note> GetNotes()
        {
            return context.Notes.ToList();
        }
        /// <summary>
        /// Add new note
        /// </summary>
        /// <param name="note">New note</param>
        /// <returns></returns>
        [Authorize(Roles = "User, Administrator, MainAdministrator")]
        [HttpPost("[action]")]
        public async Task AddNote([FromBody] Note note)
        {
            context.Notes.Add(note);
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// Change note
        /// </summary>
        /// <param name="changeNote">Note with new text and old data</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, MainAdministrator")]
        [HttpPost("[action]")]
        public async Task ChangeNote([FromBody] Note changeNote)
        {
            foreach (var note in context.Notes)  //Search old note by Id and change text.
            {                                    //text from change note
                if (note.Id == changeNote.Id)   
                    note.Text = changeNote.Text;
            }
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// Remove note
        /// </summary>
        /// <param name="note">Note is remove</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, MainAdministrator")]
        [HttpPost("[action]")]
        public async Task RemoveNote([FromBody] Note note)
        {
            context.Notes.Remove(note);
            await context.SaveChangesAsync();
        }
    }
}
