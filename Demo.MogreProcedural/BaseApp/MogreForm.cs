using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using Game.Utilitys.SdkTrayUI;
using Mogre;
using Mogre_Procedural.MogreBites;


namespace Mogre_Procedural.Game.BaseApp
{
    public partial class MogreForm : Form
    {

        private readonly Vector2 WND_SIZE = new Vector2(800, 600);
        protected OgreWindow mogreWin;

        public MogreForm() {
            InitializeComponent();
            //
            this.MinimumSize = new Size((int)WND_SIZE.x, (int)WND_SIZE.y);
            this.Resize += new EventHandler(MogreForm_Resize);

        }
        /// <summary>
        /// begin this form show,this must set.
        /// </summary>
        /// <param name="window"></param>
        public void OgreWindowSet(OgreWindow window) {
            mogreWin = window;
        }
        void MogreForm_Resize(object sender, EventArgs e) {

            mogreWin.ResizeWindow(this.Width, this.Height);
        }

        //private void MogreForm_Paint(object sender, PaintEventArgs e) {
        //    mogreWin.Paint();
        //}

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            this.Show();
            //
            //mogreWin = new OgreWindow(new Point(100, 30), this.Handle); 
            mogreWin.Go();
        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            e.Cancel = true;
            base.OnFormClosing(e);
            mogreWin.Dispose();
        }
    }

    public abstract class OgreWindow : SdkTrayListener, IDisposable
    {
        public Root Root { get { return this.mRoot; } }
        public SceneManager SceneMgr { get { return this.mSceneMgr; } }
        public RenderWindow Window { get { return this.mWindow; } }
        public MoisManager Input { get { return this.mInput; } }
        //public MiyagiMgr MiyagiMgr { get { return this.mMiyagiMgr; } }
        public Camera Camera { get { return this.mCam; } }
        public Viewport Viewport { get { return this.mViewport; } }

        private const string PLUGINS_CFG = "plugins.cfg";
        private const string RESOURCES_CFG = "resources.cfg";

        protected Root mRoot;
        protected SceneManager mSceneMgr;
        protected RenderWindow mWindow;
        protected MoisManager mInput;
        protected Camera mCam;
        protected Viewport mViewport;
        protected bool mIsShutDownRequested = false;
        private int mRenderMode = 0;
        protected SdkTrayManager mTrayMgr;
        private ParamsPanel mPanel;

        //public bool OverlayVisibility { get { return this.mDebugOverlay.Visibility; } set { this.mDebugOverlay.Visibility = value; } }
        public void ResizeWindow(int w, int h) {
            mWindow.WindowMovedOrResized();
            mInput.InputSystem_SizeChanged(w, h);
        }
        public void Go() {
#if !DEBUG
            try {
#endif
            if (!this.Setup()) { return; }
            this.mRoot.StartRendering();
            this.Shutdown();
#if !DEBUG
            }
            catch (System.Runtime.InteropServices.SEHException e) {              
                Console.WriteLine(e);
                if (OgreException.IsThrown)
                    ShowOgreException();
                else {
                    System.Windows.Forms.MessageBox.Show(
                    "An Ogre error has occurred. Check the Ogre.log file for details", "Exception",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);

                System.Windows.Forms.MessageBox.Show(
                e.Message, "Error",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
            }
#endif
        }

        private void ShowOgreException() {
            if (OgreException.IsThrown)
                System.Windows.Forms.MessageBox.Show(OgreException.LastException.FullDescription, "An exception has occured!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

        }

        void createLogManager() {

        }
        void setupResources() {
            // Load resource paths from config file
            var cf = new ConfigFile();
            cf.Load(RESOURCES_CFG, "\t:=", true);

            // Go through all sections & settings in the file
            var seci = cf.GetSectionIterator();
            while (seci.MoveNext()) {
                foreach (var pair in seci.Current) {
                    ResourceGroupManager.Singleton.AddResourceLocation(
                        pair.Value, pair.Key, seci.CurrentKey);
                }
            }
        }
        void createCompositor() {
        }
        private bool Setup() {
            createLogManager();
            this.mRoot = new Root(PLUGINS_CFG);
            //mOverlaySystem = new Mogre.OverlaySystem();
            setupResources();
            if (!this.Configure())
                return false;

            this.ChooseSceneManager();
            this.CreateCamera();
            this.CreateViewports();
            createCompositor();

            TextureManager.Singleton.DefaultNumMipmaps = 5;

            this.LoadResources();

            this.mInput = new MoisManager();
            int windowHnd;
            this.mWindow.GetCustomAttribute("WINDOW", out windowHnd);
            this.mInput.Startup(windowHnd, this.mWindow.Width, this.mWindow.Height);
            //System.Windows.Forms.Control form = System.Windows.Forms.Form.FromHandle(new IntPtr(windowHnd));
            //form.MinimumSize = new System.Drawing.Size(800, 600);

            this.mTrayMgr = new SdkTrayManager("trayMgr", this.mWindow, this.mInput.Mouse, this);
            this.mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            this.mTrayMgr.hideCursor();
            //this.mTrayMgr.hideFrameStats();
            //this.mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            //this.mTrayMgr.showLogo(TrayLocation.TL_BOTTOMLEFT);
            //this.mTrayMgr.showOkDialog("context...", "this is a test");
            //this.mTrayMgr.showLoadingBar();


            this.mPanel = this.mTrayMgr.createParamsPanel(TrayLocation.TL_NONE, "DetailsPanel", 200, new string[] { "cam.pX", "cam.pY", "cam.pZ", "cam.oW", "cam.oX", "cam.oY", "cam.oZ", "Filtering", "Poly Mode" });
            this.mPanel.setParamValue(7, "Bilinear");
            this.mPanel.setParamValue(8, "Solid");
            this.mPanel.hide();
            MaterialManager.Singleton.SetDefaultTextureFiltering(TextureFilterOptions.TFO_NONE);
            //
            this.CreateScene();
            //
            this.mTrayMgr._HookMouseEvent();
            this.AddFrameLstn(new RootLstn(RootLstn.TypeLstn.FrameRendering, this.OnFrameRendering));
            return true;
        }

        private bool Configure() {
            /*
            RenderSystem renderSys = mRoot.GetRenderSystemByName("OpenGL Rendering Subsystem");
            renderSys.SetConfigOption("Colour Depth", "16");
            renderSys.SetConfigOption("Display Frequency", "40");
            renderSys.SetConfigOption("FSAA", "0");
            renderSys.SetConfigOption("Full Screen", "No");
            renderSys.SetConfigOption("RTT Preferred Mode", "FBO");
            renderSys.SetConfigOption("VSync", "Yes");
            renderSys.SetConfigOption("VSync Interval", "1");
            renderSys.SetConfigOption("Video Mode", WND_SIZE.x + " x " + WND_SIZE.y);
            renderSys.SetConfigOption("sRGB Gamma Conversion", "No");

            this.mRoot.RenderSystem = renderSys;
            this.mRoot.Initialise(false, "SkyLands");
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = Handle.ToString();
            this.mWindow = this.mRoot.CreateRenderWindow("Main RenderWindow", (uint)WND_SIZE.x, (uint)WND_SIZE.y, false, misc);

            return true;
            */
            bool foundit = false;
            foreach (RenderSystem rs in mRoot.GetAvailableRenderers()) {
                mRoot.RenderSystem = rs;
                String rname = mRoot.RenderSystem.Name;
                if (rname == "Direct3D9 Rendering Subsystem") {//"OpenGL Rendering Subsystem"
                    foundit = true;
                    break;
                }
            }

            if (!foundit)
                return false; //we didn't find it... Raise exception?

            //we found it, we might as well use it!
            mRoot.RenderSystem.SetConfigOption("Full Screen", "No");
            mRoot.RenderSystem.SetConfigOption("Video Mode", "640 x 480 @ 32-bit colour");

            mRoot.Initialise(false);
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = hWnd.ToString();
            mWindow = mRoot.CreateRenderWindow("Simple Mogre Form Window", 0, 0, false, misc);
            return true;
        }

        private void ChooseSceneManager() {
            this.mSceneMgr = this.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
        }

        private void CreateCamera() {
            this.mCam = this.mSceneMgr.CreateCamera("MainCamera");
            this.mCam.NearClipDistance = 0.1f;
            this.mCam.FarClipDistance = 1000f;
            //
            // Position it at 500 in Z direction
            mCam.SetPosition(0, 0, 80);
            // Look back along -Z
            mCam.LookAt(new Vector3(0f, 0f, -300f));
            mCam.NearClipDistance = (0.5f);

            mSceneMgr.ShadowFarDistance = (100.0f);
            //#if OGRE_VERSION < ((2 << 16) | (0 << 8) | 0)
            mSceneMgr.ShadowTechnique = (ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE);
            mSceneMgr.SetShadowTextureSize(1024);
            //#endif
            mSceneMgr.AmbientLight = new ColourValue(0.1f, 0.1f, 0.1f); //(ColourValue.Black);
            // Setup camera and light
            setCameraPosDefault();
        }

        private void setCameraPosDefault() {
            mCam.SetPosition(0, 50, -50);
            mCam.LookAt(0, 0, 0);
        }

        private void CreateViewports() {
            this.mViewport = this.mWindow.AddViewport(this.mCam);
            this.mViewport.BackgroundColour = ColourValue.Black;
            this.mCam.AspectRatio = (this.mViewport.ActualWidth / this.mViewport.ActualHeight);
        }

        protected abstract void CreateScene();

        protected abstract void Update(FrameEvent evt);

        private void LoadResources() {
            // Load resource paths from config file       
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
        }

        private void ReloadAllTextures() { TextureManager.Singleton.ReloadAll(); }

        protected virtual void ProcessInput() {
            this.mInput.Update();

            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_F11)) { this.CycleTextureFilteringMode(); }
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_F12)) { this.CyclePolygonMode(); }
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_F5)) { this.ReloadAllTextures(); }
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_SYSRQ)) { this.TakeScreenshot(); }
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_G)) { this.mPanel.show(); }
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_H)) { this.setCameraPosDefault(); }
            //
            if (mInput.WasKeyPressed(MOIS.KeyCode.KC_ESCAPE)) { this.Dispose(); }
        }

        private void CycleTextureFilteringMode() {
            //MaterialManager.Singleton.SetDefaultTextureFiltering(TextureFilterOptions.TFO_NONE);
            string newVal;
            TextureFilterOptions tfo;
            uint aniso;

            switch (this.mPanel.getParamValue(7).ToUpper()[0]) {
                case 'B':
                    newVal = "Trilinear";
                    tfo = TextureFilterOptions.TFO_TRILINEAR;
                    aniso = 1;
                    break;
                case 'T':
                    newVal = "Anisotropic";
                    tfo = TextureFilterOptions.TFO_ANISOTROPIC;
                    aniso = 8;
                    break;
                case 'A':
                    newVal = "None";
                    tfo = TextureFilterOptions.TFO_NONE;
                    aniso = 1;
                    break;
                default:
                    newVal = "Bilinear";
                    tfo = TextureFilterOptions.TFO_BILINEAR;
                    aniso = 1;
                    break;
            }

            MaterialManager.Singleton.SetDefaultTextureFiltering(tfo);
            MaterialManager.Singleton.DefaultAnisotropy = aniso;
            this.mPanel.setParamValue(7, newVal);
        }

        private void CyclePolygonMode() {
            this.mRenderMode = (this.mRenderMode + 1) % 3;
            switch (mRenderMode) {
                case 0: mCam.PolygonMode = PolygonMode.PM_SOLID; break;
                case 1: mCam.PolygonMode = PolygonMode.PM_WIREFRAME; break;
                case 2: mCam.PolygonMode = PolygonMode.PM_POINTS; break;
            }
        }

        private void TakeScreenshot() { mWindow.WriteContentsToTimestampedFile("screenshot", ".png"); }

        public void AddFrameLstn(RootLstn listener) { listener.AddListener(this.mRoot); }
        public void RemoveFrameLstn(RootLstn listener) { listener.RemoveListener(this.mRoot); }

        private bool OnFrameRendering(FrameEvent evt) {
            if (this.mWindow.IsClosed || this.mIsShutDownRequested) { return false; }
            try {
                this.ProcessInput();
                this.Update(evt);
                this.mTrayMgr.frameRenderingQueued(evt);

                //this.mDebugOverlay.Update(evt.timeSinceLastFrame);
                return true;
            }
            catch (ShutdownException) {
                this.mIsShutDownRequested = true;
                return false;
            }
        }

        protected virtual void Shutdown() {
            if (mInput != null) {
                mInput.Shutdown();
                mInput = null;
            }
            if (this.mTrayMgr != null) {
                this.mTrayMgr.destroyAllWidgets();
                //this.mTrayMgr.clearAllTrays();
                this.mTrayMgr = null;
            }
            if (mRoot != null) {
                mRoot.Shutdown();
            }
            mRoot = null;
        }

        protected Point position;
        protected IntPtr hWnd;

        public OgreWindow(Point origin, IntPtr hWnd) {
            position = origin;
            this.hWnd = hWnd;
        }
        //public void Paint() {
        //    mRoot.RenderOneFrame();
        //}        
        public override void Dispose() {
            mIsShutDownRequested = true;
        }
    }
}
