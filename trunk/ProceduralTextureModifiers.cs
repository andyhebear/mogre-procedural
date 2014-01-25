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
//#define PROCEDURAL_USE_FREETYPE
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    //* \addtogroup texturegrp Textures
    //Elements for procedural texture creation.
    //@{
    //

    //*
    //\brief Use alpha channel as an mask for an other image.
    //\details Can take normal, height or quaternion map as second input.
    //
    //<b>Map translations</b>
    //<ul><li>%Alpha channel: a(0, 255) -> angle(0, 2pi)</li>
    //<li>Coords: r(0,255) -> x(-1, 1)</li>
    //<li>Height map: luminance(0, 255) -> angle(0, 2pi) (axis is Z)</li>
    //<li>Normal map: is blended with the source</li></ul>
    //
    //<b>Options</b>
    //<ul><li>Rotation: w(0, 1) -> angle(0, 2pi), rest are axis direction coordinates</li>
    //<li>Sensitivity: (0, 255) -> angle(0, 2pi) * map alpha</li>
    //<li>Compensation: How to deal with map</li>
    //<li>Mirror: For broken normal maps</li></ul>
    //
    //Example:
    //\code{.cpp}
    //// Image colour
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //// Filter
    //Procedural::Abnormals(&bufferGradient).setParameterImage(&bufferCell).process();
    //\endcode
    //\dotfile texture_02.gv
    //\todo Need bugfix
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Abnormals : public TextureProcessing
    public class Abnormals : TextureProcessing
    {
        //! Methods how to work with parameter map
        public enum ABNORMALS_COMPENSATION : int
        {
            COMPENSATION_NORMAL, //!< Use as a normal map
            COMPENSATION_HEIGHT, //!< Use as a height map
            COMPENSATION_QUATERNION //!< Use as a list of quaternion
        }

        //! Methods how to fix broken normal maps
        public enum ABNORMALS_MIRROR : int
        {
            MIRROR_NONE, //!< None
            MIRROR_X_YZ, //!< X : YZ
            MIRROR_Y_XZ, //!< Y : XZ
            MIRROR_X_Y_Z //!< X+Y : Z
        }

        private TextureBuffer mParam;
        private Radian mW = new Radian();
        private Vector3 mAxis = new Vector3();
        private byte mSensitivity = 0;
        private ABNORMALS_COMPENSATION mCompensation;
        private ABNORMALS_MIRROR mMirror;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Abnormals(TextureBuffer pBuffer)
            : base(pBuffer, "Abnormals") {
            mParam = null;
            mW = 0.0f;
            mAxis = new Vector3(0.0f, 0.0f, 1.0f);
            mSensitivity = 127;
            mCompensation = ABNORMALS_COMPENSATION.COMPENSATION_NORMAL;
            mMirror = ABNORMALS_MIRROR.MIRROR_NONE;
        }

        //    *
        //	Set parameter image for compensation.
        //	\param image Pointer to second image (default NULL)
        //	\note If the parameter image is set to NULL there won't be any compensation.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Abnormals setParameterImage(TextureBuffer image) {
            mParam = image;
            return this;
        }

        //    *
        //	Set rotation angle.
        //	\param rotation New rotation angle [0.0, 1.0] \(default 0.0)
        //	
        public Abnormals setRotation(float rotation) {
            mW = new Radian(rotation * Math.TWO_PI);
            return this;
        }

        //    *
        //	Set rotation angle.
        //	\param rotation New rotation angle [0.0, Ogre::Math::TWO_PI] rad \(default 0.0)
        //	
        public Abnormals setRotation(Radian rotation) {
            //
            //ORIGINAL LINE: mW = rotation;
            mW = (rotation);
            return this;
        }

        //    *
        //	Set rotation angle.
        //	\param rotation New rotation angle [0, 360] degree \(default 0)
        //	
        public Abnormals setRotation(Degree rotation) {
            mW = (Radian)rotation.ValueRadians;
            return this;
        }

        //    *
        //	Set rotation axis.
        //	\param axis New rotation axis (default Ogre::Vector3(0.0f, 0.0f, 1.0f))
        //	
        public Abnormals setAxis(Vector3 axis) {
            //
            //ORIGINAL LINE: mAxis = axis;
            mAxis = (axis);
            return this;
        }

        //    *
        //	Set rotation axis.
        //	\param x New x coordinate of rotation axis \(default 0.0)
        //	\param y New y coordinate of rotation axis \(default 0.0)
        //	\param z New z coordinate of rotation axis \(default 1.0)
        //	
        public Abnormals setAxis(float x, float y) {
            return setAxis(x, y, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Abnormals& setAxis(Ogre::float x, Ogre::float y, Ogre::float z = 1.0f)
        public Abnormals setAxis(float x, float y, float z) {
            mAxis = new Vector3(x, y, z);
            return this;
        }

        //    *
        //	Set sensitivity.
        //	\param sensitivity New sensitivity value [0, 255] (default 127)
        //	
        public Abnormals setSensitivity(byte sensitivity) {
            mSensitivity = sensitivity;
            return this;
        }

        //    *
        //	Set compensation method.
        //	\param compensation Compensation method to use (default COMPENSATION_NORMAL)
        //	
        public Abnormals setCompensation(ABNORMALS_COMPENSATION compensation) {
            mCompensation = compensation;
            return this;
        }

        //    *
        //	Set mirror method.
        //	\param mirror Compensation method to use (default MIRROR_NONE)
        //	
        public Abnormals setMirror(ABNORMALS_MIRROR mirror) {
            mMirror = mirror;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            Quaternion qion = new Quaternion();
            float sum = 0f;
            Vector3 q = new Vector3();

            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            Quaternion rotation = new Quaternion(mW, mAxis);

            if (mParam != null && (mParam.getWidth() < w || mParam.getHeight() < h))
                return mBuffer;

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    ColourValue pixel = mBuffer.getPixel(x, y);
                    Quaternion v = new Quaternion(0.0f, ((pixel.r * 255.0f) - 127.5f) / 127.5f, ((pixel.b * 255.0f) - 127.5f) / 127.5f, ((pixel.g * 255.0f) - 127.5f) / 127.5f);

                    if (mParam != null) {
                        pixel = mParam.getPixel(x, y);
                        switch (mCompensation) {
                            case ABNORMALS_COMPENSATION.COMPENSATION_NORMAL:
                                qion = new Quaternion(0.0f, (pixel.r * 255.0f) - 127.5f, (pixel.b * 255.0f) - 127.5f, (pixel.g * 255.0f) - 127.5f);
                                v = v * (float)(1 - mSensitivity);
                                v = v + qion * ((float)mSensitivity / 127.5f);
                                break;

                            case ABNORMALS_COMPENSATION.COMPENSATION_HEIGHT:
                                sum = ((pixel.r + pixel.g + pixel.b) / 3.0f) * 255.0f;
                                qion = new Quaternion(new Radian(Math.TWO_PI * sum / 765.0f * mSensitivity), new Vector3(0.0f, 1.0f, 0.0f));
                                rotation = rotation * qion;
                                break;

                            case ABNORMALS_COMPENSATION.COMPENSATION_QUATERNION:
                                q = new Vector3((pixel.r * 255.0f) - 127.5f, (pixel.b * 255.0f) - 127.5f, (pixel.g * 255.0f) - 127.5f);
                                qion = new Quaternion(new Radian(2.0f / 255.0f * Math.PI * pixel.a * mSensitivity), q);
                                rotation = rotation * qion;
                                break;
                        }
                    }

                    v = rotation * v * rotation.Inverse();
                    float norm = v.Normalise();

                    if (mMirror == ABNORMALS_MIRROR.MIRROR_X_YZ || mMirror == ABNORMALS_MIRROR.MIRROR_X_Y_Z)
                        mBuffer.setRed(x, y, (1.0f - v.x * 0.5f + 0.5f));
                    else
                        mBuffer.setRed(x, y, (v.x * 0.5f + 0.5f));
                    if (mMirror == ABNORMALS_MIRROR.MIRROR_Y_XZ || mMirror == ABNORMALS_MIRROR.MIRROR_X_Y_Z)
                        mBuffer.setGreen(x, y, (1.0f - v.z * 0.5f + 0.5f));
                    else
                        mBuffer.setGreen(x, y, (v.z * 0.5f + 0.5f));
                    mBuffer.setBlue(x, y, (v.y * 0.5f + 0.5f));
                }
            }

            Utils.log("Modify texture with abnormals filter");
            return mBuffer;
        }
    }

    //*
    //\brief Colour extraction.
    //\details Extract single color intensity, replace it with white smoke, discard the rest.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //Procedural::Alpha(&bufferCell).process();
    //\endcode
    //\dotfile texture_03.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Alpha : public TextureProcessing
    public class Alpha : TextureProcessing
    {
        private ColourValue mExtractColour = new ColourValue();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Alpha(TextureBuffer pBuffer)
            : base(pBuffer, "Alpha") {
            mExtractColour = ColourValue.White;
        }

        //    *
        //	Set the colour to extract.
        //	\param colour New colour for extraction (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Alpha setExtractColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mExtractColour = colour;
            mExtractColour = (colour);
            return this;
        }

        //    *
        //	Set the colour to extract.
        //	\param red Red value of extraction colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of extraction colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of extraction colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of extraction colour [0.0, 1.0] \(default 1.0)
        //	
        public Alpha setExtractColour(float red, float green, float blue) {
            return setExtractColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Alpha& setExtractColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Alpha setExtractColour(float red, float green, float blue, float alpha) {
            mExtractColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    float r = (float)mBuffer.getPixelRedByte(x, y) * mExtractColour.r;
                    float g = (float)mBuffer.getPixelGreenByte(x, y) * mExtractColour.g;
                    float b = (float)mBuffer.getPixelBlueByte(x, y) * mExtractColour.b;
                    byte a = (byte)(((uint)mBuffer.getPixelAlphaByte(x, y) + (uint)(r + g + b)) >> 1);
                    mBuffer.setPixel(x, y, a, a, a, a);
                }
            }

            Utils.log("Modify texture with alpha filter");
            return mBuffer;
        }
    }

    //*
    //\brief Use alpha channel as an mask for an other image.
    //\details <ol><li>Extract alpha channel as an opaque monochrome bitmap or</li><li>multiply alpha channel with prameter image luminance, or</li><li>use parameter image as color alpha mask.</li></ol> The luminance is not taken into account.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //Procedural::AlphaMask(&bufferGradient).setParameterImage(&bufferCell).process();
    //\endcode
    //\dotfile texture_04.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport AlphaMask : public TextureProcessing
    public class AlphaMask : TextureProcessing
    {
        private TextureBuffer mParam;
        private bool mColourMask;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public AlphaMask(TextureBuffer pBuffer)
            : base(pBuffer, "AlphaMask") {
            mColourMask = false;
            mParam = null;
        }

        //    *
        //	Set mode of alpha masking.
        //	\param colourmask If set to true parameter image will be used as alph mask (default false)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public AlphaMask setColourAlphaMask(bool colourmask) {
            mColourMask = colourmask;
            return this;
        }

        //    *
        //	Set parameter image for masking/colouring.
        //	\param image Pointer to second image (default NULL)
        //	\note Methode 1 is used if the parameter image is set to zero. If the size of the parameter image is smaller than the base buffer the operation will be canceled without any image manipulation.
        //	
        public AlphaMask setParameterImage(TextureBuffer image) {
            mParam = image;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();

            if (mParam != null && (mParam.getWidth() < w || mParam.getHeight() < h))
                return mBuffer;

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    if (mParam != null) {
                        if (mColourMask) {
                            ColourValue pixelA = mBuffer.getPixel(x, y);
                            ColourValue pixelB = mParam.getPixel(x, y);
                            Vector3 c1 = new Vector3(pixelA.r * 255.0f, pixelA.g * 255.0f, pixelA.b * 255.0f);
                            Vector3 c2 = new Vector3(pixelB.r * 255.0f, pixelB.g * 255.0f, pixelB.b * 255.0f);

                            float c1norm = c1.Normalise();
                            float c2norm = c2.Normalise();

                            float correctness = 0;

                            if (c1norm > 0.0f && c2norm > 0.0f)
                                correctness = c1.x * c2.x + c1.y * c2.y + c1.z * c2.z;

                            mBuffer.setAlpha(x, y, (byte)(pixelA.a * correctness));
                        }
                        else {
                            ColourValue pixel = mParam.getPixel(x, y);
                            float alpha = (pixel.r + pixel.g + pixel.b) / 3.0f;
                            mBuffer.setAlpha(x, y, mBuffer.getPixelAlphaReal(x, y) * alpha);
                        }
                    }
                    else {
                        byte a = mBuffer.getPixelAlphaByte(x, y);
                        mBuffer.setPixel(x, y, a, a, a, 255);
                    }
                }
            }

            Utils.log("Modify texture with alphamask filter");
            return mBuffer;
        }
    }

    //*
    //\brief copies a part of the input buffer towards the current buffer.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //Procedural::Blit(&bufferImage).setInputBuffer(&bufferGradient).setInputRect(0.0f, 0.0f, 0.5f, 0.5f).setOutputRect(0.25f, 0.25f, 0.75f, 0.75f).process();
    //\endcode
    //\dotfile texture_31.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Blit : public TextureProcessing
    public class Blit : TextureProcessing
    {
        private TextureBuffer mInputBuffer;
        private Rect mInputRect = new Rect();
        private Rect mOutputRect = new Rect();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Blit(TextureBuffer pBuffer)
            : base(pBuffer, "Blit") {
            mInputBuffer = null;
            mOutputRect.left = 0;
            mOutputRect.top = 0;
            mOutputRect.right = (int)pBuffer.getWidth();
            mOutputRect.bottom = (int)pBuffer.getHeight();
        }

        //    *
        //	Sets the texture buffer that must be copied towards the current texture buffer
        //	\param inputBuffer Pointer on image where to copy from
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Blit setInputBuffer(TextureBuffer inputBuffer) {
            if (inputBuffer != null)
                if (inputBuffer.getHeight() >= mBuffer.getHeight() && inputBuffer.getWidth() >= mBuffer.getWidth()) {
                    mInputBuffer = inputBuffer;
                    mInputRect.left = 0;
                    mInputRect.top = 0;
                    mInputRect.right = (int)inputBuffer.getWidth();
                    mInputRect.bottom = (int)inputBuffer.getHeight();
                }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the input buffer to copy.
        //	\param rect Full rectangle description (default: left=0.0, top=0.0, right=1.0, bottom=1.0)
        //	\param relative If this is set to true (default) the rectangle data are relative [0.0, 1.0]; else absolut [px]
        //	
        public Blit setInputRect(RealRect rect) {
            return setInputRect(rect, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Blit& setInputRect(RealRect rect, bool relative = true)
        public Blit setInputRect(RealRect rect, bool relative) {
            if (mInputBuffer == null)
                return this;
            if (relative) {
                mInputRect.left = (int)((float)mInputBuffer.getWidth() * (float)System.Math.Min(rect.left, 1.0f));
                mInputRect.top = (int)((float)mInputBuffer.getHeight() * (float)System.Math.Min(rect.top, 1.0f));
                mInputRect.right = (int)((float)mInputBuffer.getWidth() * (float)System.Math.Min(rect.right, 1.0f));
                mInputRect.bottom = (int)((float)mInputBuffer.getHeight() * (float)System.Math.Min(rect.bottom, 1.0f));
            }
            else {
                mInputRect.left = (int)System.Math.Min((int)rect.left, mInputBuffer.getWidth());
                mInputRect.top = (int)System.Math.Min((int)rect.top, mInputBuffer.getHeight());
                mInputRect.right = (int)System.Math.Min((int)rect.right, mInputBuffer.getWidth());
                mInputRect.bottom = (int)System.Math.Min((int)rect.bottom, mInputBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the input buffer to copy.
        //	\param rect Full absolute rectangle description (default: left=0, top=0, right=image width, bottom=image height)
        //	
        public Blit setInputRect(Rect rect) {
            if (mInputBuffer == null)
                return this;
            mInputRect.left = (int)System.Math.Min(rect.left, mInputBuffer.getWidth());
            mInputRect.top = (int)System.Math.Min(rect.top, mInputBuffer.getHeight());
            mInputRect.right = (int)System.Math.Min(rect.right, mInputBuffer.getWidth());
            mInputRect.bottom = (int)System.Math.Min(rect.bottom, mInputBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the input buffer to copy.
        //	\param pos1 Vector to top left start point of the rectangle (default: x=0.0, y=0.0)
        //	\param pos2 Vector to bottom right end point of the rectangle (default: x=1.0, y=1.0)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public Blit setInputRect(Vector2 pos1, Vector2 pos2) {
            return setInputRect(pos1, pos2, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Blit& setInputRect(Ogre::Vector2 pos1, Ogre::Vector2 pos2, bool relative = true)
        public Blit setInputRect(Vector2 pos1, Vector2 pos2, bool relative) {
            if (mInputBuffer == null)
                return this;
            if (relative) {
                mInputRect.left = (int)((float)mInputBuffer.getWidth() * System.Math.Min(pos1.x, 1.0f));
                mInputRect.top = (int)((float)mInputBuffer.getHeight() * System.Math.Min(pos1.y, 1.0f));
                mInputRect.right = (int)((float)mInputBuffer.getWidth() * System.Math.Min(pos2.x, 1.0f));
                mInputRect.bottom = (int)((float)mInputBuffer.getHeight() * System.Math.Min(pos2.y, 1.0f));
            }
            else {
                mInputRect.left = (int)System.Math.Min((int)pos1.x, mInputBuffer.getWidth());
                mInputRect.top = (int)System.Math.Min((int)pos1.y, mInputBuffer.getHeight());
                mInputRect.right = (int)System.Math.Min((int)pos2.x, mInputBuffer.getWidth());
                mInputRect.bottom = (int)System.Math.Min((int)pos2.y, mInputBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the input buffer to copy.
        //	\param x1 New absolute x position of rectangle start (default 0)
        //	\param y1 New absolute y position of rectangle start (default 0)
        //	\param x2 New absolute x position of rectangle end (default: image width)
        //	\param y2 New absolute y position of rectangle end (default: image height)
        //	
        public Blit setInputRect(int x1, int y1, int x2, int y2) {
            if (mInputBuffer == null)
                return this;
            mInputRect.left = (int)System.Math.Min(x1, mInputBuffer.getWidth());
            mInputRect.top = (int)System.Math.Min(y1, mInputBuffer.getHeight());
            mInputRect.right = (int)System.Math.Min(x2, mInputBuffer.getWidth());
            mInputRect.bottom = (int)System.Math.Min(y2, mInputBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the input buffer to copy.
        //	\param x1 New relative x position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param y1 New relative y position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param x2 New relative x position of rectangle end [0.0, 1.0] \(default 1.0)
        //	\param y2 New relative y position of rectangle end [0.0, 1.0] \(default 1.0)
        //	
        public Blit setInputRect(float x1, float y1, float x2, float y2) {
            if (mInputBuffer == null)
                return this;
            mInputRect.left = (int)((float)mInputBuffer.getWidth() * System.Math.Min(x1, 1.0f));
            mInputRect.top = (int)((float)mInputBuffer.getHeight() * System.Math.Min(y1, 1.0f));
            mInputRect.right = (int)((float)mInputBuffer.getWidth() * System.Math.Min(x2, 1.0f));
            mInputRect.bottom = (int)((float)mInputBuffer.getHeight() * System.Math.Min(y2, 1.0f));
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the output buffer where the input is copied to.
        //	\param rect Full rectangle description (default: left=0.0, top=0.0, right=1.0, bottom=1.0)
        //	\param relative If this is set to true (default) the rectangle data are relative [0.0, 1.0]; else absolut [px]
        //	
        public Blit setOutputRect(RealRect rect) {
            return setOutputRect(rect, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Blit& setOutputRect(RealRect rect, bool relative = true)
        public Blit setOutputRect(RealRect rect, bool relative) {
            if (relative) {
                mOutputRect.left = (int)((float)mBuffer.getWidth() * System.Math.Min(rect.left, 1.0f));
                mOutputRect.top = (int)((float)mBuffer.getHeight() * System.Math.Min(rect.top, 1.0f));
                mOutputRect.right = (int)((float)mBuffer.getWidth() * System.Math.Min(rect.right, 1.0f));
                mOutputRect.bottom = (int)((float)mBuffer.getHeight() * System.Math.Min(rect.bottom, 1.0f));
            }
            else {
                mOutputRect.left = (int)System.Math.Min((int)rect.left, mBuffer.getWidth());
                mOutputRect.top = (int)System.Math.Min((int)rect.top, mBuffer.getHeight());
                mOutputRect.right = (int)System.Math.Min((int)rect.right, mBuffer.getWidth());
                mOutputRect.bottom = (int)System.Math.Min((int)rect.bottom, mBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the output buffer where the input is copied to.
        //	\param rect Full absolute rectangle description (default: left=0, top=0, right=image width, bottom=image height)
        //	
        public Blit setOutputRect(Rect rect) {
            mOutputRect.left = (int)System.Math.Min(rect.left, mBuffer.getWidth());
            mOutputRect.top = (int)System.Math.Min(rect.top, mBuffer.getHeight());
            mOutputRect.right = (int)System.Math.Min(rect.right, mBuffer.getWidth());
            mOutputRect.bottom = (int)System.Math.Min(rect.bottom, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the output buffer where the input is copied to.
        //	\param pos1 Vector to top left start point of the rectangle (default: x=0.0, y=0.0)
        //	\param pos2 Vector to bottom right end point of the rectangle (default: x=1.0, y=1.0)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public Blit setOutputRect(Vector2 pos1, Vector2 pos2) {
            return setOutputRect(pos1, pos2, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Blit& setOutputRect(Ogre::Vector2 pos1, Ogre::Vector2 pos2, bool relative = true)
        public Blit setOutputRect(Vector2 pos1, Vector2 pos2, bool relative) {
            if (relative) {
                mOutputRect.left = (int)((float)mBuffer.getWidth() * System.Math.Min(pos1.x, 1.0f));
                mOutputRect.top = (int)((float)mBuffer.getHeight() * System.Math.Min(pos1.y, 1.0f));
                mOutputRect.right = (int)((float)mBuffer.getWidth() * System.Math.Min(pos2.x, 1.0f));
                mOutputRect.bottom = (int)((float)mBuffer.getHeight() * System.Math.Min(pos2.y, 1.0f));
            }
            else {
                mOutputRect.left = (int)System.Math.Min((int)pos1.x, mBuffer.getWidth());
                mOutputRect.top = (int)System.Math.Min((int)pos1.y, mBuffer.getHeight());
                mOutputRect.right = (int)System.Math.Min((int)pos2.x, mBuffer.getWidth());
                mOutputRect.bottom = (int)System.Math.Min((int)pos2.y, mBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the output buffer where the input is copied to.
        //	\param x1 New absolute x position of rectangle start (default 0)
        //	\param y1 New absolute y position of rectangle start (default 0)
        //	\param x2 New absolute x position of rectangle end (default: image width)
        //	\param y2 New absolute y position of rectangle end (default: image height)
        //	
        public Blit setOutputRect(int x1, int y1, int x2, int y2) {
            mOutputRect.left = (int)System.Math.Min(x1, mBuffer.getWidth());
            mOutputRect.top = (int)System.Math.Min(y1, mBuffer.getHeight());
            mOutputRect.right = (int)System.Math.Min(x2, mBuffer.getWidth());
            mOutputRect.bottom = (int)System.Math.Min(y2, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates of the output buffer where the input is copied to.
        //	\param x1 New relative x position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param y1 New relative y position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param x2 New relative x position of rectangle end [0.0, 1.0] \(default 1.0)
        //	\param y2 New relative y position of rectangle end [0.0, 1.0] \(default 1.0)
        //	
        public Blit setOutputRect(float x1, float y1, float x2, float y2) {
            mOutputRect.left = (int)((float)mBuffer.getWidth() * System.Math.Min(x1, 1.0f));
            mOutputRect.top = (int)((float)mBuffer.getHeight() * System.Math.Min(y1, 1.0f));
            mOutputRect.right = (int)((float)mBuffer.getWidth() * System.Math.Min(x2, 1.0f));
            mOutputRect.bottom = (int)((float)mBuffer.getHeight() * System.Math.Min(y2, 1.0f));
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            if (mInputBuffer == null)
                return mBuffer;
            for (int y = mOutputRect.top; y < mOutputRect.bottom; y++) {
                for (int x = mOutputRect.left; x < mOutputRect.right; x++) {
                    int x0 = (int)((float)(x - mOutputRect.left) / (float)(mOutputRect.Width) * (float)(mInputRect.Width) + (float)(mInputRect.left));
                    int y0 = (int)((float)(y - mOutputRect.top) / (float)(mOutputRect.Height) * (float)(mInputRect.Height) + (float)(mInputRect.top));
                    mBuffer.setPixel(x, y, mInputBuffer.getPixel(x0, y0));
                }
            }
            Utils.log("Modify texture with blit filter");
            return mBuffer;
        }
    }

    //*
    //\brief Reduce sharpness on input image.
    //\details Blurs the input image by a specified algorithm.
    //
    //Examples:
    //\b BLUR_MEAN (default)
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Blur(&bufferImage).setType(Procedural::Blur::BLUR_MEAN).process();
    //\endcode
    //\dotfile texture_05a.gv
    //
    //\b BLUR_GAUSSIAN
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Blur(&bufferImage).setType(Procedural::Blur::BLUR_GAUSSIAN).process();
    //\endcode
    //\dotfile texture_05b.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Blur : public TextureProcessing
    public class Blur : TextureProcessing
    {
        //! List of algorithms to blur
        public enum BLUR_TYPE : int
        {
            BLUR_BOX, //!< Use simplified block filter to blur
            BLUR_MEAN, //!< Use mean filter to blur
            BLUR_GAUSSIAN //!< Use gaussian filter to blur
        }

        private byte mSize = 0;
        private byte mSigma = 0;
        private BLUR_TYPE mType;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Blur(TextureBuffer pBuffer)
            : base(pBuffer, "Blur") {
            mSize = 5;
            mSigma = 92;
            mType = (int)BLUR_TYPE.BLUR_BOX;
        }

        //    *
        //	Set the gaussian block size.
        //	\param size New block size for gaussian blur filter [3, 255] (default 5)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Blur setSize(byte size) {
            mSize = size;
            if (mSize < 3)
                mSize = 3;
            if ((mSize % 2) == 0)
                mSize++;
            return this;
        }

        //    *
        //	Set sigma constant for gaussian filter.
        //	\param sigma New sigma constant for gaussian blur filter [0, 255] (default 92)
        //	
        public Blur setSigma(byte sigma) {
            mSigma = sigma;
            return this;
        }

        //    *
        //	Set the algorithm to blur.
        //	\param type New algorithm to blur (default BLUR_BOX)
        //	
        public Blur setType(BLUR_TYPE type) {
            mType = type;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            float[] blurKernel = { 1, 2, 3, 2, 1, 2, 4, 5, 4, 2, 3, 5, 6, 5, 3, 2, 4, 5, 4, 2, 1, 2, 3, 2, 1 };
            Convolution filter = new Convolution(mBuffer);
            switch (mType) {
                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case BLUR_TYPE.BLUR_BOX:
                    filter.setKernel(5, blurKernel);
                    break;

                case BLUR_TYPE.BLUR_MEAN:
                    filter.setKernel(new Matrix3(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f)).calculateDivisor();
                    break;

                case BLUR_TYPE.BLUR_GAUSSIAN:
                    float fSigma = 0.5f + ((3.0f - 0.5f) / 255.0f) * (float)mSigma;
                    int r = (int)mSize / 2;
                    double min = System.Math.Exp((float)(2 * r * r) / (-2.0f * fSigma * fSigma)) / (Math.TWO_PI * fSigma * fSigma);
                    int[] kernel = new int[mSize * mSize];
                    int divisor = 0;
                    int y = -r;
                    int x = -r;
                    for (int i = 0; i < mSize; i++) {
                        for (int j = 0; j < mSize; j++) {
                            kernel[i * mSize + j] = (int)((System.Math.Exp((float)(x * x + y * y) / (-2.0f * fSigma * fSigma)) / (Math.TWO_PI * fSigma * fSigma)) / min);
                            divisor += kernel[i * mSize + j];
                            x++;
                        }
                        y++;
                    }
                    filter.setKernel(mSize, kernel).setDivisor((float)divisor);
                    kernel = null;
                    break;
            }
            filter.setIncludeAlphaChannel(true).process();

            Utils.log("Modify texture with blur filter : " + (mType).ToString());
            return mBuffer;
        }
    }

    //*
    //\brief Extract one channel or create gray image.
    //\details Reduce given image to a given channel or calculate gray image of it
    //
    //Examples:
    //\b SELECT_BLUE
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Channel(&bufferImage).setSelection(Procedural::Channel::SELECT_BLUE).process();
    //\endcode
    //\dotfile texture_06a.gv
    //
    //\b SELECT_GRAY
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Channel(&bufferImage).setSelection(Procedural::Channel::SELECT_GRAY).process();
    //\endcode
    //\dotfile texture_06b.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Channel : public TextureProcessing
    public class Channel : TextureProcessing
    {
        public enum CANNEL_SELECTION : int
        {
            SELECT_RED,
            SELECT_GREEN,
            SELECT_BLUE,
            SELECT_ALPHA,
            SELECT_GRAY
        }

        private CANNEL_SELECTION mSelection;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Channel(TextureBuffer pBuffer)
            : base(pBuffer, "Channel") {
            mSelection = CANNEL_SELECTION.SELECT_GRAY;
        }

        //    *
        //	Set selection.
        //	\param selection Mode which channel should selected (default SELECT_GRAY)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Channel setSelection(CANNEL_SELECTION selection) {
            mSelection = selection;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    if (mSelection == CANNEL_SELECTION.SELECT_GRAY) {
                        ColourValue pixel = mBuffer.getPixel(x, y);
                        float gray = (pixel.r + pixel.g + pixel.b) / 3.0f;
                        mBuffer.setPixel(x, y, gray, gray, gray, pixel.a);
                    }
                    else {
                        if (mSelection != CANNEL_SELECTION.SELECT_RED)
                            mBuffer.setRed(x, y, 0.0f);
                        if (mSelection != CANNEL_SELECTION.SELECT_GREEN)
                            mBuffer.setGreen(x, y, 0.0f);
                        if (mSelection != CANNEL_SELECTION.SELECT_BLUE)
                            mBuffer.setBlue(x, y, 0.0f);
                        if (mSelection != CANNEL_SELECTION.SELECT_BLUE)
                            mBuffer.setBlue(x, y, 0.0f);
                    }
                }
            }

            Utils.log("Modify texture with channel filter : " + (mSelection).ToString());
            return mBuffer;
        }
    }

    //*
    //\brief Draw a circle.
    //\details Draw a filled circle on top of previous content.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //Procedural::CircleTexture(&bufferSolid).setColour(Ogre::ColourValue::Red).setRadius(0.3f).process();
    //\endcode
    //\dotfile texture_32.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport CircleTexture : public TextureProcessing
    public class CircleTexture : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private int mX = new int();
        private int mY = new int();
        private int mRadius = new int();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public CircleTexture(TextureBuffer pBuffer)
            : base(pBuffer, "CircleTexture") {
            mColour = ColourValue.White;
            mRadius = (int)System.Math.Min(pBuffer.getWidth(), pBuffer.getHeight()) / 2;
            mX = mRadius;
            mY = mRadius;
        }

        //    *
        //	Set the fill colour of the circle.
        //	\param colour New colour for processing (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public CircleTexture setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the fill colour of the circle.
        //	\param red Red value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the fill colour [0.0, 1.0] \(default 1.0)
        //	
        public CircleTexture setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: CircleTexture& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public CircleTexture setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the absolute radius of the circle.
        //	\param radius New absolute radius of the circle in px (default 1/2 * image width)
        //	
        public CircleTexture setRadius(int radius) {
            mRadius = radius;
            return this;
        }

        //    *
        //	Set the relative radius of the circle.
        //	\param radius New relative radius of the circle [0.0, 1.0] \(default 0.5)
        //	
        public CircleTexture setRadius(float radius) {
            mRadius = (int)((float)System.Math.Min(mBuffer.getWidth(), mBuffer.getHeight()) * Math.Abs(radius));
            return this;
        }

        //    *
        //	Set absolute x position of circle center point in px
        //	\param x New absolute x position of circle center (default 1/2 * image width)
        //	
        public CircleTexture setCenterX(int x) {
            mX = (int)System.Math.Min(x, mBuffer.getWidth() - 1);
            return this;
        }

        //    *
        //	Set relative x position of circle center point as Real
        //	\param x New relative x position of circle center [0.0, 1.0] \(default 0.5)
        //	
        public CircleTexture setCenterX(float x) {
            mX = (int)System.Math.Min((int)(x * (float)mBuffer.getWidth()), mBuffer.getWidth() - 1);
            return this;
        }

        //    *
        //	Set absolute y position of circle center point in px
        //	\param y New absolute y position of circle center (default 1/2 * image width)
        //	
        public CircleTexture setCenterY(int y) {
            mY = (int)System.Math.Min(y, mBuffer.getHeight() - 1);
            return this;
        }

        //    *
        //	Set relative y position of circle center point as Real
        //	\param y New relative y position of circle center [0.0, 1.0] \(default 0.5)
        //	
        public CircleTexture setCenterY(float y) {
            mY = (int)System.Math.Min((int)(y * (float)mBuffer.getHeight()), mBuffer.getHeight() - 1);
            return this;
        }

        //    *
        //	Set the position of circle center point.
        //	\param pos Vector to the center point of the circle (default: x=0.5, y=0.5)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public CircleTexture setCenter(Vector2 pos) {
            return setCenter(pos, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: CircleTexture& setCenter(Ogre::Vector2 pos, bool relative = true)
        public CircleTexture setCenter(Vector2 pos, bool relative) {
            setCenter(pos.x, pos.y, relative);
            return this;
        }

        //    *
        //	Set the position of circle center point.
        //	\param x New absolute x position of circle center (default 1/2 * image width)
        //	\param y New absolute y position of circle center (default 1/2 * image width)
        //	
        public CircleTexture setCenter(int x, int y) {
            setCenterX(x);
            setCenterY(y);
            return this;
        }

        //    *
        //	Set the position of circle center point.
        //	\param x New relative x position of circle center [0.0, 1.0] \(default 0.5)
        //	\param y New relative y position of circle center [0.0, 1.0] \(default 0.5)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public CircleTexture setCenter(float x, float y) {
            return setCenter(x, y, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: CircleTexture& setCenter(Ogre::float x, Ogre::float y, bool relative = true)
        public CircleTexture setCenter(float x, float y, bool relative) {
            if (relative) {
                setCenterX(x);
                setCenterY(y);
            }
            else {
                setCenterX((int)x);
                setCenterY((int)y);
            }
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int x = 0;
            int y = mRadius;
            int p = 3 - 2 * mRadius;
            while (x <= y) {
                for (int dy = -y; dy <= y; dy++) {
                    _putpixel(+x, dy);
                    _putpixel(-x, dy);
                }
                for (int dx = -x; dx <= x; dx++) {
                    _putpixel(+y, dx);
                    _putpixel(-y, dx);
                }
                if (p < 0)
                    p += 4 * x++ + 6;
                else
                    p += 4 * (x++ - y--) + 10;
            }
            Utils.log("Modify texture with circle filter : x = " + (mX).ToString() + ", y = " + (mY).ToString() + ", Radius = " + (mRadius).ToString());
            return mBuffer;
        }

        private void _putpixel(int dx, int dy) {
            if (mX + dx < 0 || mX + dx >= mBuffer.getWidth())
                return;
            if (mY + dy < 0 || mY + dy >= mBuffer.getHeight())
                return;
            mBuffer.setPixel(mX + dx, mY + dy, mColour);
        }
    }

    //*
    //\brief A colour filter.
    //\details Adjust colour, contrast, brightness and alpha.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //Procedural::Colours(&bufferGradient).setColourBase(Ogre::ColourValue::Red).setColourPercent(Ogre::ColourValue::Blue).process();
    //\endcode
    //\dotfile texture_07.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Colours : public TextureProcessing
    public class Colours : TextureProcessing
    {
        private ColourValue mColourBase = new ColourValue();
        private ColourValue mColourPercent = new ColourValue();
        private byte mBrightness = 0;
        private byte mContrast = 0;
        private byte mSaturation = 0;
        private byte mAlpha = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Colours(TextureBuffer pBuffer)
            : base(pBuffer, "Colours") {
            mColourBase = ColourValue.Black;
            mColourPercent = ColourValue.White;
            mBrightness = 127;
            mContrast = 127;
            mSaturation = 127;
            mAlpha = 127;
        }

        //    *
        //	Set the base colour to work on.
        //	\param colour New colour to work on (default Ogre::ColourValue::Black)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Colours setColourBase(ColourValue colour) {
            //
            //ORIGINAL LINE: mColourBase = colour;
            mColourBase = (colour);
            return this;
        }

        //    *
        //	Set the base colour to work on.
        //	\param red Red value of base colour [0.0, 1.0] \(default 0.0)
        //	\param green Green value of base colour [0.0, 1.0] \(default 0.0)
        //	\param blue Blue value of base colour [0.0, 1.0] \(default 0.0)
        //	\param alpha %Alpha value of base colour [0.0, 1.0] \(default 0.0)
        //	
        public Colours setColourBase(float red, float green, float blue) {
            return setColourBase(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Colours& setColourBase(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Colours setColourBase(float red, float green, float blue, float alpha) {
            mColourBase = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the percent colour to add on image.
        //	\param colour New colour to add (default Ogre::ColourValue::White)
        //	
        public Colours setColourPercent(ColourValue colour) {
            //
            //ORIGINAL LINE: mColourPercent = colour;
            mColourPercent = (colour);
            return this;
        }

        //    *
        //	Set the percent colour to add on image.
        //	\param red Red value of percent colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of percent colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of percent colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of percent colour [0.0, 1.0] \(default 1.0)
        //	
        public Colours setColourPercent(float red, float green, float blue) {
            return setColourPercent(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Colours& setColourPercent(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Colours setColourPercent(float red, float green, float blue, float alpha) {
            mColourPercent = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set brightness of the image.
        //	\param brightness New image brightness (default 127)
        //	
        public Colours setBrightness(byte brightness) {
            mBrightness = brightness;
            return this;
        }

        //    *
        //	Set contrast of the image.
        //	\param contrast New image contrast (default 127)
        //	
        public Colours setContrast(byte contrast) {
            mContrast = contrast;
            return this;
        }

        //    *
        //	Set saturation of the image.
        //	\param saturation New image saturation (default 127)
        //	
        public Colours setSaturation(byte saturation) {
            mSaturation = saturation;
            return this;
        }

        //    *
        //	Set alpha of the image.
        //	\param alpha New image alpha (default 127)
        //	
        public Colours setAlpha(byte alpha) {
            mAlpha = alpha;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            int brightness = (((int)mBrightness) * 2) - 256;
            int contrast = (((int)mContrast));
            float fconstrast = (float)mContrast / 128.0f;
            fconstrast = fconstrast * fconstrast * fconstrast;
            contrast = (int)(fconstrast * 256.0f);
            byte minalpha = (mAlpha >= 127) ? (byte)((mAlpha - 127) * 2.0f - (mAlpha - 127) / 128.0f) : (byte)0;
            byte maxalpha = (mAlpha <= 127) ? (byte)(mAlpha * 2.0f + mAlpha / 127.0f) : (byte)255;
            float alphamult = (float)(maxalpha - minalpha) / 255.0f;

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    int r = (int)(mColourBase.r * 255.0f) + (((int)mBuffer.getPixelRedByte(x, y) * (int)(mColourPercent.r * 255.0f)) >> 8) + brightness;
                    int g = (int)(mColourBase.g * 255.0f) + (((int)mBuffer.getPixelGreenByte(x, y) * (int)(mColourPercent.g * 255.0f)) >> 8) + brightness;
                    int b = (int)(mColourBase.b * 255.0f) + (((int)mBuffer.getPixelBlueByte(x, y) * (int)(mColourPercent.b * 255.0f)) >> 8) + brightness;

                    int c = (int)(((r - 127) * contrast) >> 8) + 127;
                    r = (c < 0x00) ? 0x00 : (c > 0xff) ? 0xff : c;

                    c = (int)(((g - 127) * contrast) >> 8) + 127;
                    g = (c < 0x00) ? 0x00 : (c > 0xff) ? 0xff : c;

                    c = (int)(((b - 127) * contrast) >> 8) + 127;
                    b = (c < 0x00) ? 0x00 : (c > 0xff) ? 0xff : c;

                    if (mSaturation != 127) {
                        int l = r + g + b;
                        int u = (3 * r - l) * mSaturation / 127;
                        int v = (3 * b - l) * mSaturation / 127;
                        r = (u + l) / 3;
                        g = (l - (u + v)) / 3;
                        b = (v + l) / 3;
                    }

                    mBuffer.setRed(x, y, (byte)System.Math.Min(System.Math.Max(r, 0), 255));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(System.Math.Max(g, 0), 255));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(System.Math.Max(b, 0), 255));
                    mBuffer.setAlpha(x, y, (byte)System.Math.Min(System.Math.Max((byte)((float)mBuffer.getPixelAlphaByte(x, y) * alphamult) + minalpha, 0), 255));
                }
            }

            Utils.log("Modify texture with colours filter");
            return mBuffer;
        }
    }

    //*
    //\brief %Combine inputs together.
    //\details Provides several ways of combining. Clamps bitmap to base input size.
    //
    //Example:
    //\code{.cpp}
    //// Image input
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCloud(256);
    //Procedural::Cloud(&bufferCloud).process();
    //
    //// Filter
    //Procedural::Combine(&bufferCloud).addImage(&bufferGradient, Procedural::Combine::METHOD_ADD_CLAMP).process();
    //\endcode
    //\dotfile texture_08.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Combine : public TextureProcessing
    public class Combine : TextureProcessing
    {
        //! Methods how to combine image
        public enum COMBINE_METHOD : int
        {
            METHOD_ADD_CLAMP,
            METHOD_ADD_WRAP,
            METHOD_SUB_CLAMP,
            METHOD_SUB_WRAP,
            METHOD_MULTIPLY,
            METHOD_MULTIPLY2,
            METHOD_BLEND,
            METHOD_ALPHA,
            METHOD_LAYER
        }

        private class LAYER
        {
            public TextureBuffer image;
            public COMBINE_METHOD action;
        }
        private ColourValue mColour = new ColourValue();
        private Queue<LAYER> mQueue = new Queue<LAYER>();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Combine(TextureBuffer pBuffer)
            : base(pBuffer, "Combine") {
            mColour = ColourValue.White;
        }

        //    *
        //	Add image to process queue.
        //	\param image Pointer on image to process
        //	\param method Method how to process the image
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Combine addImage(TextureBuffer image) {
            return addImage(image, COMBINE_METHOD.METHOD_LAYER);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Combine& addImage(TextureBuffer* image, COMBINE_METHOD method = METHOD_LAYER)
        public Combine addImage(TextureBuffer image, COMBINE_METHOD method) {
            if (image != null)
                if (image.getHeight() >= mBuffer.getHeight() && image.getWidth() >= mBuffer.getWidth()) {
                    LAYER l = new LAYER();
                    l.action = method;
                    l.image = image;
                    mQueue.Enqueue(l);
                }
            return this;
        }

        //    *
        //	Set the percent colour to add on image.
        //	\param colour New colour for drawing (default Ogre::ColourValue::White)
        //	
        public Combine setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the percent colour to add on image.
        //	\param red Red value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of drawing colour [0.0, 1.0] \(default 1.0)
        //	
        public Combine setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Combine& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Combine setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int i = 0;
            while (mQueue.Count > 0) {
                LAYER l = mQueue.Peek();
                _process(l.image, l.action);
                mQueue.Dequeue();
                i++;
                Utils.log("Combine textures : " + (l.action).ToString());
            }

            return mBuffer;
        }

        private void _process(TextureBuffer image, COMBINE_METHOD method) {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            uint rcolPercent = (uint)(mColour.r * 255.0f);
            uint gcolPercent = (uint)(mColour.g * 255.0f);
            uint bcolPercent = (uint)(mColour.b * 255.0f);

            switch (method) {
                case COMBINE_METHOD.METHOD_ADD_CLAMP:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint r = (uint)(pxDst.r * 255.0f) + (((uint)(pxSrc.r * 255.0f) * rcolPercent) >> 8);
                            uint g = (uint)(pxDst.g * 255.0f) + (((uint)(pxSrc.g * 255.0f) * gcolPercent) >> 8);
                            uint b = (uint)(pxDst.b * 255.0f) + (((uint)(pxSrc.b * 255.0f) * bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)((r < 255) ? r : 255), (byte)((g < 255) ? g : 255), (byte)((b < 255) ? b : 255), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_ADD_WRAP:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint r = (uint)(pxDst.r * 255.0f) + (((uint)(pxSrc.r * 255.0f) * rcolPercent) >> 8);
                            uint g = (uint)(pxDst.g * 255.0f) + (((uint)(pxSrc.g * 255.0f) * gcolPercent) >> 8);
                            uint b = (uint)(pxDst.b * 255.0f) + (((uint)(pxSrc.b * 255.0f) * bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)(r % 255), (byte)(g % 255), (byte)(b % 255), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_SUB_CLAMP:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            int r = (int)(pxDst.r * 255.0f) - (((int)(pxSrc.r * 255.0f) * (int)rcolPercent) >> 8);
                            int g = (int)(pxDst.g * 255.0f) - (((int)(pxSrc.g * 255.0f) * (int)gcolPercent) >> 8);
                            int b = (int)(pxDst.b * 255.0f) - (((int)(pxSrc.b * 255.0f) * (int)bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)((r > 0) ? r : 0), (byte)((g > 0) ? g : 0), (byte)((b > 0) ? b : 0), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_SUB_WRAP:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            int r = (int)(pxDst.r * 255.0f) - (((int)(pxSrc.r * 255.0f) * (int)rcolPercent) >> 8);
                            int g = (int)(pxDst.g * 255.0f) - (((int)(pxSrc.g * 255.0f) * (int)gcolPercent) >> 8);
                            int b = (int)(pxDst.b * 255.0f) - (((int)(pxSrc.b * 255.0f) * (int)bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)(r % 255), (byte)(g % 255), (byte)(b % 255), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_MULTIPLY:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint r = (uint)(pxDst.r * 255.0f) * (((uint)(pxSrc.r * 255.0f) * rcolPercent) >> 8);
                            uint g = (uint)(pxDst.g * 255.0f) * (((uint)(pxSrc.g * 255.0f) * gcolPercent) >> 8);
                            uint b = (uint)(pxDst.b * 255.0f) * (((uint)(pxSrc.b * 255.0f) * bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)(r >> 8), (byte)(g >> 8), (byte)(b >> 8), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_MULTIPLY2:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint r = (uint)(pxDst.r * 255.0f) * (((uint)(pxSrc.r * 255.0f) * rcolPercent) >> 8);
                            r >>= 7;
                            uint g = (uint)(pxDst.g * 255.0f) * (((uint)(pxSrc.g * 255.0f) * gcolPercent) >> 8);
                            g >>= 7;
                            uint b = (uint)(pxDst.b * 255.0f) * (((uint)(pxSrc.b * 255.0f) * bcolPercent) >> 8);
                            b >>= 7;
                            mBuffer.setPixel(x, y, (byte)((r < 255) ? r : 255), (byte)((g < 255) ? g : 255), (byte)((b < 255) ? b : 255), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_BLEND:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint r = (uint)(pxDst.r * 255.0f) + (((uint)(pxSrc.r * 255.0f) * rcolPercent) >> 8);
                            uint g = (uint)(pxDst.g * 255.0f) + (((uint)(pxSrc.g * 255.0f) * gcolPercent) >> 8);
                            uint b = (uint)(pxDst.b * 255.0f) + (((uint)(pxSrc.b * 255.0f) * bcolPercent) >> 8);
                            mBuffer.setPixel(x, y, (byte)(r >> 1), (byte)(g >> 1), (byte)(b >> 1), (byte)(pxDst.a * 255.0f));
                        }
                    }
                    break;

                case COMBINE_METHOD.METHOD_ALPHA:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            uint a = (uint)(pxDst.a * 255.0f) + (((uint)(pxSrc.a * 255.0f) * bcolPercent) >> 8);
                            mBuffer.setAlpha(x, y, (byte)(a >> 1));
                        }
                    }
                    break;

                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case COMBINE_METHOD.METHOD_LAYER:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            ColourValue pxSrc = image.getPixel(x, y);
                            ColourValue pxDst = mBuffer.getPixel(x, y);
                            mBuffer.setPixel(x, y, (byte)(pxSrc.r * pxSrc.a * 255.0f + pxDst.r * 255.0f * (1.0f - pxSrc.a)), (byte)(pxSrc.g * pxSrc.a * 255.0f + pxDst.g * 255.0f * (1.0f - pxSrc.a)), (byte)(pxSrc.b * pxSrc.a * 255.0f + pxDst.b * 255.0f * (1.0f - pxSrc.a)), (byte)((pxDst.a - pxDst.a * pxSrc.a) * 255.0f + pxSrc.a * 255.0f));
                        }
                    }
                    break;
            }
        }
    }

    //*
    //\brief %Convolution filter.
    //\details The filter calculates each pixel of the result image as weighted sum of the correspond pixel and its neighbors.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Convolution(&bufferImage).setKernel(
    //	Ogre::Matrix3(10.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, -10.0f)
    //	).process();
    //\endcode
    //\dotfile texture_09.gv
    //
    //\note All kernels must be square matrices and larger than 2x2 (min. 3x3)!
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Convolution : public TextureProcessing
    public class Convolution : TextureProcessing
    {
        //private byte mKernelSize = 0;
        private int mKernelSize = 0;
        private float[] mKernelData;
        private float mDivisor = 0f;
        //private byte mThreshold = 0;
        private int mThreshold = 0;
        private bool mCalculateEdgeDivisor;
        private bool mIncludeAlphaChannel;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Convolution(TextureBuffer pBuffer)
            : base(pBuffer, "Convolution") {
            mKernelSize = 3;
            mDivisor = 1.0f;
            mThreshold = 128;
            mCalculateEdgeDivisor = true;
            mIncludeAlphaChannel = false;
            int mid = 2;
            if ((mKernelSize % 2) == 0)
                mid = mKernelSize / 2;
            else
                mid = (mKernelSize - 1) / 2 + 1;
            mKernelData = new float[mKernelSize * mKernelSize];
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
            memset<float>(mKernelData, 0f, (uint)mKernelSize * (uint)mKernelSize * sizeof(float));
            mKernelData[mKernelSize * mid + mid] = 1.0f;
        }

        //    *
        //	Default destructor to release memory.
        //	
        public new void Dispose() {
            mKernelData = null;
            base.Dispose();
        }

        //    *
        //	Set a new kernel.
        //	\param size Number of lines/rows of the quadratic kernel (default 3)
        //	\param data Array with data for new kernel by rows
        //	\remark setKernel calls calculateDivisor after changeing the kernel memory! If you like to set a user defined devisor call setDivisor always after setKernel!
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Convolution setKernel(byte size, float[] data) {
            if (size < 3 || size % 2 == 0)
                return this;
            mKernelData = null;
            mKernelSize = size;
            mKernelData = new float[(int)mKernelSize * mKernelSize];
            for (int y = 0; y < mKernelSize; y++) {
                for (int x = 0; x < mKernelSize; x++) {
                    mKernelData[y * mKernelSize + x] = data[y * mKernelSize + x];
                }
            }
            calculateDivisor();
            return this;
        }

        //    *
        //	Set a new kernel.
        //	\param size Number of lines/rows of the quadratic kernel
        //	\param data Array with data for new kernel by rows
        //	\remark setKernel calls calculateDivisor after changeing the kernel memory! If you like to set a user defined devisor call setDivisor always after setKernel!
        //	
        public Convolution setKernel(byte size, int[] data) {
            if (size < 3 || size % 2 == 0)
                return this;
            mKernelData = null;
            mKernelSize = size;
            mKernelData = new float[(int)mKernelSize * mKernelSize];
            for (int y = 0; y < mKernelSize; y++) {
                for (int x = 0; x < mKernelSize; x++) {
                    mKernelData[y * mKernelSize + x] = (float)data[y * mKernelSize + x];
                }
            }
            return this;
        }

        //    *
        //	Set a new kernel.
        //	\param data Matrix with data for new kernel
        //	\remark setKernel calls calculateDivisor after changeing the kernel memory! If you like to set a user defined devisor call setDivisor always after setKernel!
        //	
        public Convolution setKernel(Matrix3 data) {
            mKernelData = null;
            mKernelSize = 3;
            mKernelData = new float[(int)mKernelSize * mKernelSize];
            for (int y = 0; y < mKernelSize; y++) {
                for (int x = 0; x < mKernelSize; x++) {
                    //mKernelData[y * mKernelSize + x] = data[y][x];
                    mKernelData[y * mKernelSize + x] = data[y, x];
                }
            }
            return this;
        }

        //    *
        //	Set a devisor.
        //	\param divisor Set specific devisor \(default 1.0)
        //	\remark setKernel calls calculateDivisor after changeing the kernel memory! If you like to set a user defined devisor call setDivisor always after setKernel!
        //	
        public Convolution setDivisor(float divisor) {
            mDivisor = divisor;
            if (mDivisor == 0.0)
                mDivisor = 1.0f;
            return this;
        }

        //    *
        //	Calculate a new devisor from given kernel.
        //	\remark setKernel calls calculateDivisor after changeing the kernel memory!
        //	
        public Convolution calculateDivisor() {
            mDivisor = 0.0f;
            for (int y = 0; y < mKernelSize; y++) {
                for (int x = 0; x < mKernelSize; x++) {
                    mDivisor += mKernelData[y * mKernelSize + x];
                }
            }
            if (mDivisor == 0.0f)
                mDivisor = 1.0f;

            return this;
        }

        //    *
        //	Set threshold value.
        //	\param threshold New threshold value [0, 255] (default 128)
        //	
        public Convolution setThreshold(byte threshold) {
            mThreshold = threshold;
            return this;
        }

        //    *
        //	Switch dynamic divisor for edges on or off.
        //	\param calculateedgedivisor Set true to use dynamic divisor for edges (default true)
        //	
        public Convolution setCalculateEdgeDivisor(bool calculateedgedivisor) {
            mCalculateEdgeDivisor = calculateedgedivisor;
            return this;
        }

        //    *
        //	Switch on/off the use of the alpha channel.
        //	\param usealpha Set true to also modify the alpha channel (default false)
        //	
        public Convolution setIncludeAlphaChannel(bool usealpha) {
            mIncludeAlphaChannel = usealpha;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int radius = ((int)mKernelSize) >> 1;
            TextureBuffer tmpBuffer = mBuffer.clone();

            for (int y = 0; y < (int)mBuffer.getWidth(); y++) {
                for (int x = 0; x < (int)mBuffer.getHeight(); x++) {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int a = 0;
                    int div = 0;
                    int processedKernelSize = 0;

                    for (int i = 0; i < mKernelSize; i++) {
                        int ir = i - radius;

                        if ((y + ir) < 0)
                            continue;
                        if ((y + ir) >= (int)mBuffer.getHeight())
                            break;

                        for (int j = 0; j < (int)mKernelSize; j++) {
                            int jr = j - radius;

                            if ((x + jr) < 0)
                                continue;
                            if ((x + jr) < (int)mBuffer.getWidth()) {
                                float k = mKernelData[i * mKernelSize + j];
                                ColourValue pixel = mBuffer.getPixel(y + ir, x + jr);
                                div += (int)k;
                                k *= 255.0f;
                                r += (int)(k * pixel.r);
                                g += (int)(k * pixel.g);
                                b += (int)(k * pixel.b);
                                a += (int)(k * pixel.a);

                                processedKernelSize++;
                            }
                        }
                    }

                    if (processedKernelSize == ((int)mKernelSize * mKernelSize))
                        div = (int)mDivisor;
                    else {
                        if (!mCalculateEdgeDivisor)
                            div = (int)mDivisor;
                    }

                    if (div != 0) {
                        r /= div;
                        g /= div;
                        b /= div;
                        a /= div;
                    }
                    r += ((int)mThreshold - 128);
                    g += ((int)mThreshold - 128);
                    b += ((int)mThreshold - 128);
                    if (mIncludeAlphaChannel)
                        a += ((int)mThreshold - 128);
                    else
                        a = (int)mBuffer.getPixelAlphaByte(x, y);

                    tmpBuffer.setPixel(y, x, (byte)((r > 255) ? 255 : ((r < 0) ? 0 : r)), (byte)((g > 255) ? 255 : ((g < 0) ? 0 : g)), (byte)((b > 255) ? 255 : ((b < 0) ? 0 : b)), (byte)((a > 255) ? 255 : ((a < 0) ? 0 : a)));
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            if (mLog) {
                StringBuilder strKernel = new StringBuilder();
                strKernel.AppendLine("Modify texture with convolution filter :\n");
                for (int i = 0; i < mKernelSize; i++) {
                    strKernel.AppendLine("\t");
                    for (int j = 0; j < mKernelSize; j++) {
                        strKernel.AppendLine((mKernelData[i * mKernelSize + j]).ToString());
                        if (j < (mKernelSize - 1))
                            strKernel.AppendLine("\t");
                    }
                    strKernel.AppendLine("\n");
                }
                Utils.log(strKernel.ToString());
            }
            return mBuffer;
        }
    }

    //*
    //\brief Create lines that can be randomly cracked or follow a normal map.
    //\details Number, length and variation of cracked lines can be set; high quality mode is available.
    //
    //<b>Normal tracking</b><br />Lines are drawn where normals look at sides (r, g channels are set tosomething different than 127), and according the difference between alpha channel and normals elevation. By default, normals are rotated 90 degrees with X axis flipped, so that they "wrap" objects.
    //
    //Example:
    //\code{.cpp}
    //// Image input
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCloud(256);
    //Procedural::Cloud(&bufferCloud).process();
    //
    //// Filter
    //Procedural::Crack(&bufferCloud).setParameterImage(&bufferGradient).process();
    //\endcode
    //\dotfile texture_10.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Crack : public TextureProcessing
    public class Crack : TextureProcessing
    {
        //! Methods how to create line length
        public enum CRACK_LENGTH_DECISION : int
        {
            LENGTH_DECISION_RANDOM, //!< Use a random number generator
            LENGTH_DECISION_CONSTANT, //!< Use a constant value
            LENGTH_DECISION_NORMAL_BASED //!< Use a normal map
        }

        //! High quality settings
        public enum CRACK_QUALITY : int
        {
            QUALITY_HIGH_OFF, //!< Switch high quality off
            QUALITY_ALPHA, //!< Use alpha channel
            QUALITY_SUBPIXEL //!< Use sub pixel block
        }

        private TextureBuffer mParam;
        private ColourValue mColour = new ColourValue();
        private uint mCount = 0;
        private byte mVariation = 0;
        private byte mLength = 0;
        private uint mSeed = 0;
        private CRACK_LENGTH_DECISION mLengthDecision;
        private CRACK_QUALITY mQuality;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Crack(TextureBuffer pBuffer)
            : base(pBuffer, "Crack") {
            mParam = null;
            mColour = ColourValue.White;
            mCount = 100;
            mVariation = 64;
            mLength = 255;
            mSeed = 5120;
            mLengthDecision = CRACK_LENGTH_DECISION.LENGTH_DECISION_RANDOM;
            mQuality = CRACK_QUALITY.QUALITY_HIGH_OFF;
        }

        //    *
        //	Set parameter image for normal mapping.
        //	\param image Pointer to second image (default NULL)
        //	\note If the parameter image is set to NULL there won't be any compensation.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Crack setParameterImage(TextureBuffer image) {
            mParam = image;
            return this;
        }

        //    *
        //	Set the colour to draw.
        //	\param colour New colour for drawing (default Ogre::ColourValue::White)
        //	
        public Crack setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour to draw.
        //	\param red Red value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of drawing colour [0.0, 1.0] \(default 1.0)
        //	
        public Crack setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Crack& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Crack setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the number of generated lines.
        //	\param count New number of lines to generate (default 100)
        //	
        public Crack setCount(uint count) {
            mCount = count;
            return this;
        }

        //    *
        //	Set the smoothness of generated lines.
        //	\param variation New value for the smoothness of the generated lines (default 64)
        //	
        public Crack setVariation(byte variation) {
            mVariation = variation;
            return this;
        }

        //    *
        //	Set the minimum length of generated line segments.
        //	\param length New minimal length of the generated line segments (default 255)
        //	
        public Crack setLength(byte length) {
            mLength = length;
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Crack setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Set method for calculating the line segments length.
        //	\param lengthdecision New decision length (default LENGTH_DECISION_RANDOM)
        //	
        public Crack setLengthDecision(CRACK_LENGTH_DECISION lengthdecision) {
            mLengthDecision = lengthdecision;
            return this;
        }

        //    *
        //	Set method for high quality painting.
        //	\param quality New high quality setting (default QUALITY_HIGH_OFF)
        //	
        public Crack setQuality(CRACK_QUALITY quality) {
            mQuality = quality;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            ColourValue x1 = new ColourValue();
            ColourValue y1 = new ColourValue();
            ColourValue x2 = new ColourValue();
            ColourValue y2 = new ColourValue();
            double cy2;
            double cy1;
            double cx2;
            double cx1;
            int oxn = new int();
            int oyn = new int();

            RandomNumbers.Seed((int)mSeed);

            if (mParam == null)
                return mBuffer;

            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();

            if (mParam.getWidth() < w || mParam.getHeight() < h)
                return mBuffer;

            TextureBuffer tmpBuffer = mBuffer.clone();

            for (uint n = 0; n < mCount; n++) {
                double x = ((double)RandomNumbers.NextNumber() / RAND_MAX) * (double)w;
                double y = ((double)RandomNumbers.NextNumber() / RAND_MAX) * (double)h;
                double a = Math.TWO_PI * ((double)RandomNumbers.NextNumber() / RAND_MAX);
                int count = (int)mLength;
                ColourValue pixel = mParam.getPixel((int)x, (int)y);

                if (mParam.getWidth() != null && mLengthDecision == CRACK_LENGTH_DECISION.LENGTH_DECISION_NORMAL_BASED) {
                    Vector3 normal = new Vector3(pixel.r * 255.0f - 127.0f, pixel.g * 255.0f - 127.0f, 0.0f);
                    float norm = normal.x * normal.x + normal.y * normal.y;
                    norm = (norm > 0) ? Math.Sqrt(norm) : 0;
                    count = System.Math.Min((int)(count * norm * norm / 8.0f), (int)mLength);
                }

                if (mLengthDecision == CRACK_LENGTH_DECISION.LENGTH_DECISION_RANDOM)
                    count = (int)(count * ((double)RandomNumbers.NextNumber() / RAND_MAX) * 2.0);

                while (--count >= 0) {
                    a += (double)mVariation / 256.0 * (2.0 * ((double)RandomNumbers.NextNumber() / RAND_MAX) - 1.0);

                    x = x + Math.Cos((float)a);
                    y = y + Math.Sin((float)a);
                    if ((int)x >= w || (int)y >= h)
                        break;

                    if (mParam.getWidth() != null) {
                        Vector3 normal = new Vector3(127.0f - pixel.r * 255.0f, pixel.g * 255.0f - 127.0f, 0.0f);
                        if (normal.x == 0.0) {
                            if (normal.y > 0.0)
                                a = Math.PI;
                            else
                                a = Math.TWO_PI;
                        }
                        else if (normal.x < 0)
                            a = Math.ATan(normal.y / normal.x).ValueRadians + 1.5f * Math.PI;
                        else if (normal.y < 0)
                            a = Math.ATan(normal.y / normal.x).ValueRadians + 2.5f * Math.PI;
                        else
                            a = Math.ATan(normal.y / normal.x).ValueRadians + Math.HALF_PI;
                        float norm = normal.x * normal.x + normal.y * normal.y;
                        norm = (norm > 0) ? Math.Sqrt(norm) : 0;
                        if (norm < (255.0f - pixel.a * 255.0f) / 4.0f)
                            continue;
                    }

                    switch (mQuality) {
                        case CRACK_QUALITY.QUALITY_SUBPIXEL:
                            cy2 = (x - System.Math.Floor(x)) * (y - System.Math.Floor(y));
                            cy1 = (y - System.Math.Floor(y)) * (System.Math.Ceiling(x) - x);
                            cx2 = (x - System.Math.Floor(x)) * (System.Math.Ceiling(y) - y);
                            cx1 = 1 - (cx2 + cy1 + cy2);
                            oxn = System.Math.Min((int)x + 1, w);
                            oyn = System.Math.Min((int)x + 1, h);

                            x1 = mBuffer.getPixel((int)x, (int)y);
                            y1 = mBuffer.getPixel((int)x, oyn);
                            x2 = mBuffer.getPixel(oxn, (int)y);
                            y2 = mBuffer.getPixel(oxn, oyn);

                            x1 *= (float)(1 - cx1);
                            x2 *= (float)(1 - cx2);
                            y1 *= (float)(1 - cy1);
                            y2 *= (float)(1 - cy2);

                            x1 += mColour * (float)cx1;
                            y1 += mColour * (float)cy1;
                            x2 += mColour * (float)cx2;
                            y2 += mColour * (float)cy2;

                            tmpBuffer.setPixel((int)x, (int)y, x1);
                            tmpBuffer.setPixel((int)x, oyn, y1);
                            tmpBuffer.setPixel(oxn, (int)y, x2);
                            tmpBuffer.setPixel(oxn, oyn, y2);
                            break;

                        case CRACK_QUALITY.QUALITY_ALPHA:
                            tmpBuffer.setPixel((int)x, (int)y, mBuffer.getPixel((int)x, (int)y) + mColour);
                            break;

                        default:
                            tmpBuffer.setPixel((int)x, (int)y, mColour);
                            break;
                    }
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with crack filter");
            return mBuffer;
        }
    }

    //*
    //\brief Draw a cycloid.
    //\details Draw a cycloid on top of previous content.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //Procedural::Cycloid(&bufferSolid).setColour(Ogre::ColourValue::Red).setPenSize(2).setType(Procedural::Cycloid::HYPOTROCHOID).process();
    //\endcode
    //\dotfile texture_30.gv
    //
    //\par Cycloid types
    //<table><tr><th>%CYCLOID_TYPE</th><th>Name</th><th>%Image</th></tr>
    //<tr><td><tt>HYPOCYCLOID</tt></td><td><b>Hypocycloid</b><p><a href="http://en.wikipedia.org/wiki/Hypocycloid" target="_blank">http://en.wikipedia.org/wiki/Hypocycloid</a></p></td><td>\image html texture_cycloid_hypocycloid.png</td></tr>
    //<tr><td><tt>HYPOTROCHOID</tt></td><td><b>Hypotrochoid</b><p><a href="http://en.wikipedia.org/wiki/Hypotrochoid" target="_blank">http://en.wikipedia.org/wiki/Hypotrochoid</a></p></td><td>\image html texture_cycloid_hypotrochoid.png</td></tr>
    //<tr><td><tt>EPICYCLOID</tt></td><td><b>Epicycloid</b><p><a href="http://en.wikipedia.org/wiki/Epicycloid" target="_blank">http://en.wikipedia.org/wiki/Epicycloid</a></p></td><td>\image html texture_cycloid_epicycloid.png</td></tr>
    //<tr><td><tt>EPITROCHOID</tt></td><td><b>Epitrochoid</b><p><a href="http://en.wikipedia.org/wiki/Epitrochoid" target="_blank">http://en.wikipedia.org/wiki/Epitrochoid</a></p></td><td>\image html texture_cycloid_epitrochoid.png</td></tr>
    //<tr><td><tt>ROSE_CURVE</tt></td><td><b>Rose curve</b><p><a href="http://en.wikipedia.org/wiki/Rose_curve" target="_blank">http://en.wikipedia.org/wiki/Rose_curve</a></p></td><td>\image html texture_cycloid_rose.png</td></tr>
    //<tr><td><tt>LISSAJOUS_CURVE</tt></td><td><b>Lissajous curve</b><p><a href="http://en.wikipedia.org/wiki/Lissajous_curve" target="_blank">http://en.wikipedia.org/wiki/Lissajous_curve</a></p></td><td>\image html texture_cycloid_lissajous.png</td></tr></table>
    //
    //\anchor cycloiddefaultparameter
    //\par Default parameters
    //<table><tr><th>Name</th><th>Parameter <em>R</em></th><th>Parameter <em>r</em></th><th>Parameter <em>d</em></th><th>Parameter <em>e</em></th><th colspan="2">Parameter <em>k</em></th></tr>
    //<tr><td>Hypocycloid</td><td>3/6 * Size</td><td>1/6 * Size</td><td align="center"><em>unsused</em></td><td align="center" rowspan="5"><em>unsused</em><td rowspan="4">k = R / r</td><td>3</td></tr>
    //<tr><td>Hypotrochoid</td><td>5/14 * Size</td><td>3/14 * Size</td><td>5/14 * Size</td><td>2</td></tr>
    //<tr><td>Epicycloid</td><td>3/10 * Size</td><td>1/10 * Size</td><td align="center"><em>unsused</em><td>3</td></tr>
    //<tr><td>Epitrochoid</td><td>3/10 * Size</td><td>1/10 * Size</td><td>1/20 * Size</td><td>3</td></tr>
    //<tr><td>Rose curve</td><td>1/2 * Size</td><td>4</td><td>1</td><td rowspan="2">k = r / d</td><td>4</td></tr>
    //<tr><td>Lissajous curve</td><td>1/2 * Size</td><td>5</td><td>4</td><td>&pi;/2</td><td>5/4</td></tr></table>
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Cycloid : public TextureProcessing
    public class Cycloid : TextureProcessing
    {
        //    *
        //	Which type of cycloid should be painted.
        //	
        public enum CYCLOID_TYPE : int
        {
            HYPOCYCLOID, //!< Draw a Hypocycloid.
            HYPOTROCHOID, //!< Draw a Hypotrochoid.
            EPICYCLOID, //!< Draw a Epicycloid.
            EPITROCHOID, //!< Draw a Epitrochoid.
            ROSE_CURVE, //!< Draw a Rose curve.
            LISSAJOUS_CURVE //!< Draw a Lissajous curve.
        }

        //    *
        //	Which parameter should be set.
        //	
        public enum CYCLOID_PARAMETER : int
        {
            PARAMETER_R, //!< Set parameter R.
            PARAMETER_r, //!< Set parameter r.
            PARAMETER_d, //!< Set parameter d.
            PARAMETER_e, //!< Set parameter e.
            PARAMETER_k //!< Set parameter k.
        }

        private CYCLOID_TYPE mType;
        private ColourValue mColour = new ColourValue();
        private float mCenterX = 0f;
        private float mCenterY = 0f;
        private float mParam_R = 0f;
        private float mParam_r = 0f;
        private float mParam_d = 0f;
        private float mParam_e = 0f;
        private uint mPenSize = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Cycloid(TextureBuffer pBuffer)
            : base(pBuffer, "Cycloid") {
            mColour = ColourValue.White;
            mCenterX = 0.5f;
            mCenterY = 0.5f;
            mPenSize = 1;
            setType(CYCLOID_TYPE.HYPOCYCLOID);
        }

        //    *
        //	Set the algorithm to for drawing.
        //	\param type New algorithm to draw (default HYPOCYCLOID)
        //	\note Call this function on first place! setType resets all numerical parameter to special defaults according on used algorithm.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Cycloid setType(CYCLOID_TYPE type) {
            mType = type;
            float size = (float)System.Math.Min(mBuffer.getHeight(), mBuffer.getWidth());
            switch (mType) {
                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case CYCLOID_TYPE.HYPOCYCLOID:
                    mParam_R = 3.0f / 6.0f * size;
                    mParam_r = 1.0f / 6.0f * size;
                    mParam_d = 0.0f;
                    mParam_e = 0.0f;
                    break;
                case CYCLOID_TYPE.HYPOTROCHOID:
                    mParam_R = 5.0f / 14.0f * size;
                    mParam_r = 3.0f / 14.0f * size;
                    mParam_d = 5.0f / 14.0f * size;
                    mParam_e = 0.0f;
                    break;
                case CYCLOID_TYPE.EPICYCLOID:
                    mParam_R = 3.0f / 10.0f * size;
                    mParam_r = 1.0f / 10.0f * size;
                    mParam_d = 0.0f;
                    mParam_e = 0.0f;
                    break;
                case CYCLOID_TYPE.EPITROCHOID:
                    mParam_R = 3.0f / 10.0f * size;
                    mParam_r = 1.0f / 10.0f * size;
                    mParam_d = 1.0f / 20.0f * size;
                    mParam_e = 0.0f;
                    break;
                case CYCLOID_TYPE.ROSE_CURVE:
                    mParam_R = 0.5f * size;
                    mParam_r = 4.0f;
                    mParam_d = 1.0f;
                    mParam_e = 0.0f;
                    break;
                case CYCLOID_TYPE.LISSAJOUS_CURVE:
                    mParam_R = 0.5f * size;
                    mParam_r = 5.0f;
                    mParam_d = 4.0f;
                    mParam_e = Math.HALF_PI;
                    break;
            }
            return this;
        }

        //    *
        //	Set the drawing colour for cycloid structure.
        //	\param colour New colour for drawing (default Ogre::ColourValue::White)
        //	
        public Cycloid setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the drawing colour for cycloid structure.
        //	\param red Red value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of drawing colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of drawing colour [0.0, 1.0] \(default 1.0)
        //	
        public Cycloid setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Cycloid& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Cycloid setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the relative center position of the cycloid main circle on x axis.
        //	\param centerx New relative center of the cycloid main circle [0.0, 1.0] \(default 0.5)
        //	
        public Cycloid setCenterX(float centerx) {
            mCenterX = centerx;
            return this;
        }

        //    *
        //	Set the relative center position of the cycloid main circle on y axis.
        //	\param centery New relative center of the cycloid main circle [0.0, 1.0] \(default 0.5)
        //	
        public Cycloid setCenterY(float centery) {
            mCenterY = centery;
            return this;
        }

        //    *
        //	Set the parameter value.
        //	\param paramType Selection which parameter should be set
        //	\param value New value for selected parameter
        //	\see \ref cycloiddefaultparameter "Default parameters" for default values
        //	\note Unsused paramerters will be ignored. Setting <em>k</em> parameter calculates the first used parameter. For example <tt>k = R / r</tt> will calculate <em>R</em> by <tt>R = k * r</tt> (also <em>r</em> from <tt>r = k * d</tt>).
        //	
        public Cycloid setParameter(CYCLOID_PARAMETER paramType, float @value) {
            switch (paramType) {
                case CYCLOID_PARAMETER.PARAMETER_R:
                    mParam_R = @value;
                    break;
                case CYCLOID_PARAMETER.PARAMETER_r:
                    mParam_r = @value;
                    break;
                case CYCLOID_PARAMETER.PARAMETER_d:
                    mParam_d = @value;
                    break;
                case CYCLOID_PARAMETER.PARAMETER_e:
                    mParam_e = @value;
                    break;
                case CYCLOID_PARAMETER.PARAMETER_k:
                    switch (mType) {
                        default:
                        //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                        case CYCLOID_TYPE.HYPOCYCLOID:
                        case CYCLOID_TYPE.HYPOTROCHOID:
                        case CYCLOID_TYPE.EPICYCLOID:
                        case CYCLOID_TYPE.EPITROCHOID:
                            mParam_R = @value * mParam_r;
                            break;
                        case CYCLOID_TYPE.ROSE_CURVE:
                        case CYCLOID_TYPE.LISSAJOUS_CURVE:
                            mParam_r = @value * mParam_d;
                            break;
                    }
                    break;
                default:
                    break;
            }
            return this;
        }

        //    *
        //	Set the size for the pen to draw.
        //	\param size New size for the drawing pen (default 1)
        //	
        public Cycloid setPenSize(uint size) {
            mPenSize = size;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            if (mPenSize == 0)
                return mBuffer;
            int xpos = (int)((float)mBuffer.getWidth() * mCenterX);
            int ypos = (int)((float)mBuffer.getHeight() * mCenterY);
            float step = Math.PI / (float)System.Math.Min(mBuffer.getHeight(), mBuffer.getWidth());
            switch (mType) {
                default:
                    break;
                case CYCLOID_TYPE.HYPOCYCLOID:
                    _process_hypocycloid(xpos, ypos, step);
                    break;
                case CYCLOID_TYPE.HYPOTROCHOID:
                    _process_hypotrochoid(xpos, ypos, step);
                    break;
                case CYCLOID_TYPE.EPICYCLOID:
                    _process_epicycloid(xpos, ypos, step);
                    break;
                case CYCLOID_TYPE.EPITROCHOID:
                    _process_epitrochoid(xpos, ypos, step);
                    break;
                case CYCLOID_TYPE.ROSE_CURVE:
                    _process_rose_curve(xpos, ypos, step);
                    break;
                case CYCLOID_TYPE.LISSAJOUS_CURVE:
                    _process_lissajous_curve(xpos, ypos, step);
                    break;
            }
            return mBuffer;
        }

        private void _process_hypocycloid(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float phi = 0;

            int sx = x + (int)System.Math.Floor(mParam_R + 0.5f);
            int sy = y;
            do {
                float dx = (mParam_R - mParam_r) * Math.Cos(phi) + mParam_r * Math.Cos(((mParam_R - mParam_r) / mParam_r) * phi);
                float dy = (mParam_R - mParam_r) * Math.Sin(phi) - mParam_r * Math.Sin(((mParam_R - mParam_r) / mParam_r) * phi);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                phi += step;
            }
            while (!(sx == px && sy == py && phi < 100.0f * Math.PI) || phi < Math.TWO_PI);
            Utils.log("Modify texture with hypocycloid drawing");
        }
        private void _process_hypotrochoid(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float phi = 0;

            int sx = x + (int)System.Math.Floor((mParam_R - mParam_r) + mParam_d + 0.5f);
            int sy = y;
            do {
                float dx = (mParam_R - mParam_r) * Math.Cos(phi) + mParam_d * Math.Cos(((mParam_R - mParam_r) / mParam_r) * phi);
                float dy = (mParam_R - mParam_r) * Math.Sin(phi) - mParam_d * Math.Sin(((mParam_R - mParam_r) / mParam_r) * phi);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                phi += step;
            }
            while (!(sx == px && sy == py && phi < 100.0f * Math.PI) || phi < Math.TWO_PI);
            Utils.log("Modify texture with hypotrochid drawing");
        }
        private void _process_epicycloid(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float phi = 0;

            int sx = x + (int)System.Math.Floor((mParam_R + mParam_r) - mParam_r + 0.5f);
            int sy = y;
            do {
                float dx = (mParam_R + mParam_r) * Math.Cos(phi) - mParam_r * Math.Cos(((mParam_R + mParam_r) / mParam_r) * phi);
                float dy = (mParam_R + mParam_r) * Math.Sin(phi) - mParam_r * Math.Sin(((mParam_R + mParam_r) / mParam_r) * phi);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                phi += step;
            }
            while (!(sx == px && sy == py && phi < 100.0f * Math.PI) || phi < Math.TWO_PI);
            Utils.log("Modify texture with epicycloid drawing");
        }
        private void _process_epitrochoid(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float phi = 0;

            int sx = x + (int)System.Math.Floor((mParam_R + mParam_r) - mParam_d + 0.5f);
            int sy = y;
            do {
                float dx = (mParam_R + mParam_r) * Math.Cos(phi) - mParam_d * Math.Cos(((mParam_R + mParam_r) / mParam_r) * phi);
                float dy = (mParam_R + mParam_r) * Math.Sin(phi) - mParam_d * Math.Sin(((mParam_R + mParam_r) / mParam_r) * phi);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                phi += step;
            }
            while (!(sx == px && sy == py && phi < 100.0f * Math.PI) || phi < Math.TWO_PI);
            Utils.log("Modify texture with epitrochoid drawing");
        }
        private void _process_rose_curve(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float t = 0;
            float k = mParam_r / mParam_d;

            step = step / 10.0f;

            int sx = x;
            int sy = y;
            do {
                float dx = mParam_R * Math.Cos(k * t) * Math.Sin(t);
                float dy = mParam_R * Math.Cos(k * t) * Math.Cos(t);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                t += step;
            }
            while (t <= Math.TWO_PI);
            Utils.log("Modify texture with rose curve drawing");
        }
        private void _process_lissajous_curve(int x, int y, float step) {
            int px = 0;
            int py = 0;
            float t = 0;

            step = step / 10.0f;

            int sx = x;
            int sy = y;
            do {
                float dx = mParam_R * Math.Sin(mParam_r * t + mParam_e);
                float dy = mParam_R * Math.Cos(mParam_d * t + mParam_e);

                px = x + (int)System.Math.Floor(dx + 0.5f);
                py = y - (int)System.Math.Floor(dy + 0.5f);
                _process_paint(px, py, step);

                t += step;
            }
            while (t <= Math.TWO_PI);
            Utils.log("Modify texture with lissajous curve drawing");
        }
        private void _process_paint(int x, int y, float step) {
            if (mPenSize == 1) {
                if (x < 0 || y < 0 || x >= (int)mBuffer.getWidth() || y >= (int)mBuffer.getHeight())
                    return;
                mBuffer.setPixel(x, y, mColour);
            }
            else {
                for (float phi = 0; phi <= Math.TWO_PI; phi += step) {
                    float dx = Math.Cos(phi);
                    float dy = Math.Sin(phi);
                    for (uint r = 0; r < mPenSize; r++) {
                        int px = x + (int)System.Math.Floor((float)r * dx + 0.5f);
                        int py = y - (int)System.Math.Floor((float)r * dy + 0.5f);
                        if (px >= 0 && py >= 0 && px < (int)mBuffer.getWidth() && py < (int)mBuffer.getHeight())
                            mBuffer.setPixel(px, py, mColour);
                    }
                }
            }
        }
    }

    //*
    //\brief Expands bright areas over darker areas.
    //\details This filter dilate mid range area of the input image.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCloud(256);
    //Procedural::Cloud(&bufferCloud).process();
    //Procedural::Dilate(&bufferCloud).process();
    //\endcode
    //\dotfile texture_11.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Dilate : public TextureProcessing
    public class Dilate : TextureProcessing
    {
        private byte mIterations = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Dilate(TextureBuffer pBuffer)
            : base(pBuffer, "Dilate") {
            mIterations = 10;
        }

        //    *
        //	Set number of iterations for dilating.
        //	\param iterations New number of dilating iterations [1, 255] (default 10)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Dilate setIterations(byte iterations) {
            mIterations = iterations;
            if (mIterations == 0)
                mIterations = 1;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            TextureBuffer intBuffer = mBuffer.clone();
            TextureBuffer dstBuffer = mBuffer.clone();

            TextureBuffer pSrc = null;
            TextureBuffer pDst = null;

            for (int i = 0; i < (int)mIterations; i++) {
                if (i == 0)
                    pSrc = mBuffer;
                else {
                    pSrc = ((i % 2) != (mIterations % 2)) ? dstBuffer : intBuffer;
                }
                pDst = ((i % 2) == (mIterations % 2)) ? dstBuffer : intBuffer;

                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        int sum = -1;
                        pDst.setPixel(x, y, pSrc.getPixel(x, y));

                        for (int v = -1; v < 2; v++) {
                            for (int u = -1; u < 2; u++) {
                                ColourValue pixel = pSrc.getPixel((x + w + u) % w, (y + h + v) % h);
                                if ((pixel.r + pixel.g + pixel.b) * 255.0f > sum) {
                                    sum = (int)((pixel.r + pixel.g + pixel.b) * 255.0f);
                                    pDst.setPixel(x, y, pixel);
                                }
                            }
                        }
                    }
                }
            }

            mBuffer.setData(dstBuffer);
            intBuffer.Dispose();
            dstBuffer.Dispose();

            Utils.log("Modify texture with dilate filter");
            return mBuffer;
        }
    }

    //*
    //\brief Apply normal (ie. vector) map to a bitmap.
    //\details Example:
    //\code{.cpp}
    //// Image colour
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //// Filter
    //Procedural::Distort(&bufferGradient).setParameterImage(&bufferCell).setPower(255).process();
    //\endcode
    //\dotfile texture_12.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Distort : public TextureProcessing
    public class Distort : TextureProcessing
    {
        private TextureBuffer mParam;
        private byte mPower = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Distort(TextureBuffer pBuffer)
            : base(pBuffer, "Distort") {
            mParam = null;
            mPower = 0;
        }

        //    *
        //	Set parameter image for normal mapping.
        //	\param image Pointer to second image (default NULL)
        //	\note If the parameter image is set to NULL there won't be any image manipulation.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Distort setParameterImage(TextureBuffer image) {
            mParam = image;
            return this;
        }

        //    *
        //	Set power for distort effect.
        //	\param power New power for calculation (default 0)
        //	
        public Distort setPower(byte power) {
            mPower = power;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            if (mParam == null)
                return mBuffer;

            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            TextureBuffer tmpBuffer = mBuffer.clone();

            if (mParam.getWidth() < w || mParam.getHeight() < h)
                return mBuffer;

            float fPower = (float)mPower;

            for (int y = 0; y < h; ++y) {
                for (int x = 0; x < w; ++x) {
                    ColourValue pixel = mParam.getPixel(x, y);
                    Vector3 n = new Vector3(pixel.r * 255.0f - 127.0f, pixel.g * 255.0f - 127.0f, pixel.b * 255.0f - 127.0f);
                    //n.Normalise();
                    n = n.NormalisedCopy;
                    float u = (float)System.Math.IEEERemainder((float)(x + (n.x * fPower)), (float)w);
                    float v = (float)System.Math.IEEERemainder((float)(y + (n.y * fPower)), (float)h);
                    float uf = (u >= 0) ? (u - (int)u) : 1 + (u - (int)u);
                    float vf = (v >= 0) ? (v - (int)v) : 1 + (v - (int)v);
                    uint ut = (u >= 0) ? (uint)u : (uint)u - 1;
                    uint vt = (v >= 0) ? (uint)v : (uint)v - 1;
                    ColourValue texel = mBuffer.getPixel((int)vt % h, (int)ut % w) * (1.0f - uf) * (1.0f - vf);
                    texel += mBuffer.getPixel((int)vt % h, ((int)ut + 1) % w) * uf * (1.0f - vf);
                    texel += mBuffer.getPixel(((int)vt + 1) % h, (int)ut % w) * (1.0f - uf) * vf;
                    texel += mBuffer.getPixel(((int)vt + 1) % h, ((int)ut + 1) % w) * uf * vf;
                    tmpBuffer.setPixel(x, y, texel);
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with distort filter");
            return mBuffer;
        }
    }

    //*
    //\brief Edge detection on input image.
    //\details Perform an edge detection with specified algorithm.
    //
    //Example (Sobel):
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //Procedural::EdgeDetection(&bufferCell).setType(Procedural::EdgeDetection::DETECTION_SOBEL).process();
    //\endcode
    //\dotfile texture_13.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport EdgeDetection : public TextureProcessing
    public class EdgeDetection : TextureProcessing
    {
        //! List of algorithms used for edge detection
        public enum DETECTION_TYPE : int
        {
            DETECTION_HOMOGENITY, //!< Homogenity edge detector
            DETECTION_DIFFERENCE, //!< Difference edge detector
            DETECTION_SOBEL, //!< Sobel edge detector
            DETECTION_CANNY //!< Canny edge detector
        }

        private byte mThresholdLow = 0;
        private byte mThresholdHigh = 0;
        private byte mSigma = 0;
        private DETECTION_TYPE mType;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public EdgeDetection(TextureBuffer pBuffer)
            : base(pBuffer, "EdgeDetection") {
            mThresholdLow = 20;
            mThresholdHigh = 100;
            mSigma = 92;
            mType = DETECTION_TYPE.DETECTION_SOBEL;
        }

        //    *
        //	Set the lower threshold for canny filter.
        //	\param threshold New lower threshold value for canny filter [0, 255] (default 20)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public EdgeDetection setThresholdLow(byte threshold) {
            mThresholdLow = threshold;
            return this;
        }

        //    *
        //	Set the upper threshold for canny filter.
        //	\param threshold New upper threshold value for canny filter [0, 255] (default 100)
        //	
        public EdgeDetection setThresholdHigh(byte threshold) {
            mThresholdHigh = threshold;
            return this;
        }

        //    *
        //	Set sigma constant for canny filter.
        //	\param sigma New sigma constant for gaussian filter in canny filter [0, 255] (default 92)
        //	
        public EdgeDetection setSigma(byte sigma) {
            mSigma = sigma;
            return this;
        }

        //    *
        //	Set the algorithm to sharp.
        //	\param type New algorithm to sharp (default SHARP_BASIC)
        //	
        public EdgeDetection setType(DETECTION_TYPE type) {
            mType = type;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            ColourValue pixel = new ColourValue();
            Vector3[] block;
            Vector3 d = new Vector3();
            Vector3 v = new Vector3();
            Vector3 n = new Vector3();
            // Canny specific
            Vector3[] orientation;
            Vector3[] gradients;
            float div = 0f;

            TextureBuffer tmpBuffer = mBuffer.clone();
            (new Solid(tmpBuffer)).setColour(ColourValue.Black).process();

            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();

            switch (mType) {
                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case DETECTION_TYPE.DETECTION_SOBEL:
                    //n = Ogre::Vector3::ZERO;
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            pixel = mBuffer.getPixel((int)x, (int)y);
                            block = getBlock(x, y);
                            d = block[0] + 2.0f * block[1] + block[2] - block[6] - 2.0f * block[7] - block[8];
                            d = new Vector3(Math.Abs(d.x), Math.Abs(d.y), Math.Abs(d.z));
                            v = block[2] + 2.0f * block[5] + block[8] - block[0] - 2.0f * block[3] - block[6];
                            v = new Vector3(Math.Abs(v.x), Math.Abs(v.y), Math.Abs(v.z));
                            d = d + v;
                            //                if(d.x > n.x) n.x = d.x;
                            //				if(d.y > n.y) n.y = d.y;
                            //				if(d.z > n.z) n.z = d.z;

                            //block.Dispose();//没有dispose

                            tmpBuffer.setPixel((int)x, (int)y, d.x, d.y, d.z, pixel.a);
                        }
                    }
                    break;

                case DETECTION_TYPE.DETECTION_DIFFERENCE:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            pixel = mBuffer.getPixel((int)x, (int)y);
                            block = getBlock(x, y);
                            n = Vector3.ZERO;
                            for (int j = 0; j < 3; j++) {
                                d = block[j] - block[6 + (2 - j)];
                                if (Math.Abs(d.x) > n.x)
                                    n.x = Math.Abs(d.x);
                                if (Math.Abs(d.y) > n.y)
                                    n.y = Math.Abs(d.y);
                                if (Math.Abs(d.z) > n.z)
                                    n.z = Math.Abs(d.z);
                            }
                            d = block[5] - block[3];
                            if (Math.Abs(d.x) > n.x)
                                n.x = Math.Abs(d.x);
                            if (Math.Abs(d.y) > n.y)
                                n.y = Math.Abs(d.y);
                            if (Math.Abs(d.z) > n.z)
                                n.z = Math.Abs(d.z);
                            // block.Dispose();//没有dispose

                            tmpBuffer.setPixel((int)x, (int)y, n.x, n.y, n.z, pixel.a);
                        }
                    }
                    break;

                case DETECTION_TYPE.DETECTION_HOMOGENITY:
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            pixel = mBuffer.getPixel((int)x, (int)y);
                            block = getBlock(x, y);
                            v = block[4];
                            n = Vector3.ZERO;
                            for (int j = 0; j < 3; j++) {
                                for (int i = 0; i < 3; i++) {
                                    if (j == 1 && i == 1)
                                        continue;
                                    d = v - block[j * 3 + i];
                                    if (Math.Abs(d.x) > n.x)
                                        n.x = Math.Abs(d.x);
                                    if (Math.Abs(d.y) > n.y)
                                        n.y = Math.Abs(d.y);
                                    if (Math.Abs(d.z) > n.z)
                                        n.z = Math.Abs(d.z);
                                }
                            }
                            //block.Dispose();//没有dispose
                            tmpBuffer.setPixel((int)x, (int)y, n.x, n.y, n.z, pixel.a);
                        }
                    }
                    break;

                case DETECTION_TYPE.DETECTION_CANNY:
                    // STEP 1 - blur image
                    new Blur(mBuffer).setSigma(mSigma).setType(Blur.BLUR_TYPE.BLUR_GAUSSIAN).process();

                    // STEP 2 - calculate magnitude and edge orientation
                    orientation = new Vector3[(int)w * h];
                    gradients = new Vector3[(int)w * h];
                    //n =new Vector3(-std.numeric_limits<float>.infinity(), -std.numeric_limits<float>.infinity(), -std.numeric_limits<float>.infinity());
                    n = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);//溢出数
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            pixel = mBuffer.getPixel((int)x, (int)y);
                            block = getBlock(x, y);
                            d = block[2] + block[8] - block[0] - block[6] + 2.0f * (block[5] - block[3]);
                            v = block[0] + block[2] - block[6] - block[8] + 2.0f * (block[1] - block[7]);
                            gradients[y * w + x] = new Vector3(Math.Sqrt(d.x * d.x + v.x * v.x), Math.Sqrt(d.y * d.y + v.y * v.y), Math.Sqrt(d.z * d.z + v.z * v.z));
                            if (gradients[y * w + x].x > n.x)
                                n.x = gradients[y * w + x].x;
                            if (gradients[y * w + x].y > n.y)
                                n.y = gradients[y * w + x].y;
                            if (gradients[y * w + x].z > n.z)
                                n.z = gradients[y * w + x].z;
                            //block.Dispose();
                            orientation[y * w + x] = Vector3.ZERO;
                            if (d.x == 0.0f) {
                                orientation[y * w + x].x = (v.x == 0.0f) ? 0.0f : 90.0f;
                            }
                            else {
                                div = v.x / d.x;
                                if (div < 0.0f)
                                    orientation[y * w + x].x = 180.0f - Math.ATan(-div).ValueDegrees;
                                else
                                    orientation[y * w + x].x = Math.ATan(div).ValueDegrees;

                                if (orientation[y * w + x].x < 22.5f)
                                    orientation[y * w + x].x = 0.0f;
                                else if (orientation[y * w + x].x < 67.5f)
                                    orientation[y * w + x].x = 45.0f;
                                else if (orientation[y * w + x].x < 112.5f)
                                    orientation[y * w + x].x = 90.0f;
                                else if (orientation[y * w + x].x < 157.5f)
                                    orientation[y * w + x].x = 135.0f;
                                else
                                    orientation[y * w + x].x = 0.0f;
                            }
                            if (d.y == 0.0f) {
                                orientation[y * w + x].y = (v.y == 0.0f) ? 0.0f : 90.0f;
                            }
                            else {
                                div = v.y / d.y;
                                if (div < 0.0f)
                                    orientation[y * w + x].y = 180.0f - Math.ATan(-div).ValueDegrees;
                                else
                                    orientation[y * w + x].y = Math.ATan(div).ValueDegrees;

                                if (orientation[y * w + x].y < 22.5f)
                                    orientation[y * w + x].y = 0.0f;
                                else if (orientation[y * w + x].y < 67.5f)
                                    orientation[y * w + x].y = 45.0f;
                                else if (orientation[y * w + x].y < 112.5f)
                                    orientation[y * w + x].y = 90.0f;
                                else if (orientation[y * w + x].y < 157.5f)
                                    orientation[y * w + x].y = 135.0f;
                                else
                                    orientation[y * w + x].y = 0.0f;
                            }
                            if (d.z == 0.0f) {
                                orientation[y * w + x].z = (v.z == 0.0f) ? 0.0f : 90.0f;
                            }
                            else {
                                div = v.z / d.z;
                                if (div < 0.0f)
                                    orientation[y * w + x].z = 180.0f - Math.ATan(-div).ValueDegrees;
                                else
                                    orientation[y * w + x].z = Math.ATan(div).ValueDegrees;

                                if (orientation[y * w + x].z < 22.5f)
                                    orientation[y * w + x].z = 0.0f;
                                else if (orientation[y * w + x].z < 67.5f)
                                    orientation[y * w + x].z = 45.0f;
                                else if (orientation[y * w + x].z < 112.5f)
                                    orientation[y * w + x].z = 90.0f;
                                else if (orientation[y * w + x].z < 157.5f)
                                    orientation[y * w + x].z = 135.0f;
                                else
                                    orientation[y * w + x].z = 0.0f;
                            }
                        }
                    }

                    // STEP 3 - suppres non maximums
                    for (int y = 1; y < (h - 1); y++) {
                        for (int x = 1; x < (w - 1); x++) {
                            div = gradients[y * w + x].x / n.x;
                            switch (((int)orientation[y * w + x].x)) {
                                default:
                                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                                case 0:
                                    if ((gradients[y * w + x].x < gradients[y * w + (x - 1)].x) || (gradients[y * w + x].x < gradients[y * w + (x + 1)].x))
                                        div = 0.0f;
                                    break;
                                case 45:
                                    if ((gradients[y * w + x].x < gradients[(y + 1) * w + (x - 1)].x) || (gradients[y * w + x].x < gradients[(y - 1) * w + (x + 1)].x))
                                        div = 0.0f;
                                    break;
                                case 90:
                                    if ((gradients[y * w + x].x < gradients[(y + 1) * w + x].x) || (gradients[y * w + x].x < gradients[(y - 1) * w + x].x))
                                        div = 0.0f;
                                    break;
                                case 135:
                                    if ((gradients[y * w + x].x < gradients[(y + 1) * w + (x + 1)].x) || (gradients[y * w + x].x < gradients[(y - 1) * w + (x - 1)].x))
                                        div = 0.0f;
                                    break;
                            }
                            tmpBuffer.setRed((int)x, (int)y, div);
                            div = gradients[y * w + x].y / n.y;
                            switch (((int)orientation[y * w + x].y)) {
                                default:
                                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                                case 0:
                                    if ((gradients[y * w + x].y < gradients[y * w + (x - 1)].y) || (gradients[y * w + x].y < gradients[y * w + (x + 1)].y))
                                        div = 0.0f;
                                    break;
                                case 45:
                                    if ((gradients[y * w + x].y < gradients[(y + 1) * w + (x - 1)].y) || (gradients[y * w + x].y < gradients[(y - 1) * w + (x + 1)].y))
                                        div = 0.0f;
                                    break;
                                case 90:
                                    if ((gradients[y * w + x].y < gradients[(y + 1) * w + x].y) || (gradients[y * w + x].y < gradients[(y - 1) * w + x].y))
                                        div = 0.0f;
                                    break;
                                case 135:
                                    if ((gradients[y * w + x].y < gradients[(y + 1) * w + (x + 1)].y) || (gradients[y * w + x].y < gradients[(y - 1) * w + (x - 1)].y))
                                        div = 0.0f;
                                    break;
                            }
                            tmpBuffer.setGreen((int)x, (int)y, div);
                            div = gradients[y * w + x].z / n.z;
                            switch (((int)orientation[y * w + x].z)) {
                                default:
                                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                                case 0:
                                    if ((gradients[y * w + x].z < gradients[y * w + (x - 1)].z) || (gradients[y * w + x].z < gradients[y * w + (x + 1)].z))
                                        div = 0.0f;
                                    break;
                                case 45:
                                    if ((gradients[y * w + x].z < gradients[(y + 1) * w + (x - 1)].z) || (gradients[y * w + x].z < gradients[(y - 1) * w + (x + 1)].z))
                                        div = 0.0f;
                                    break;
                                case 90:
                                    if ((gradients[y * w + x].z < gradients[(y + 1) * w + x].z) || (gradients[y * w + x].z < gradients[(y - 1) * w + x].z))
                                        div = 0.0f;
                                    break;
                                case 135:
                                    if ((gradients[y * w + x].z < gradients[(y + 1) * w + (x + 1)].z) || (gradients[y * w + x].z < gradients[(y - 1) * w + (x - 1)].z))
                                        div = 0.0f;
                                    break;
                            }
                            tmpBuffer.setBlue((int)x, (int)y, div);
                        }
                    }

                    // STEP 4 - hysteresis
                    mBuffer.setData(tmpBuffer);
                    div = (float)mThresholdHigh / 255.0f;
                    for (int y = 1; y < (h - 1); y++) {
                        for (int x = 1; x < (w - 1); x++) {
                            pixel = mBuffer.getPixel((int)x, (int)y);
                            if (pixel.r < div) {
                                if (pixel.r < (float)mThresholdLow / 255.0f)
                                    tmpBuffer.setRed((int)x, (int)y, 0.0f);
                                else {
                                    if ((mBuffer.getPixelRedReal((int)(x - 1), (int)(y)) < div) && (mBuffer.getPixelRedReal((int)(x + 1), (int)(y)) < div) && (mBuffer.getPixelRedReal((int)(x - 1), (int)(y - 1)) < div) && (mBuffer.getPixelRedReal((int)(x), (int)(y - 1)) < div) && (mBuffer.getPixelRedReal((int)(x + 1), (int)(y - 1)) < div) && (mBuffer.getPixelRedReal((int)(x - 1), (int)(y + 1)) < div) && (mBuffer.getPixelRedReal((int)(x), (int)(y + 1)) < div) && (mBuffer.getPixelRedReal((int)(x + 1), (int)(y + 1)) < div)) {
                                        tmpBuffer.setRed((int)x, (int)y, 0.0f);
                                    }
                                }
                            }
                            if (pixel.g < div) {
                                if (pixel.g < (float)mThresholdLow / 255.0f)
                                    tmpBuffer.setGreen((int)x, (int)y, 0.0f);
                                else {
                                    if ((mBuffer.getPixelGreenReal((int)(x - 1), (int)(y)) < div) && (mBuffer.getPixelGreenReal((int)(x + 1), (int)(y)) < div) && (mBuffer.getPixelGreenReal((int)(x - 1), (int)(y - 1)) < div) && (mBuffer.getPixelGreenReal((int)(x), (int)(y - 1)) < div) && (mBuffer.getPixelGreenReal((int)(x + 1), (int)(y - 1)) < div) && (mBuffer.getPixelGreenReal((int)(x - 1), (int)(y + 1)) < div) && (mBuffer.getPixelGreenReal((int)(x), (int)(y + 1)) < div) && (mBuffer.getPixelGreenReal((int)(x + 1), (int)(y + 1)) < div)) {
                                        tmpBuffer.setGreen((int)x, (int)y, 0.0f);
                                    }
                                }
                            }
                            if (pixel.b < div) {
                                if (pixel.b < (float)mThresholdLow / 255.0f)
                                    tmpBuffer.setBlue((int)x, (int)y, 0.0f);
                                else {
                                    if ((mBuffer.getPixelBlueReal((int)(x - 1), (int)(y)) < div) && (mBuffer.getPixelBlueReal((int)(x + 1), (int)(y)) < div) && (mBuffer.getPixelBlueReal((int)(x - 1), (int)(y - 1)) < div) && (mBuffer.getPixelBlueReal((int)(x), (int)(y - 1)) < div) && (mBuffer.getPixelBlueReal((int)(x + 1), (int)(y - 1)) < div) && (mBuffer.getPixelBlueReal((int)(x - 1), (int)(y + 1)) < div) && (mBuffer.getPixelBlueReal((int)(x), (int)(y + 1)) < div) && (mBuffer.getPixelBlueReal((int)(x + 1), (int)(y + 1)) < div)) {
                                        tmpBuffer.setBlue((int)x, (int)y, 0.0f);
                                    }
                                }
                            }
                        }
                    }

                    //orientation.Dispose();
                    //gradients.Dispose();
                    break;
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with edgedetection filter : " + (mType).ToString());
            return mBuffer;
        }

        private Vector3[] getBlock(int x, int y) {
            ColourValue pixel = mBuffer.getPixel((int)x, (int)y);
            Vector3[] block = new Vector3[9];
            for (int j = -1; j < 2; j++) {
                for (int i = -1; i < 2; i++) {
                    block[(j + 1) * 3 + (i + 1)] = new Vector3(pixel.r, pixel.g, pixel.b);
                    if (j == 0 && i == 0)
                        continue;
                    if ((x + i) < 0 || (x + i) >= (int)mBuffer.getWidth())
                        continue;
                    if ((y + j) < 0 || (y + j) >= (int)mBuffer.getHeight())
                        continue;
                    block[(j + 1) * 3 + (i + 1)] = new Vector3((float)mBuffer.getPixelRedReal((int)(x + i), (int)(y + j)), (float)mBuffer.getPixelGreenReal((int)(x + i), (int)(y + j)), (float)mBuffer.getPixelBlueReal((int)(x + i), (int)(y + j)));
                }
            }
            return block;
        }
    }

    //*
    //\brief Draw an ellipse.
    //\details Draw a filled ellipse on top of previous content.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //Procedural::EllipseTexture(&bufferSolid).setColour(Ogre::ColourValue::Red).setRadiusX(0.4f).setRadiusY(0.2f).process();
    //\endcode
    //\dotfile texture_33.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport EllipseTexture : public TextureProcessing
    public class EllipseTexture : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private int mX = new int();
        private int mY = new int();
        private int mRadiusX = new int();
        private int mRadiusY = new int();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public EllipseTexture(TextureBuffer pBuffer)
            : base(pBuffer, "EllipseTexture") {
            mColour = ColourValue.White;
            mRadiusX = (int)pBuffer.getWidth() / 2;
            mX = mRadiusX;
            mRadiusY = (int)pBuffer.getHeight() / 2;
            mY = mRadiusY;
        }

        //    *
        //	Set the fill colour of the ellipse.
        //	\param colour New colour for processing (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public EllipseTexture setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the fill colour of the ellipse.
        //	\param red Red value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the fill colour [0.0, 1.0] \(default 1.0)
        //	
        public EllipseTexture setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: EllipseTexture& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public EllipseTexture setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the absolute radius of the ellipse on x axis.
        //	\param radiusx New absolute radius of the ellipse on x axis in px (default 1/2 * image width)
        //	
        public EllipseTexture setRadiusX(int radiusx) {
            mRadiusX = radiusx;
            return this;
        }

        //    *
        //	Set the relative radius of the ellipse on x axis.
        //	\param radiusx New relative radius of the ellipse on x axis [0.0, 1.0] \(default 0.5)
        //	
        public EllipseTexture setRadiusX(float radiusx) {
            mRadiusX = (int)((float)System.Math.Min(mBuffer.getWidth(), mBuffer.getHeight()) * Math.Abs(radiusx));
            return this;
        }

        //    *
        //	Set the absolute radius of the ellipse on y axis.
        //	\param radiusy New absolute radius of the ellipse on y axis in px (default 1/2 * image width)
        //	
        public EllipseTexture setRadiusY(int radiusy) {
            mRadiusY = radiusy;
            return this;
        }

        //    *
        //	Set the relative radius of the ellipse on y axis.
        //	\param radiusy New relative radius of the ellipse on y axis [0.0, 1.0] \(default 0.5)
        //	
        public EllipseTexture setRadiusY(float radiusy) {
            mRadiusY = (int)((float)System.Math.Min(mBuffer.getWidth(), mBuffer.getHeight()) * Math.Abs(radiusy));
            return this;
        }

        //    *
        //	Set the absolute radius of the ellipse.
        //	\param radiusx New absolute radius of the ellipse on x axis in px (default 1/2 * image width)
        //	\param radiusy New absolute radius of the ellipse on y axis in px (default 1/2 * image width)
        //	
        public EllipseTexture setRadius(int radiusx, int radiusy) {
            mRadiusX = radiusx;
            mRadiusY = radiusy;
            return this;
        }

        //    *
        //	Set the relative radius of the ellipse.
        //	\param radiusx New relative radius of the ellipse on x axis [0.0, 1.0] \(default 0.5)
        //	\param radiusy New relative radius of the ellipse on y axis [0.0, 1.0] \(default 0.5)
        //	
        public EllipseTexture setRadius(float radiusx, float radiusy) {
            mRadiusX = (int)((float)System.Math.Min(mBuffer.getWidth(), mBuffer.getHeight()) * Math.Abs(radiusx));
            mRadiusY = (int)((float)System.Math.Min(mBuffer.getWidth(), mBuffer.getHeight()) * Math.Abs(radiusy));
            return this;
        }

        //    *
        //	Set absolute x position of ellipse center point in px
        //	\param x New absolute x position of ellipse center (default 1/2 * image width)
        //	
        public EllipseTexture setCenterX(int x) {
            mX = System.Math.Min(x, (int)mBuffer.getWidth() - 1);
            return this;
        }

        //    *
        //	Set relative x position of ellipse center point as Real
        //	\param x New relative x position of ellipse center [0.0, 1.0] \(default 0.5)
        //	
        public EllipseTexture setCenterX(float x) {
            mX = System.Math.Min((int)(x * (float)mBuffer.getWidth()), (int)mBuffer.getWidth() - 1);
            return this;
        }

        //    *
        //	Set absolute y position of ellipse center point in px
        //	\param y New absolute y position of ellipse center (default 1/2 * image width)
        //	
        public EllipseTexture setCenterY(int y) {
            mY = System.Math.Min(y, (int)mBuffer.getHeight() - 1);
            return this;
        }

        //    *
        //	Set relative y position of ellipse center point as Real
        //	\param y New relative y position of ellipse center [0.0, 1.0] \(default 0.5)
        //	
        public EllipseTexture setCenterY(float y) {
            mY = System.Math.Min((int)(y * (float)mBuffer.getHeight()), (int)mBuffer.getHeight() - 1);
            return this;
        }

        //    *
        //	Set the position of ellipse center point.
        //	\param pos Vector to the center point of the ellipse (default: x=0.5, y=0.5)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public EllipseTexture setCenter(Vector2 pos) {
            return setCenter(pos, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: EllipseTexture& setCenter(Ogre::Vector2 pos, bool relative = true)
        public EllipseTexture setCenter(Vector2 pos, bool relative) {
            setCenter(pos.x, pos.y, relative);
            return this;
        }

        //    *
        //	Set the position of ellipse center point.
        //	\param x New absolute x position of ellipse center (default 1/2 * image width)
        //	\param y New absolute y position of ellipse center (default 1/2 * image width)
        //	
        public EllipseTexture setCenter(int x, int y) {
            setCenterX(x);
            setCenterY(y);
            return this;
        }

        //    *
        //	Set the position of ellipse center point.
        //	\param x New relative x position of ellipse center [0.0, 1.0] \(default 0.5)
        //	\param y New relative y position of ellipse center [0.0, 1.0] \(default 0.5)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public EllipseTexture setCenter(float x, float y) {
            return setCenter(x, y, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: EllipseTexture& setCenter(Ogre::float x, Ogre::float y, bool relative = true)
        public EllipseTexture setCenter(float x, float y, bool relative) {
            if (relative) {
                setCenterX(x);
                setCenterY(y);
            }
            else {
                setCenterX((int)x);
                setCenterY((int)y);
            }
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int dx = 0;
            int dy = mRadiusY;
            int rx2 = mRadiusX * mRadiusX;
            int ry2 = mRadiusY * mRadiusY;
            int err = ry2 - (2 * mRadiusY - 1) * rx2;
            int e2 = 0;

            do {
                for (int qy = -dy; qy <= dy; qy++) {
                    _putpixel(+dx, qy);
                    _putpixel(-dx, qy);
                }

                e2 = 2 * err;
                if (e2 < (2 * dx + 1) * ry2) {
                    dx++;
                    err += (2 * dx + 1) * ry2;
                }
                if (e2 > -(2 * dy - 1) * rx2) {
                    dy--;
                    err -= (2 * dy - 1) * rx2;
                }
            }
            while (dy >= 0);

            while (dx++ < (int)mRadiusX) {
                _putpixel(+dx, 0);
                _putpixel(-dx, 0);
            }

            Utils.log("Modify texture with ellipse filter : x = " + (mX).ToString() + ", y = " + (mY).ToString() + ", Radius x = " + (mRadiusX).ToString() + ", Radius y = " + (mRadiusY).ToString());
            return mBuffer;
        }

        private void _putpixel(int dx, int dy) {
            if (mX + dx < 0 || mX + dx >= mBuffer.getWidth())
                return;
            if (mY + dy < 0 || mY + dy >= mBuffer.getHeight())
                return;
            mBuffer.setPixel(mX + dx, mY + dy, mColour);
        }
    }

    //*
    //\brief %Flip the image.
    //\details Flip the input image on different axis.
    //
    //Examples:
    //\b FLIP_POINT
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Flip(&bufferImage).setAxis(Procedural::Flip::FLIP_POINT).process();
    //\endcode
    //\dotfile texture_14a.gv
    //
    //\b FLIP_VERTICAL
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Flip(&bufferImage).setAxis(Procedural::Flip::FLIP_VERTICAL).process();
    //\endcode
    //\dotfile texture_14b.gv
    //
    //\b FLIP_HORIZONTAL
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Flip(&bufferImage).setAxis(Procedural::Flip::FLIP_HORIZONTAL).process();
    //\endcode
    //\dotfile texture_14c.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Flip : public TextureProcessing
    public class Flip : TextureProcessing
    {
        //! Flip axis selection
        public enum FLIP_AXIS : int
        {
            FLIP_HORIZONTAL, //!< Flip horizontal
            FLIP_VERTICAL, //!< Flip vertical
            FLIP_POINT //!< Flip middle
        }

        private FLIP_AXIS mAxis;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Flip(TextureBuffer pBuffer)
            : base(pBuffer, "Flip") {
            mAxis = FLIP_AXIS.FLIP_VERTICAL;
        }

        //    *
        //	Set the axis to flip.
        //	\param axis Axis where to flip the image arround (default FLIP_VERTICAL)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Flip setAxis(FLIP_AXIS axis) {
            mAxis = axis;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            TextureBuffer tmpBuffer = mBuffer.clone();
            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    switch (mAxis) {
                        case FLIP_AXIS.FLIP_HORIZONTAL:
                            tmpBuffer.setPixel(x, (int)mBuffer.getHeight() - 1 - y, mBuffer.getPixel(x, y));
                            break;

                        default:
                        //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                        case FLIP_AXIS.FLIP_VERTICAL:
                            tmpBuffer.setPixel(mBuffer.getWidth() - 1 - x, y, mBuffer.getPixel(x, y));
                            break;

                        case FLIP_AXIS.FLIP_POINT:
                            tmpBuffer.setPixel(mBuffer.getWidth() - 1 - x, mBuffer.getHeight() - 1 - y, mBuffer.getPixel(x, y));
                            break;
                    }
                }
            }
            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with flip filter : " + (mAxis).ToString());
            return mBuffer;
        }
    }

    //*
    //\brief %Render a glow.
    //\details %Glow is a blurred, filled ellipse of given color, that is added to existing content.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).process();
    //Procedural::Glow(&bufferGradient).process();
    //\endcode
    //\dotfile texture_15.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Glow : public TextureProcessing
    public class Glow : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private float mCenterX = 0f;
        private float mCenterY = 0f;
        private float mRadiusX = 0f;
        private float mRadiusY = 0f;
        private float mAlpha = 0f;
        private float mGamma = 0f;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Glow(TextureBuffer pBuffer)
            : base(pBuffer, "Glow") {
            mColour = ColourValue.White;
            mCenterX = 0.5f;
            mCenterY = 0.5f;
            mRadiusX = 0.5f;
            mRadiusY = 0.5f;
            mAlpha = 1.0f;
            mGamma = 1.0f;
        }

        //    *
        //	Set the colour of the glow ellipse.
        //	\param colour New colour for glow ellipse (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Glow setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the glow ellipse.
        //	\param red Red value of the glow ellipse [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the glow ellipse [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the glow ellipse [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the glow ellipse [0.0, 1.0] \(default 1.0)
        //	
        public Glow setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Glow& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Glow setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the relative center position of the blur circle on x axis.
        //	\param centerx New relative center of the blur circle [0.0, 1.0] \(default 0.5)
        //	
        public Glow setCenterX(float centerx) {
            mCenterX = centerx;
            return this;
        }

        //    *
        //	Set the relative center position of the blur circle on y axis.
        //	\param centery New relative center of the blur circle [0.0, 1.0] \(default 0.5)
        //	
        public Glow setCenterY(float centery) {
            mCenterY = centery;
            return this;
        }

        //    *
        //	Set the relative radius of the blur circle in x direction.
        //	\param radiusx New relative radius of the blur circle [0.0, 1.0] \(default 0.5)
        //	
        public Glow setRadiusX(float radiusx) {
            mRadiusX = radiusx;
            return this;
        }

        //    *
        //	Set the relative radius of the blur circle in y direction.
        //	\param radiusy New relative radius of the blur circle [0.0, 1.0] \(default 0.5)
        //	
        public Glow setRadiusY(float radiusy) {
            mRadiusY = radiusy;
            return this;
        }

        //    *
        //	Set alpha value of blur effect.
        //	\param alpha New alpha value for blur effect (default 1)
        //	
        public Glow setAlpha(float alpha) {
            mAlpha = alpha;
            return this;
        }

        //    *
        //	Set gamma value of blur effect.
        //	\param gamma New gamma value for blur effect (default 1)
        //	
        public Glow setGamma(float gamma) {
            mGamma = gamma;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            int dwCenterX = (int)(mCenterX * (float)w);
            int dwCenterY = (int)(mCenterY * (float)h);
            int dwRadiusX = (int)(mRadiusX * (float)w);
            int dwRadiusY = (int)(mRadiusY * (float)h);
            float fRed = mColour.r * 255.0f;
            float fGreen = mColour.g * 255.0f;
            float fBlue = mColour.b * 255.0f;
            float f1_RadiusX = 1.0f / (float)dwRadiusX;
            float f1_RadiusY = 1.0f / (float)dwRadiusY;

            for (int y = 0; y < h; y++) {
                float dy = (float)(y - dwCenterY) * f1_RadiusY;

                for (int x = 0; x < w; x++) {
                    float dx = (float)(x - dwCenterX) * f1_RadiusX;
                    float d = Math.Sqrt(dx * dx + dy * dy);
                    if (d > 1.0f)
                        d = 1.0f;
                    d = 1.0f - d;

                    uint r = (uint)(((float)mBuffer.getPixelRedByte(x, y)) + ((mGamma * d * fRed) * mAlpha));
                    uint g = (uint)(((float)mBuffer.getPixelGreenByte(x, y)) + ((mGamma * d * fGreen) * mAlpha));
                    uint b = (uint)(((float)mBuffer.getPixelBlueByte(x, y)) + ((mGamma * d * fBlue) * mAlpha));
                    byte a = mBuffer.getPixelAlphaByte(x, y);

                    mBuffer.setPixel(x, y, (byte)((r < 255) ? r : 255), (byte)((g < 255) ? g : 255), (byte)((b < 255) ? b : 255), a);
                }
            }

            Utils.log("Modify texture with glow filter");
            return mBuffer;
        }
    }

    //*
    //\brief %Invert image.
    //\details %Invert all three color channels, %Alpha is left unchanged (use Colours filter to adjust alpha).
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //Procedural::Invert(&bufferGradient).process();
    //\endcode
    //\dotfile texture_16.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Invert : public TextureProcessing
    public class Invert : TextureProcessing
    {
        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Invert(TextureBuffer pBuffer)
            : base(pBuffer, "Invert") {
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public override TextureBuffer process() {
            int w = mBuffer.getWidth();
            int h = mBuffer.getHeight();

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    ColourValue pixel = mBuffer.getPixel(x, y);
                    mBuffer.setPixel(x, y, 1.0f - pixel.r, 1.0f - pixel.g, 1.0f - pixel.b, pixel.a);
                }
            }

            Utils.log("Modify texture with invert filter");
            return mBuffer;
        }
    }

    //*
    //\brief Exchange pixels at random positions.
    //\details Exchange pixels in a small area randomly.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Jitter(&bufferImage).process();
    //\endcode
    //\dotfile texture_17.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Jitter : public TextureProcessing
    public class Jitter : TextureProcessing
    {
        private uint mSeed = 0;
        private byte mRadius = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Jitter(TextureBuffer pBuffer)
            : base(pBuffer, "Jitter") {
            mRadius = 57;
            mSeed = 5120;
        }

        //    *
        //	Set the radius of the detection area.
        //	\param radius New radius for detection area [0, 255] (default 57)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Jitter setRadius(byte radius) {
            mRadius = radius;
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Jitter setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            TextureBuffer tmpBuffer = mBuffer.clone();
            RandomNumbers.Seed((int)mSeed);
            int radius = (int)(1.0f + (9.0f / 255.0f) * ((float)mRadius - 1.0f));
            int max = radius * 2 + 1;
            for (int y = 0; y < (int)mBuffer.getHeight(); y++) {
                for (int x = 0; x < (int)mBuffer.getWidth(); x++) {
                    int rx = x + (RandomNumbers.NextNumber() % (radius * 2 + 1)) - radius;
                    int ry = y + (RandomNumbers.NextNumber() % (radius * 2 + 1)) - radius;

                    if (rx >= 0 && rx < (int)mBuffer.getWidth() && ry >= 0 && ry < (int)mBuffer.getHeight())
                        tmpBuffer.setPixel((int)rx, (int)ry, mBuffer.getPixel((int)x, (int)y));
                }
            }
            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with jitter filter : " + (mRadius).ToString());
            return mBuffer;
        }
    }

    //*
    //\brief Linear interpolation.
    //\details Mix first two inputs (a, b) in proportions given by base input (c). Each color channel is processed separately. Equation: x = bc + a(1 - c)
    //
    //Example:
    //\code{.cpp}
    //// Image C
    //Procedural::TextureBuffer bufferCloud(256);
    //Procedural::Cloud(&bufferCloud).process();
    //
    //// Image A
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).process();
    //
    //// Image B
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(233).process();
    //
    //// Filter
    //Procedural::Lerp(&bufferCloud).setImageA(&bufferGradient).setImageB(&bufferCell).process();
    //\endcode
    //\dotfile texture_18.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Lerp : public TextureProcessing
    public class Lerp : TextureProcessing
    {
        private TextureBuffer mBufferA;
        private TextureBuffer mBufferB;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	\note This is the third image (c).
        //	
        public Lerp(TextureBuffer pBuffer)
            : base(pBuffer, "Lerp") {
            mBufferA = null;
            mBufferB = null;
        }

        //    *
        //	Set first image (a).
        //	\param image1 Pointer to a new first image (default NULL)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Lerp setImageA(TextureBuffer image1) {
            mBufferA = image1;
            return this;
        }

        //    *
        //	Set second image (b).
        //	\param image2 Pointer to a new second image (default NULL)
        //	
        public Lerp setImageB(TextureBuffer image2) {
            mBufferB = image2;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    ColourValue pixelA = mBufferA.getPixel(x, y);
                    ColourValue pixelB = mBufferB.getPixel(x, y);
                    ColourValue pixelC = mBuffer.getPixel(x, y);

                    mBuffer.setPixel(x, y, pixelA.r * (1.0f - pixelC.r) + pixelB.r * pixelC.r, pixelA.g * (1.0f - pixelC.g) + pixelB.g * pixelC.g, pixelA.b * (1.0f - pixelC.b) + pixelB.b * pixelC.b, pixelA.a * (1.0f - pixelC.a) + pixelB.a * pixelC.a);
                }
            }

            Utils.log("Modify texture with lerp filter");
            return mBuffer;
        }
    }

    //*
    //\brief Apply an illumination on a surface.
    //\details The parameter image is the required normal map. You can also set ambient, diffuse and specular light with different colors and intensity.
    //
    //Example:
    //\code{.cpp}
    //// Generate structure
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //// Create normal map
    //Procedural::TextureBufferPtr bufferNormalMap = bufferColour.clone();
    //Procedural::Normals(bufferNormalMap).process();
    //
    //Procedural::Light(&bufferCell).setNormalMap(bufferNormalMap).setColourAmbient(127, 60, 0, 0).setColourDiffuse(60, 25, 0, 0).setBumpPower(255).process();
    //delete bufferNormalMap;
    //\endcode
    //\dotfile texture_19a.gv
    //\note If you don't set the normal map a clone of the base input image will be used as normal map with Normals filter. \dotfile texture_19b.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TextureLightBaker : public TextureProcessing
    public class TextureLightBaker : TextureProcessing
    {
        private TextureBuffer mNormal;
        private ColourValue mColourAmbient = new ColourValue();
        private ColourValue mColourDiffuse = new ColourValue();
        private ColourValue mColourSpecular = new ColourValue();
        private Vector3 mPosition = new Vector3();
        private byte mSpecularPower = 0;
        private byte mBumpPower = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public TextureLightBaker(TextureBuffer pBuffer)
            : base(pBuffer, "Light") {
            mNormal = null;
            mColourAmbient = ColourValue.Black;
            mColourDiffuse = new ColourValue(0.5f, 0.5f, 0.5f, 1.0f);
            mColourSpecular = ColourValue.White;
            mPosition = new Vector3(255.0f, 255.0f, 127.0f);
            mSpecularPower = 0;
            mBumpPower = 0;
        }

        //    *
        //	Set parameter image for compensation.
        //	\param normal Pointer to an normal map image (default NULL)
        //	\note If the parameter normal is set to NULL a clone of the base input image will be used as normal map with Normals filter.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public TextureLightBaker setNormalMap(TextureBuffer normal) {
            mNormal = normal;
            return this;
        }

        //    *
        //	Set the ambient light colour.
        //	\param colour New ambient light colour (default Ogre::ColourValue::Black)
        //	
        public TextureLightBaker setColourAmbient(ColourValue colour) {
            //
            //ORIGINAL LINE: mColourAmbient = colour;
            mColourAmbient = (colour);
            return this;
        }

        //    *
        //	Set the ambient light colour.
        //	\param red Red value of ambient light colour [0.0, 1.0] \(default 0.0)
        //	\param green Green value of ambient light colour [0.0, 1.0] \(default 0.0)
        //	\param blue Blue value of ambient light colour [0.0, 1.0] \(default 0.0)
        //	\param alpha %Alpha value of ambient light colour [0.0, 1.0] \(default 0.0)
        //	
        public TextureLightBaker setColourAmbient(float red, float green, float blue) {
            return setColourAmbient(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: TextureLightBaker& setColourAmbient(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public TextureLightBaker setColourAmbient(float red, float green, float blue, float alpha) {
            mColourAmbient = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the diffuse light colour.
        //	\param colour New diffuse light colour (default Ogre::ColourValue(0.5f, 0.5f, 0.5f, 1.0f))
        //	
        public TextureLightBaker setColourDiffuse(ColourValue colour) {
            //
            //ORIGINAL LINE: mColourDiffuse = colour;
            mColourDiffuse = (colour);
            return this;
        }

        //    *
        //	Set the diffuse light colour.
        //	\param red Red value of diffuse light colour [0.0, 1.0] \(default 0.5)
        //	\param green Green value of diffuse light colour [0.0, 1.0] \(default 0.5)
        //	\param blue Blue value of diffuse light colour [0.0, 1.0] \(default 0.5)
        //	\param alpha %Alpha value of diffuse light colour [0.0, 1.0] \(default 1.0)
        //	
        public TextureLightBaker setColourDiffuse(float red, float green, float blue) {
            return setColourDiffuse(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: TextureLightBaker& setColourDiffuse(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public TextureLightBaker setColourDiffuse(float red, float green, float blue, float alpha) {
            mColourDiffuse = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the specular light colour.
        //	\param colour New specular light colour (default Ogre::ColourValue::White)
        //	
        public TextureLightBaker setColourSpecular(ColourValue colour) {
            //
            //ORIGINAL LINE: mColourSpecular = colour;
            mColourSpecular = (colour);
            return this;
        }

        //    *
        //	Set the specular light colour.
        //	\param red Red value of specular light colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of specular light colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of specular light colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of specular light colour [0.0, 1.0] \(default 1.0)
        //	
        public TextureLightBaker setColourSpecular(float red, float green, float blue) {
            return setColourSpecular(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: TextureLightBaker& setColourSpecular(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public TextureLightBaker setColourSpecular(float red, float green, float blue, float alpha) {
            mColourSpecular = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the light colours.
        //	\param ambient New ambient light colour (default Ogre::ColourValue::Black)
        //	\param diffuse New diffuse light colour (default Ogre::ColourValue(0.5f, 0.5f, 0.5f, 1.0f))
        //	\param specular New specular light colour (default Ogre::ColourValue::White)
        //	
        public TextureLightBaker setColours(ColourValue ambient, ColourValue diffuse, ColourValue specular) {
            //
            //ORIGINAL LINE: mColourAmbient = ambient;
            mColourAmbient = (ambient);
            //
            //ORIGINAL LINE: mColourDiffuse = diffuse;
            mColourDiffuse = (diffuse);
            //
            //ORIGINAL LINE: mColourSpecular = specular;
            mColourSpecular = (specular);
            return this;
        }

        //    *
        //	Set the position of light on/over the image.
        //	\param position New light position (default Ogre::Vector3(255.0f, 255.0f, 127.0f))
        //	
        public TextureLightBaker setPosition(Vector3 position) {
            return setPosition(position.x, position.y, position.z);
        }

        //    *
        //	Set the position of light on/over the image.
        //	\param x New light position on x axis \(default 255.0f)
        //	\param y New light position on y axis \(default 255.0f)
        //	\param z New light position on z axis \(default 127.0f)
        //	
        public TextureLightBaker setPosition(float x, float y, float z) {
            mPosition = new Vector3(System.Math.Max(System.Math.Min(x, 255.0f), 0.0f), System.Math.Max(System.Math.Min(y, 255.0f), 0.0f), System.Math.Max(System.Math.Min(z, 255.0f), 0.0f));
            return this;
        }

        //    *
        //	Set specular light power.
        //	\param power New power value for specular light (default 0)
        //	
        public TextureLightBaker setSpecularPower(byte power) {
            mSpecularPower = power;
            return this;
        }

        //    *
        //	Set bump mapping power.
        //	\param power New power value for bump mapping (default 0)
        //	
        public TextureLightBaker setBumpPower(byte power) {
            mBumpPower = power;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = mBuffer.getWidth();
            int h = mBuffer.getHeight();
            Vector3 light = new Vector3(mPosition.x - 127.0f, -(mPosition.y - 127.0f), -(127.0f - mPosition.z));
            //light.Normalise();
            light = light.NormalisedCopy;
            float fSpecularPower = ((float)mSpecularPower) / 32.0f;
            float fBumpPower = ((float)mBumpPower) / 32.0f;

            if (mNormal != null && (mNormal.getWidth() < w || mNormal.getHeight() < h))
                return mBuffer;

            TextureBuffer normalMap;
            if (mNormal != null)
                normalMap = mNormal.clone();
            else {
                normalMap = mBuffer.clone();
                new Normals(normalMap).process();
            }

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    ColourValue pixel = normalMap.getPixel(x, y);
                    Vector3 n = new Vector3(pixel.r * 255.0f - 127.0f, pixel.g * 255.0f - 127.0f, pixel.b * 255.0f - 127.0f);
                    //n.Normalise();
                    n = n.NormalisedCopy;

                    float fdot = n.x * light.x + n.y * light.y + n.z * light.z;
                    if (fdot < 0.0f)
                        fdot = 0.0f;
                    fdot *= fBumpPower;

                    int r = (int)(mColourAmbient.r * 255.0f + (fdot * mColourDiffuse.r * 255.0f) + (fdot * fdot * mColourSpecular.r * fSpecularPower));
                    int g = (int)(mColourAmbient.g * 255.0f + (fdot * mColourDiffuse.g * 255.0f) + (fdot * fdot * mColourSpecular.g * fSpecularPower));
                    int b = (int)(mColourAmbient.b * 255.0f + (fdot * mColourDiffuse.b * 255.0f) + (fdot * fdot * mColourSpecular.b * fSpecularPower));

                    pixel = mBuffer.getPixel(x, y);
                    r = ((int)(pixel.r * 255.0f) + r) / 2;
                    g = ((int)(pixel.g * 255.0f) + g) / 2;
                    b = ((int)(pixel.b * 255.0f) + b) / 2;

                    mBuffer.setPixel(x, y, (byte)((r < 255) ? r : 255), (byte)((g < 255) ? g : 255), (byte)((b < 255) ? b : 255), (byte)(pixel.a * 255.0f));
                }
            }

            normalMap.Dispose();
            Utils.log("Modify texture with light filter");
            return mBuffer;
        }
    }

    //*
    //\brief Copy pixels from base input (x, y) to given coordinates from parameter image (red, green).
    //\details Use the red and green value of the parameter image as coordinates for colour painting.
    //
    //Example:
    //\code{.cpp}
    //// Image colour
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //// Filter
    //Procedural::Lookup(&bufferGradient).setParameterImage(&bufferCell).process();
    //\endcode
    //\dotfile texture_20.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Lookup : public TextureProcessing
    public class Lookup : TextureProcessing
    {
        private TextureBuffer mParam;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Lookup(TextureBuffer pBuffer)
            : base(pBuffer, "Lookup") {
            mParam = null;
        }

        //    *
        //	Set parameter image for coordinates mapping.
        //	\param image Pointer to second image (default NULL)
        //	\note If the parameter image is set to NULL there won't be any image manipulation.
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Lookup setParameterImage(TextureBuffer image) {
            mParam = image;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            if (mParam == null)
                return mBuffer;

            int tw = mBuffer.getWidth();
            int th = mBuffer.getHeight();
            int w = mParam.getWidth();
            int h = mParam.getHeight();
            TextureBuffer tmpBuffer = mBuffer.clone();

            if (w < tw || h < th)
                return mBuffer;

            float scaleW = tw / 256.0f;
            float scaleH = th / 256.0f;

            for (int y = 0; y < h; ++y) {
                for (int x = 0; x < w; ++x) {
                    ColourValue pixel = mParam.getPixel(x, y);
                    int u = (int)(pixel.r * (float)w);
                    int v = (int)(pixel.g * (float)h);
                    u = Utils.Clamp(u, 0, w - 1);
                    v = Utils.Clamp(v, 0, h - 1);
                    tmpBuffer.setPixel(x, y, mBuffer.getPixel(v, u));
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with lookup filter");
            return mBuffer;
        }
    }

    //*
    //\brief Convert height map to normal map.
    //\details Use Sobel algorithm to calculate a normal map out of the given image.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //Procedural::Normals(&bufferCell).process();
    //\endcode
    //\dotfile texture_21a.gv
    //\par Tip
    //Create a copy of your working TextureBuffer for normal mapping before you colour it:
    //\code{.cpp}
    //// Generate structure
    //Procedural::TextureBuffer bufferColour(256);
    //Procedural::Cell(&bufferColour).setDensity(4).setRegularity(234).process();
    //
    //// Create normal map
    //Procedural::TextureBufferPtr bufferNormalMap = bufferColour.clone();
    //Procedural::Normals(bufferNormalMap).process();
    //
    //// Colourize structure
    //Procedural::Colours(&bufferColour).setColourBase(Ogre::ColourValue::Red).setColourPercent(Ogre::ColourValue::Blue).process();
    //delete bufferNormalMap;
    //\endcode
    //\dotfile texture_21b.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Normals : public TextureProcessing
    public class Normals : TextureProcessing
    {
        private byte mAmplify = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Normals(TextureBuffer pBuffer)
            : base(pBuffer, "Normals") {
            mAmplify = 64;
        }

        //    *
        //	Set amplify for normal calculation
        //	\param amplify New amplify for calculation (default 64)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Normals setAmplify(byte amplify) {
            mAmplify = amplify;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int w = (int)mBuffer.getWidth();
            int h = (int)mBuffer.getHeight();
            float fAmp = (float)mAmplify * 4.0f / 255.0f;
            TextureBuffer tmpBuffer = mBuffer.clone();

            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    int xp = (x < 1) ? 0 : (x - 1) % w;
                    int xn = (x + 1) % w;
                    int yp = (y < 1) ? 0 : (y - 1) % h;
                    int yn = (y + 1) % h;

                    //Y Sobel filter
                    float fPix = (float)mBuffer.getPixelRedByte(xp, yn);
                    float dY = fPix * -1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(x, yn);
                    dY += fPix * -2.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xn, yn);
                    dY += fPix * -1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xp, yp);
                    dY += fPix * 1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(x, yp);
                    dY += fPix * 2.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xn, yp);
                    dY += fPix * 1.0f;

                    //X Sobel filter
                    fPix = (float)mBuffer.getPixelRedByte(xp, yp);
                    float dX = fPix * -1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xp, y);
                    dX += fPix * -2.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xp, yn);
                    dX += fPix * -1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xn, yp);
                    dX += fPix * 1.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xn, y);
                    dX += fPix * 2.0f;
                    fPix = (float)mBuffer.getPixelRedByte(xn, yn);
                    dX += fPix * 1.0f;

                    float normx = -dX * fAmp / 255.0f;
                    float normy = -dY * fAmp / 255.0f;
                    float norm = Math.Sqrt(normx * normx + normy * normy + 1.0f);
                    if (norm > (float)10e-6)
                        norm = 1.0f / norm;
                    else
                        norm = 0.0f;
                    normx = (normx * norm) * 127.0f + 128.0f;
                    normy = (normy * norm) * 127.0f + 128.0f;
                    float normz = norm * 127.0f + 128.0f;

                    tmpBuffer.setPixel(x, y, (byte)normx, (byte)normy, (byte)normz, mBuffer.getPixelAlphaByte(x, y));
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with normals filter");
            return mBuffer;
        }
    }

    //*
    //\brief Create an oil painted image.
    //\details Combine similar pixels to a small group which gives the effect of an oil painted picture.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::OilPaint(&bufferImage).setRadius(5).process();
    //\endcode
    //\dotfile texture_22.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport OilPaint : public TextureProcessing
    public class OilPaint : TextureProcessing
    {
        private byte mRadius = 0;
        private float mIntensity = 0f;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public OilPaint(TextureBuffer pBuffer)
            : base(pBuffer, "OilPaint") {
            mRadius = 3;
            mIntensity = 20.0f;
        }

        //    *
        //	Set radius size for calculation.
        //	\param radius New radius for detection arround current pixel [3, 255] (default 3)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public OilPaint setRadius(byte radius) {
            mRadius = radius;
            if (mRadius < 3)
                mRadius = 3;
            return this;
        }

        //    *
        //	Set intensity for painting.
        //	\param intensity New intensity factor which affects brush size \(default 20.0)
        //	
        public OilPaint setIntensity(float intensity) {
            mIntensity = intensity;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            TextureBuffer tmpBuffer = mBuffer.clone();

            int[] intensities = new int[256];
            int[] red = new int[256];
            int[] green = new int[256];
            int[] blue = new int[256];

            for (int y = mRadius; y < (int)(mBuffer.getHeight() - mRadius); y++) {
                for (int x = mRadius; x < (int)(mBuffer.getWidth() - mRadius); x++) {
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset<int>(intensities, 0, 256 * sizeof(int));
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset<int>(red, 0, (uint)red.Length);
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset<int>(green, 0, (uint)green.Length);
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset<int>(blue, 0, (uint)(blue.Length));

                    for (int j = -mRadius; j <= mRadius; j++) {
                        for (int i = -mRadius; i <= mRadius; i++) {
                            int r = mBuffer.getPixelRedByte((int)(x + i), (int)(y + j));
                            int g = mBuffer.getPixelGreenByte((int)(x + i), (int)(y + j));
                            int b = mBuffer.getPixelBlueByte((int)(x + i), (int)(y + j));

                            int curr = (int)((((float)(r + g + b) / 3.0f) * mIntensity) / 255.0f);
                            if (curr > 255)
                                curr = 255;
                            intensities[curr]++;

                            red[curr] += r;
                            green[curr] += g;
                            blue[curr] += b;
                        }
                    }

                    int maxInt = 0;
                    int maxIndex = 0;
                    for (int i = 0; i < 256; i++) {
                        if (intensities[i] > maxInt) {
                            maxInt = intensities[i];
                            maxIndex = i;
                        }
                    }

                    tmpBuffer.setPixel((int)x, (int)y, (byte)(red[maxIndex] / maxInt), (byte)(green[maxIndex] / maxInt), (byte)(blue[maxIndex] / maxInt));
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with oilpaint filter");
            return mBuffer;
        }
    }

    //*
    //\brief Draw a number of pixels at random positions.
    //\details Paint a specific number of pixels at random positions in a given colour.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //Procedural::RandomPixels(&bufferSolid).setColour(Ogre::ColourValue::Red).setCount(200).process();
    //\endcode
    //\dotfile texture_23.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport RandomPixels : public TextureProcessing
    public class RandomPixels : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;
        private uint mCount = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public RandomPixels(TextureBuffer pBuffer)
            : base(pBuffer, "RandomPixels") {
            mColour = ColourValue.White;
            mSeed = 5120;
            mCount = ((uint)Math.Sqrt((float)mBuffer.getWidth()) + (uint)Math.Sqrt((float)mBuffer.getHeight())) * 10;
        }

        //    *
        //	Set the colour of the pixel to paint.
        //	\param colour New colour for painting pixels (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public RandomPixels setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the pixel to paint.
        //	\param red Red value of the pixel colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the pixel colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the pixel colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the pixel colour [0.0, 1.0] \(default 1.0)
        //	
        public RandomPixels setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: RandomPixels& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public RandomPixels setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public RandomPixels setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Set the number of random painted pixels.
        //	\param count Number of pixels to paint (maximum: image height * image weight, default: (Sqrt(image width) + Sqrt(image height)) * 10)
        //	
        public RandomPixels setCount(uint count) {
            mCount = count;
            int area = mBuffer.getWidth() * mBuffer.getHeight();
            if (mCount > area)
                mCount = (uint)(0.9f * (float)area);
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            IntVector2 pt = new IntVector2();
            List<IntVector2> list = new List<IntVector2>();

            RandomNumbers.Seed((int)mSeed);
            int area = mBuffer.getWidth() * mBuffer.getHeight();
            if (mCount == area)
                new RectangleTexture(mBuffer).setColour(mColour).process();
            else {
                while (list.Count != mCount) {
                    pt.x = RandomNumbers.NextNumber() % mBuffer.getWidth();
                    pt.y = RandomNumbers.NextNumber() % mBuffer.getHeight();

                    bool bInList = false;
                    //for (List<IntVector2>.Enumerator iter = list.GetEnumerator(); iter.MoveNext(); iter++)
                    foreach (var iter in list) {
                        if (iter.x == pt.x && iter.y == pt.y) {
                            bInList = true;
                            break;
                        }
                    }
                    if (!bInList) {
                        list.Add(pt);
                        mBuffer.setPixel(pt.x, pt.y, mColour);
                    }
                }
            }

            Utils.log("Modify texture with randompixel filter");
            return mBuffer;
        }
    }

    //*
    //\brief Draw a rectangle.
    //\details Draw a filled rectangle on top of previous content.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //Procedural::RectangleTexture(&bufferSolid).setColour(Ogre::ColourValue::Red).setRectangle(0.25f, 0.25f, 0.75f, 0.75f).process();
    //\endcode
    //\dotfile texture_24.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport RectangleTexture : public TextureProcessing
    public class RectangleTexture : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private int mX1 = new int();
        private int mY1 = new int();
        private int mX2 = new int();
        private int mY2 = new int();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public RectangleTexture(TextureBuffer pBuffer)
            : base(pBuffer, "RectangleTexture") {
            mColour = ColourValue.White;
            mX1 = 0;
            mY1 = 0;
            mX2 = pBuffer.getWidth();
            mY2 = pBuffer.getHeight();
        }

        //    *
        //	Set the fill colour of the rectangle.
        //	\param colour New colour for processing (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public RectangleTexture setColour(ColourValue colour) {
            //
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the fill colour of the rectangle.
        //	\param red Red value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the fill colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the fill colour [0.0, 1.0] \(default 1.0)
        //	
        public RectangleTexture setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: RectangleTexture& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public RectangleTexture setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set absolute x position of top left start point of the rectangle in px
        //	\param x1 New absolute x position of rectangle start (default 0)
        //	
        public RectangleTexture setX1(int x1) {
            mX1 = System.Math.Min(x1, mBuffer.getWidth());
            return this;
        }

        //    *
        //	Set relative x position of top left start point of the rectangle as Real
        //	\param x1 New relative x position of rectangle start [0.0, 1.0] \(default 0.0)
        //	
        public RectangleTexture setX1(float x1) {
            mX1 = (int)((float)mBuffer.getWidth() * System.Math.Min(x1, 1.0f));
            return this;
        }

        //    *
        //	Set absolute y position of top left start point of the rectangle in px
        //	\param y1 New absolute x position of rectangle start (default 0)
        //	
        public RectangleTexture setY1(int y1) {
            mY1 = System.Math.Min(y1, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set relative y position of top left start point of the rectangle as Real
        //	\param y1 New relative y position of rectangle start [0.0, 1.0] \(default 0.0)
        //	
        public RectangleTexture setY1(float y1) {
            mY1 = (int)((float)mBuffer.getHeight() * System.Math.Min(y1, 1.0f));
            return this;
        }

        //    *
        //	Set absolute x position of bottom right end point of the rectangle in px
        //	\param x2 New absolute x position of rectangle end (default: image width)
        //	
        public RectangleTexture setX2(int x2) {
            mX2 = System.Math.Min(x2, mBuffer.getWidth());
            return this;
        }

        //    *
        //	Set relative x position of bottom right end point of the rectangle as Real
        //	\param x2 New relative x position of rectangle end [0.0, 1.0] \(default 1.0)
        //	
        public RectangleTexture setX2(float x2) {
            mX2 = (int)((float)mBuffer.getWidth() * System.Math.Min(x2, 1.0f));
            return this;
        }

        //    *
        //	Set absolute y position of bottom right end point of the rectangle in px
        //	\param y2 New absolute x position of rectangle end (default: image height)
        //	
        public RectangleTexture setY2(int y2) {
            mY2 = (int)System.Math.Min(y2, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set relative y position of bottom right end point of the rectangle as Real
        //	\param y2 New relative y position of rectangle end [0.0, 1.0] \(default 1.0)
        //	
        public RectangleTexture setY2(float y2) {
            mY2 = (int)((float)mBuffer.getHeight() * System.Math.Min(y2, 1.0f));
            return this;
        }

        //    *
        //	Set the full rectangle coordinates.
        //	\param rect Full rectangle description (default: left=0.0, top=0.0, right=1.0, bottom=1.0)
        //	\param relative If this is set to true (default) the rectangle data are relative [0.0, 1.0]; else absolut [px]
        //	
        public RectangleTexture setRectangle(RealRect rect) {
            return setRectangle(rect, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: RectangleTexture& setRectangle(RealRect rect, bool relative = true)
        public RectangleTexture setRectangle(RealRect rect, bool relative) {
            if (relative) {
                mX1 = (int)((float)mBuffer.getWidth() * System.Math.Min(rect.left, 1.0f));
                mY1 = (int)((float)mBuffer.getHeight() * System.Math.Min(rect.top, 1.0f));
                mX2 = (int)((float)mBuffer.getWidth() * System.Math.Min(rect.right, 1.0f));
                mY2 = (int)((float)mBuffer.getHeight() * System.Math.Min(rect.bottom, 1.0f));
            }
            else {
                mX1 = System.Math.Min((int)rect.left, mBuffer.getWidth());
                mY1 = System.Math.Min((int)rect.top, mBuffer.getHeight());
                mX2 = System.Math.Min((int)rect.right, mBuffer.getWidth());
                mY2 = System.Math.Min((int)rect.bottom, mBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates.
        //	\param rect Full absolute rectangle description (default: left=0, top=0, right=image width, bottom=image height)
        //	
        public RectangleTexture setRectangle(Mogre.Rect rect) {
            mX1 = System.Math.Min(rect.left, mBuffer.getWidth());
            mY1 = System.Math.Min(rect.top, mBuffer.getHeight());
            mX2 = System.Math.Min(rect.right, mBuffer.getWidth());
            mY2 = System.Math.Min(rect.bottom, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates.
        //	\param pos1 Vector to top left start point of the rectangle (default: x=0.0, y=0.0)
        //	\param pos2 Vector to bottom right end point of the rectangle (default: x=1.0, y=1.0)
        //	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
        //	
        public RectangleTexture setRectangle(Vector2 pos1, Vector2 pos2) {
            return setRectangle(pos1, pos2, true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: RectangleTexture& setRectangle(Ogre::Vector2 pos1, Ogre::Vector2 pos2, bool relative = true)
        public RectangleTexture setRectangle(Vector2 pos1, Vector2 pos2, bool relative) {
            if (relative) {
                mX1 = (int)((float)mBuffer.getWidth() * System.Math.Min(pos1.x, 1.0f));
                mY1 = (int)((float)mBuffer.getHeight() * System.Math.Min(pos1.y, 1.0f));
                mX2 = (int)((float)mBuffer.getWidth() * System.Math.Min(pos2.x, 1.0f));
                mY2 = (int)((float)mBuffer.getHeight() * System.Math.Min(pos2.y, 1.0f));
            }
            else {
                mX1 = System.Math.Min((int)pos1.x, mBuffer.getWidth());
                mY1 = System.Math.Min((int)pos1.y, mBuffer.getHeight());
                mX2 = System.Math.Min((int)pos2.x, mBuffer.getWidth());
                mY2 = System.Math.Min((int)pos2.y, mBuffer.getHeight());
            }
            return this;
        }

        //    *
        //	Set the full rectangle coordinates.
        //	\param x1 New absolute x position of rectangle start (default 0)
        //	\param y1 New absolute y position of rectangle start (default 0)
        //	\param x2 New absolute x position of rectangle end (default: image width)
        //	\param y2 New absolute y position of rectangle end (default: image height)
        //	
        public RectangleTexture setRectangle(int x1, int y1, int x2, int y2) {
            mX1 = System.Math.Min(x1, mBuffer.getWidth());
            mY1 = System.Math.Min(y1, mBuffer.getHeight());
            mX2 = System.Math.Min(x2, mBuffer.getWidth());
            mY2 = System.Math.Min(y2, mBuffer.getHeight());
            return this;
        }

        //    *
        //	Set the full rectangle coordinates.
        //	\param x1 New relative x position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param y1 New relative y position of rectangle start [0.0, 1.0] \(default 0.0)
        //	\param x2 New relative x position of rectangle end [0.0, 1.0] \(default 1.0)
        //	\param y2 New relative y position of rectangle end [0.0, 1.0] \(default 1.0)
        //	
        public RectangleTexture setRectangle(float x1, float y1, float x2, float y2) {
            mX1 = (int)((float)mBuffer.getWidth() * System.Math.Min(x1, 1.0f));
            mY1 = (int)((float)mBuffer.getHeight() * System.Math.Min(y1, 1.0f));
            mX2 = (int)((float)mBuffer.getWidth() * System.Math.Min(x2, 1.0f));
            mY2 = (int)((float)mBuffer.getHeight() * System.Math.Min(y2, 1.0f));
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int xStart = System.Math.Min(mX1, mX2);
            int yStart = System.Math.Min(mY1, mY2);
            int xEnd = System.Math.Max(mX1, mX2);
            int yEnd = System.Math.Max(mY1, mY2);

            for (int y = yStart; y < yEnd; y++) {
                for (int x = xStart; x < xEnd; x++) {
                    mBuffer.setPixel(x, y, mColour);
                }
            }

            Utils.log("Modify texture with rectangle filter");
            return mBuffer;
        }
    }

    //*
    //\brief Rotate & zoom image.
    //\details Rotate the image and/or zoom on a specific pat of it.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //Procedural::RotationZoom(&bufferGradient).setRotation(0.125f).process();
    //\endcode
    //\dotfile texture_25.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport RotationZoom : public TextureProcessing
    public class RotationZoom : TextureProcessing
    {
        private float mCenterX = 0f;
        private float mCenterY = 0f;
        private float mZoomX = 0f;
        private float mZoomY = 0f;
        private Radian mRotation = new Radian();
        private bool mWrap;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public RotationZoom(TextureBuffer pBuffer)
            : base(pBuffer, "RotationZoom") {
            mCenterX = 0.5f;
            mCenterY = 0.5f;
            mZoomX = 1.0f;
            mZoomY = 1.0f;
            mRotation = 0.0f;
            mWrap = true;
        }

        //    *
        //	Set the relative center position of the rotation on x axis.
        //	\param centerx New relative center of the rotation center [0.0, 1.0] \(default 0.5)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public RotationZoom setCenterX(float centerx) {
            mCenterX = centerx;
            return this;
        }

        //    *
        //	Set the relative center position of the rotation on y axis.
        //	\param centery New relative center of the rotation center [0.0, 1.0] \(default 0.5)
        //	
        public RotationZoom setCenterY(float centery) {
            mCenterY = centery;
            return this;
        }

        //    *
        //	Set the zoom factor in x direction.
        //	\param zoomx New factor for zoom in x direction \(default 1.0)
        //	
        public RotationZoom setZoomX(float zoomx) {
            mZoomX = zoomx;
            return this;
        }

        //    *
        //	Set the zoom factor in y direction.
        //	\param zoomy New factor for zoom in y direction \(default 1.0)
        //	
        public RotationZoom setZoomY(float zoomy) {
            mZoomY = zoomy;
            return this;
        }

        //    *
        //	Set the rotation angle.
        //	\param rotation New rotation angle [0.0, 1.0] \(default 0.0)
        //	
        public RotationZoom setRotation(float rotation) {
            mRotation = (Radian)(rotation * Math.TWO_PI);
            return this;
        }

        //    *
        //	Set the rotation angle.
        //	\param rotation New rotation angle [0.0, Ogre::Math::TWO_PI] rad (default 0.0)
        //	
        public RotationZoom setRotation(Radian rotation) {
            //
            //ORIGINAL LINE: mRotation = rotation;
            mRotation = (rotation);
            return this;
        }

        //    *
        //	Set the rotation angle.
        //	\param rotation New rotation angle [0, 360] degree (default 0)
        //	
        public RotationZoom setRotation(Degree rotation) {
            mRotation = (Radian)(rotation.ValueRadians);
            return this;
        }

        //    *
        //	Set wrap.
        //	\param wrap New wrap value (default true)
        //	
        public RotationZoom setWrap(bool wrap) {
            mWrap = wrap;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            int tw = mBuffer.getWidth();
            int th = mBuffer.getHeight();
            TextureBuffer tmpBuffer = mBuffer.clone();

            float fZoomX = (float)System.Math.Pow(0.5f, mZoomX - 1);
            float fZoomY = (float)System.Math.Pow(0.5f, mZoomY - 1);
            float c = Math.Cos(mRotation.ValueRadians);
            float s = Math.Sin(mRotation.ValueRadians);
            float tw2 = (float)tw / 2.0f;
            float th2 = (float)th / 2.0f;
            float ys = s * -th2;
            float yc = c * -th2;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                float u = (((c * -tw2) - ys) * fZoomX) + (mCenterX * (float)tw);
                float v = (((s * -tw2) + yc) * fZoomY) + (mCenterY * (float)th);
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float uf = (u >= 0) ? (u - (int)u) : 1 + (u - (int)u);
                    float vf = (v >= 0) ? (v - (int)v) : 1 + (v - (int)v);
                    int ut = (u >= 0) ? (int)u : (int)u - 1;
                    int vt = (v >= 0) ? (int)v : (int)v - 1;

                    ColourValue texel = mBuffer.getPixel(vt % th, ut % tw) * (1.0f - uf) * (1.0f - vf);
                    texel += mBuffer.getPixel(vt % th, (ut + 1) % tw) * uf * (1.0f - vf);
                    texel += mBuffer.getPixel((vt + 1) % th, ut % tw) * (1.0f - uf) * vf;
                    texel += mBuffer.getPixel((vt + 1) % th, (ut + 1) % tw) * uf * vf;
                    tmpBuffer.setPixel(x, y, texel);
                    u += c * fZoomX;
                    v += s * fZoomY;
                }
                ys += s;
                yc += c;
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();

            Utils.log("Modify texture with rotationzoom filter");
            return mBuffer;
        }
    }

    //*
    //\brief %Colours image segments based on threshold.
    //\details Takes segments of base image based on a threshold, and colors each segment based on .
    //
    //Example:
    //\code{.cpp}
    //// Image colour
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //
    //// Image structure
    //Procedural::TextureBuffer bufferCell(256);
    //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
    //
    //// Filter
    //Procedural::Segment(&bufferCell).setColourSource(&bufferGradient).process();
    //\endcode
    //\dotfile texture_26.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Segment : public TextureProcessing
    public class Segment : TextureProcessing
    {
        private TextureBuffer mColourSource;
        private byte mThreshold = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Segment(TextureBuffer pBuffer)
            : base(pBuffer, "Segment") {
            mThreshold = 128;
            mColourSource = null;
        }

        //    *
        //	Set parameter image for colour source.
        //	\param coloursource Pointer to an input image (default NULL)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Segment setColourSource(TextureBuffer coloursource) {
            mColourSource = coloursource;
            return this;
        }

        //    *
        //	Set threshold value.
        //	\param threshold New threshold value [0, 255] (default 128)
        //	
        public Segment setThreshold(byte threshold) {
            mThreshold = threshold;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            if (mColourSource == null)
                return mBuffer;

            int w = mBuffer.getWidth();
            int h = mBuffer.getHeight();

            if (mColourSource.getWidth() < w || mColourSource.getHeight() < h)
                return mBuffer;

            byte[] pCoverage = new byte[(int)w * h];
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
            memset<byte>(pCoverage, 0, (uint)(w * h));
            IntVector2[] pStack = new IntVector2[(int)w * h * 4];
            TextureBuffer tmpBuffer = mBuffer.clone();

            int stackPtr = 0;
            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    ColourValue pixelA = mBuffer.getPixel(x, y);
                    ColourValue pixelB = mColourSource.getPixel(x, y);

                    if ((pixelA.r + pixelA.g + pixelA.b) * 255.0f > (float)mThreshold * 3.0f) {
                        pStack[stackPtr].x = x;
                        pStack[stackPtr].y = y;
                        stackPtr++;
                    }

                    while (stackPtr > 0) {
                        IntVector2 current = pStack[--stackPtr];
                        if (pCoverage[current.x + current.y * w] != 0)
                            continue;

                        pixelA = mBuffer.getPixel(current.x, current.y);
                        if ((pixelA.r + pixelA.g + pixelA.b) * 255.0f > (float)mThreshold * 3.0f) {
                            pStack[stackPtr].x = current.x;
                            pStack[stackPtr].y = (current.y + h - 1) % h;
                            stackPtr++;
                            pStack[stackPtr].x = current.x;
                            pStack[stackPtr].y = (current.y + 1) % h;
                            stackPtr++;
                            pStack[stackPtr].x = (current.x + 1) % w;
                            pStack[stackPtr].y = current.y;
                            stackPtr++;
                            pStack[stackPtr].x = (current.x + w - 1) % w;
                            pStack[stackPtr].y = current.y;
                            stackPtr++;

                            pCoverage[current.x + current.y * w] = 1;
                            tmpBuffer.setPixel(current.x, current.y, pixelB);
                        }
                    }

                    if (pCoverage[x + y * w] == 0)
                        tmpBuffer.setPixel(x, y, ColourValue.Black);
                }
            }

            mBuffer.setData(tmpBuffer);
            tmpBuffer.Dispose();
            pCoverage = null;
            //pStack.Dispose();
            pStack = null;

            Utils.log("Modify texture with segment filter");
            return mBuffer;
        }
    }

    //*
    //\brief Increase sharpness on input image.
    //\details Sharp the input image by a specified algorithm.
    //
    //Examples:
    //\b SHARP_BASIC
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Sharpen(&bufferImage).setType(Procedural::Sharpen::SHARP_BASIC).process();
    //\endcode
    //\dotfile texture_27a.gv
    //
    //\b SHARP_GAUSSIAN
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //Procedural::Sharpen(&bufferImage).setType(Procedural::Sharpen::SHARP_GAUSSIAN).process();
    //\endcode
    //\dotfile texture_27b.gv
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Sharpen : public TextureProcessing
    public class Sharpen : TextureProcessing
    {
        //! List of algorithms to blur
        public enum SHARP_TYPE : int
        {
            SHARP_BASIC, //!< Use simplified block filter to sharp
            SHARP_GAUSSIAN //!< Use gaussian filter to sharp
        }

        private byte mSize = 0;
        private byte mSigma = 0;
        private SHARP_TYPE mType;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to modify the image.
        //	
        public Sharpen(TextureBuffer pBuffer)
            : base(pBuffer, "Sharpen") {
            mSize = 5;
            mSigma = 92;
            mType = (int)SHARP_TYPE.SHARP_BASIC;
        }

        //    *
        //	Set the gaussian block size.
        //	\param size New block size for gaussian sharp filter [3, 255] (default 5)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Sharpen setSize(byte size) {
            mSize = size;
            if (mSize < 3)
                mSize = 3;
            if ((mSize % 2) == 0)
                mSize++;
            return this;
        }

        //    *
        //	Set sigma constant for gaussian filter.
        //	\param sigma New sigma constant for gaussian sharp filter [0, 255] (default 92)
        //	
        public Sharpen setSigma(byte sigma) {
            mSigma = sigma;
            return this;
        }

        //    *
        //	Set the algorithm to sharp.
        //	\param type New algorithm to sharp (default SHARP_BASIC)
        //	
        public Sharpen setType(SHARP_TYPE type) {
            mType = type;
            return this;
        }

        //    *
        //	Run image manipulation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            Convolution filter = new Convolution(mBuffer);
            switch (mType) {
                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case SHARP_TYPE.SHARP_BASIC:
                    filter.setKernel(new Matrix3(0.0f, -1.0f, 0.0f, -1.0f, 5.0f, -1.0f, 0.0f, -1.0f, 0.0f)).calculateDivisor();
                    break;

                case SHARP_TYPE.SHARP_GAUSSIAN:
                    float fSigma = 0.5f + ((5.0f - 0.5f) / 255.0f) * (float)mSigma;
                    int r = (int)mSize / 2;
                    double min = System.Math.Exp((float)(2 * r * r) / (-2.0f * fSigma * fSigma)) / (Math.TWO_PI * fSigma * fSigma);
                    int[] kernel = new int[mSize * mSize];
                    int sum = 0;
                    int y = -r;
                    int x = -r;
                    for (int i = 0; i < mSize; i++) {
                        for (int j = 0; j < mSize; j++) {
                            kernel[i * mSize + j] = (int)((System.Math.Exp((float)(x * x + y * y) / (-2.0f * fSigma * fSigma)) / (Math.TWO_PI * fSigma * fSigma)) / min);
                            sum += kernel[i * mSize + j];
                            x++;
                        }
                        y++;
                    }
                    int c = (int)mSize >> 1;
                    int divisor = 0;
                    for (int i = 0; i < mSize; i++) {
                        for (int j = 0; j < mSize; j++) {
                            if ((i == c) && (j == c))
                                kernel[i * mSize + j] = 2 * sum - kernel[i * mSize + j];
                            else
                                kernel[i * mSize + j] = -kernel[i * mSize + j];

                            divisor += kernel[i * mSize + j];
                        }
                    }
                    filter.setKernel(mSize, kernel).setDivisor((float)divisor);
                    kernel = null;
                    break;
            }
            filter.setIncludeAlphaChannel(true).process();

            Utils.log("Modify texture with sharpen filter : " + (mType).ToString());
            return mBuffer;
        }


#if PROCEDURAL_USE_FREETYPE
//*
//\brief Draw text on Texture.
//\details Draw a given text on texture.
//
//Example:
//\code{.cpp}
//// Extract and save font file from Ogre resources (SdkTrays.zip)
//Ogre::DataStreamPtr stream = Ogre::ResourceGroupManager::getSingleton().openResource("cuckoo.ttf", "Essential");
//std::ofstream fontFile("cuckoo.ttf", std::ios::out | std::ios::binary);
//char block[1024];
//while(!stream->eof())
//{
//	int len = stream->read(block, 1024);
//	fontFile.write(block, len);
//	if(len < 1024) break;
//}
//fontFile.close();
//
//// Use font file to write a text on a texture
//Procedural::TextureBuffer bufferCell(256);
//Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
//Procedural::TextTexture(&bufferCell).setFont("cuckoo.ttf", 30).setColour(Ogre::ColourValue::Red).setPosition((int)20, (int)20).setText("OGRE").process();
//Procedural::TextTexture(&bufferCell).setFont("cuckoo.ttf", 20).setColour(Ogre::ColourValue::Green).setPosition((int)10, (int)60).setText("Procedural").process();
//\endcode
//\dotfile texture_34.gv
//
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport TextTexture : public TextureProcessing
public class TextTexture : TextureProcessing
{
	private string mText = "";
	private string mFontName ="";
	private byte mFontSize = 0;
	private ColourValue mColour = new ColourValue();
	private int mX = new int();
	private int mY = new int();

//    *
//	Default constructor.
//	\param pBuffer Image buffer where to modify the image.
//	
	public TextTexture(TextureBuffer pBuffer) : base(pBuffer, "TextTexture")
	{
		mText = "OgreProcedural";
		mFontSize = 12;
		mColour = ColourValue.Black;
		mX = pBuffer.getWidth() / 2;
		mY = pBuffer.getHeight() / 2;
	}

//    *
//	Set the text content.
//	\param text New text for processing (default "OgreProcedural")
//	
	public TextTexture setText(string text)
	{
		mText = text;
		return this;
	}

//    *
//	Set absolute x position where to start painting the text in px
//	\param x New absolute x position of text start (default 1/2 * image width)
//	
	public TextTexture setPositionX(int x)
	{
		mX = System.Math.Min(x, mBuffer.getWidth() - 1);
		return this;
	}

//    *
//	Set relative x position where to start painting the text as Real
//	\param x New relative x position of text start [0.0, 1.0] \(default 0.5)
//	
	public TextTexture setPositionX(float x)
	{
		mX = System.Math.Min((int)(x * (float)mBuffer.getWidth()), mBuffer.getWidth() - 1);
		return this;
	}

//    *
//	Set absolute y position where to start painting the text in px
//	\param y New absolute y position of text start (default 1/2 * image width)
//	
	public TextTexture setPositionY(int y)
	{
		mY = System.Math.Min(y, mBuffer.getHeight() - 1);
		return this;
	}

//    *
//	Set relative y position where to start painting the text as Real
//	\param y New relative y position of text start [0.0, 1.0] \(default 0.5)
//	
	public TextTexture setPositionY(float y)
	{
		mY = System.Math.Min((int)(y * (float)mBuffer.getHeight()), mBuffer.getHeight() - 1);
		return this;
	}

//    *
//	Set the position of text start point.
//	\param pos Vector to the start point where to draw the text (default: x=0.5, y=0.5)
//	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
//	
	public TextTexture setPosition(Vector2 pos)
	{
		return setPosition(pos, true);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: TextTexture& setPosition(Ogre::Vector2 pos, bool relative = true)
	public TextTexture setPosition(Vector2 pos, bool relative)
	{
		setPosition(pos.x, pos.y, relative);
		return this;
	}

//    *
//	Set the position of text start point.
//	\param x New absolute x position of text start (default 1/2 * image width)
//	\param y New absolute y position of text start (default 1/2 * image width)
//	
	public TextTexture setPosition(int x, int y)
	{
		setPositionX(x);
		setPositionY(y);
		return this;
	}

//    *
//	Set the position of text start point.
//	\param x New relative x position of text start [0.0, 1.0] \(default 0.5)
//	\param y New relative y position of text start [0.0, 1.0] \(default 0.5)
//	\param relative If this is set to true (default) the vector data are relative [0.0, 1.0]; else absolut [px]
//	
	public TextTexture setPosition(float x, float y)
	{
		return setPosition(x, y, true);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: TextTexture& setPosition(Ogre::float x, Ogre::float y, bool relative = true)
	public TextTexture setPosition(float x, float y, bool relative)
	{
		if (relative)
		{
			setPositionX(x);
			setPositionY(y);
		}
		else
		{
			setPositionX((int)x);
			setPositionY((int)y);
		}
		return this;
	}

//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if PROCEDURAL_PLATFORM == PROCEDURAL_PLATFORM_WIN32
//    *
//	Set the position of text start point.
//	\param pos Absolute center point of the text (default: x=1/2 * image width, y=1/2 * image width)
//	
	public TextTexture setPosition(Vector2 pos)
	{
		setPosition((int)pos.x, (int)pos.y);
		return this;
	}
#endif

//    *
//	Set the font for the text.
//	\param fontName Filenpath of a font or name of font (only on windows desktops)
//	\param fontSize Size of font [px] (default 12)
//	\todo Add search for font names on non windows systems.
//	
	public TextTexture setFont(string fontName, byte fontSize)
	{
		if (string.IsNullOrEmpty(fontName) || fontSize < 4)
			return this;
		mFontName = fontName;
		mFontSize = fontSize;
		return this;
	}

//    *
//	Set the drawing colour of the text.
//	\param colour New colour for processing (default Ogre::ColourValue::Black)
//	
	public TextTexture setColour(ColourValue colour)
	{
//
//ORIGINAL LINE: mColour = colour;
		mColour=(colour);
		return this;
	}

//    *
//	Set the drawing colour of the text.
//	\param red Red value of the fill colour [0, 255] (default 0)
//	\param green Green value of the fill colour [0, 255] (default 0)
//	\param blue Blue value of the fill colour [0, 255] (default 0)
//	\param alpha %Alpha value of the fill colour [0, 255] (default 255)
//	
	public TextTexture setColour(byte red, byte green, byte blue)
	{
		return setColour(red, green, blue, 255);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: TextTexture& setColour(Ogre::byte red, Ogre::byte green, Ogre::byte blue, Ogre::byte alpha = 255)
	public TextTexture setColour(byte red, byte green, byte blue, byte alpha)
	{
		mColour =new ColourValue((float)red / 255.0f, (float)green / 255.0f, (float)blue / 255.0f, (float)alpha / 255.0f);
		return this;
	}

//    *
//	Set the drawing colour of the text.
//	\param red Red value of the fill colour [0.0, 1.0] \(default 0.0)
//	\param green Green value of the fill colour [0.0, 1.0] \(default 0.0)
//	\param blue Blue value of the fill colour [0.0, 1.0] \(default 0.0)
//	\param alpha %Alpha value of the fill colour [0.0, 1.0] \(default 1.0)
//	
	public TextTexture setColour(float red, float green, float blue)
	{
		return setColour(red, green, blue, 1.0f);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: TextTexture& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
	public TextTexture setColour(float red, float green, float blue, float alpha)
	{
		mColour =new ColourValue(red, green, blue, alpha);
		return this;
	}

//    *
//	Run image manipulation
//	\return Pointer to image buffer which has been set in the constructor.
//	
	public override TextureBuffer process()
	{
		FT_Library ftlib = new FT_Library();
		FT_Face face = new FT_Face();
		FT_GlyphSlot slot = new FT_GlyphSlot();
	
		FT_Error error = FT_Init_FreeType(ftlib);
		if (error == 0)
		{
			error = FT_New_Face(ftlib, getFontFileByName().c_str(), 0, face);
			if (error == FT_Err_Unknown_File_Format)
				Utils.log("FreeType ERROR: FT_Err_Unknown_File_Format");
			else if (error != null)
				Utils.log("FreeType ERROR: FT_New_Face - " + StringConverter.toString(error));
			else
			{
				FT_Set_Pixel_Sizes(face, 0, mFontSize);
	
				int px = (int)mX;
				int py = (int)mY;
				slot = face.glyph;
	
				for (int n = 0; n < mText.length(); n++)
				{
					error = FT_Load_Char(face, mText[n], FT_LOAD_RENDER);
					if (error != null)
						continue;
	
					for (int i = 0; i < (int)slot.bitmap.width; i++)
					{
						for (int j = 0; j < (int)slot.bitmap.rows; j++)
						{
							if (slot.bitmap.buffer[j * slot.bitmap.width + i] > 127)
								mBuffer.setPixel(px + i, py + j, mColour);
						}
					}
	
					px += slot.advance.x >> 6;
					py += slot.advance.y >> 6;
				}
				FT_Done_Face(face);
				Utils.log("Modify texture with text processing : " + mText);
			}
			FT_Done_FreeType(ftlib);
		}
		else
			Utils.log("FreeType ERROR: FT_Init_FreeType");
		return mBuffer;
	}

	private string getFontFileByName()
	{
		string ff = "";
		string tmp = "";
	
	//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if PROCEDURAL_PLATFORM == PROCEDURAL_PLATFORM_WIN32
		string windows = new string(new char[MAX_PATH]);
		GetWindowsDirectory(windows, MAX_PATH);
	
		bool result = getFontFile(mFontName, ref tmp, ref ff);
		if (result == null)
			return mFontName;
		if (!(ff[0] == '\\' && ff[1] == '\\') && !(ff[1] == ':' && ff[2] == '\\'))
			return  (windows) + "\\fonts\\" + ff;
		else
			return ff;
#else
		return mFontName;
#endif
	}

//C++ TO C# CONVERTER TODO TASK: C# does not allow setting or comparing #define constants:
#if PROCEDURAL_PLATFORM == PROCEDURAL_PLATFORM_WIN32
	private bool getFontFile(string fontName, ref String displayName, ref String filePath)
	{
		if (string.IsNullOrEmpty( fontName))
			return false;
	
		if ((fontName[0] == '\\' && fontName[1] == '\\') || (fontName[1] == ':' && fontName[2] == '\\'))
		{
			displayName = fontName;
			filePath = fontName;
			return true;
		}
	
		string name = new string(new char[2 * MAX_PATH]);
		string data = new string(new char[2 * MAX_PATH]);
		filePath.empty();
		bool retVal = false;
	
		IntPtr hkFont;
		if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", 0, KEY_READ, hkFont) == ERROR_SUCCESS)
		{
			string cname = new string(new char[MAX_PATH]);
			uint icname = 0;
			uint isubkeys = 0;
			uint imaxsubkey = 0;
			uint imaxclass = 0;
			uint ivalues = 0;
			uint imaxvalues = 0;
			uint imaxnamevalues = 0;
			uint isecurity = 0;
			FILETIME dtlast = new FILETIME();
	
			uint retCode = RegQueryInfoKey(hkFont, cname, icname, null, isubkeys, imaxsubkey, imaxclass, ivalues, imaxnamevalues, imaxvalues, isecurity, dtlast);
			if (ivalues != 0)
			{
				for (uint i = 0; i < ivalues; i++)
				{
					retCode = ERROR_SUCCESS;
					uint nsize = MAX_PATH - 1;
					uint dsize = MAX_PATH - 1;
					name = null;
					data = null;
					retCode = RegEnumValue(hkFont, i, name, nsize, null, null, (byte)data, dsize);
					if (retCode == ERROR_SUCCESS)
						if (strnicmp(name, fontName.c_str(), System.Math.Min(name.Length, fontName.length())) == 0)
						{
							displayName = name;
							filePath = data;
							retVal = true;
							break;
						}
				}
			}
		}
		RegCloseKey(hkFont);
		return retVal;
	}
#endif
}
#endif // PROCEDURAL_USE_FREETYPE

        //*
        //\brief Simple threshold filter.
        //\details Change luminance curve around given value.
        //
        //Example:
        //\code{.cpp}
        //Procedural::TextureBuffer bufferCell(256);
        //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
        //Procedural::Threshold(&bufferCell).process();
        //\endcode
        //\dotfile texture_28.gv
        //
        //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
        //ORIGINAL LINE: class _ProceduralExport Threshold : public TextureProcessing
        public class Threshold : TextureProcessing
        {
            //! Selection what to do with image parts below/above the threshould value.
            public enum THRESHOLD_MODE : int
            {
                MODE_EXPAND_DOWNWARDS, //!< Set pixel below threshould value to black
                MODE_EXPAND_UPWARDS, //!< Set pixel above threshould value to white
                MODE_COMPRESS_BELOW, //!< Set pixel below threshould value to an avarage value of all pixel below threshould value
                MODE_COMPRESS_ABOVE //!< Set pixel above threshould value to an avarage value of all pixel above threshould value
            }

            private byte mThreshold = 0;
            private byte mRatio = 0;
            private THRESHOLD_MODE mMode;

            //    *
            //	Default constructor.
            //	\param pBuffer Image buffer where to modify the image.
            //	
            public Threshold(TextureBuffer pBuffer)
                : base(pBuffer, "Threshold") {
                mThreshold = 128;
                mRatio = 128;
                mMode = THRESHOLD_MODE.MODE_EXPAND_DOWNWARDS;
            }

            //    *
            //	Set threshold value.
            //	\param threshold New threshold value [0, 255] (default 128)
            //	

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            public Threshold setThreshold(byte threshold) {
                mThreshold = threshold;
                return this;
            }

            //    *
            //	Set threshold ratio which affects the painting mode.
            //	\param ratio New painting factor [0, 255] (default 128)
            //	
            public Threshold setRatio(byte ratio) {
                mRatio = ratio;
                return this;
            }

            //    *
            //	Set threshold mode.
            //	\param mode New mode what to do with pixels below/above threshold value (default MODE_EXPAND_DOWNWARDS)
            //	
            public Threshold setMode(THRESHOLD_MODE mode) {
                mMode = mode;
                return this;
            }

            //    *
            //	Run image manipulation
            //	\return Pointer to image buffer which has been set in the constructor.
            //	
            public override TextureBuffer process() {
                int t;
                int w = mBuffer.getWidth();
                int h = mBuffer.getHeight();
                float ratio = (mMode == THRESHOLD_MODE.MODE_EXPAND_DOWNWARDS || mMode == THRESHOLD_MODE.MODE_EXPAND_UPWARDS) ? 1 + mRatio * 0.1f : 1 + mRatio * 0.05f;

                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        byte r = mBuffer.getPixelRedByte(x, y);
                        byte g = mBuffer.getPixelGreenByte(x, y);
                        byte b = mBuffer.getPixelBlueByte(x, y);
                        byte a = mBuffer.getPixelAlphaByte(x, y);

                        if (mMode == THRESHOLD_MODE.MODE_EXPAND_DOWNWARDS) {
                            if (r < mThreshold) {
                                t = mThreshold - (int)((mThreshold - r) * ratio);
                                r = (t < 0) ? (byte)0 : (byte)t;
                            }
                            if (g < mThreshold) {
                                t = mThreshold - (int)((mThreshold - g) * ratio);
                                g = (t < 0) ? (byte)0 : (byte)t;
                            }
                            if (b < mThreshold) {
                                t = mThreshold - (int)((mThreshold - b) * ratio);
                                b = (t < 0) ? (byte)0 : (byte)t;
                            }
                        }
                        else if (mMode == THRESHOLD_MODE.MODE_EXPAND_UPWARDS) {
                            if (r > mThreshold) {
                                t = (int)((r - mThreshold) * ratio) - mThreshold;
                                r = (t > 255) ? (byte)255 : (byte)t;
                            }
                            if (g > mThreshold) {
                                t = (int)((g - mThreshold) * ratio) - mThreshold;
                                g = (t > 255) ? (byte)255 : (byte)t;
                            }
                            if (b > mThreshold) {
                                t = (int)((b - mThreshold) * ratio) - mThreshold;
                                b = (t > 255) ? (byte)255 : (byte)t;
                            }
                        }
                        else if (mMode == THRESHOLD_MODE.MODE_COMPRESS_BELOW) {
                            if (r < mThreshold) {
                                t = mThreshold - (int)((mThreshold - r) / ratio);
                                r = (t < 0) ? (byte)0 : (byte)t;
                            }
                            if (g < mThreshold) {
                                t = mThreshold - (int)((mThreshold - g) / ratio);
                                g = (t < 0) ? (byte)0 : (byte)t;
                            }
                            if (b < mThreshold) {
                                t = mThreshold - (int)((mThreshold - b) / ratio);
                                b = (t < 0) ? (byte)0 : (byte)t;
                            }
                        }
                        else if (mMode == THRESHOLD_MODE.MODE_COMPRESS_ABOVE) {
                            if (r > mThreshold) {
                                t = (int)((r - mThreshold) / ratio) - mThreshold;
                                r = (t > 255) ? (byte)255 : (byte)t;
                            }
                            if (g > mThreshold) {
                                t = (int)((g - mThreshold) / ratio) - mThreshold;
                                g = (t > 255) ? (byte)255 : (byte)t;
                            }
                            if (b > mThreshold) {
                                t = (int)((b - mThreshold) / ratio) - mThreshold;
                                b = (t > 255) ? (byte)255 : (byte)t;
                            }
                        }
                        mBuffer.setPixel(x, y, r, g, b, a);
                    }
                }

                Utils.log("Modify texture with threshold filter");
                return mBuffer;
            }
        }

        //*
        //\brief Twist some fragment of input image.
        //\details Twist parts of the input image arround a given point within a given radius.
        //
        //Example:
        //\code{.cpp}
        //Procedural::TextureBuffer bufferCell(256);
        //Procedural::Cell(&bufferCell).setDensity(4).setRegularity(234).process();
        //Procedural::Vortex(&bufferCell).process();
        //\endcode
        //\dotfile texture_29.gv
        //
        //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
        //ORIGINAL LINE: class _ProceduralExport Vortex : public TextureProcessing
        public class Vortex : TextureProcessing
        {
            private float mCenterX = 0f;
            private float mCenterY = 0f;
            private float mRadiusX = 0f;
            private float mRadiusY = 0f;
            private Radian mTwist = new Radian();

            //    *
            //	Default constructor.
            //	\param pBuffer Image buffer where to modify the image.
            //	
            public Vortex(TextureBuffer pBuffer)
                : base(pBuffer, "Vortex") {
                mCenterX = 0.5f;
                mCenterY = 0.5f;
                mRadiusX = 0.5f;
                mRadiusY = 0.5f;
                mTwist = Math.HALF_PI;
            }

            //    *
            //	Set the relative position of the twist center on x axis.
            //	\param centerx New relative x position of the twist center [0.0, 1.0] \(default 0.5)
            //	

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            public Vortex setCenterX(float centerx) {
                mCenterX = centerx;
                return this;
            }

            //    *
            //	Set the relative position of the twist center on y axis.
            //	\param centery New relative y position of the twist center [0.0, 1.0] \(default 0.5)
            //	
            public Vortex setCenterY(float centery) {
                mCenterY = centery;
                return this;
            }

            //    *
            //	Set the relative radius of the twist area on x axis.
            //	\param radiusx New relative x radius of the twist area [0.0, 1.0] \(default 0.5)
            //	
            public Vortex setRadiusX(float radiusx) {
                mRadiusX = radiusx;
                return this;
            }

            //    *
            //	Set the relative radius of the twist area on y axis.
            //	\param radiusy New relative y radius of the twist area [0.0, 1.0] \(default 0.5)
            //	
            public Vortex setRadiusY(float radiusy) {
                mRadiusY = radiusy;
                return this;
            }

            //    *
            //	Set the twist angle.
            //	\param twist New twist angle for deformation [0.0, 1.0] \(default 0.25)
            //	
            public Vortex setTwist(float twist) {
                mTwist = new Radian(twist * Math.TWO_PI);
                return this;
            }

            //    *
            //	Set the twist angle.
            //	\param twist New twist angle for deformation [0.0, Ogre::Math::TWO_PI] rad (default Ogre::Math::HALF_PI)
            //	
            public Vortex setTwist(Radian twist) {
                //
                //ORIGINAL LINE: mTwist = twist;
                mTwist = (twist);
                return this;
            }

            //    *
            //	Set the twist angle.
            //	\param twist New twist angle for deformation [0, 360] degree (default 90)
            //	
            public Vortex setTwist(Degree twist) {
                mTwist = twist;
                return this;
            }

            //    *
            //	Run image manipulation
            //	\return Pointer to image buffer which has been set in the constructor.
            //	
            public override TextureBuffer process() {
                int w = (int)mBuffer.getWidth();
                int h = (int)mBuffer.getHeight();
                int dwCenterX = (int)(mCenterX * (float)w);
                int dwCenterY = (int)(mCenterY * (float)h);
                int dwRadiusX = (int)(mRadiusX * (float)w);
                int dwRadiusY = (int)(mRadiusY * (float)h);
                float f1_RadiusX = 1.0f / (float)dwRadiusX;
                float f1_RadiusY = 1.0f / (float)dwRadiusY;
                TextureBuffer tmpBuffer = mBuffer.clone();

                for (int y = 0; y < h; y++) {
                    float dy = (float)(y - dwCenterY) * f1_RadiusY;

                    for (int x = 0; x < w; x++) {
                        float dx = (float)(x - dwCenterX) * f1_RadiusX;
                        float d = Math.Sqrt(dx * dx + dy * dy);

                        if (d > 1.0f)
                            tmpBuffer.setPixel(x, y, mBuffer.getPixel(x, y));
                        else {
                            d = Math.Cos(d * Math.HALF_PI - Math.HALF_PI);
                            d = 1.0f - d;
                            float nx = (float)(x - dwCenterX);
                            float ny = (float)(y - dwCenterY);
                            float rad = mTwist.ValueRadians * d;

                            float bx = nx;
                            nx = bx * Math.Cos(rad) - ny * Math.Sin(rad) + dwCenterX;
                            ny = bx * Math.Sin(rad) + ny * Math.Cos(rad) + dwCenterY;

                            if (nx >= w)
                                nx = nx - w;
                            if (ny >= h)
                                ny = ny - h;
                            if (nx < 0)
                                nx = w + nx;
                            if (ny < 0)
                                ny = h + ny;

                            int ix = (int)nx;
                            int iy = (int)ny;

                            float fracX = nx - ix;
                            float fracY = ny - iy;

                            float ul = (1.0f - fracX) * (1.0f - fracY);
                            float ll = (1.0f - fracX) * fracY;
                            float ur = fracX * (1.0f - fracY);
                            float lr = fracX * fracY;

                            int wrapx = (ix + 1) % w;
                            int wrapy = (iy + 1) % h;
                            ColourValue pixelUL = mBuffer.getPixel(ix, iy);
                            ColourValue pixelUR = mBuffer.getPixel(wrapx, iy);
                            ColourValue pixelLL = mBuffer.getPixel(ix, wrapy);
                            ColourValue pixelLR = mBuffer.getPixel(wrapx, wrapy);

                            tmpBuffer.setPixel(x, y, (byte)(ul * pixelUL.r * 255.0f + ll * pixelLL.r * 255.0f + ur * pixelUR.r * 255.0f + lr * pixelLR.r * 255.0f), (byte)(ul * pixelUL.g * 255.0f + ll * pixelLL.g * 255.0f + ur * pixelUR.g * 255.0f + lr * pixelLR.g * 255.0f), (byte)(ul * pixelUL.b * 255.0f + ll * pixelLL.b * 255.0f + ur * pixelUR.b * 255.0f + lr * pixelLR.b * 255.0f), (byte)(ul * pixelUL.a * 255.0f + ll * pixelLL.a * 255.0f + ur * pixelUR.a * 255.0f + lr * pixelLR.a * 255.0f));
                        }
                    }
                }

                mBuffer.setData(tmpBuffer);
                tmpBuffer.Dispose();

                Utils.log("Modify texture with vortex filter : " + (mTwist.ValueDegrees).ToString());
                return mBuffer;
            }
        }
        //* @} 
    }



}