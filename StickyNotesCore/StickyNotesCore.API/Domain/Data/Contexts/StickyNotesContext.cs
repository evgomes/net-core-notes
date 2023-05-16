using Microsoft.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Models;

namespace StickyNotesCore.API.Domain.Data.Contexts
{
	public class StickyNotesContext : DbContext
	{
		public virtual DbSet<Note> Notes { get; set; }

        public StickyNotesContext()
        {
        }

        public StickyNotesContext(DbContextOptions<StickyNotesContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Note>().Property(note => note.Text).HasMaxLength(255);
		}
	}
}