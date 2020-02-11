using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_iOS_ListView_Cell_Bug
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();

			AbsoluteLayout.SizeChanged += AbsoluteLayout_SizeChanged;
		}

		private Rectangle _animationDestinationLeft, _animationStartLeft, _animationDestinationRight, _animationStartRight;
		private Stack<View> _viewStack = new Stack<View>();

		private void AbsoluteLayout_SizeChanged(object sender, EventArgs e)
		{
			// On Portrait we want to the view to be 75% of the width, on landscape 50%
			double newWidth = (AbsoluteLayout.Width > AbsoluteLayout.Height) ? 0.5 : 0.75;

			// Setting the rectangles used for the animation
			_animationStartLeft = new Rectangle(-2d * newWidth, 0d, newWidth, 1d);
			_animationDestinationLeft = new Rectangle(0d, 0d, newWidth, 1d);
			_animationStartRight = new Rectangle(1d + 2d * newWidth, 0d, newWidth, 1d);
			_animationDestinationRight = new Rectangle(1d, 0d, newWidth, 1d);

			// Changing the size of the animated in views
			foreach (View view in _viewStack)
			{
				AbsoluteLayout.SetLayoutBounds(view, _animationDestinationRight);
			}
		}

		readonly List<string> items = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n" };

		private void Button_Clicked(object sender, EventArgs e)
		{
			ListView listView = new ListView() { BackgroundColor = Color.Blue };
			listView.ItemTemplate = new DataTemplate(() =>
			{
				ViewCell viewCell = new ViewCell();
				BoxView box = new BoxView() { Color = Color.Black };
				viewCell.View = box;
				return viewCell;
			});
			listView.ItemsSource = items;

			_viewStack.Push(listView);
			AbsoluteLayout.Children.Add(listView, new Rectangle(1d, 0d, 0d, 1d), AbsoluteLayoutFlags.All);
			_ = listView.LayoutBoundsTo(_animationStartRight, _animationDestinationRight, 200, Easing.CubicInOut);
		}
	}
}
