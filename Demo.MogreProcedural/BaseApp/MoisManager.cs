
using System.Globalization;
using MOIS;
using System;


namespace Mogre_Procedural.Game.BaseApp
{
    public class MoisManager
    {
        private InputManager mInputMgr;
        private readonly bool[] mKeyDown;
        private readonly bool[] mKeyPressed;
        private readonly bool[] mKeyReleased;
        private readonly bool[] mMouseDown;
        private readonly bool[] mMousePressed;
        private readonly bool[] mMouseReleased;
        private Vector3 mMouseMove;
        private Vector3 mMousePos;
        private Vector3 mMousePressedPos;
        private Vector3 mMouseReleasedPos;
        public delegate bool IsKeyEvent(KeyCode key); // Represents WasKeyPressed, WasKeyReleased or IsKeyDown

        public Keyboard KeyBoard { get; private set; }
        public Mouse Mouse { get; private set; }

        public bool IsShiftDown { get { return this.IsOneKeyEventTrue(this.IsKeyDown, KeyCode.KC_LSHIFT, KeyCode.KC_RSHIFT); } }
        public bool IsCtrltDown { get { return this.IsOneKeyEventTrue(this.IsKeyDown, KeyCode.KC_LCONTROL, KeyCode.KC_RCONTROL); } }

        // Get last relative mouse movement (and wheel movement on Z axis)
        public int MouseMoveX { get { return (int)this.mMouseMove.x; } }
        public int MouseMoveY { get { return (int)this.mMouseMove.y; } }
        public int MouseMoveZ { get { return (int)this.mMouseMove.z; } }

        // Get absolute mouse position within window bounds
        public int MousePosX { get { return (int)this.mMousePos.x; } }
        public int MousePosY { get { return (int)this.mMousePos.y; } }

        // Get last absolute mouse position when a mouse button was pressed
        public int MousePressedPosX { get { return (int)this.mMousePressedPos.x; } }
        public int MousePressedPosY { get { return (int)this.mMousePressedPos.y; } }

        // Get last absolute mouse position when a mouse button was released
        public int MouseReleasedPosX { get { return (int)this.mMouseReleasedPos.x; } }
        public int MouseReleasedPosY { get { return (int)this.mMouseReleasedPos.y; } }

        internal MoisManager() {
            this.mInputMgr = null;
            this.KeyBoard = null;
            this.Mouse = null;
            this.mKeyDown = new bool[256];
            this.mKeyPressed = new bool[256];
            this.mKeyReleased = new bool[256];
            this.mMouseDown = new bool[8];
            this.mMousePressed = new bool[8];
            this.mMouseReleased = new bool[8];
            this.mMouseMove = new Vector3();
            this.mMousePos = new Vector3();
            this.mMousePressedPos = new Vector3();
            this.mMouseReleasedPos = new Vector3();
        }
        int mWindowHanldle;
        internal bool Startup(int windowHandle, uint width, uint height) {
            mWindowHanldle = windowHandle;
            if (this.mInputMgr != null)
                return false;

            // initialize input manager
            ParamList pl = new ParamList();
            pl.Insert("WINDOW", windowHandle.ToString());
            pl.Insert("w32_mouse", "DISCL_NONEXCLUSIVE");
            pl.Insert("w32_mouse", "DISCL_FOREGROUND");
            pl.Insert("w32_keyboard", "DISCL_FOREGROUND");
            pl.Insert("w32_keyboard", "DISCL_NONEXCLUSIVE");

            this.mInputMgr = InputManager.CreateInputSystem(pl);
            if (this.mInputMgr == null)
                return false;

            // initialize keyboard
            this.KeyBoard = (Keyboard)this.mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            if (this.KeyBoard == null)
                return false;

            // set up keyboard event handlers
            this.KeyBoard.KeyPressed += OnKeyPressed;
            this.KeyBoard.KeyReleased += OnKeyReleased;

            // initialize mouse
            this.Mouse = (Mouse)this.mInputMgr.CreateInputObject(MOIS.Type.OISMouse, true);
            if (this.Mouse == null)
                return false;

            // set up area for absolute mouse positions
            MouseState_NativePtr state = this.Mouse.MouseState;
            state.width = (int)width;
            state.height = (int)height;

            // set up mouse event handlers
            this.Mouse.MouseMoved += OnMouseMoved;
            this.Mouse.MousePressed += OnMousePressed;
            this.Mouse.MouseReleased += OnMouseReleased;

            this.Clear();

            return true;
        }

        internal void Update() {
            this.ClearKeyPressed();
            this.ClearKeyReleased();
            this.ClearMousePressed();
            this.ClearMouseReleased();
            this.ClearMouseMove();

            this.KeyBoard.Capture();
            this.Mouse.Capture();
        }
        //public void Capture() { 
            
        //}
        public void Clear() {
            this.ClearKeyPressed();
            this.ClearKeyReleased();
            this.ClearKeyDown();
            this.ClearMousePressed();
            this.ClearMouseReleased();
            this.ClearMouseDown();
            this.ClearMouseMove();
        }

        public bool IsKeyDown(KeyCode key) { return this.mKeyDown[(int)key]; }
        public bool WasKeyPressed(KeyCode key) {
            return WasKeyPressed(key, false);
        }
        public bool WasKeyPressed(KeyCode key, bool setToFalse) {
            if (setToFalse) { this.mKeyPressed[(int)key] = false; }
            return this.mKeyPressed[(int)key];
        }
        public bool WasKeyReleased(KeyCode key) { return this.mKeyReleased[(int)key]; }
        public bool IsMouseButtonDown(MouseButtonID button) { return this.mMouseDown[(int)button]; }
        public bool WasMouseButtonPressed(MouseButtonID button) { return this.mMousePressed[(int)button]; }
        public bool WasMouseButtonReleased(MouseButtonID button) { return this.mMouseReleased[(int)button]; }
        public bool WasMouseMoved() { return this.mMouseMove.x != 0 || this.mMouseMove.y != 0 || this.mMouseMove.z != 0; }

        public bool IsOneKeyEventTrue(IsKeyEvent keyEvent, params KeyCode[] keys) {
            foreach (KeyCode key in keys)
                if (keyEvent(key)) { return true; }

            return false;
        }

        public bool AreAllKeyEventTrue(IsKeyEvent keyEvent, params KeyCode[] keys) {
            foreach (KeyCode key in keys)
                if (!keyEvent(key)) { return false; }

            return true;
        }

        private void ClearKeyPressed() {
            for (int i = 0; i < this.mKeyPressed.Length; ++i)
                this.mKeyPressed[i] = false;
        }

        private void ClearKeyReleased() {
            for (int i = 0; i < this.mKeyReleased.Length; ++i)
                this.mKeyReleased[i] = false;
        }

        private void ClearKeyDown() {
            for (int i = 0; i < this.mKeyDown.Length; ++i)
                this.mKeyDown[i] = false;
        }

        private void ClearMousePressed() {
            for (int i = 0; i < this.mMousePressed.Length; ++i)
                this.mMousePressed[i] = false;
        }

        private void ClearMouseReleased() {
            for (int i = 0; i < this.mMouseReleased.Length; ++i)
                this.mMouseReleased[i] = false;
        }

        private void ClearMouseDown() {
            for (int i = 0; i < this.mMouseDown.Length; ++i)
                this.mMouseDown[i] = false;
        }

        private void ClearMouseMove() {
            this.mMouseMove.x = 0;
            this.mMouseMove.y = 0;
            this.mMouseMove.z = 0;
        }

        private bool OnKeyPressed(KeyEvent arg) {
            this.mKeyDown[(int)arg.key] = true;
            this.mKeyPressed[(int)arg.key] = true;
            return true;
        }

        private bool OnKeyReleased(KeyEvent arg) {
            this.mKeyDown[(int)arg.key] = false;
            this.mKeyReleased[(int)arg.key] = true;
            return true;
        }

        /*public string GetText()
{
string txt = "";
foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
{
if (this.WasKeyPressed(code))
{
char c = this.GetKeyCodeChar(code);
if (c != '\0')
{
//if (c == (char)127)
txt += c;
}
}
}

return txt;
}

public char GetKeyCodeChar(KeyCode code)
{
char c = '\0';
string txt;
if (code == KeyCode.KC_SPACE) { c = ' '; }
else if (code == KeyCode.KC_LBRACKET) { c = '('; }
else if (code == KeyCode.KC_RBRACKET) { c = ')'; }
else if (code == KeyCode.KC_MINUS) { c = '_'; }
else if (code == KeyCode.KC_SLASH) { c = '/'; }
else if (code == KeyCode.KC_BACK) { c = (char)127; } // Supr
else if (code == KeyCode.KC_DELETE) { c = (char)8; } // Del
if(c == '\0') // The char isn't a special character
{
txt = Enum.GetName(typeof(KeyCode), code);
txt = txt.Substring(3);

if (txt.Length == 1) // Consider the letters and numbers
{
char tmp;

if (!this.IsShiftDown) { tmp = txt.ToLower()[0]; }
else { tmp = txt.ToUpper()[0]; }

if ((tmp >= 'a' && tmp <= 'z') || (tmp >= '0' && tmp <= '9') || (tmp >= 'A' && tmp <= 'Z'))
c = tmp;
}
}

return c;
}*/

        private bool OnMouseMoved(MouseEvent arg) {
            this.mMouseMove.x = arg.state.X.rel;
            this.mMouseMove.y = arg.state.Y.rel;
            this.mMouseMove.z = arg.state.Z.rel;
            this.mMousePos.x = arg.state.X.abs;
            this.mMousePos.y = arg.state.Y.abs;
            return true;
        }

        private bool OnMousePressed(MouseEvent arg, MouseButtonID id) {
            this.mMouseDown[(int)id] = true;
            this.mMousePressed[(int)id] = true;
            this.mMousePos.x = arg.state.X.abs;
            this.mMousePos.y = arg.state.Y.abs;
            this.mMousePressedPos.x = arg.state.X.abs;
            this.mMousePressedPos.y = arg.state.Y.abs;
            return true;
        }

        private bool OnMouseReleased(MouseEvent arg, MouseButtonID id) {
            this.mMouseDown[(int)id] = false;
            this.mMouseReleased[(int)id] = true;
            this.mMousePos.x = arg.state.X.abs;
            this.mMousePos.y = arg.state.Y.abs;
            this.mMouseReleasedPos.x = arg.state.X.abs;
            this.mMouseReleasedPos.y = arg.state.Y.abs;
#if DEBUG
            Console.WriteLine(string.Format( "mouse: x:{0} y:{1}",this.mMousePos.x,this.mMousePos.y));       
#endif
            return true;
        }
        public System.Drawing.Point GetMousePosInControl() {
            IntPtr ptr = new IntPtr(mWindowHanldle);
            System.Windows.Forms.Control f = System.Windows.Forms.Control.FromHandle(ptr);
            System.Drawing.Point p = f.PointToScreen(new System.Drawing.Point(Mouse.MouseState.X.abs, Mouse.MouseState.Y.abs));
            System.Drawing.Point cp = System.Windows.Forms.Control.FromHandle(ptr).PointToClient(p);

            return cp;
        }

       public void InputSystem_SizeChanged(int cWidth,int cHeight) {            
            MOIS.MouseState_NativePtr mptr = this.Mouse.MouseState;
            if (cWidth != 0 && cHeight != 0) {
                mptr.width = cWidth;
                mptr.height = cHeight;
            }


        }
        internal void Shutdown() {
            if (this.Mouse != null) {
                this.mInputMgr.DestroyInputObject(this.Mouse);
                this.Mouse = null;
            }

            if (this.KeyBoard != null) {
                this.mInputMgr.DestroyInputObject(this.KeyBoard);
                this.KeyBoard = null;
            }

            if (this.mInputMgr != null) {
                InputManager.DestroyInputSystem(this.mInputMgr);
                this.mInputMgr = null;
            }
        }
    }
}