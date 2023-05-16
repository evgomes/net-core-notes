using AutoMapper;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Queries.Notes;
using StickyNotesCore.API.Domain.Data.Queries.Shared;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Resources.Notes;
using StickyNotesCore.Shared.Resources.Queries;

namespace StickyNotesCore.API.Mapping
{
	public class Maps : Profile
	{
		public Maps()
		{
			CreateMap<Note, NoteResource>();

			CreateMap<CreateNoteResource, CreateNoteCommand>();
			CreateMap<PatchNoteResource, PatchNoteCommand>();

			CreateMap<NotesQueryResource, NotesQuery>();
			CreateMap<QueryResult<Note>, QueryResultResource<NoteResource>>();
		}
	}
}
