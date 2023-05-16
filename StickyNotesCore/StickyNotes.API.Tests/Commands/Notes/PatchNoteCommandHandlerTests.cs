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
	public class PatchNoteCommandHandlerTests
	{
		private Mock<StickyNotesContext> _context;
		private Mock<ILogger<PatchNoteCommandHandler>> _logger;

		private PatchNoteCommandHandler _commandHandler;

		public PatchNoteCommandHandlerTests()
		{
			_context = new();
			_logger = new();

			_commandHandler = new PatchNoteCommandHandler(_context.Object, _logger.Object);
		}

		[Trait("Category", "Patch Note")]
		[Fact]
		public async Task Should_Return_Not_Found_For_Invalid_Note_Id()
		{
			// Arrange
			var notes = new List<Note>();

			_context
				.SetupGet(x => x.Notes)
				.ReturnsDbSet(notes);

			var command = BuildCommand();

			// Act
			var response = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(response.Success);
			Assert.Equal(Status.NotFound, response.Status);
			Assert.Matches("not found", response.Message);
		}

		[Trait("Category", "Patch Note")]
		[Fact]
		public async Task Should_Patch_Note()
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

			var command = BuildCommand();

			// Act
			var response = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(response.Success);
			Assert.Equal(Status.Patched, response.Status);
			Assert.NotNull(response.Resource);
			Assert.NotNull(response.Resource.ModifiedOn);
			Assert.Equal(Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"), response.Resource.Id);
			Assert.Equal("Edited Text", response.Resource.Text);
		}

		private PatchNoteCommand BuildCommand()
		{
			return new PatchNoteCommand
			{
				Id = Guid.Parse("b87762bb-3340-4baa-9f94-529ba9a3a44c"),
				Text = "Edited Text",
			};
		}
	}
}
