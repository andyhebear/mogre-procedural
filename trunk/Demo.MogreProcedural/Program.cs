/*
-----------------------------------------------------------------------------
This source file is part of mogre-procedural
For the latest info, see http://code.google.com/p/mogre-procedural/
my blog:http://hi.baidu.com/rainssoft
this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/
   
Copyright (c) 2013-2020 rains soft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/

namespace Demo.MogreProcedural
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Mogre;
    using Mogre_Procedural.Game.BaseApp;
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
#if !DEBUG
            try {
#endif
                Demo_MogreProcedural app = new Demo_MogreProcedural();
                app.Go();

                //MogreForm mf = new MogreForm();
                //Demo_MogreProcedural app = new Demo_MogreProcedural(new System.Drawing.Point(30, 30), mf.Handle);
                //mf.OgreWindowSet(app);
                //mf.Show();
#if !DEBUG
            }
            catch (System.Runtime.InteropServices.SEHException) {
                // Check if it's an Ogre Exception
                if (OgreException.IsThrown)
                    ShowOgreException();
                else
                    throw;
            }
#endif
        }
        public static void ShowOgreException() {
            if (OgreException.IsThrown)
                System.Windows.Forms.MessageBox.Show(OgreException.LastException.FullDescription, "An exception has occured!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
