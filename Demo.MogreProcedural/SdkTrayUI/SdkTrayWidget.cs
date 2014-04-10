

//namespace Game.Utilitys.SdkTrayUI
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Text;
//    using Mogre;

//    public enum ButtonState   // enumerator values for button states
//    {
//        BS_UP,
//        BS_OVER,
//        BS_DOWN
//    }



//    /*=============================================================================
//    | Abstract base class for all widgets.
//    =============================================================================*/
//    public class Widget {
//        protected TrayLocation mTrayLoc;
//        protected OverlayElement mElement;
//        protected SdkTrayListener mListener;

//        public Widget() {
//            this.mTrayLoc = TrayLocation.TL_NONE;
//            this.mElement = null;
//            this.mListener = null;
//        }

//        public void cleanup() {
//            if (this.mElement != null) nukeOverlayElement(this.mElement);
//            this.mElement = null;
//        }

//        /*-----------------------------------------------------------------------------
//        | Static utility method to recursively delete an overlay element plus
//        | all of its children from the system.
//        -----------------------------------------------------------------------------*/
//        public static void nukeOverlayElement(OverlayElement element) {
//            if (element == null || !element.GetType().IsInstanceOfType(typeof(OverlayContainer))) { return; }

//            OverlayContainer container = (OverlayContainer)element;
//            List<OverlayElement> toDelete = new List<OverlayElement>();

//            OverlayContainer.ChildIterator children = container.GetChildIterator();
//            do {
//                toDelete.Add(children.Current);
//            } while (children.MoveNext());

//            foreach (OverlayElement t in toDelete)
//                nukeOverlayElement(t);

//            OverlayContainer parent = element.Parent;
//            if (parent != null) { parent.RemoveChild(element.Name); }
//            OverlayManager.Singleton.DestroyOverlayElement(element);
//        }

//        /*-----------------------------------------------------------------------------
//        | Static utility method to check if the cursor is over an overlay element.
//        -----------------------------------------------------------------------------*/
//        public static bool isCursorOver(OverlayElement element, Vector2 cursorPos) {
//            return isCursorOver(element, cursorPos, 0.0f);
//        }
//        public static bool isCursorOver(OverlayElement element, Vector2 cursorPos, float voidBorder) {
//            if (element == null) return false;
//            OverlayManager om = OverlayManager.Singleton;
//            float l = element._getDerivedLeft() * om.ViewportWidth;
//            float t = element._getDerivedTop() * om.ViewportHeight;
//            float r = l + element.Width;
//            float b = t + element.Height;

//            return (cursorPos.x >= l + voidBorder && cursorPos.x <= r - voidBorder &&
//                cursorPos.y >= t + voidBorder && cursorPos.y <= b - voidBorder);
//        }

//        /*-----------------------------------------------------------------------------
//        | Static utility method used to get the cursor's offset from the center
//        | of an overlay element in pixels.
//        -----------------------------------------------------------------------------*/
//        public static Vector2 cursorOffset(OverlayElement element, Vector2 cursorPos) {
//            OverlayManager om = OverlayManager.Singleton;
//            return new Vector2(cursorPos.x - (element._getDerivedLeft() * om.ViewportWidth + element.Width / 2),
//                cursorPos.y - (element._getDerivedTop() * om.ViewportHeight + element.Height / 2));
//        }

//        /*-----------------------------------------------------------------------------
//        | Static utility method used to get the width of a caption in a text area.
//        -----------------------------------------------------------------------------*/
//        public static float getCaptionWidth(string caption, TextAreaOverlayElement area) {
//            ResourcePtr ft = FontManager.Singleton.GetByName(area.FontName);
//            Font font = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);

//            string current = caption;
//            float lineWidth = 0;

//            for (int i = 0; i < current.Length; i++) {
//                // be sure to provide a line width in the text area
//                if (current[i] == ' ') {
//                    if (area.SpaceWidth != 0) lineWidth += area.SpaceWidth;
//                    else lineWidth += font.GetGlyphAspectRatio(' ') * area.CharHeight;
//                }
//                else if (current[i] == '\n') break;
//                // use glyph information to calculate line width
//                else lineWidth += font.GetGlyphAspectRatio(current[i]) * area.CharHeight;
//            }
//            return (int)lineWidth;
//        }

//        /*-----------------------------------------------------------------------------
//        | Static utility method to cut off a string to fit in a text area.
//        -----------------------------------------------------------------------------*/
//        public static void fitCaptionToArea(string caption, TextAreaOverlayElement area, float maxWidth) {
//            ResourcePtr ft = FontManager.Singleton.GetByName(area.FontName);
//            Font f = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);

//            string s = caption;

//            int nl = s.IndexOf('\n');
//            if (nl != -1) s = s.Substring(0, nl);

//            float width = 0;

//            for (int i = 0; i < s.Length; i++) {
//                if (s[i] == ' ' && area.SpaceWidth != 0) width += area.SpaceWidth;
//                else width += f.GetGlyphAspectRatio(s[i]) * area.CharHeight;
//                if (width > maxWidth) {
//                    s = s.Substring(0, i);
//                    break;
//                }
//            }

//            area.Caption = s;
//        }

//        public OverlayElement getOverlayElement() { return this.mElement; }
//        public String getName() { return this.mElement != null ? this.mElement.Name : ""; }   // Not supposed to test != null
//        public TrayLocation getTrayLocation() { return mTrayLoc; }
//        public void hide() { this.mElement.Hide(); }
//        public void show() { this.mElement.Show(); }
//        public bool IsVisible() { return this.mElement.IsVisible; }

//        // callbacks

//        public virtual void _cursorPressed(Vector2 cursorPos) { }
//        public virtual void _cursorReleased(Vector2 cursorPos) { }
//        public virtual void _cursorMoved(Vector2 cursorPos) { }
//        public virtual void _focusLost() { }

//        // internal methods used by SdkTrayManager. do not call directly.
//        public void _assignToTray(TrayLocation trayLoc) { this.mTrayLoc = trayLoc; }
//        public void _assignListener(SdkTrayListener listener) { this.mListener = listener; }

//    }

//    /*=============================================================================
//    | Basic button class.
//    =============================================================================*/
//    public class Button : Widget {
//        // Do not instantiate any widgets directly. Use SdkTrayManager.

//        protected ButtonState mState;
//        protected BorderPanelOverlayElement mBP;
//        protected TextAreaOverlayElement mTextArea;
//        protected bool mFitToContents;

//        public Button(String name, string caption, float width) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Button", "BorderPanel", name);
//            this.mBP = (BorderPanelOverlayElement)this.mElement;
//            this.mTextArea = (TextAreaOverlayElement)this.mBP.GetChild(this.mBP.Name + "/ButtonCaption");
//            this.mTextArea.Top = -(this.mTextArea.CharHeight / 2);

//            if (width > 0) {
//                this.mElement.Width = width;
//                mFitToContents = false;
//            }
//            else mFitToContents = true;

//            this.setCaption(caption);
//            this.mState = ButtonState.BS_UP;
//        }


//        public string getCaption() { return this.mTextArea.Caption; }
//        public void setCaption(string caption) {
//            this.mTextArea.Caption = caption;
//            if (this.mFitToContents) { this.mElement.Width = getCaptionWidth(caption, mTextArea) + this.mElement.Height - 12; }
//        }

//        public ButtonState getState() { return mState; }
//        public override void _cursorPressed(Vector2 cursorPos) { if (isCursorOver(mElement, cursorPos, 4)) setState(ButtonState.BS_DOWN); }

//        public override void _cursorReleased(Vector2 cursorPos) {
//            if (mState == ButtonState.BS_DOWN) {
//                setState(ButtonState.BS_OVER);
//                if (mListener != null) mListener.buttonHit(this);
//            }
//        }

//        public override void _cursorMoved(Vector2 cursorPos) {
//            if (isCursorOver(this.mElement, cursorPos, 4)) { if (mState == ButtonState.BS_UP) setState(ButtonState.BS_OVER); }
//            else { if (mState != ButtonState.BS_UP) setState(ButtonState.BS_UP); }
//        }

//        // reset button if cursor was lost
//        public override void _focusLost() { setState(ButtonState.BS_UP); }

//        public void setState(ButtonState bs) {
//            if (bs == ButtonState.BS_OVER) {
//                this.mBP.BorderMaterialName = "SdkTrays/Button/Over";
//                this.mBP.MaterialName = "SdkTrays/Button/Over";
//            }
//            else if (bs == ButtonState.BS_UP) {
//                this.mBP.BorderMaterialName = "SdkTrays/Button/Up";
//                this.mBP.MaterialName = "SdkTrays/Button/Up";
//            }
//            else {
//                this.mBP.BorderMaterialName = "SdkTrays/Button/Down";
//                this.mBP.MaterialName = "SdkTrays/Button/Down";
//            }

//            this.mState = bs;
//        }
//    }

//    /*=============================================================================
//    | Scrollable text box widget.
//    =============================================================================*/
//    public class TextBox : Widget {
//        // Do not instantiate any widgets directly. Use SdkTrayManager.

//        protected TextAreaOverlayElement mTextArea;
//        protected BorderPanelOverlayElement mCaptionBar;
//        protected TextAreaOverlayElement mCaptionTextArea;
//        protected BorderPanelOverlayElement mScrollTrack;
//        protected PanelOverlayElement mScrollHandle;
//        protected string mText;
//        protected List<String> mLines;
//        protected float mPadding;
//        protected bool mDragging;
//        protected float mScrollPercentage;
//        protected float mDragOffset;
//        protected int mStartingLine;


//        public TextBox(String name, string caption, float width, float height) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/TextBox", "BorderPanel", name);
//            this.mElement.Width = width;
//            this.mElement.Height = height;
//            OverlayContainer container = (OverlayContainer)this.mElement;
//            this.mTextArea = (TextAreaOverlayElement)container.GetChild(this.getName() + "/TextBoxText");
//            this.mCaptionBar = (BorderPanelOverlayElement)container.GetChild(this.getName() + "/TextBoxCaptionBar");
//            this.mCaptionBar.Width = width - 4;
//            this.mCaptionTextArea = (TextAreaOverlayElement)this.mCaptionBar.GetChild(this.mCaptionBar.Name + "/TextBoxCaption");
//            this.setCaption(caption);
//            this.mScrollTrack = (BorderPanelOverlayElement)container.GetChild(this.getName() + "/TextBoxScrollTrack");
//            this.mScrollHandle = (PanelOverlayElement)this.mScrollTrack.GetChild(this.mScrollTrack.Name + "/TextBoxScrollHandle");
//            this.mScrollHandle.Hide();
//            this.mDragging = false;
//            this.mScrollPercentage = 0;
//            this.mStartingLine = 0;
//            this.mPadding = 15;
//            this.mText = "";
//            this.mLines = new List<string>();
//            this.refitContents();
//        }

//        public void setPadding(float padding) {
//            this.mPadding = padding;
//            this.refitContents();
//        }

//        public float getPadding() { return mPadding; }
//        public string getCaption() { return this.mCaptionTextArea.Caption; }
//        public void setCaption(string caption) { this.mCaptionTextArea.Caption = caption; }
//        public string getText() { return mText; }

//        /*-----------------------------------------------------------------------------
//        | Sets text box content. Most of this method is for wordwrap.
//        -----------------------------------------------------------------------------*/
//        public void setText(string text) {
//            this.mText = text;
//            this.mLines.Clear();

//            ResourcePtr ft = FontManager.Singleton.GetByName(this.mTextArea.FontName);
//            Font font = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);

//            String current = text;
//            bool firstWord = true;
//            int lastSpace = 0;
//            int lineBegin = 0;
//            float lineWidth = 0;
//            float rightBoundary = this.mElement.Width - 2 * mPadding + this.mScrollTrack.Left + 10;

//            for (int i = 0; i < current.Length; i++) {
//                if (current[i] == ' ') {
//                    if (this.mTextArea.SpaceWidth != 0) lineWidth += this.mTextArea.SpaceWidth;
//                    else lineWidth += font.GetGlyphAspectRatio(' ') * this.mTextArea.CharHeight;
//                    firstWord = false;
//                    lastSpace = i;
//                }
//                else if (current[i] == '\n') {
//                    firstWord = true;
//                    lineWidth = 0;
//                    mLines.Add(current.Substring(lineBegin, i - lineBegin));
//                    lineBegin = i + 1;
//                }
//                else {
//                    // use glyph information to calculate line width
//                    lineWidth += font.GetGlyphAspectRatio(current[i]) * this.mTextArea.CharHeight;
//                    if (lineWidth > rightBoundary) {
//                        if (firstWord) {
//                            current.Insert(i, "\n");
//                            i = i - 1;
//                        }
//                        else {
//                            char[] str = current.ToCharArray();
//                            str[lastSpace] = '\n';
//                            current = new String(str);
//                            i = lastSpace - 1;
//                        }
//                    }
//                }
//            }

//            mLines.Add(current.Substring(lineBegin));

//            int maxLines = getHeightInLines();

//            if (mLines.Count > maxLines)     // if too much text, filter based on scroll percentage
//            {
//                this.mScrollHandle.Show();
//                this.filterLines();
//            }
//            else       // otherwise just show all the text
//            {
//                this.mTextArea.Caption = current;
//                this.mScrollHandle.Hide();
//                this.mScrollPercentage = 0;
//                this.mScrollHandle.Top = 0;
//            }
//        }

//        /*-----------------------------------------------------------------------------
//        | Sets text box content horizontal alignment.
//        -----------------------------------------------------------------------------*/
//        public void setTextAlignment(TextAreaOverlayElement.Alignment ta) {
//            if (ta == TextAreaOverlayElement.Alignment.Left) this.mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
//            else if (ta == TextAreaOverlayElement.Alignment.Center) this.mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;
//            else this.mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_RIGHT;
//            refitContents();
//        }

//        public void clearText() { setText(""); }

//        public void appendText(string text) { setText(getText() + text); }

//        /*-----------------------------------------------------------------------------
//        | Makes adjustments based on new padding, size, or alignment info.
//        -----------------------------------------------------------------------------*/
//        public void refitContents() {
//            this.mScrollTrack.Height = this.mElement.Height - this.mCaptionBar.Height - 20;
//            this.mScrollTrack.Top = this.mCaptionBar.Height + 10;

//            this.mTextArea.Top = this.mCaptionBar.Height + this.mPadding - 5;
//            if (this.mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_RIGHT) this.mTextArea.Left = -this.mPadding + this.mScrollTrack.Left;
//            else if (this.mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_LEFT) this.mTextArea.Left = this.mPadding;
//            else this.mTextArea.Left = this.mScrollTrack.Left / 2;

//            this.setText(getText());
//        }

//        /*-----------------------------------------------------------------------------
//        | Sets how far scrolled down the text is as a percentage.
//        -----------------------------------------------------------------------------*/
//        public void setScrollPercentage(float percentage) {
//            this.mScrollPercentage = SdkTrayMathHelper.clamp(percentage, 0, 1);
//            this.mScrollHandle.Top = (int)(percentage * (this.mScrollTrack.Height - this.mScrollHandle.Height));
//            this.filterLines();
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets how far scrolled down the text is as a percentage.
//        -----------------------------------------------------------------------------*/
//        public float getScrollPercentage() { return this.mScrollPercentage; }

//        /*-----------------------------------------------------------------------------
//        | Gets how many lines of text can fit in this window.
//        -----------------------------------------------------------------------------*/
//        public int getHeightInLines() { return (int)((this.mElement.Height - 2 * mPadding - this.mCaptionBar.Height + 5) / this.mTextArea.CharHeight); }

//        public override void _cursorPressed(Vector2 cursorPos) {
//            if (!this.mScrollHandle.IsVisible) return;   // don't care about clicks if text not scrollable

//            Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);

//            if (co.SquaredLength <= 81) {
//                this.mDragging = true;
//                this.mDragOffset = co.y;
//            }
//            else if (Widget.isCursorOver(mScrollTrack, cursorPos)) {
//                float newTop = this.mScrollHandle.Top + co.y;
//                float lowerBoundary = this.mScrollTrack.Height - this.mScrollHandle.Height;
//                this.mScrollHandle.Top = SdkTrayMathHelper.clamp((int)newTop, 0, (int)lowerBoundary);

//                // update text area contents based on new scroll percentage
//                this.mScrollPercentage = SdkTrayMathHelper.clamp(newTop / lowerBoundary, 0, 1);
//                this.filterLines();
//            }
//        }

//        public override void _cursorReleased(Vector2 cursorPos) { this.mDragging = false; }

//        public override void _cursorMoved(Vector2 cursorPos) {
//            if (this.mDragging) {
//                Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);
//                float newTop = this.mScrollHandle.Top + co.y - this.mDragOffset;
//                float lowerBoundary = this.mScrollTrack.Height - this.mScrollHandle.Height;
//                this.mScrollHandle.Top = SdkTrayMathHelper.clamp((int)newTop, 0, (int)lowerBoundary);

//                // update text area contents based on new scroll percentage
//                this.mScrollPercentage = SdkTrayMathHelper.clamp(newTop / lowerBoundary, 0, 1);
//                this.filterLines();
//            }
//        }

//        // stop dragging if cursor was lost
//        public override void _focusLost() { mDragging = false; }

//        /*-----------------------------------------------------------------------------
//        | Decides which lines to show.
//        -----------------------------------------------------------------------------*/
//        protected void filterLines() {
//            String shown = "";
//            int maxLines = this.getHeightInLines();
//            int newStart = (int)(this.mScrollPercentage * (this.mLines.Count - maxLines) + 0.5);

//            mStartingLine = newStart;

//            for (int i = 0; i < maxLines; i++) {
//                shown += mLines[mStartingLine + i] + "\n";
//            }

//            this.mTextArea.Caption = shown;    // show just the filtered lines
//        }
//    }

//    /*=============================================================================
//    | Basic selection menu widget.
//    =============================================================================*/
//    public class SelectMenu : Widget {
//        protected BorderPanelOverlayElement mSmallBox;
//        protected BorderPanelOverlayElement mExpandedBox;
//        protected TextAreaOverlayElement mTextArea;
//        protected TextAreaOverlayElement mSmallTextArea;
//        protected BorderPanelOverlayElement mScrollTrack;
//        protected PanelOverlayElement mScrollHandle;
//        protected List<BorderPanelOverlayElement> mItemElements;
//        protected uint mMaxItemsShown;
//        protected uint mItemsShown;
//        protected bool mCursorOver;
//        protected bool mExpanded;
//        protected bool mFitToContents;
//        protected bool mDragging;
//        protected List<string> mItems;
//        protected int mSelectionIndex;
//        protected int mHighlightIndex;
//        protected int mDisplayIndex;
//        protected float mDragOffset;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.	
//        public SelectMenu(string name, string caption, float width, float boxWidth, uint maxItemsShown) {
//            this.mHighlightIndex = 0;
//            this.mDisplayIndex = 0;
//            this.mDragOffset = 0.0f;
//            this.mSelectionIndex = -1;
//            this.mFitToContents = false;
//            this.mCursorOver = false;
//            this.mExpanded = false;
//            this.mDragging = false;
//            this.mMaxItemsShown = maxItemsShown;
//            this.mItemsShown = 0;
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenu", "BorderPanel", name);
//            this.mTextArea = (TextAreaOverlayElement)((OverlayContainer)this.mElement).GetChild(name + "/MenuCaption");
//            this.mSmallBox = (BorderPanelOverlayElement)((OverlayContainer)this.mElement).GetChild(name + "/MenuSmallBox");
//            this.mSmallBox.Width = width - 10;
//            this.mSmallTextArea = (TextAreaOverlayElement)this.mSmallBox.GetChild(name + "/MenuSmallBox/MenuSmallText");
//            this.mElement.Width = width;
//            this.mItemElements = new List<BorderPanelOverlayElement>();
//            this.mItems = new List<string>();

//            if (boxWidth > 0)  // long style
//            {
//                if (width <= 0) { this.mFitToContents = true; }
//                this.mSmallBox.Width = boxWidth;
//                this.mSmallBox.Top = 2;
//                this.mSmallBox.Left = width - boxWidth - 5;
//                this.mElement.Height = this.mSmallBox.Height + 4;
//                this.mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
//                this.mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Left);
//                this.mTextArea.Left = 12;
//                this.mTextArea.Top = 10;
//            }

//            this.mExpandedBox = (BorderPanelOverlayElement)((OverlayContainer)this.mElement).GetChild(name + "/MenuExpandedBox");
//            this.mExpandedBox.Width = this.mSmallBox.Width + 10;
//            this.mExpandedBox.Hide();
//            this.mScrollTrack = (BorderPanelOverlayElement)this.mExpandedBox.GetChild(this.mExpandedBox.Name + "/MenuScrollTrack");
//            this.mScrollHandle = (PanelOverlayElement)this.mScrollTrack.GetChild(this.mScrollTrack.Name + "/MenuScrollHandle");

//            this.setCaption(caption);
//        }

//        public bool isExpanded() { return mExpanded; }

//        public string getCaption() { return mTextArea.Caption; }

//        public void setCaption(string caption) {
//            this.mTextArea.Caption = caption;
//            if (this.mFitToContents) {
//                this.mElement.Width = getCaptionWidth(caption, this.mTextArea) + this.mSmallBox.Width + 23;
//                this.mSmallBox.Left = this.mElement.Width - this.mSmallBox.Width - 5;
//            }
//        }

//        public List<string> getItems() { return this.mItems; }

//        public uint getNumItems() { return (uint)this.mItems.Count; }

//        public void setItems(List<string> items) {
//            this.mItems = items;
//            this.mSelectionIndex = -1;

//            foreach (BorderPanelOverlayElement t in this.mItemElements)
//                nukeOverlayElement(t);

//            this.mItemElements.Clear();

//            this.mItemsShown = (uint)System.Math.Max(2, System.Math.Min(this.mMaxItemsShown, this.mItems.Count));

//            for (int i = 0; i < this.mItemsShown; i++)   // create all the item elements
//            {
//                BorderPanelOverlayElement e = (BorderPanelOverlayElement)OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenuItem", "BorderPanel", this.mExpandedBox.Name + "/Item" + (i + 1));

//                e.Top = 6 + i * (this.mSmallBox.Height - 8);
//                e.Width = this.mExpandedBox.Width - 32;

//                this.mExpandedBox._addChild(e);
//                this.mItemElements.Add(e);
//            }

//            if (items.Count != 0) { this.selectItem(0, false); }
//            else { this.mSmallTextArea.Caption = ""; }
//        }

//        public void addItem(string item) {
//            this.mItems.Add(item);
//            this.setItems(this.mItems);
//        }

//        public void removeItem(string item) {
//            int it = 0;

//            for (; it < this.mItems.Count; it++)
//                if (item == this.mItems[it]) { break; }

//            if (it < this.mItems.Count) {
//                this.mItems.RemoveAt(it);
//                if (this.mItems.Count < this.mItemsShown) {
//                    this.mItemsShown = (uint)this.mItems.Count;
//                    nukeOverlayElement(this.mItemElements[this.mItemElements.Count - 1]);
//                    this.mItemElements.RemoveAt(this.mItemElements.Count - 1);
//                }
//            }
//            else
//                throw new Exception("Menu \"" + this.getName() + "\" contains no item \"" + item + "\".");
//        }

//        public void removeItem(uint index) {
//            int it = 0;
//            uint i = 0;

//            for (; it < this.mItems.Count; it++) {
//                if (i == index) break;
//                i++;
//            }

//            if (it < this.mItems.Count) {
//                this.mItems.RemoveAt(it);
//                if (this.mItems.Count < this.mItemsShown) {
//                    this.mItemsShown = (uint)this.mItems.Count;
//                    nukeOverlayElement(this.mItemElements[this.mItemElements.Count - 1]);
//                    this.mItemElements.RemoveAt(this.mItemElements.Count - 1);
//                }
//            }
//            else
//                throw new Exception("Menu \"" + this.getName() + "\" contains no item at position " + index + ".");
//        }

//        public void clearItems() {
//            this.mItems.Clear();
//            this.mSelectionIndex = -1;
//            this.mSmallTextArea.Caption = "";
//        }
//        public void selectItem(uint index) {
//            selectItem(index, true);
//        }
//        public void selectItem(uint index, bool notifyListener) {
//            if (index >= mItems.Count)
//                throw new Exception("Menu \"" + this.getName() + "\" contains no item at position " + index + ".");

//            this.mSelectionIndex = (int)index;
//            fitCaptionToArea(this.mItems[(int)index], this.mSmallTextArea, this.mSmallBox.Width - this.mSmallTextArea.Left * 2);

//            if (this.mListener != null && notifyListener) this.mListener.itemSelected(this);
//        }
//        public void selectItem(string item) {
//            selectItem(item, true);
//        }
//        public void selectItem(string item, bool notifyListener) {
//            for (int i = 0; i < this.mItems.Count; i++) {
//                if (item == this.mItems[i]) {
//                    this.selectItem((uint)i, notifyListener);
//                    return;
//                }
//            }

//            throw new Exception("Menu \"" + this.getName() + "\" contains no item \"" + item + "\".");
//        }

//        public string getSelectedItem() {
//            if (this.mSelectionIndex != -1) { return this.mItems[mSelectionIndex]; }

//            throw new Exception("Menu \"" + this.getName() + "\" has no item selected.");
//        }

//        public int getSelectionIndex() { return this.mSelectionIndex; }

//        public override void _cursorPressed(Vector2 cursorPos) {
//            OverlayManager om = OverlayManager.Singleton;

//            if (this.mExpanded) {
//                if (this.mScrollHandle.IsVisible)   // check for scrolling
//                {
//                    Vector2 co = Widget.cursorOffset(this.mScrollHandle, cursorPos);

//                    if (co.SquaredLength <= 81) {
//                        this.mDragging = true;
//                        this.mDragOffset = co.y;
//                        return;
//                    }

//                    if (Widget.isCursorOver(this.mScrollTrack, cursorPos)) {
//                        float newTop = this.mScrollHandle.Top + co.y;
//                        float lowerBoundary = this.mScrollTrack.Height - this.mScrollHandle.Height;
//                        this.mScrollHandle.Top = SdkTrayMathHelper.clamp((int)newTop, 0, (int)lowerBoundary);

//                        float scrollPercentage = SdkTrayMathHelper.clamp(newTop / lowerBoundary, 0, 1);
//                        this.setDisplayIndex((int)(scrollPercentage * (this.mItems.Count - this.mItemElements.Count) + 0.5));
//                        return;
//                    }
//                }

//                if (!isCursorOver(this.mExpandedBox, cursorPos, 3)) { this.retract(); }
//                else {
//                    float l = this.mItemElements[0]._getDerivedLeft() * om.ViewportWidth + 5;
//                    float t = this.mItemElements[0]._getDerivedTop() * om.ViewportHeight + 5;
//                    float r = l + this.mItemElements[this.mItemElements.Count - 1].Width - 10;
//                    float b = this.mItemElements[this.mItemElements.Count - 1]._getDerivedTop() * om.ViewportHeight +
//                              this.mItemElements[this.mItemElements.Count - 1].Height - 5;

//                    if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b) {
//                        if (this.mHighlightIndex != this.mSelectionIndex) { this.selectItem((uint)this.mHighlightIndex); }
//                        this.retract();
//                    }
//                }
//            }
//            else {
//                if (this.mItems.Count < 2) return;   // don't waste time showing a menu if there's no choice

//                if (isCursorOver(this.mSmallBox, cursorPos, 4)) {
//                    this.mExpandedBox.Show();
//                    this.mSmallBox.Hide();

//                    // calculate how much vertical space we need
//                    float idealHeight = this.mItemsShown * (this.mSmallBox.Height - 8) + 20;
//                    this.mExpandedBox.Height = idealHeight;
//                    this.mScrollTrack.Height = this.mExpandedBox.Height - 20;

//                    this.mExpandedBox.Left = this.mSmallBox.Left - 4;

//                    // if the expanded menu goes down off the screen, make it go up instead
//                    if (this.mSmallBox._getDerivedTop() * om.ViewportHeight + idealHeight > om.ViewportHeight) {
//                        this.mExpandedBox.Top = this.mSmallBox.Top + this.mSmallBox.Height - idealHeight + 3;
//                        // if we're in thick style, hide the caption because it will interfere with the expanded menu
//                        if (this.mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_CENTER) { this.mTextArea.Hide(); }
//                    }
//                    else { this.mExpandedBox.Top = this.mSmallBox.Top + 3; }

//                    this.mExpanded = true;
//                    this.mHighlightIndex = this.mSelectionIndex;
//                    this.setDisplayIndex(this.mHighlightIndex);

//                    if (this.mItemsShown < this.mItems.Count)  // update scrollbar position
//                    {
//                        this.mScrollHandle.Show();
//                        float lowerBoundary = this.mScrollTrack.Height - this.mScrollHandle.Height;
//                        this.mScrollHandle.Top = (int)(mDisplayIndex * lowerBoundary / (mItems.Count - this.mItemElements.Count));
//                    }
//                    else this.mScrollHandle.Hide();
//                }
//            }
//        }

//        public override void _cursorReleased(Vector2 cursorPos) { this.mDragging = false; }

//        public override void _cursorMoved(Vector2 cursorPos) {
//            OverlayManager om = OverlayManager.Singleton;

//            if (this.mExpanded) {
//                if (this.mDragging) {
//                    Vector2 co = Widget.cursorOffset(this.mScrollHandle, cursorPos);
//                    float newTop = this.mScrollHandle.Top + co.y - this.mDragOffset;
//                    float lowerBoundary = this.mScrollTrack.Height - this.mScrollHandle.Height;
//                    this.mScrollHandle.Top = SdkTrayMathHelper.clamp((int)newTop, 0, (int)lowerBoundary);

//                    float scrollPercentage = SdkTrayMathHelper.clamp(newTop / lowerBoundary, 0, 1);
//                    int newIndex = (int)(scrollPercentage * (this.mItems.Count - this.mItemElements.Count) + 0.5);
//                    if (newIndex != this.mDisplayIndex) { this.setDisplayIndex(newIndex); }
//                    return;
//                }

//                float l = this.mItemElements[0]._getDerivedLeft() * om.ViewportWidth + 5;
//                float t = this.mItemElements[0]._getDerivedTop() * om.ViewportHeight + 5;
//                float r = l + this.mItemElements[this.mItemElements.Count - 1].Width - 10;
//                float b = this.mItemElements[this.mItemElements.Count - 1]._getDerivedTop() * om.ViewportHeight +
//                    this.mItemElements[this.mItemElements.Count - 1].Height - 5;

//                if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b) {
//                    int newIndex = (int)(this.mDisplayIndex + (cursorPos.y - t) / (b - t) * this.mItemElements.Count);
//                    if (this.mHighlightIndex != newIndex) {
//                        this.mHighlightIndex = newIndex;
//                        this.setDisplayIndex(this.mDisplayIndex);
//                    }
//                }
//            }
//            else {
//                if (isCursorOver(mSmallBox, cursorPos, 4)) {
//                    this.mSmallBox.MaterialName = "SdkTrays/MiniTextBox/Over";
//                    this.mSmallBox.BorderMaterialName = "SdkTrays/MiniTextBox/Over";
//                    this.mCursorOver = true;
//                }
//                else {
//                    if (this.mCursorOver) {
//                        this.mSmallBox.MaterialName = "SdkTrays/MiniTextBox";
//                        this.mSmallBox.BorderMaterialName = "SdkTrays/MiniTextBox";
//                        this.mCursorOver = false;
//                    }
//                }
//            }
//        }

//        public override void _focusLost() { if (this.mExpandedBox.IsVisible) { this.retract(); } }

//        /*-----------------------------------------------------------------------------
//        | Internal method - sets which item goes at the top of the expanded menu.
//        -----------------------------------------------------------------------------*/
//        protected void setDisplayIndex(int index) {
//            index = System.Math.Min(index, this.mItems.Count - this.mItemElements.Count);
//            this.mDisplayIndex = index;
//            BorderPanelOverlayElement ie;
//            TextAreaOverlayElement ta;

//            for (int i = 0; i < this.mItemElements.Count; i++) {
//                ie = this.mItemElements[i];
//                ta = (TextAreaOverlayElement)ie.GetChild(ie.Name + "/MenuItemText");

//                fitCaptionToArea(this.mItems[this.mDisplayIndex + i], ta, ie.Width - 2 * ta.Left);

//                if ((mDisplayIndex + i) == this.mHighlightIndex) {
//                    ie.MaterialName = "SdkTrays/MiniTextBox/Over";
//                    ie.BorderMaterialName = "SdkTrays/MiniTextBox/Over";
//                }
//                else {
//                    ie.MaterialName = "SdkTrays/MiniTextBox";
//                    ie.BorderMaterialName = "SdkTrays/MiniTextBox";
//                }
//            }
//        }

//        /*-----------------------------------------------------------------------------
//        | Internal method - cleans up an expanded menu.
//        -----------------------------------------------------------------------------*/
//        internal void retract() {
//            this.mDragging = false;
//            this.mExpanded = false;
//            this.mExpandedBox.Hide();
//            this.mTextArea.Show();
//            this.mSmallBox.Show();
//            this.mSmallBox.MaterialName = "SdkTrays/MiniTextBox";
//            this.mSmallBox.BorderMaterialName = "SdkTrays/MiniTextBox";
//        }
//    }

//    /*=============================================================================
//    | Basic label widget.
//    =============================================================================*/
//    public class Label : Widget {
//        protected TextAreaOverlayElement mTextArea;
//        protected bool mFitToTray;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public Label(String name, string caption, float width) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Label", "BorderPanel", name);
//            this.mTextArea = (TextAreaOverlayElement)((OverlayContainer)this.mElement).GetChild(this.getName() + "/LabelCaption");
//            this.setCaption(caption);
//            if (width <= 0) mFitToTray = true;
//            else {
//                this.mFitToTray = false;
//                this.mElement.Width = width;
//            }
//        }

//        public string getCaption() { return this.mTextArea.Caption; }
//        public void setCaption(string caption) { this.mTextArea.Caption = caption; }
//        public override void _cursorPressed(Vector2 cursorPos) { if (this.mListener != null && isCursorOver(this.mElement, cursorPos, 3)) this.mListener.labelHit(this); }
//        public bool _isFitToTray() { return mFitToTray; }

//    }

//    /*=============================================================================
//    | Basic separator widget.
//    =============================================================================*/
//    public class Separator : Widget {

//        protected bool mFitToTray;
//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public Separator(String name, float width) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Separator", "Panel", name);
//            if (width <= 0) mFitToTray = true;
//            else {
//                mFitToTray = false;
//                this.mElement.Width = width;
//            }
//        }

//        public bool _isFitToTray() { return mFitToTray; }

//    }

//    /*=============================================================================
//    | Basic slider widget.
//    =============================================================================*/
//    public class Slider : Widget {
//        protected TextAreaOverlayElement mTextArea;
//        protected TextAreaOverlayElement mValueTextArea;
//        protected BorderPanelOverlayElement mTrack;
//        protected PanelOverlayElement mHandle;
//        protected bool mDragging;
//        protected bool mFitToContents;
//        protected float mDragOffset;
//        protected float mValue;
//        protected float mMinValue;
//        protected float mMaxValue;
//        protected float mInterval;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public Slider(string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps) {
//            this.mDragOffset = 0;
//            this.mValue = 0;
//            this.mMinValue = 0;
//            this.mMaxValue = 0;
//            this.mInterval = 0;
//            this.mDragging = false;
//            this.mFitToContents = false;
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Slider", "BorderPanel", name);
//            this.mElement.Width = width;
//            OverlayContainer c = (OverlayContainer)this.mElement;
//            this.mTextArea = (TextAreaOverlayElement)c.GetChild(this.getName() + "/SliderCaption");
//            OverlayContainer valueBox = (OverlayContainer)c.GetChild(this.getName() + "/SliderValueBox");
//            valueBox.Width = valueBoxWidth;
//            valueBox.Left = -(valueBoxWidth + 5);
//            this.mValueTextArea = (TextAreaOverlayElement)valueBox.GetChild(valueBox.Name + "/SliderValueText");
//            this.mTrack = (BorderPanelOverlayElement)c.GetChild(this.getName() + "/SliderTrack");
//            this.mHandle = (PanelOverlayElement)this.mTrack.GetChild(this.mTrack.Name + "/SliderHandle");

//            if (trackWidth <= 0)  // tall style
//            {
//                this.mTrack.Width = width - 16;
//            }
//            else  // long style
//            {
//                if (width <= 0) this.mFitToContents = true;
//                this.mElement.Height = 34;
//                this.mTextArea.Top = 10;
//                valueBox.Top = 2;
//                this.mTrack.Top = -23;
//                this.mTrack.Width = trackWidth;
//                this.mTrack.HorizontalAlignment = GuiHorizontalAlignment.GHA_RIGHT;
//                this.mTrack.Left = -(trackWidth + valueBoxWidth + 5);
//            }

//            this.setCaption(caption);
//            this.setRange(minValue, maxValue, snaps, false);
//        }

//        /*-----------------------------------------------------------------------------
//        | Sets the minimum value, maximum value, and the number of snapping points.
//        -----------------------------------------------------------------------------*/
//        public void setRange(float minValue, float maxValue, uint snaps) {
//            setRange(minValue, maxValue, snaps, true);

//        }
//        public void setRange(float minValue, float maxValue, uint snaps, bool notifyListener) {
//            this.mMinValue = minValue;
//            this.mMaxValue = maxValue;

//            if (snaps <= 1 || this.mMinValue >= this.mMaxValue) {
//                this.mInterval = 0;
//                this.mHandle.Hide();
//                this.mValue = minValue;
//                this.mValueTextArea.Caption = snaps == 1 ? this.mMinValue.ToString() : "";
//            }
//            else {
//                this.mHandle.Show();
//                this.mInterval = (maxValue - minValue) / (snaps - 1);
//                setValue(minValue, notifyListener);
//            }
//        }

//        public string getValueCaption() { return this.mValueTextArea.Caption; }

//        /*-----------------------------------------------------------------------------
//        | You can use this method to manually format how the value is displayed.
//        -----------------------------------------------------------------------------*/
//        public void setValueCaption(string caption) { this.mValueTextArea.Caption = caption; }
//        public void setValue(float value) {
//            setValue(value, true);
//        }
//        public void setValue(float value, bool notifyListener) {
//            if (this.mInterval == 0) { return; }

//            this.mValue = SdkTrayMathHelper.clamp(value, this.mMinValue, this.mMaxValue);

//            this.setValueCaption(this.mValue.ToString());

//            if (this.mListener != null && notifyListener) { this.mListener.sliderMoved(this); }

//            if (!this.mDragging)
//                this.mHandle.Left = (int)((this.mValue - this.mMinValue) / (this.mMaxValue - this.mMinValue) * (this.mTrack.Width - this.mHandle.Width));
//        }

//        public float getValue() { return this.mValue; }

//        public string getCaption() { return this.mTextArea.Caption; }

//        public void setCaption(string caption) {
//            this.mTextArea.Caption = caption;

//            if (this.mFitToContents)
//                this.mElement.Width = getCaptionWidth(caption, this.mTextArea) + this.mValueTextArea.Parent.Width + this.mTrack.Width + 26;
//        }

//        public override void _cursorPressed(Vector2 cursorPos) {
//            if (!this.mHandle.IsVisible) { return; }

//            Vector2 co = Widget.cursorOffset(this.mHandle, cursorPos);

//            if (co.SquaredLength <= 81) {
//                this.mDragging = true;
//                this.mDragOffset = co.x;
//            }
//            else if (Widget.isCursorOver(this.mTrack, cursorPos)) {
//                float newLeft = this.mHandle.Left + co.x;
//                float rightBoundary = this.mTrack.Width - this.mHandle.Width;

//                this.mHandle.Left = SdkTrayMathHelper.clamp((int)newLeft, 0, (int)rightBoundary);
//                this.setValue(getSnappedValue(newLeft / rightBoundary));
//            }
//        }

//        public override void _cursorReleased(Vector2 cursorPos) {
//            if (this.mDragging) {
//                this.mDragging = false;
//                this.mHandle.Left = (int)((this.mValue - this.mMinValue) / (this.mMaxValue - this.mMinValue) *
//                    (this.mTrack.Width - this.mHandle.Width));
//            }
//        }

//        public override void _cursorMoved(Vector2 cursorPos) {
//            if (this.mDragging) {
//                Vector2 co = Widget.cursorOffset(this.mHandle, cursorPos);
//                float newLeft = this.mHandle.Left + co.x - this.mDragOffset;
//                float rightBoundary = this.mTrack.Width - this.mHandle.Width;

//                this.mHandle.Left = SdkTrayMathHelper.clamp((int)newLeft, 0, (int)rightBoundary);
//                setValue(getSnappedValue(newLeft / rightBoundary));
//            }
//        }

//        public override void _focusLost() { this.mDragging = false; }

//        /*-----------------------------------------------------------------------------
//        | Internal method - given a percentage (from left to right), gets the
//        | value of the nearest marker.
//        -----------------------------------------------------------------------------*/
//        protected float getSnappedValue(float percentage) {
//            percentage = SdkTrayMathHelper.clamp(percentage, 0, 1);
//            uint whichMarker = (uint)(percentage * (this.mMaxValue - this.mMinValue) / this.mInterval + 0.5);
//            return whichMarker * this.mInterval + this.mMinValue;
//        }
//    }

//    /*=============================================================================
//    | Basic parameters panel widget.
//    =============================================================================*/
//    public class ParamsPanel : Widget {

//        protected TextAreaOverlayElement mNamesArea;
//        protected TextAreaOverlayElement mValuesArea;
//        protected string[] mNames;
//        protected string[] mValues;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public ParamsPanel(String name, float width, int lines) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ParamsPanel", "BorderPanel", name);
//            OverlayContainer c = (OverlayContainer)this.mElement;
//            this.mNamesArea = (TextAreaOverlayElement)c.GetChild(this.getName() + "/ParamsPanelNames");
//            this.mValuesArea = (TextAreaOverlayElement)c.GetChild(this.getName() + "/ParamsPanelValues");
//            this.mElement.Width = width;
//            this.mElement.Height = this.mNamesArea.Top * 2 + lines * this.mNamesArea.CharHeight;

//            this.mValues = new string[0];
//            this.mNames = new string[0];
//        }

//        public void setAllParamNames(string[] paramNames) {
//            this.mNames = paramNames;
//            this.mValues = new string[0];
//            Array.Resize<string>(ref this.mValues, mNames.Length);
//            this.mElement.Height = this.mNamesArea.Top * 2 + mNames.Length * this.mNamesArea.CharHeight;
//            this.updateText();
//        }

//        public string[] getAllParamNames() { return mNames; }

//        public void setAllParamValues(string[] paramValues) {
//            this.mValues = paramValues;
//            Array.Resize<string>(ref this.mValues, mNames.Length);
//            this.updateText();
//        }

//        public void setParamValue(string paramName, string paramValue) {
//            for (int i = 0; i < this.mNames.Length; i++) {
//                if (mNames[i] == paramName) {
//                    mValues[i] = paramValue;
//                    this.updateText();
//                    return;
//                }
//            }
//            String desc = "ParamsPanel \"" + getName() + "\" has no parameter \"" + paramName + "\".";
//            throw new Exception("Item not found : " + desc + " ParamsPanel.setParamValue");
//        }

//        public void setParamValue(int index, string paramValue) {
//            if (index >= this.mNames.Length) {
//                String desc = "ParamsPanel \"" + getName() + "\" has no parameter at position " +
//                    index.ToString() + ".";
//                throw new Exception("Item not found : " + desc + "ParamsPanel.setParamValue");
//            }

//            this.mValues[index] = paramValue;
//            this.updateText();
//        }

//        public string getParamValue(string paramName) {
//            for (int i = 0; i < mNames.Length; i++) {
//                if (mNames[i] == paramName) return mValues[i];
//            }

//            String desc = "ParamsPanel \"" + getName() + "\" has no parameter \"" + paramName + "\".";
//            throw new Exception("Item not found : " + desc + "ParamsPanel.getParamValue");
//        }

//        public string getParamValue(int index) {
//            if (index >= this.mNames.Length) {
//                String desc = "ParamsPanel \"" + getName() + "\" has no parameter at position " +
//                    index.ToString() + ".";
//                throw new Exception("Item not found : " + desc + "ParamsPanel.getParamValue");
//            }
//            return mValues[index];
//        }

//        public string[] getAllParamValues() { return mValues; }


//        /*-----------------------------------------------------------------------------
//        | Internal method - updates text areas based on name and value lists.
//        -----------------------------------------------------------------------------*/
//        protected void updateText() {
//            string namesDS = "";
//            string valuesDS = "";

//            for (int i = 0; i < mNames.Length; i++) {
//                namesDS += mNames[i] + ":\n";
//                valuesDS += mValues[i] + "\n";
//            }

//            mNamesArea.Caption = namesDS;
//            mValuesArea.Caption = valuesDS;
//        }
//    }

//    /*=============================================================================
//    | Basic check box widget.
//    =============================================================================*/
//    public class CheckBox : Widget {

//        protected TextAreaOverlayElement mTextArea;
//        protected BorderPanelOverlayElement mSquare;
//        protected OverlayElement mX;
//        protected bool mFitToContents;
//        protected bool mCursorOver;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public CheckBox(string name, string caption, float width) {
//            this.mCursorOver = false;
//            this.mFitToContents = width <= 0;
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/CheckBox", "BorderPanel", name);
//            OverlayContainer c = (OverlayContainer)mElement;
//            this.mTextArea = (TextAreaOverlayElement)c.GetChild(getName() + "/CheckBoxCaption");
//            this.mSquare = (BorderPanelOverlayElement)c.GetChild(getName() + "/CheckBoxSquare");
//            this.mX = this.mSquare.GetChild(this.mSquare.Name + "/CheckBoxX");
//            this.mX.Hide();
//            this.mElement.Width = width;
//            this.setCaption(caption);
//        }

//        public string getCaption() { return mTextArea.Caption; }

//        public void setCaption(string caption) {
//            this.mTextArea.Caption = caption;
//            if (mFitToContents) this.mElement.Width = getCaptionWidth(caption, mTextArea) + mSquare.Width + 23;
//        }

//        public bool isChecked() { return mX.IsVisible; }
//        public void setChecked(bool chkd) {
//            setChecked(chkd, true);
//        }
//        public void setChecked(bool chkd, bool notifyListener) {
//            if (chkd) mX.Show();
//            else mX.Hide();
//            if (this.mListener != null && notifyListener) this.mListener.checkBoxToggled(this);
//        }
//        public void toggle() {
//            toggle(true);
//        }
//        public void toggle(bool notifyListener) { this.setChecked(!isChecked(), notifyListener); }

//        public override void _cursorPressed(Vector2 cursorPos) { if (this.mCursorOver && this.mListener != null) toggle(); }

//        public override void _cursorMoved(Vector2 cursorPos) {
//            if (isCursorOver(this.mSquare, cursorPos, 5)) {
//                if (!mCursorOver) {
//                    this.mCursorOver = true;
//                    this.mSquare.MaterialName = "SdkTrays/MiniTextBox/Over";
//                    this.mSquare.BorderMaterialName = "SdkTrays/MiniTextBox/Over";
//                }
//            }
//            else {
//                if (this.mCursorOver) {
//                    this.mCursorOver = false;
//                    this.mSquare.MaterialName = "SdkTrays/MiniTextBox";
//                    this.mSquare.BorderMaterialName = "SdkTrays/MiniTextBox";
//                }
//            }
//        }

//        public override void _focusLost() {
//            this.mSquare.MaterialName = "SdkTrays/MiniTextBox";
//            this.mSquare.BorderMaterialName = "SdkTrays/MiniTextBox";
//            mCursorOver = false;
//        }
//    }

//    /*=============================================================================
//    | Custom, decorative widget created from a template.
//    =============================================================================*/
//    public class DecorWidget : Widget {

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public DecorWidget(string name, string templateName) {
//            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate(templateName, "", name);
//        }
//    }

//    /*=============================================================================
//    | Basic progress bar widget.
//    =============================================================================*/
//    public class ProgressBar : Widget {
//        protected TextAreaOverlayElement mTextArea;
//        protected TextAreaOverlayElement mCommentTextArea;
//        protected OverlayElement mMeter;
//        protected OverlayElement mFill;
//        protected float mProgress;

//        // Do not instantiate any widgets directly. Use SdkTrayManager.
//        public ProgressBar(string name, string caption, float width, float commentBoxWidth) {
//            mProgress = 0;
//            mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ProgressBar", "BorderPanel", name);
//            mElement.Width = width;
//            OverlayContainer c = (OverlayContainer)mElement;
//            mTextArea = (TextAreaOverlayElement)c.GetChild(getName() + "/ProgressCaption");
//            OverlayContainer commentBox = (OverlayContainer)c.GetChild(getName() + "/ProgressCommentBox");
//            commentBox.Width = commentBoxWidth;
//            commentBox.Left = -(commentBoxWidth + 5);
//            mCommentTextArea = (TextAreaOverlayElement)commentBox.GetChild(commentBox.Name + "/ProgressCommentText");
//            mMeter = c.GetChild(getName() + "/ProgressMeter");
//            mMeter.Width = width - 10;
//            mFill = ((OverlayContainer)mMeter).GetChild(mMeter.Name + "/ProgressFill");
//            setCaption(caption);
//        }

//        /*-----------------------------------------------------------------------------
//        | Sets the progress as a percentage.
//        -----------------------------------------------------------------------------*/
//        public void setProgress(float progress) {
//            mProgress = SdkTrayMathHelper.clamp(progress, 0, 1);
//            mFill.Width = System.Math.Min((int)mFill.Height, (int)(mProgress * (mMeter.Width - 2 * mFill.Left)));
//        }

//        /*-----------------------------------------------------------------------------
//        | Gets the progress as a percentage.
//        -----------------------------------------------------------------------------*/
//        public float getProgress() {
//            return mProgress;
//        }

//        public string getCaption() {
//            return mTextArea.Caption;
//        }

//        public void setCaption(string caption) {
//            mTextArea.Caption = caption;
//        }

//        public string getComment() {
//            return mCommentTextArea.Caption;
//        }

//        public void setComment(string comment) {
//            mCommentTextArea.Caption = comment;
//        }
//    }
//}
