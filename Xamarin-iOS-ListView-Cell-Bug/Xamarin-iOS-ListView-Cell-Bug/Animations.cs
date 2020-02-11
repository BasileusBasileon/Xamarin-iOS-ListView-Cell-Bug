using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_iOS_ListView_Cell_Bug
{
	public static class Animations
	{
		public static Task<bool> LayoutBoundsTo(this VisualElement element, Rectangle posFrom, Rectangle posTo, uint length = 250, Easing easing = null)
		{
			Rectangle transform(double t)
				=> new Rectangle(
					posFrom.X + t * (posTo.X - posFrom.X),
					posFrom.Y + t * (posTo.Y - posFrom.Y),
					posFrom.Width + t * (posTo.Width - posFrom.Width),
					posFrom.Height + t * (posTo.Height - posFrom.Height)
				);

			void callback(Rectangle rect)
				=> AbsoluteLayout.SetLayoutBounds(element, rect);

			return DoAnimation(element, nameof(LayoutBoundsTo), transform, callback, length, easing);
		}

		private static Task<bool> DoAnimation<T>(VisualElement element, string name, Func<double, T> transform, Action<T> callback, uint length, Easing easing)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			element.Animate(name, transform, callback, 16, length, easing ?? Easing.Linear, (v, r) => tcs.TrySetResult(r));
			return tcs.Task;
		}
	}
}
