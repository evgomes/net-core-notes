using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Tests.EndToEnd
{
	[TestClass]
	public class IndexPageTests : PageTest
	{
		[TestMethod]
		public async Task Should_Render_Add_Note_Card()
		{
			await Page.GotoAsync("http://localhost:5001/");
			await Expect(Page.Locator("text=Add Note").First).ToBeVisibleAsync();
		}

		[TestMethod]
		public async Task Should_Render_Status_Card()
		{
			await Page.GotoAsync("http://localhost:5001/");
			await Expect(Page.Locator("text=Status").First).ToBeVisibleAsync();
			await Expect(Page.Locator("text=Total notes").First).ToBeVisibleAsync();
			await Expect(Page.Locator("text=Last update").First).ToBeVisibleAsync();
		}

		[TestMethod]
		public async Task Given_Empty_Note_On_Add_Note_Text_Field_Should_Display_Error_When_Users_Tries_To_Add_Note()
		{
			await Page.GotoAsync("http://localhost:5001/");

			var button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add Note" });
			await button!.ClickAsync();

			await Expect(Page.Locator("text=The text is required.")).ToBeVisibleAsync();
		}

		[TestMethod]
		public async Task Given_Valid_Note_On_Add_Note_Text_Field_Should_Add_Note()
		{
			await Page.GotoAsync("http://localhost:5001/");

			var input = Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "note" });
			await input.FillAsync("Sample Note - Browser Test");

			var button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add Note" });
			await button!.ClickAsync();

			await Expect(Page.Locator("text=Created on").Last).ToBeVisibleAsync();

			var inputs = await Page.QuerySelectorAllAsync("textarea");
			var textarea = inputs.Last();

			Assert.AreEqual("Sample Note - Browser Test", await textarea.InputValueAsync());
		}
	}
}