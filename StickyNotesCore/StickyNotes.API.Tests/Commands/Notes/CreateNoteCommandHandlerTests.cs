using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using StickyNotesCore.API.Commands.Notes;
using StickyNotesCore.API.Domain.Commands.Notes;
using StickyNotesCore.API.Domain.Data.Contexts;
using StickyNotesCore.API.Domain.Models;
using StickyNotesCore.Shared.Responses;

namespace StickyNotesCore.API.Tests.Tests.Commands.Notes
{
	public class CreateNoteCommandHandlerTests
	{
		private Mock<StickyNotesContext> _context;
		private Mock<ILogger<CreateNoteCommandHandler>> _logger;

		private CreateNoteCommandHandler _commandHandler;

		public CreateNoteCommandHandlerTests()
		{
			_context = new();
			_logger = new();

			_commandHandler = new CreateNoteCommandHandler(_context.Object, _logger.Object);
		}


		[Trait("Category", "Create Note")]
		[Fact]
		public async Task Should_Create_Note()
		{
			// Arrange
			var notes = new List<Note>();

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			_context
				.Setup(x => x.Notes.Add(It.IsAny<Note>()))
				.Callback((Note note) =>
				{
					note.Id = Guid.NewGuid();
					note.CreatedOn = DateTime.UtcNow;
				});

			// Act
			var response = await _commandHandler.Handle(new CreateNoteCommand { Text = "Sample Text" }, CancellationToken.None);

			// Assert
			Assert.True(response.Success);
			Assert.Equal(Status.Created, response.Status);
			Assert.NotNull(response.Resource);

			Assert.NotEqual(Guid.Empty, response.Resource.Id);
			Assert.NotEqual(default, response.Resource.CreatedOn);
			Assert.Equal("Sample Text", response.Resource.Text);
		}
	}
}
