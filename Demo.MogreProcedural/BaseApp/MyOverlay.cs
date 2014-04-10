
using Mogre;


namespace Mogre_Procedural.Game.BaseApp
{
    public class MyOverlay
    {
        private readonly RenderWindow mWindow;
        private readonly Overlay mOverlay;
        private float mTimeSinceLastDebugUpdate = 1;
        private readonly OverlayElement mGuiAvg;
        private readonly OverlayElement mGuiCurr;
        private readonly OverlayElement mGuiBest;
        private readonly OverlayElement mGuiWorst;
        private readonly OverlayElement mGuiTris;
        private readonly OverlayElement mModesText;
        private string mAdditionalInfo = "";
        private bool mIsVisible;

        public bool Visibility {
            get { return this.mIsVisible; }
            set {
                this.mIsVisible = value;
                if (this.mIsVisible) { this.mOverlay.Show(); }
                else { this.mOverlay.Hide(); }
            }
        }

        public MyOverlay(RenderWindow window) {
            this.mWindow = window;

            this.mOverlay = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
            this.Visibility = false;

            this.mGuiAvg = OverlayManager.Singleton.GetOverlayElement("Core/AverageFps");
            this.mGuiCurr = OverlayManager.Singleton.GetOverlayElement("Core/CurrFps");
            this.mGuiBest = OverlayManager.Singleton.GetOverlayElement("Core/BestFps");
            this.mGuiWorst = OverlayManager.Singleton.GetOverlayElement("Core/WorstFps");
            this.mGuiTris = OverlayManager.Singleton.GetOverlayElement("Core/NumTris");
            this.mModesText = OverlayManager.Singleton.GetOverlayElement("Core/NumBatches");
        }

        public string AdditionalInfo { set { mAdditionalInfo = value; } get { return mAdditionalInfo; } }

        public void Update(float timeFragment) {
            if (mTimeSinceLastDebugUpdate > 0.5f) {
                var stats = mWindow.GetStatistics();

                this.mGuiAvg.Caption = "Average FPS: " + stats.AvgFPS;
                this.mGuiCurr.Caption = "Current FPS: " + stats.LastFPS;
                this.mGuiBest.Caption = "Best FPS: " + stats.BestFPS + " " + stats.BestFrameTime + " ms";
                this.mGuiWorst.Caption = "Worst FPS: " + stats.WorstFPS + " " + stats.WorstFrameTime + " ms";
                this.mGuiTris.Caption = "Triangle Count: " + stats.TriangleCount;
                mModesText.Caption = mAdditionalInfo;

                mTimeSinceLastDebugUpdate = 0;
            }
            else
                mTimeSinceLastDebugUpdate += timeFragment;
        }
    }
}