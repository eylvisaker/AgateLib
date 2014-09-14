using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AgateLib.Platform.WindowsStore
{
	public abstract class RenderTargetAdapterBase : IRenderTargetAdapter
	{
		static Dictionary<VirtualKey, KeyCode> mKeyMap = new Dictionary<VirtualKey, KeyCode>();

		static RenderTargetAdapterBase()
		{
			mKeyMap.Add(VirtualKey.A, KeyCode.A);
			mKeyMap.Add(VirtualKey.B, KeyCode.B);
		}

		Size mSize;

		protected abstract Grid RenderTargetControl { get; }

		public DisplayLib.DisplayWindow Owner { get; set; }

		public Geometry.Size Size
		{
			get { return mSize; }
			private set { mSize = value; }
		}

		public event EventHandler Disposed;
		public abstract void BindContextToRenderTarget(SharpDX.SimpleInitializer.SharpDXContext context);

		public void DetachEvents()
		{
		}
		public void AttachEvents()
		{
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
			RenderTargetControl.SizeChanged += mRenderTarget_SizeChanged;

			RenderTargetControl.PointerWheelChanged += mRenderTarget_PointerWheelChanged;
			RenderTargetControl.PointerMoved += mRenderTarget_PointerMoved;
			RenderTargetControl.PointerPressed += mRenderTarget_PointerPressed;
			RenderTargetControl.PointerReleased += mRenderTarget_PointerReleased;

			RenderTargetControl.KeyDown += mRenderTarget_KeyDown;
			RenderTargetControl.KeyUp += mRenderTarget_KeyUp;

			RenderTargetControl.Unloaded += renderTarget_Unloaded;
		}

		void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
		{
			Input.QueueInputEvent(AgateInputEventArgs.KeyDown(
				TransformKey(args.VirtualKey),
				GetKeyModifiers()));
		}

		void renderTarget_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			OnDisposed();
		}

		private void OnDisposed()
		{
			if (Disposed != null)
				Disposed(this, EventArgs.Empty);
		}

		private KeyModifiers GetKeyModifiers()
		{
			return new KeyModifiers(false, false, false);
		}

		private KeyCode TransformKey(Windows.System.VirtualKey virtualKey)
		{
			if (mKeyMap.ContainsKey(virtualKey))
				return mKeyMap[virtualKey];
			else
			{
				System.Diagnostics.Debug.WriteLine("Could not find key map for {0}", virtualKey);
				return KeyCode.None;
			}
		}

		void mRenderTarget_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.KeyDown(
				TransformKey(e.Key),
				GetKeyModifiers()));
		}
		void mRenderTarget_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.KeyUp(
				TransformKey(e.Key),
				GetKeyModifiers()));
		}

		void mRenderTarget_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.MouseUp(Owner,
				e.GetCurrentPoint(RenderTargetControl).Position.ToAgatePoint(), MouseButton.Primary));
		}

		void mRenderTarget_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.MouseDown(Owner,
				e.GetCurrentPoint(RenderTargetControl).Position.ToAgatePoint(), MouseButton.Primary));
				
		}

		void mRenderTarget_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			Input.QueueInputEvent(AgateInputEventArgs.MouseMove(Owner,
				e.GetCurrentPoint(RenderTargetControl).Position.ToAgatePoint()));
		}

		void mRenderTarget_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
		}
		void mRenderTarget_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			mSize = e.NewSize.ToAgateSize();
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
