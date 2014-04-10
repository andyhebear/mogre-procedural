
//namespace Game.Utilitys.SdkTrayUI
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Globalization;

//    using Mogre;
//    using GHA = Mogre.GuiHorizontalAlignment;
//    using TL = TrayLocation;
//    using Math = System.Math;

//    public enum TrayLocation   // enumerator values for widget tray anchoring locations
//    {
//        TL_TOPLEFT,
//        TL_TOP,
//        TL_TOPRIGHT,
//        TL_LEFT,
//        TL_CENTER,
//        TL_RIGHT,
//        TL_BOTTOMLEFT,
//        TL_BOTTOM,
//        TL_BOTTOMRIGHT,
//        TL_NONE
//    }



//    /*=============================================================================
//    | Main class to manage a cursor, backdrop, trays and widgets.
//    =============================================================================*/
//    public class SdkTrayManager : SdkTrayListener/*, ResourceGroupListener*/ {

//        protected String mName;                   // name of this tray system
//        protected RenderWindow mWindow;          // render window
//        protected MOIS.Mouse mMouse;                   // mouse device
//        protected Overlay mBackdropLayer;        // backdrop layer
//        protected Overlay mTraysLayer;           // widget layer
//        protected Overlay mPriorityLayer;        // top priority layer
//        protected Overlay mCursorLayer;          // cursor layer
//        protected OverlayContainer mBackdrop;    // backdrop
//        protected OverlayContainer[] mTrays = new OverlayContainer[10];   // widget trays
//        protected List<Widget>[] mWidgets = new List<Widget>[10];              // widgets
//        protected List<Widget> mWidgetDeathRow = new List<Widget>();           // widget queue for deletion
//        protected OverlayContainer mCursor;      // cursor
//        protected SdkTrayListener mListener;           // tray listener
//        protected float mWidgetPadding;            // widget padding
//        protected float mWidgetSpacing;            // widget spacing
//        protected float mTrayPadding;              // tray padding
//        protected bool mTrayDrag;                       // a mouse press was initiated on a tray
//        protected SelectMenu mExpandedMenu;            // top priority expanded menu widget
//        protected TextBox mDialog;                     // top priority dialog widget
//        protected OverlayContainer mDialogShade; // top priority dialog shade
//        protected Button mOk;                          // top priority OK button
//        protected Button mYes;                         // top priority Yes button
//        protected Button mNo;                          // top priority No button
//        protected bool mCursorWasVisible;               // cursor state before showing dialog
//        protected Label mFpsLabel;                     // FPS label
//        protected ParamsPanel mStatsPanel;             // frame stats panel
//        protected DecorWidget mLogo;                   // logo
//        protected ProgressBar mLoadBar;                // loading bar
//        protected float mGroupInitProportion;      // proportion of load job assigned to initialising one resource group
//        protected float mGroupLoadProportion;      // proportion of load job assigned to loading one resource group
//        protected float mLoadInc;                  // loading increment
//        protected GuiHorizontalAlignment[] mTrayWidgetAlign = new GuiHorizontalAlignment[10];   // tray widget alignments


//        /*-----------------------------------------------------------------------------
//        | Creates backdrop, cursor, and trays.
//        -----------------------------------------------------------------------------*/
//        public SdkTrayManager(String name, RenderWindow window, MOIS.Mouse mouse) :
//            this(name, window, mouse, null) {
//        }
//        public SdkTrayManager(String name, RenderWindow window, MOIS.Mouse mouse, SdkTrayListener listener) {
//            this.mName = name; this.mWindow = window; this.mMouse = mouse; this.mListener = listener; this.mWidgetPadding = 8;
//            this.mWidgetSpacing = 2; this.mExpandedMenu = null; this.mDialog = null; this.mOk = null; this.mYes = null;
//            this.mNo = null; this.mFpsLabel = null; this.mStatsPanel = null; this.mLogo = null; this.mLoadBar = null;

//            OverlayManager om = OverlayManager.Singleton;

//            String nameBase = mName + "/";
//            nameBase.Replace(' ', '_');

//            // create overlay layers for everything

//            this.mBackdropLayer = om.Create(nameBase + "BackdropLayer");
//            this.mTraysLayer = om.Create(nameBase + "WidgetsLayer");
//            this.mPriorityLayer = om.Create(nameBase + "PriorityLayer");
//            this.mCursorLayer = om.Create(nameBase + "CursorLayer");

//            this.mBackdropLayer.ZOrder = 100;
//            this.mTraysLayer.ZOrder = 200;
//            this.mPriorityLayer.ZOrder = 300;
//            this.mCursorLayer.ZOrder = 400;

//            // make backdrop and cursor overlay containers

//            this.mCursor = (OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
//            this.mCursorLayer.Add2D(mCursor);
//            this.mBackdrop = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "Backdrop");
//            this.mBackdropLayer.Add2D(mBackdrop);
//            this.mDialogShade = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "DialogShade");
//            this.mDialogShade.MaterialName = "SdkTrays/Shade";
//            this.mDialogShade.Hide();
//            this.mPriorityLayer.Add2D(mDialogShade);

//            for (int i = 0; i < this.mWidgets.Length; i++)
//                this.mWidgets[i] = new List<Widget>();

//            String[] trayNames = new String[] { "TopLeft", "Top", "TopRight", "Left", "Center", "Right", "BottomLeft", "Bottom", "BottomRight" };

//            for (int i = 0; i < 9; i++)    // make the float trays
//            {
//                this.mTrays[i] = (OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Tray", "BorderPanel", nameBase + trayNames[i] + "Tray");
//                this.mTraysLayer.Add2D(mTrays[i]);

//                this.mTrayWidgetAlign[i] = GHA.GHA_CENTER;

//                // align trays based on location
//                if (i == (int)TL.TL_TOP || i == (int)TL.TL_CENTER || i == (int)TL.TL_BOTTOM) this.mTrays[i].HorizontalAlignment = GHA.GHA_CENTER;
//                if (i == (int)TL.TL_LEFT || i == (int)TL.TL_CENTER || i == (int)TL.TL_RIGHT) this.mTrays[i].VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//                if (i == (int)TL.TL_TOPRIGHT || i == (int)TL.TL_RIGHT || i == (int)TL.TL_BOTTOMRIGHT) this.mTrays[i].HorizontalAlignment = GHA.GHA_RIGHT;
//                if (i == (int)TL.TL_BOTTOMLEFT || i == (int)TL.TL_BOTTOM || i == (int)TL.TL_BOTTOMRIGHT) this.mTrays[i].VerticalAlignment = GuiVerticalAlignment.GVA_BOTTOM;
//            }

//            // create the null tray for free-floating widgets
//            this.mTrays[9] = (OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "NullTray");
//            this.mTrayWidgetAlign[9] = GHA.GHA_LEFT;
//            this.mTraysLayer.Add2D(mTrays[9]);

//            this.adjustTrays();

//            this.showTrays();
//            this.showCursor();
//        }

//        /*-----------------------------------------------------------------------------
//        | Destroys background, cursor, widgets, and trays.
//        -----------------------------------------------------------------------------*/
//        ~SdkTrayManager() {
//            OverlayManager om = OverlayManager.Singleton;

//            this.destroyAllWidgets();

//            // delete widgets queued for destruction
//            for (int i = 0; i < mWidgetDeathRow.Count; i++) { mWidgetDeathRow[i] = null; }
//            mWidgetDeathRow.Clear();

//            om.Destroy(mBackdropLayer);
//            om.Destroy(mTraysLayer);
//            om.Destroy(mPriorityLayer);
//            om.Destroy(mCursorLayer);

//            this.closeDialog();
//            this.hideLoadingBar();

//            Widget.nukeOverlayElement(mBackdrop);
//            Widget.nukeOverlayElement(mCursor);
//            Widget.nukeOverlayElement(mDialogShade);

//            for (int i = 0; i < 10; i++) { Widget.nukeOverlayElement(mTrays[i]); }
//        }

//        /*-----------------------------------------------------------------------------
//        | Converts a 2D screen coordinate (in pixels) to a 3D ray into the scene.
//        -----------------------------------------------------------------------------*/
//        public static Ray screenToScene(Camera cam, Vector2 pt) { return cam.GetCameraToViewportRay(pt.x, pt.y); }

//        /*-----------------------------------------------------------------------------
//        | Converts a 3D scene position to a 2D screen coordinate (in pixels).
//        -----------------------------------------------------------------------------*/
//        public static Vector2 sceneToScreen(Camera cam, Vector3 pt) {
//            Vector3 result = cam.ProjectionMatrix * cam.ViewMatrix * pt;
//            return new Vector2((result.x + 1) / 2, -(result.y + 1) / 2);
//        }

//        // these methods get the underlying overlays and overlay elements

//        public OverlayContainer getTrayContainer(TrayLocation trayLoc) { return this.mTrays[(int)trayLoc]; }
//        public Overlay getBackdropLayer() { return this.mBackdropLayer; }
//        public Overlay getTraysLayer() { return this.mTraysLayer; }
//        public Overlay getCursorLayer() { return this.mCursorLayer; }
//        public OverlayContainer getBackdropContainer() { return this.mBackdrop; }
//        public OverlayContainer getCursorContainer() { return this.mCursor; }
//        public OverlayElement getCursorImage() { return mCursor.GetChild(mCursor.Name + "/CursorImage"); }

//        public void setListener(SdkTrayListener listener) { mListener = listener; }
//        public SdkTrayListener getListener() { return mListener; }

//        public void showAll() {
//            showBackdrop();
//            showTrays();
//            showCursor();
//        }

//        public void hideAll() {
//            hideBackdrop();
//            hideTrays();
//            hideCursor();
//        }

//        /*-----------------------------------------------------------------------------
//        | Displays specified material on backdrop, or the last material used if
//        | none specified. Good for pause menus like in the browser.
//        -----------------------------------------------------------------------------*/
//        public void showBackdrop() {
//            showBackdrop("");
//        }
//        public void showBackdrop(string materialName) {
//            if (materialName != "") this.mBackdrop.MaterialName = materialName;
//            mBackdropLayer.Show();
//        }

//        public void hideBackdrop() { this.mBackdropLayer.Hide(); }

//        /*-----------------------------------------------------------------------------
//        | Displays specified material on cursor, or the last material used if
//        | none specified. Used to change cursor type.
//        -----------------------------------------------------------------------------*/
//        public void showCursor() {
//            showCursor("");
//        }
//        public void showCursor(string materialName) {
//            if (materialName != "") getCursorImage().MaterialName = materialName;

//            if (!mCursorLayer.IsVisible) {
//                this.mCursorLayer.Show();
//                this.refreshCursor();
//            }
//        }

//        public void hideCursor() {
//            this.mCursorLayer.Hide();

//            // give widgets a chance to reset in case they're in the middle of something
//            for (int i = 0; i < 10; i++) { for (int j = 0; j < this.mWidgets[i].Count; j++) { this.mWidgets[i][j]._focusLost(); } }

//            this.setExpandedMenu(null);
//        }

//        /*-----------------------------------------------------------------------------
//        | Updates cursor position based on unbuffered mouse state. This is necessary
//        | because if the tray manager has been cut off from mouse events for a time,
//        | the cursor position will be out of date.
//        -----------------------------------------------------------------------------*/
//        public void refreshCursor() { this.mCursor.SetPosition(mMouse.MouseState.X.abs, mMouse.MouseState.Y.abs); }

//        public void showTrays() {
//            this.mTraysLayer.Show();
//            this.mPriorityLayer.Show();
//        }

//        public void hideTrays() {
//            this.mTraysLayer.Hide();
//            this.mPriorityLayer.Hide();

//            // give widgets a chance to reset in case they're in the middle of something
//            for (int i = 0; i < 10; i++) { for (int j = 0; j < mWidgets[i].Count; j++) { this.mWidgets[i][j]._focusLost(); } }

//            this.setExpandedMenu(null);
//        }

//        public bool isCursorVisible() { return this.mCursorLayer.IsVisible; }
//        public bool isBackdropVisible() { return this.mBackdropLayer.IsVisible; }
//        public bool areTraysVisible() { return this.mTraysLayer.IsVisible; }

//        /*-----------------------------------------------------------------------------
//        | Sets horizontal alignment of a tray's contents.
//        -----------------------------------------------------------------------------*/
//        public void setTrayWidgetAlignment(TrayLocation trayLoc, GuiHorizontalAlignment gha) {
//            mTrayWidgetAlign[(int)trayLoc] = gha;

//            for (int i = 0; i < mWidgets[(int)trayLoc].Count; i++) { mWidgets[(int)trayLoc][i].getOverlayElement().HorizontalAlignment = gha; }
//        }

//        // padding and spacing methods

//        public void setWidgetPadding(float padding) {
//            this.mWidgetPadding = System.Math.Max((int)padding, 0);
//            this.adjustTrays();
//        }

//        public void setWidgetSpacing(float spacing) {
//            this.mWidgetSpacing = System.Math.Max((int)spacing, 0);
//            this.adjustTrays();
//        }
//        public void setTrayPadding(float padding) {
//            this.mTrayPadding = System.Math.Max((int)padding, 0);
//            this.adjustTrays();
//        }

//        public virtual float getWidgetPadding() { return this.mWidgetPadding; }
//        public virtual float getWidgetSpacing() { return this.mWidgetSpacing; }
//        public virtual float getTrayPadding() { return this.mTrayPadding; }

//        /*-----------------------------------------------------------------------------
//        | Fits trays to their contents and snaps them to their anchor locations.
//        -----------------------------------------------------------------------------*/
//        public virtual void adjustTrays() {
//            // resizes and hides trays if necessary
//            for (int i = 0; i < 9; i++) {
//                float trayWidth = 0;
//                float trayHeight = mWidgetPadding;
//                List<OverlayElement> labelsAndSeps = new List<OverlayElement>();

//                // hide tray if empty
//                if (mWidgets[i].Count == 0) {
//                    this.mTrays[i].Hide();
//                    continue;
//                }
//                this.mTrays[i].Show();
//                // arrange widgets and calculate final tray size and position
//                for (int j = 0; j < mWidgets[i].Count; j++) {
//                    OverlayElement e = this.mWidgets[i][j].getOverlayElement();

//                    if (j != 0) trayHeight += mWidgetSpacing;   // don't space first widget

//                    e.VerticalAlignment = GuiVerticalAlignment.GVA_TOP;
//                    e.Top = trayHeight;

//                    switch (e.HorizontalAlignment) {
//                        case GHA.GHA_LEFT: e.Left = this.mWidgetPadding; break;
//                        case GHA.GHA_RIGHT: e.Left = -(e.Width + this.mWidgetPadding); break;
//                        default: e.Left = -(e.Width / 2); break;
//                    }

//                    // prevents some weird texture filtering problems (just some)
//                    e.SetPosition((int)e.Left, (int)e.Top);
//                    e.SetDimensions((int)e.Width, (int)e.Height);
//                    trayHeight += e.Height;

//                    if (mWidgets[i][j].GetType().IsInstanceOfType(typeof(Label))) {
//                        Label l = (Label)mWidgets[i][j];
//                        if (l != null && l._isFitToTray()) {
//                            labelsAndSeps.Add(e);
//                            continue;
//                        }
//                    }

//                    if (mWidgets[i][j].GetType().IsInstanceOfType(typeof(Separator))) {
//                        Separator s = (Separator)mWidgets[i][j];
//                        if (s != null && s._isFitToTray()) {
//                            labelsAndSeps.Add(e);
//                            continue;
//                        }
//                    }

//                    if (e.Width > trayWidth) trayWidth = e.Width;
//                }

//                // add paddings and resize trays
//                mTrays[i].Width = trayWidth + 2 * mWidgetPadding;
//                mTrays[i].Height = trayHeight + mWidgetPadding;

//                for (int j = 0; j < labelsAndSeps.Count; j++) {
//                    labelsAndSeps[j].Width = (int)trayWidth;
//                    labelsAndSeps[j].Left = -(int)(trayWidth / 2);
//                }
//            }
//            // snap trays to anchors
//            for (int i = 0; i < 9; i++) {

//                if (i == (int)TrayLocation.TL_TOPLEFT || i == (int)TrayLocation.TL_LEFT || i == (int)TrayLocation.TL_BOTTOMLEFT)
//                    mTrays[i].Left = mTrayPadding;
//                if (i == (int)TrayLocation.TL_TOP || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_BOTTOM)
//                    mTrays[i].Left = -mTrays[i].Width / 2;
//                if (i == (int)TrayLocation.TL_TOPRIGHT || i == (int)TrayLocation.TL_RIGHT || i == (int)TrayLocation.TL_BOTTOMRIGHT)
//                    mTrays[i].Left = -(mTrays[i].Width + mTrayPadding);

//                if (i == (int)TrayLocation.TL_TOPLEFT || i == (int)TrayLocation.TL_TOP || i == (int)TrayLocation.TL_TOPRIGHT)
//                    mTrays[i].Top = mTrayPadding;
//                if (i == (int)TrayLocation.TL_LEFT || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_RIGHT)
//                    mTrays[i].Top = -mTrays[i].Height / 2;
//                if (i == (int)TrayLocation.TL_BOTTOMLEFT || i == (int)TrayLocation.TL_BOTTOM || i == (int)TrayLocation.TL_BOTTOMRIGHT)
//                    mTrays[i].Top = -mTrays[i].Height - mTrayPadding;

//                // prevents some weird texture filtering problems (just some)
//                mTrays[i].SetPosition((int)mTrays[i].Left, (int)mTrays[i].Top);
//                mTrays[i].SetDimensions((int)mTrays[i].Width, (int)mTrays[i].Height);
//            }
//        }

//        /*-----------------------------------------------------------------------------
//        | Returns a 3D ray into the scene that is directly underneath the cursor.
//        -----------------------------------------------------------------------------*/
//        public Ray getCursorRay(Camera cam) { return screenToScene(cam, new Vector2(mCursor._getLeft(), mCursor._getTop())); }
//        public Button createButton(TrayLocation trayLoc, string name, string caption) {
//           return createButton(trayLoc, name, caption, 0.0f);
//        }
//        public Button createButton(TrayLocation trayLoc, string name, string caption, float width) {
//            Button b = new Button(name, caption, width);
//            this.moveWidgetToTray(b, trayLoc);
//            b._assignListener(mListener);
//            return b;
//        }

//        public TextBox createTextBox(TrayLocation trayLoc, string name, string caption, float width, float height) {
//            TextBox tb = new TextBox(name, caption, width, height);
//            this.moveWidgetToTray(tb, trayLoc);
//            tb._assignListener(mListener);
//            return tb;
//        }
//        public SelectMenu createThickSelectMenu(TrayLocation trayLoc, string name, string caption, float width, int maxItemsShown) {
//            return createThickSelectMenu(trayLoc, name, caption, width, maxItemsShown, null);
//        }
//        public SelectMenu createThickSelectMenu(TrayLocation trayLoc, string name, string caption, float width, int maxItemsShown, List<string> items) {
//            if (items == null) { items = new List<string>(); }
//            SelectMenu sm = new SelectMenu(name, caption, width, 0, (uint)maxItemsShown);
//            this.moveWidgetToTray(sm, trayLoc);
//            sm._assignListener(mListener);
//            if (items.Count != 0) sm.setItems(items);
//            return sm;
//        }
//        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float boxWidth, int maxItemsShown) {
//            return createLongSelectMenu(trayLoc, name, caption, 0, boxWidth, maxItemsShown, null);
//        }
//        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float width, float boxWidth, int maxItemsShown, List<string> items) {
//            if (items == null) { items = new List<string>(); }
//            SelectMenu sm = new SelectMenu(name, caption, width, boxWidth, (uint)maxItemsShown);
//            this.moveWidgetToTray(sm, trayLoc);
//            sm._assignListener(mListener);
//            if (items.Count != 0) sm.setItems(items);
//            return sm;
//        }


//        public Label createLabel(TrayLocation trayLoc, string name, string caption) {
//            return createLabel(trayLoc, name, caption, 0.0f);
//        }
//        public Label createLabel(TrayLocation trayLoc, string name, string caption, float width) {
//            Label l = new Label(name, caption, width);
//            this.moveWidgetToTray(l, trayLoc);
//            l._assignListener(mListener);
//            return l;
//        }
//        public Separator createSeparator(TrayLocation trayLoc, string name) {
//            return createSeparator(trayLoc, name, 0.0f);
//        }
//        public Separator createSeparator(TrayLocation trayLoc, string name, float width) {
//            Separator s = new Separator(name, width);
//            moveWidgetToTray(s, trayLoc);
//            return s;
//        }

//        public Slider createThickSlider(TrayLocation trayLoc, string name, string caption,
//            float width, float valueBoxWidth, float minValue, float maxValue, int snaps) {

//            Slider s = new Slider(name, caption, width, 0, valueBoxWidth, minValue, maxValue, (uint)snaps);
//            this.moveWidgetToTray(s, trayLoc);
//            s._assignListener(mListener);
//            return s;
//        }

//        public Slider createLongSlider(TrayLocation trayLoc, string name, string caption, float width,
//            float trackWidth, float valueBoxWidth, float minValue, float maxValue, int snaps) {

//            if (trackWidth <= 0) trackWidth = 1;
//            Slider s = new Slider(name, caption, width, trackWidth, valueBoxWidth, minValue, maxValue, (uint)snaps);
//            this.moveWidgetToTray(s, trayLoc);
//            s._assignListener(mListener);
//            return s;
//        }

//        public Slider createLongSlider(TrayLocation trayLoc, string name, string caption, float trackWidth, float valueBoxWidth, float minValue, float maxValue, int snaps) { return createLongSlider(trayLoc, name, caption, 0, trackWidth, valueBoxWidth, minValue, maxValue, snaps); }

//        public ParamsPanel createParamsPanel(TrayLocation trayLoc, string name, float width, int lines) {
//            ParamsPanel pp = new ParamsPanel(name, width, lines);
//            this.moveWidgetToTray(pp, trayLoc);
//            return pp;
//        }

//        public ParamsPanel createParamsPanel(TrayLocation trayLoc, string name, float width, string[] paramNames) {
//            ParamsPanel pp = new ParamsPanel(name, width, paramNames.Length);
//            pp.setAllParamNames(paramNames);
//            this.moveWidgetToTray(pp, trayLoc);
//            return pp;
//        }
//        public CheckBox createCheckBox(TrayLocation trayLoc, string name, string caption) {
//            return createCheckBox(trayLoc, name, caption, 0.0f);
//        }
//        public CheckBox createCheckBox(TrayLocation trayLoc, string name, string caption, float width) {
//            CheckBox cb = new CheckBox(name, caption, width);
//            this.moveWidgetToTray(cb, trayLoc);
//            cb._assignListener(mListener);
//            return cb;
//        }

//        public DecorWidget createDecorWidget(TrayLocation trayLoc, string name, string templateName) {
//            DecorWidget dw = new DecorWidget(name, templateName);
//            this.moveWidgetToTray(dw, trayLoc);
//            return dw;
//        }

//        public ProgressBar createProgressBar(TrayLocation trayLoc, string name, string caption, float width, float commentBoxWidth) {
//            ProgressBar pb = new ProgressBar(name, caption, width, commentBoxWidth);
//            this.moveWidgetToTray(pb, trayLoc);
//            return pb;
//        }

//        /*-----------------------------------------------------------------------------
//        | Shows frame statistics widget set in the specified location.
//        -----------------------------------------------------------------------------*/
//        public void showFrameStats(TrayLocation trayLoc) {
//            showFrameStats(trayLoc, -1);
//        }
//        public void showFrameStats(TrayLocation trayLoc, int place) {
//            if (areFrameStatsVisible()) {
//                return;
//            }
//            else {
//                if (mFpsLabel != null) {
//                    mFpsLabel.show();
//                    mStatsPanel.show();
//                    return;
//                }
//                string[] stats = new string[] { "Average FPS", "Best FPS", "Worst FPS", "Triangles", "Batches" };

//                mFpsLabel = createLabel(TrayLocation.TL_NONE, mName + "/FpsLabel", "FPS:", 180);
//                mFpsLabel._assignListener(this);
//                mStatsPanel = createParamsPanel(TrayLocation.TL_NONE, mName + "/StatsPanel", 180, stats);
//            }

//            this.moveWidgetToTray(mFpsLabel, trayLoc, place);
//            this.moveWidgetToTray(mStatsPanel, trayLoc, locateWidgetInTray(mFpsLabel) + 1);
//        }

//        /*-----------------------------------------------------------------------------
//        | Hides frame statistics widget set.
//        -----------------------------------------------------------------------------*/
//        public void hideFrameStats() {
//            if (areFrameStatsVisible()) {
//                //this.destroyWidget(mFpsLabel);
//                //this.destroyWidget(mStatsPanel);
//                //this.mFpsLabel = null;
//                //this.mStatsPanel = null;
//                this.mFpsLabel.hide();
//                this.mStatsPanel.hide();
//            }
//        }

//        public bool areFrameStatsVisible() {
//            if (this.mFpsLabel != null) {
//                return this.mFpsLabel.IsVisible();
//            }
//            return false;
//        }

//        /*-----------------------------------------------------------------------------
//        | Toggles visibility of advanced statistics.
//        -----------------------------------------------------------------------------*/
//        public void toggleAdvancedFrameStats() { if (this.mFpsLabel != null) labelHit(this.mFpsLabel); }

//        /*-----------------------------------------------------------------------------
//        | Shows logo in the specified location.
//        -----------------------------------------------------------------------------*/
//        public void showLogo(TrayLocation trayLoc) {
//            showLogo(trayLoc, -1);
//        }
//        public void showLogo(TrayLocation trayLoc, int place) {
//            if (!isLogoVisible()) this.mLogo = this.createDecorWidget(TrayLocation.TL_NONE, this.mName + "/Logo", "SdkTrays/Logo");
//            this.moveWidgetToTray(this.mLogo, trayLoc, place);
//        }

//        public void hideLogo() {
//            if (isLogoVisible()) {
//                this.destroyWidget(mLogo);
//                this.mLogo = null;
//            }
//        }

//        public bool isLogoVisible() { return this.mLogo != null; }

//        /*-----------------------------------------------------------------------------
//        | Shows loading bar. Also takes job settings: the number of resource groups
//        | to be initialised, the number of resource groups to be loaded, and the
//        | proportion of the job that will be taken up by initialisation. Usually,
//        | script parsing takes up most time, so the default value is 0.7.
//        -----------------------------------------------------------------------------*/
//        public void showLoadingBar() {
//            showLoadingBar(1, 1, 0.7f);
//        }
//        public void showLoadingBar(int numGroupsInit) {
//            showLoadingBar(numGroupsInit, 1, 0.7f);
//        }
//        public void showLoadingBar(int numGroupsInit, int numGroupsLoad) {
//            showLoadingBar(numGroupsInit, numGroupsLoad, 0.7f);
//        }
//        public void showLoadingBar(int numGroupsInit, int numGroupsLoad, float initProportion) {
//            if (this.mDialog != null) closeDialog();
//            if (this.mLoadBar != null) hideLoadingBar();

//            this.mLoadBar = new ProgressBar(mName + "/LoadingBar", "Loading...", 400, 308);
//            OverlayElement e = mLoadBar.getOverlayElement();
//            mDialogShade.AddChild(e);
//            e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//            e.Left = -(e.Width / 2);
//            e.Top = -(e.Height / 2);

//            //Ogre::ResourceGroupManager::getSingleton().addResourceGroupListener(this);
//            //Has not been implemented ?

//            this.mCursorWasVisible = this.isCursorVisible();
//            this.hideCursor();
//            this.mDialogShade.Show();

//            // calculate the proportion of job required to init/load one group

//            if (numGroupsInit == 0 && numGroupsLoad != 0) {
//                this.mGroupInitProportion = 0;
//                this.mGroupLoadProportion = 1;
//            }
//            else if (numGroupsLoad == 0 && numGroupsInit != 0) {
//                this.mGroupLoadProportion = 0;
//                if (numGroupsInit != 0) this.mGroupInitProportion = 1;
//            }
//            else if (numGroupsInit == 0 && numGroupsLoad == 0) {
//                this.mGroupInitProportion = 0;
//                this.mGroupLoadProportion = 0;
//            }
//            else {
//                this.mGroupInitProportion = initProportion / numGroupsInit;
//                this.mGroupLoadProportion = (1 - initProportion) / numGroupsLoad;
//            }
//        }

//        public void hideLoadingBar() {
//            if (this.mLoadBar != null) {
//                this.mLoadBar.cleanup();
//                this.mLoadBar = null;

//                //ResourceGroupManager.Singleton.removeResourceGroupListener(this);
//                if (this.mCursorWasVisible) this.showCursor();
//                this.mDialogShade.Hide();
//            }
//        }

//        public bool isLoadingBarVisible() {
//            return mLoadBar != null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Pops up a message dialog with an OK button.
//        -----------------------------------------------------------------------------*/
//        public void showOkDialog(string caption, string message) {
//            if (mLoadBar != null) hideLoadingBar();

//            OverlayElement e;

//            if (mDialog != null) {
//                mDialog.setCaption(caption);
//                mDialog.setText(message);

//                if (mOk != null) return;

//                mYes.cleanup();
//                mNo.cleanup();
//                mYes = null;
//                mNo = null;
//            }
//            else {
//                // give widgets a chance to reset in case they're in the middle of something
//                for (int i = 0; i < 10; i++) {
//                    for (int j = 0; j < mWidgets[i].Count; j++) {
//                        mWidgets[i][j]._focusLost();
//                    }
//                }

//                mDialogShade.Show();

//                mDialog = new TextBox(mName + "/DialogBox", caption, 300, 208);
//                mDialog.setText(message);
//                e = mDialog.getOverlayElement();
//                mDialogShade.AddChild(e);
//                e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//                e.Left = -(e.Width / 2);
//                e.Top = -(e.Height / 2);

//                mCursorWasVisible = isCursorVisible();
//                showCursor();
//            }

//            mOk = new Button(mName + "/OkButton", "OK", 60);
//            mOk._assignListener(this);
//            e = mOk.getOverlayElement();
//            mDialogShade.AddChild(e);
//            e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//            e.Left = -(e.Width / 2);
//            e.Top = mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5;
//        }

//        /*-----------------------------------------------------------------------------
//        | Pops up a question dialog with Yes and No buttons.
//        -----------------------------------------------------------------------------*/
//        public void showYesNoDialog(string caption, string question) {
//            if (mLoadBar != null) hideLoadingBar();

//            OverlayElement e;

//            if (mDialog != null) {
//                mDialog.setCaption(caption);
//                mDialog.setText(question);

//                if (mOk != null) {
//                    mOk.cleanup();
//                    mOk = null;
//                }
//                else return;
//            }
//            else {
//                // give widgets a chance to reset in case they're in the middle of something
//                for (int i = 0; i < 10; i++) {
//                    for (int j = 0; j < mWidgets[i].Count; j++)
//                        mWidgets[i][j]._focusLost();
//                }

//                mDialogShade.Show();

//                mDialog = new TextBox(mName + "/DialogBox", caption, 300, 208);
//                mDialog.setText(question);
//                e = mDialog.getOverlayElement();
//                mDialogShade.AddChild(e);
//                e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//                e.Left = -(e.Width / 2);
//                e.Top = -(e.Height / 2);

//                mCursorWasVisible = isCursorVisible();
//                showCursor();
//            }

//            mYes = new Button(mName + "/YesButton", "Yes", 58);
//            mYes._assignListener(this);
//            e = mYes.getOverlayElement();
//            mDialogShade.AddChild(e);
//            e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//            e.Left = -(e.Width + 2);
//            e.Top = mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5;

//            mNo = new Button(mName + "/NoButton", "No", 50);
//            mNo._assignListener(this);
//            e = mNo.getOverlayElement();
//            mDialogShade.AddChild(e);
//            e.VerticalAlignment = GuiVerticalAlignment.GVA_CENTER;
//            e.Left = 3;
//            e.Top = mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5;
//        }

//        /*-----------------------------------------------------------------------------
//        | Hides whatever dialog is currently showing.
//        -----------------------------------------------------------------------------*/
//        public void closeDialog() {
//            if (mDialog != null) {
//                if (mOk != null) {
//                    mOk.cleanup();
//                    mOk = null;
//                }
//                else {
//                    mYes.cleanup();
//                    mNo.cleanup();
//                    mYes = null;
//                    mNo = null;
//                }

//                mDialogShade.Hide();
//                mDialog.cleanup();
//                mDialog = null;

//                if (!mCursorWasVisible) hideCursor();
//            }
//        }

//        /*-----------------------------------------------------------------------------
//        | Determines if any dialog is currently visible.
//        -----------------------------------------------------------------------------*/
//        public bool isDialogVisible() {
//            return mDialog != null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets a widget from a tray by place.
//        -----------------------------------------------------------------------------*/
//        public Widget getWidget(TrayLocation trayLoc, int place) {
//            return place < mWidgets[(int)trayLoc].Count ? mWidgets[(int)trayLoc][place] : null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets a widget from a tray by name.
//        -----------------------------------------------------------------------------*/
//        public Widget getWidget(TrayLocation trayLoc, string name) {
//            for (int i = 0; i < mWidgets[(int)trayLoc].Count; i++)
//                if (mWidgets[(int)trayLoc][i].getName() == name) { return mWidgets[(int)trayLoc][i]; }

//            return null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets a widget by name.
//        -----------------------------------------------------------------------------*/
//        public Widget getWidget(string name) {
//            for (int i = 0; i < 10; i++)
//                for (int j = 0; j < mWidgets[i].Count; j++)
//                    if (mWidgets[i][j].getName() == name) { return mWidgets[i][j]; }

//            return null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets the number of widgets in total.
//        -----------------------------------------------------------------------------*/
//        public int getNumWidgets() {
//            int total = 0;

//            for (int i = 0; i < 10; i++)
//                total += mWidgets[i].Count;

//            return total;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets the number of widgets in a tray.
//        -----------------------------------------------------------------------------*/
//        public int getNumWidgets(TrayLocation trayLoc) {
//            return mWidgets[(int)trayLoc].Count;
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets a widget's position in its tray.
//        -----------------------------------------------------------------------------*/
//        public int locateWidgetInTray(Widget widget) {
//            for (int i = 0; i < mWidgets[(int)widget.getTrayLocation()].Count; i++)
//                if (mWidgets[(int)widget.getTrayLocation()][i] == widget) { return i; }

//            return -1;
//        }

//        /*-----------------------------------------------------------------------------
//        | Destroys a widget.
//        -----------------------------------------------------------------------------*/
//        public void destroyWidget(Widget widget) {
//            if (widget == null) throw new Exception("Widget does not exist.");

//            // in case special widgets are destroyed manually, set them to 0
//            if (widget == mLogo) mLogo = null;
//            else if (widget == mStatsPanel) mStatsPanel = null;
//            else if (widget == mFpsLabel) mFpsLabel = null;

//            // Not supposed to do the if
//            if (widget.getName() != "") {
//                mTrays[(int)widget.getTrayLocation()].RemoveChild(widget.getName());
//                widget.getOverlayElement().Hide(); //this seems to fix the non deletion of the overlay image
//            }

//            List<Widget> wList = mWidgets[(int)widget.getTrayLocation()];
//            wList.Remove(widget);
//            if (widget == mExpandedMenu) setExpandedMenu(null);

//            widget.cleanup();

//            mWidgetDeathRow.Add(widget);

//            adjustTrays();
//        }

//        public void destroyWidget(TrayLocation trayLoc, int place) {
//            destroyWidget(getWidget(trayLoc, place));
//        }

//        public void destroyWidget(TrayLocation trayLoc, string name) {
//            destroyWidget(getWidget(trayLoc, name));
//        }

//        public void destroyWidget(string name) {
//            destroyWidget(getWidget(name));
//        }

//        /*-----------------------------------------------------------------------------
//        | Destroys all widgets in a tray.
//        -----------------------------------------------------------------------------*/
//        public void destroyAllWidgetsInTray(TrayLocation trayLoc) {
//            while (mWidgets[(int)trayLoc].Count != 0) destroyWidget(mWidgets[(int)trayLoc][0]);
//        }

//        /*-----------------------------------------------------------------------------
//        | Destroys all widgets.
//        -----------------------------------------------------------------------------*/
//        public void destroyAllWidgets() {
//            for (int i = 0; i < 10; i++)  // destroy every widget in every tray (including null tray)
//                destroyAllWidgetsInTray((TrayLocation)i);
//        }

//        /*-----------------------------------------------------------------------------
//        | Adds a widget to a specified tray.
//        -----------------------------------------------------------------------------*/
//        public void moveWidgetToTray(Widget widget, TrayLocation trayLoc) {
//            moveWidgetToTray(widget, trayLoc, -1);
//        }
//        public void moveWidgetToTray(Widget widget, TrayLocation trayLoc, int place) {
//            if (widget == null) throw new Exception("Widget does not exist.");

//            // remove widget from old tray
//            List<Widget> wList = mWidgets[(int)widget.getTrayLocation()];
//            int it = wList.IndexOf(widget);
//            if (it != wList.Count - 1 && it > 0) {
//                wList.RemoveAt(it);
//                mTrays[(int)widget.getTrayLocation()].RemoveChild(widget.getName());
//            }

//            // insert widget into new tray at given position, or at the end if unspecified or invalid
//            if (place == -1 || place > mWidgets[(int)trayLoc].Count) place = mWidgets[(int)trayLoc].Count;
//            mWidgets[(int)trayLoc].Insert(place, widget);
//            mTrays[(int)trayLoc].AddChild(widget.getOverlayElement());

//            widget.getOverlayElement().HorizontalAlignment = mTrayWidgetAlign[(int)trayLoc];

//            // adjust trays if necessary
//            if (widget.getTrayLocation() != TrayLocation.TL_NONE || trayLoc != TrayLocation.TL_NONE) adjustTrays();

//            widget._assignToTray(trayLoc);
//        }
//        public void moveWidgetToTray(string name, TrayLocation trayLoc) {
//            moveWidgetToTray(name, trayLoc, -1);
//        }
//        public void moveWidgetToTray(string name, TrayLocation trayLoc, int place) {
//            moveWidgetToTray(getWidget(name), trayLoc, place);
//        }
//        public void moveWidgetToTray(TrayLocation currentTrayLoc, string name, TrayLocation targetTrayLoc) {
//            moveWidgetToTray(currentTrayLoc, name, targetTrayLoc, -1);
//        }
//        public void moveWidgetToTray(TrayLocation currentTrayLoc, string name, TrayLocation targetTrayLoc,
//            int place) {
//            moveWidgetToTray(getWidget(currentTrayLoc, name), targetTrayLoc, place);
//        }
//        public void moveWidgetToTray(TrayLocation currentTrayLoc, int currentPlace, TrayLocation targetTrayLoc) {
//            moveWidgetToTray(currentTrayLoc, currentPlace, targetTrayLoc, -1);
//        }
//        public void moveWidgetToTray(TrayLocation currentTrayLoc, int currentPlace, TrayLocation targetTrayLoc,
//            int targetPlace) {
//            moveWidgetToTray(getWidget(currentTrayLoc, currentPlace), targetTrayLoc, targetPlace);
//        }

//        /*-----------------------------------------------------------------------------
//        | Removes a widget from its tray. Same as moving it to the null tray.
//        -----------------------------------------------------------------------------*/
//        public void removeWidgetFromTray(Widget widget) {
//            moveWidgetToTray(widget, TrayLocation.TL_NONE);
//        }

//        public void removeWidgetFromTray(string name) {
//            removeWidgetFromTray(getWidget(name));
//        }

//        public void removeWidgetFromTray(TrayLocation trayLoc, string name) {
//            removeWidgetFromTray(getWidget(trayLoc, name));
//        }

//        public void removeWidgetFromTray(TrayLocation trayLoc, int place) {
//            removeWidgetFromTray(getWidget(trayLoc, place));
//        }

//        /*-----------------------------------------------------------------------------
//        | Removes all widgets from a widget tray.
//        -----------------------------------------------------------------------------*/
//        public void clearTray(TrayLocation trayLoc) {
//            if (trayLoc == TrayLocation.TL_NONE) return;      // can't clear the null tray

//            while (mWidgets[(int)trayLoc].Count != 0)   // remove every widget from given tray
//                removeWidgetFromTray(mWidgets[(int)trayLoc][0]);
//        }

//        /*-----------------------------------------------------------------------------
//        | Removes all widgets from all widget trays.
//        -----------------------------------------------------------------------------*/
//        public void clearAllTrays() {
//            for (int i = 0; i < 9; i++)
//                clearTray((TrayLocation)i);
//        }

//        /*-----------------------------------------------------------------------------
//        | Process frame events. Updates frame statistics widget set and deletes
//        | all widgets queued for destruction.
//        -----------------------------------------------------------------------------*/
//        public bool frameRenderingQueued(FrameEvent evt) {
//            for (int i = 0; i < mWidgetDeathRow.Count; i++)
//                mWidgetDeathRow[i] = null;

//            mWidgetDeathRow.Clear();

//            if (areFrameStatsVisible()) {
//                RenderTarget.FrameStats stats = mWindow.GetStatistics();
//                mFpsLabel.setCaption("FPS: " + stats.LastFPS.ToString("N", CultureInfo.InvariantCulture));

//                if (mStatsPanel.getOverlayElement().IsVisible) {
//                    mStatsPanel.setAllParamValues(new string[]
//                    {
//                        stats.AvgFPS.ToString("N", CultureInfo.InvariantCulture),
//                        stats.BestFPS.ToString("N", CultureInfo.InvariantCulture),
//                        stats.WorstFPS.ToString("N", CultureInfo.InvariantCulture),
//                        stats.TriangleCount.ToString("N", CultureInfo.InvariantCulture),
//                        stats.BatchCount.ToString("N", CultureInfo.InvariantCulture)
//                    });
//                }
//            }

//            return true;
//        }

//        public void resourceGroupScriptingStarted(string groupName, int scriptCount) {
//            mLoadInc = mGroupInitProportion / scriptCount;
//            mLoadBar.setCaption("Parsing...");
//            mWindow.Update();

//        }

//        public void scriptParseStarted(string scriptName, ref bool skipThisScript) {
//            mLoadBar.setComment(scriptName);
//            mWindow.Update();

//        }

//        public void scriptParseEnded(string scriptName, bool skipped) {
//            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
//            mWindow.Update();

//        }

//        public void resourceGroupScriptingEnded(string groupName) { }

//        public void resourceGroupLoadStarted(string groupName, int resourceCount) {
//            mLoadInc = mGroupLoadProportion / resourceCount;
//            mLoadBar.setCaption("Loading...");
//            mWindow.Update();

//        }

//        public void resourceLoadStarted(ResourcePtr resource) {
//            mLoadBar.setComment(resource.Name);
//            mWindow.Update();
//        }

//        public void resourceLoadEnded() {
//            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
//            mWindow.Update();
//        }

//        public void worldGeometryStageStarted(string description) {
//            mLoadBar.setComment(description);
//            mWindow.Update();
//        }

//        public void worldGeometryStageEnded() {
//            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
//            mWindow.Update();
//        }

//        public void resourceGroupLoadEnded(string groupName) { }

//        /*-----------------------------------------------------------------------------
//        | Toggles visibility of advanced statistics.
//        -----------------------------------------------------------------------------*/
//        public new void labelHit(Label label) {
//            if (mStatsPanel.getOverlayElement().IsVisible) {
//                mStatsPanel.getOverlayElement().Hide();
//                mFpsLabel.getOverlayElement().Width = 150;
//                removeWidgetFromTray(mStatsPanel);
//            }
//            else {
//                mStatsPanel.getOverlayElement().Show();
//                mFpsLabel.getOverlayElement().Width = 180;
//                moveWidgetToTray(mStatsPanel, mFpsLabel.getTrayLocation(), locateWidgetInTray(mFpsLabel) + 1);
//            }
//        }

//        /*-----------------------------------------------------------------------------
//        | Destroys dialog widgets, notifies listener, and ends high priority session.
//        -----------------------------------------------------------------------------*/
//        public new void buttonHit(Button button) {
//            if (mListener != null) {
//                if (button == mOk) mListener.okDialogClosed(mDialog.getText());
//                else mListener.yesNoDialogClosed(mDialog.getText(), button == mYes);
//            }
//            closeDialog();
//        }

//        /*-----------------------------------------------------------------------------
//        | Processes mouse button down events. Returns true if the event was
//        | consumed and should not be passed on to other handlers.
//        -----------------------------------------------------------------------------*/
//        public bool injectMouseDown(MOIS.MouseEvent evt, MOIS.MouseButtonID id) {
//            // only process left button when stuff is visible
//            if (!mCursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left) return false;
//            Vector2 cursorPos = new Vector2(mCursor.Left, mCursor.Top);

//            mTrayDrag = false;

//            if (mExpandedMenu != null)   // only check top priority widget until it passes on
//            {
//                mExpandedMenu._cursorPressed(cursorPos);
//                if (!mExpandedMenu.isExpanded()) setExpandedMenu(null);
//                return true;
//            }

//            if (mDialog != null)   // only check top priority widget until it passes on
//            {
//                mDialog._cursorPressed(cursorPos);
//                if (mOk != null) mOk._cursorPressed(cursorPos);
//                else {
//                    mYes._cursorPressed(cursorPos);
//                    mNo._cursorPressed(cursorPos);
//                }
//                return true;
//            }

//            for (int i = 0; i < 9; i++)   // check if mouse is over a non-null tray
//            {
//                if (mTrays[i]!=null && mTrays[i].IsVisible && Widget.isCursorOver(mTrays[i], cursorPos, 2)) {
//                    mTrayDrag = true;   // initiate a drag that originates in a tray
//                    break;
//                }
//            }

//            for (int i = 0; i < mWidgets[9].Count; i++)  // check if mouse is over a non-null tray's widgets
//            {
//                if (mWidgets[9]!= null && mWidgets[9][i]!=null&& mWidgets[9][i].getOverlayElement()!=null&& mWidgets[9][i].getOverlayElement().IsVisible &&
//                    Widget.isCursorOver(mWidgets[9][i].getOverlayElement(), cursorPos)) {
//                    mTrayDrag = true;   // initiate a drag that originates in a tray
//                    break;
//                }
//            }

//            if (!mTrayDrag) return false;   // don't process if mouse press is not in tray

//            for (int i = 0; i < 10; i++) {
//                if (!mTrays[i].IsVisible) continue;

//                for (int j = 0; j < mWidgets[i].Count; j++) {
//                    Widget w = mWidgets[i][j];
//                    if (w != null || w.getOverlayElement() != null) continue;
//                    if (!w.getOverlayElement().IsVisible) continue;
//                    w._cursorPressed(cursorPos);    // send event to widget

//                    if (w.GetType().IsInstanceOfType(typeof(SelectMenu))) {
//                        SelectMenu m = (SelectMenu)w;
//                        if (m != null && m.isExpanded()) // a menu has begun a top priority session
//                        {
//                            setExpandedMenu(m);
//                            return true;
//                        }
//                    }
//                }
//            }

//            return true;   // a tray click is not to be handled by another party
//        }

//        /*-----------------------------------------------------------------------------
//        | Processes mouse button up events. Returns true if the event was
//        | consumed and should not be passed on to other handlers.
//        -----------------------------------------------------------------------------*/
//        public bool injectMouseUp(MOIS.MouseEvent evt, MOIS.MouseButtonID id) {
//            // only process left button when stuff is visible
//            if (!mCursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left) return false;
//            Vector2 cursorPos = new Vector2(mCursor.Left, mCursor.Top);

//            if (mExpandedMenu != null)   // only check top priority widget until it passes on
//            {
//                mExpandedMenu._cursorReleased(cursorPos);
//                return true;
//            }

//            if (mDialog != null)   // only check top priority widget until it passes on
//            {
//                mDialog._cursorReleased(cursorPos);
//                if (mOk != null) mOk._cursorReleased(cursorPos);
//                else {
//                    mYes._cursorReleased(cursorPos);
//                    // very important to check if second button still exists, because first button could've closed the popup
//                    if (mNo != null) mNo._cursorReleased(cursorPos);
//                }
//                return true;
//            }

//            if (!mTrayDrag) return false;    // this click did not originate in a tray, so don't process

//            Widget w;

//            for (int i = 0; i < 10; i++) {
//                if (!mTrays[i].IsVisible) continue;

//                for (int j = 0; j < mWidgets[i].Count; j++) {
//                    w = mWidgets[i][j];
//                    if (w==null||w.getOverlayElement() == null) continue;
//                    if (!w.getOverlayElement().IsVisible) continue;
//                    w._cursorReleased(cursorPos);    // send event to widget
//                }
//            }

//            mTrayDrag = false;   // stop this drag
//            return true;         // this click did originate in this tray, so don't pass it on
//        }

//        /*-----------------------------------------------------------------------------
//        | Updates cursor position. Returns true if the event was
//        | consumed and should not be passed on to other handlers.
//        -----------------------------------------------------------------------------*/
//        public bool injectMouseMove(MOIS.MouseEvent evt) {
//            if (!mCursorLayer.IsVisible) return false;   // don't process if cursor layer is invisible

//            Vector2 cursorPos = new Vector2(evt.state.X.abs, evt.state.Y.abs);
//            mCursor.SetPosition(cursorPos.x, cursorPos.y);

//            if (mExpandedMenu != null)   // only check top priority widget until it passes on
//            {
//                mExpandedMenu._cursorMoved(cursorPos);
//                return true;
//            }

//            if (mDialog != null)   // only check top priority widget until it passes on
//            {
//                mDialog._cursorMoved(cursorPos);
//                if (mOk != null) mOk._cursorMoved(cursorPos);
//                else {
//                    mYes._cursorMoved(cursorPos);
//                    mNo._cursorMoved(cursorPos);
//                }
//                return true;
//            }

//            Widget w;

//            for (int i = 0; i < 10; i++) {
//                if (!mTrays[i].IsVisible) continue;

//                for (int j = 0; j < mWidgets[i].Count; j++) {
//                    w = mWidgets[i][j];
//                    if (w != null || w.getOverlayElement() != null) continue;
//                    if (!w.getOverlayElement().IsVisible) continue;
//                    w._cursorMoved(cursorPos);    // send event to widget
//                }
//            }

//            return mTrayDrag;
//        }

//        /*-----------------------------------------------------------------------------
//        | Internal method to prioritise / deprioritise expanded menus.
//        -----------------------------------------------------------------------------*/
//        protected void setExpandedMenu(SelectMenu m) {
//            if (mExpandedMenu == null && m != null) {
//                OverlayContainer c = (OverlayContainer)m.getOverlayElement();
//                OverlayContainer eb = (OverlayContainer)c.GetChild(m.getName() + "/MenuExpandedBox");
//                eb._update();
//                eb.SetPosition
//                    ((int)(eb._getDerivedLeft() * OverlayManager.Singleton.ViewportWidth),
//                    (int)(eb._getDerivedTop() * OverlayManager.Singleton.ViewportHeight));
//                c.RemoveChild(eb.Name);
//                mPriorityLayer.Add2D(eb);
//            }
//            else if (mExpandedMenu != null && m != null) {
//                OverlayContainer eb = mPriorityLayer.GetChild(mExpandedMenu.getName() + "/MenuExpandedBox");
//                mPriorityLayer.Remove2D(eb);
//                ((OverlayContainer)mExpandedMenu.getOverlayElement()).AddChild(eb);
//            }

//            mExpandedMenu = m;
//        }
//    }

   
//}

