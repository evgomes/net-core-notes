using Moq;
using Moq.EntityFrameworkCore;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.API.Domain.Queries.Notes;
using StickyNotesCore.API.Queries.Notes;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Tests.Tests.Queries.Notes
{
	public class GetNoteByIdRequestHandlerTests
	{
		private Mock<StickyNotesContext> _context; 
		private GetNoteByIdRequestHandler _commandHandler;

		public GetNoteByIdRequestHandlerTests()
		{
			_context = new();
			_commandHandler = new GetNoteByIdRequestHandler(_context.Object);
		}

		[Trait("Category", "Get Note by ID")]
		[Fact]
		public async Task Should_Return_Not_Found_For_Invalid_Note_Id()
		{
			// Arrange
			var notes = new List<Note>();

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			var query = BuildQuery();

			// Act
			var response = await _commandHandler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(response.Success);
			Assert.Equal(Status.NotFound, response.Status);
			Assert.Matches("not found", response.Message);
		}

		[Trait("Category", "Get Note by ID")]
		[Fact]
		public async Task Should_Return_Note_by_ID()
		{

			var notes = new List<Note>
			{
				new Note
				{
					Id = Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"),
					Text = "Sample Note",
					CreatedOn = DateTime.UtcNow,
				}
			};

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			var query = BuildQuery();

			// ACt
			var response = await _commandHandler.Handle(query, CancellationToken.None);

			Assert.True(response.Success);
			Assert.Equal(Status.Completed, response.Status);
			Assert.NotNull(response.Resource);
			Assert.Equal(Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"), response.Resource.Id);
			Assert.Equal("Sample Note", response.Resource.Text);
			Assert.Equal(Status.Completed, response.Status);
		}

		private GetNoteByIdRequest BuildQuery()
		{
			return new GetNoteByIdRequest
			{
				Id = Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"),
			};
		}
	}
}
