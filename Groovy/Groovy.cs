using Sifteo;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Groovy
{
  public class Groovy : BaseApp
  {
	
	public String[] mImageNames;
	public List<CubeWrapper> mWrappers = new List<CubeWrapper>();
	Random mRandomizer = new Random();	
		
    override public int FrameRate
    {
      get { return 20; }
    }

    override public void Setup()
    {
      	Log.Debug("Setup()");
		mImageNames = LoadImageIndex();
		foreach (Cube cube in CubeSet) {
			CubeWrapper wrapper = new CubeWrapper(this, cube);
			mWrappers.Add(wrapper);
			wrapper.ClearScreen();
			DrawRandomWolf();
		}
    }

		
	private String[] LoadImageIndex() {
		Log.Debug ("LoadImageIndex()");
		ImageSet imageSet = this.Images;
		ArrayList nameList = new ArrayList();
		foreach (ImageInfo image in imageSet) {
			nameList.Add(image.name);
			Log.Debug(image.name);
		}
		String[] rv = new String[nameList.Count];
		for (int i=0; i<nameList.Count; i++) {
			rv[i] = (string)nameList[i];
		}
		return rv;
    }
	
	public void DrawRandomWolf() {
		CubeWrapper mWolfCubeWrapper = mWrappers[mRandomizer.Next (mWrappers.Count)];
		foreach (CubeWrapper cubeWrapper in mWrappers) {
			if (mWolfCubeWrapper.Equals(cubeWrapper)) {
				cubeWrapper.ShowMeTheWolf();
			} else {
				cubeWrapper.ClearScreen();
			}
		}
	}

    // development mode only
    // start Groovy as an executable and run it, waiting for Siftrunner to connect
    static void Main(string[] args) { new Groovy().Run(); }
  }
	
	public class CubeWrapper {
		public Groovy mApp;
		public Cube mCube;
		public int mIndex;
	    public int mXOffset = 0;
	    public int mYOffset = 0;
	    public int mScale = 1;
	    public int mRotation = 0;
			
		public bool mNeedDraw = false;
		public bool mShowingWolf = false;
		public bool mWasShowingWolf = false;
		
		public CubeWrapper(Groovy app, Cube cube) {
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mIndex = 0;
			
			// Here we attach more event handlers for button and accelerometer actions.
			mCube.ButtonEvent += OnButton;
			// mCube.TiltEvent += OnTilt;
			mCube.ShakeStartedEvent += OnShakeStarted;
			mCube.ShakeStoppedEvent += OnShakeStopped;
			// mCube.FlipEvent += OnFlip;
		}
		
		public void OnButton(Cube cube, bool pressed) {
			if (!pressed) {
				ToggleWolf();
			}
		}
		
		public void OnShakeStarted(Cube cube) {
			ClearScreen();
		}
		
		public void OnShakeStopped(Cube cube, int duration) {
			mApp.DrawRandomWolf();
		}
		
		public void ToggleWolf() {
			if (mShowingWolf) {
				ClearScreen();
			} else {
				ShowMeTheWolf();
			}
		}
		
		public void ShowMeTheWolf() {
			mWasShowingWolf = false;
			mShowingWolf = true;
			String imageName = this.mApp.mImageNames[0];
			int screenX = mXOffset;
      		int screenY = mYOffset;
			int imageX = 0;
      		int imageY = 0;
			int width = 128;
			int height = 128;
			int scale = mScale;
			int rotation = mRotation;
			mCube.Image(imageName, screenX, screenY, imageX, imageY, width, height, scale, rotation);
			mCube.Paint();
		}
		
		public void ClearScreen() {
			mCube.FillScreen (new Color(0,0,0));
			mCube.Paint ();
			if (mShowingWolf) {
				mWasShowingWolf = true;
			}
			mShowingWolf = false;
		}

    }
}
