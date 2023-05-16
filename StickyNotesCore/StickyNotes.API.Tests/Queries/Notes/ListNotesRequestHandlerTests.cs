using Moq;
using Moq.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Data.Queries.Notes;
using StickyNotesCore.API.Domain.Data.Queries.Shared;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.API.Domain.Queries.Notes;
using StickyNotesCore.API.Queries.Notes;

namespace StickyNotesCore.API.Tests.Tests.Queries.Notes
{
	public class ListNotesRequestHandlerTests
	{
		private Mock<StickyNotesContext> _context;
		private ListNotesRequestHandler _commandHandler;

		public ListNotesRequestHandlerTests()
		{
			_context = new();
			_commandHandler = new ListNotesRequestHandler(_context.Object);
		}

		[Trait("Category", "List Notes")]
		[Fact]
		public async Task Should_Return_Empty_Query_Result_When_There_Are_No_Notes_In_The_Database()
		{
			// Arrange
			var notes = new List<Note>();

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			// Act
			var response = await _commandHandler.Handle(new ListNotesRequest
			{
				Query = new NotesQuery
				{
					Page = 1,
					ItemsPerPage = 10,
					SortOrder = SortOrder.Ascending,
				}
			}, CancellationToken.None);

			Assert.NotNull(response);
			Assert.Equal(0, response.TotalItems);
			Assert.Empty(response.Items);
		}

		[Trait("Category", "List Notes")]
		[Fact]
		public async Task Should_Return_Notes_According_To_Query()
		{
			// Arrange
			var notes = new List<Note>
			{
				new Note
				{
					Id = Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"),
					Text = "A Note",
					CreatedOn = DateTime.UtcNow,
				},
				new Note
				{
					Id = Guid.Parse("e0abfb9c-a178-4cf4-b3fa-fa50dc790e5a"),
					Text = "B Note",
					CreatedOn = DateTime.UtcNow,
				},
				new Note
				{
					Id = Guid.Parse("2d7b3fed-f68e-40e3-9ce3-fdefcd29c9e1"),
					Text = "C Note",
					CreatedOn = DateTime.UtcNow,
				},
			};

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			// Act
			var response = await _commandHandler.Handle(new ListNotesRequest
			{
				Query = new NotesQuery
				{
					Text = "Note",
					Page = 1,
					ItemsPerPage = 10,
					OrderBy = nameof(Note.Text),
					SortOrder = SortOrder.Descending,
				}
			}, CancellationToken.None);

			// Assert
			Assert.NotNull(response);
			Assert.Equal(3, response.TotalItems);
			Assert.Equal("C Note", response.Items[0].Text);
			Assert.Equal("B Note", response.Items[1].Text);
			Assert.Equal("A Note", response.Items[2].Text);
		}
	}
}
