using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AgateLib.Platform.WindowsStoreCommon
{
	public class SwapChainBackgroundPanelAdapter : IRenderTargetAdapter
	{
		SwapChainBackgroundPanel mRenderTarget;

		public SwapChainBackgroundPanelAdapter(SwapChainBackgroundPanel renderTarget)
		{
			mRenderTarget = renderTarget;
		}

		public SwapChainBackgroundPanel RenderTarget { get { return mRenderTarget; } }

		public DisplayLib.DisplayWindow Owner { get; set; }

		public event EventHandler Disposed;

		public Geometry.Size Size
		{
			get { return mRenderTarget.RenderSize.ToAgateSize(); }
		}

		public void DetachEvents()
		{
			throw new NotImplementedException();
		}



		public void AttachEvents()
		{
			mRenderTarget.SizeChanged += mRenderTarget_SizeChanged;

			mRenderTarget.PointerWheelChanged += mRenderTarget_PointerWheelChanged;
			mRenderTarget.PointerMoved += mRenderTarget_PointerMoved;
			mRenderTarget.PointerPressed += mRenderTarget_PointerPressed;
			mRenderTarget.PointerReleased += mRenderTarget_PointerReleased;

			mRenderTarget.KeyDown += mRenderTarget_KeyDown;
			mRenderTarget.KeyUp += mRenderTarget_KeyUp;
		}

		void mRenderTarget_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
		}
		void mRenderTarget_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
		}

		void mRenderTarget_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
		}

		void mRenderTarget_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
		}

		void mRenderTarget_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
		}

		void mRenderTarget_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
		}
		void mRenderTarget_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
		}

		//	private void DetachEvents()
		//	{
		//		if (mRenderTarget == null)
		//			return;

		//		throw new NotImplementedException();
		//	}

		//	private void AttachEvents()
		//	{
		//		mRenderTarget.SizeChanged += mRenderTarget_SizeChanged;

		//		mRenderTarget.MouseWheel += mRenderTarget_MouseWheel;
		//		mRenderTarget.MouseMove += mRenderTarget_MouseMove;
		//		mRenderTarget.MouseLeftButtonDown += mRenderTarget_MouseLeftButtonDown;
		//		mRenderTarget.MouseLeftButtonUp += mRenderTarget_MouseLeftButtonUp;
		//		mRenderTarget.DoubleTap += mRenderTarget_DoubleTap;

		//		mRenderTarget.KeyDown += mRenderTarget_KeyDown;
		//		mRenderTarget.KeyUp += mRenderTarget_KeyUp;
		//	}

		//	void mRenderTarget_KeyUp(object sender, KeyEventArgs e)
		//	{
		//		//Input.QueueInputEvent(AgateInputEventArgs.KeyUp(
		//		//	TransformKey(e.Key), e))
		//		//Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
		//	}

		//	void mRenderTarget_KeyDown(object sender, KeyEventArgs e)
		//	{
		//		//Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = true;
		//	}

		//	void mRenderTarget_DoubleTap(object sender, GestureEventArgs e)
		//	{
		//		Mouse.OnMouseDoubleClick(MouseButton.Primary);
		//	}

		//	void mRenderTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		//	{
		//		Debug.WriteLine("Mouse down at: {0}", e.GetPosition(mRenderTarget));
		//		Input.QueueInputEvent(AgateInputEventArgs.MouseDown(
		//			this, e.GetPosition(mRenderTarget).ToAgatePoint(), MouseButton.Primary));
		//	}
		//	void mRenderTarget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//	{
		//		Input.QueueInputEvent(AgateInputEventArgs.MouseUp(
		//			this, e.GetPosition(mRenderTarget).ToAgatePoint(), MouseButton.Primary));
		//	}
		//	void mRenderTarget_MouseMove(object sender, MouseEventArgs e)
		//	{
		//		Input.QueueInputEvent(AgateInputEventArgs.MouseMove(
		//			this, e.GetPosition(mRenderTarget).ToAgatePoint()));
		//	}


		//	void mRenderTarget_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		//	{
		//		mFrameBuffer.SetSize(Size);
		//	}

		//	void mRenderTarget_MouseWheel(object sender, MouseWheelEventArgs e)
		//	{
		//		Mouse.OnMouseWheel(-(e.Delta * 100) / 120);
		//	}


		//	public DisplayLib.DisplayWindow Owner
		//	{
		//		get { throw new NotImplementedException(); }
		//	}


		//	public void AttachEvents()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public void DetachEvents()
		//	{
		//		throw new NotImplementedException();
		//	}

		//	public event EventHandler Disposed;

	}
}
