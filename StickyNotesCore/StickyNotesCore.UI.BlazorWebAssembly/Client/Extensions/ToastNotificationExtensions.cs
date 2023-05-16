using BlazorStrap;

namespace StickyNotesCore.UI.BlazorWebAssembly.Client.Extensions
{
	public static class ToastNotificationExtensions
	{
		public static void Success(this Toaster toaster, string message)
		{
			toaster.Add(null, message, options =>
			{
				options.Color = BSColor.Success;
				options.CloseAfter = 5000;
				options.Toast = Toast.BottomCenter;
			});
		}

		public static void Error(this Toaster toaster, string message)
		{
			toaster.Add("Error", message, options =>
			{
				options.Color = BSColor.Danger;
				options.CloseAfter = 5000;
				options.Toast = Toast.BottomCenter;
			});
		}
	}
}
