using AutoMapper;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Resources.Notes;

namespace StickyNotesCore.API.Mapping
{
	public class Maps : Profile
	{
		public Maps()
		{
			CreateMap<Note, NoteResource>();
			CreateMap<CreateNoteResource, CreateNoteCommand>();
			CreateMap<PatchNoteResource, PatchNoteCommand>();
		}
	}
}
