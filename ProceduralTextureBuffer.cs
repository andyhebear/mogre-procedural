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

#define  OGRE_ENDIAN_LITTLE
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using System.Runtime.InteropServices;
    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class TextureBuffer;
    //! Type for a TextureBuffer pointer


    //*
    //\brief class to store image data while processing
    //\details Create a TextureBuffer object and move it to all classes inherited from TextureProcessing
    //\todo check byte order for image generation (OGRE_ENDIAN, OGRE_ENDIAN_LITTLE, OGRE_ENDIAN_BIG), see <a href="http://www.ogre3d.org/forums/viewtopic.php?f=2&t=72832" target="_blank">Ogre forum</a> for details.
    //\todo Increase speed of reading and writeing pixel values.
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TextureBuffer
    public class TextureBuffer
    {
        //#if OGRE_ENDIAN == OGRE_ENDIAN_LITTLE
#if  OGRE_ENDIAN_LITTLE
        public const int PROCEDURAL_RED = 3;
        public const int PROCEDURAL_GREEN = 2;
        public const int PROCEDURAL_BLUE = 1;
        public const int PROCEDURAL_ALPHA = 0;
#else
    public const int PROCEDURAL_RED =0;
    public const int PROCEDURAL_GREEN =1;
    public const int PROCEDURAL_BLUE =2;
    public const int PROCEDURAL_ALPHA =3;
#endif

        private byte[] mPixels;
        private uint mWidth = 0;
        private uint mHeight = 0;


        //    *
        //	\brief Set colour of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param colour New colour of pixel
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setPixel(int x, int y, ColourValue colour) {
            setPixel(x, y, colour.r, colour.g, colour.b, colour.a);
        }

        //    *
        //	\brief Set colour of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param red New red value of pixel colour [0, 255]
        //	\param green New green value of pixel colour [0, 255]
        //	\param blue New blue value of pixel colour [0, 255]
        //	\param alpha New alpha value of pixel colour [0, 255]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setPixel(int x, int y, byte red, byte green, byte blue) {
            setPixel(x, y, red, green, blue, 255);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: void setPixel(int x, int y, Ogre::byte red, Ogre::byte green, Ogre::byte blue, Ogre::byte alpha = 255)
        public void setPixel(int x, int y, byte red, byte green, byte blue, byte alpha) {
            setRed(x, y, red);
            setGreen(x, y, green);
            setBlue(x, y, blue);
            setAlpha(x, y, alpha);
        }

        //    *
        //	\brief Set colour of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param red New red value of pixel colour
        //	\param green New green value of pixel colour [0.0, 1.0]
        //	\param blue New blue value of pixel colour [0.0, 1.0]
        //	\param alpha New alpha value of pixel colour [0.0, 1.0]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	\exception Ogre::InvalidParametersException Colour value must be between 0 and 1!
        //	
        public void setPixel(int x, int y, float red, float green, float blue) {
            setPixel(x, y, red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: void setPixel(int x, int y, Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public void setPixel(int x, int y, float red, float green, float blue, float alpha) {
            setRed(x, y, red);
            setGreen(x, y, green);
            setBlue(x, y, blue);
            setAlpha(x, y, alpha);
        }

        //    *
        //	\brief Set red colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param red New red value of pixel colour [0, 255]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setRed(int x, int y, byte red) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setRed(int, int, Ogre::uchar)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_RED] = red;
        }

        //    *
        //	\brief Set green colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param green New green value of pixel colour [0, 255]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setGreen(int x, int y, byte green) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setGreen(int, int, Ogre::uchar)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_GREEN] = green;
        }

        //    *
        //	\brief Set blue colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param blue New blue value of pixel colour [0, 255]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setBlue(int x, int y, byte blue) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setBlue(int, int, Ogre::uchar)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_BLUE] = blue;
        }

        //    *
        //	\brief Set alpha colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param alpha New alpha value of pixel colour [0, 255]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setAlpha(int x, int y, byte alpha) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setAlpha(int, int, Ogre::uchar)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_ALPHA] = alpha;
        }

        //    *
        //	\brief Set red colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param red New red value of pixel colour [0.0, 1.0]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	\exception Ogre::InvalidParametersException Colour value must be between 0 and 1!
        //	
        public void setRed(int x, int y, float red) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setRed(int, int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_RED] = (byte)(System.Math.Min(System.Math.Max(red * 255.0f, 0.0f), 255.0f));
        }

        //    *
        //	\brief Set green colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param green New green value of pixel colour [0.0, 1.0]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setGreen(int x, int y, float green) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setGreen(int, int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_GREEN] = (byte)(System.Math.Min(System.Math.Max(green * 255.0f, 0.0f), 255.0f));
        }

        //    *
        //	\brief Set blue colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param blue New blue value of pixel colour [0.0, 1.0]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setBlue(int x, int y, float blue) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setBlue(int, int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_BLUE] = (byte)(System.Math.Min(System.Math.Max(blue * 255.0f, 0.0f), 255.0f));
        }

        //    *
        //	\brief Set alpha colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\param alpha New alpha value of pixel colour [0.0, 1.0]
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        public void setAlpha(int x, int y, float alpha) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::setAlpha(int, int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_ALPHA] = (byte)(System.Math.Min(System.Math.Max(alpha * 255.0f, 0.0f), 255.0f));
        }

        //    *
        //	\brief Copy image data (RGBA) from a buffer to this object
        //	\param width Width of the image in buffer
        //	\param height Height of the image in buffer
        //	\param data Image buffer as source for copy
        /// <summary>
        ///  外部保证长宽等于当前图片的长宽
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>

        public void setData(int width, int height, ref byte[] data) {
            if (data == null)
                return;
            if (width != mWidth || height != mHeight)
                return;
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
            memcpy(mPixels, data, mWidth * mHeight * 4 * sizeof(byte));
        }

        //    *
        //	\brief Copy image data (RGBA) from an other TextureBuffer object
        //	\param buffer Image buffer as source for copy
        /// <summary>
        /// 外部保证buffer的长宽等于当前的长宽
        /// </summary>
        /// <param name="buffer"></param>	
        public void setData(TextureBuffer buffer) {
            if (buffer == null)
                return;
            if (buffer.getWidth() != mWidth || buffer.getHeight() != mHeight)
                return;
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
            memcpy(mPixels, buffer.mPixels, (mWidth * mHeight * 4 * sizeof(byte)));
        }

        //private int Sizeof(string p) {
        //    return sizeof(byte);
        //    throw new NotImplementedException();
        //    return 8;
        //}



        //    *
        //	\brief Get colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return colour value as an Ogre::ColourValue object.
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::ColourValue getPixel(int x, int y) const
        public ColourValue getPixel(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixel(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return new ColourValue(getPixelRedReal(x, y), getPixelGreenReal(x, y), getPixelBlueReal(x, y), getPixelAlphaReal(x, y));
        }

        //    *
        //	\brief Get red colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return red colour value as a byte [0, 255].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::byte getPixelRedByte(int x, int y) const
        public byte getPixelRedByte(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelRedByte(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_RED];
        }

        //    *
        //	\brief Get green colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return green colour value as a byte [0, 255].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::byte getPixelGreenByte(int x, int y) const
        public byte getPixelGreenByte(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelGreenByte(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_GREEN];
        }

        //    *
        //	\brief Get blue colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return blue colour value as a byte [0, 255].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::byte getPixelBlueByte(int x, int y) const
        public byte getPixelBlueByte(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelBlueByte(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_BLUE];
        }

        //    *
        //	\brief Get alpha colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return alpha colour value as a byte [0, 255].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::byte getPixelAlphaByte(int x, int y) const
        public byte getPixelAlphaByte(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelAlphaByte(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_ALPHA];
        }

        //    *
        //	\brief Get red colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return red colour value as a Ogre::float [0.0, 1.0].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::float getPixelRedReal(int x, int y) const
        public float getPixelRedReal(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelRedReal(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return ((float)mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_RED]) / 255.0f;
        }

        //    *
        //	\brief Get green colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return green colour value as a Ogre::float [0.0, 1.0].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::float getPixelGreenReal(int x, int y) const
        public float getPixelGreenReal(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelGreenReal(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return ((float)mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_GREEN]) / 255.0f;
        }

        //    *
        //	\brief Get blue colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return blue colour value as a Ogre::float [0.0, 1.0].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::float getPixelBlueReal(int x, int y) const
        public float getPixelBlueReal(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelBlueReal(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return ((float)mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_BLUE]) / 255.0f;
        }

        //    *
        //	\brief Get alpha colour value of a specified pixel
        //	\param x X position of pixel to paint on (0 <= x < width)
        //	\param y Y position of pixel to paint on (0 <= y < height)
        //	\return Return alpha colour value as a Ogre::float [0.0, 1.0].
        //	\exception Ogre::InvalidParametersException Pixel location is out of bounds!
        //	
        //
        //ORIGINAL LINE: Ogre::float getPixelAlphaReal(int x, int y) const
        public float getPixelAlphaReal(int x, int y) {
            if (x >= mWidth || y >= mHeight)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pixel location is out of bounds!", "Procedural::TextureBuffer::getPixelAlphaReal(int, int)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;

            return ((float)mPixels[y * mWidth * 4 + x * 4 + PROCEDURAL_ALPHA]) / 255.0f;
        }

        //    *
        //	\brief Create a copy of the current texture image buffer
        //	\note You have to delete cloned object by yourself!
        //	
        //
        //ORIGINAL LINE: TextureBuffer* clone() const
        public TextureBuffer clone() {
            TextureBuffer clon = new TextureBuffer(mWidth);
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
            memcpy(clon.mPixels, mPixels, mWidth * mHeight * 4 * sizeof(byte));
            return clon;
        }

        //    *
        //	\brief Standard constructor which copy a given image
        //	\param tocopy Image which to copy
        //	\exception Ogre::InvalidParametersException Pointer to source image must not be NULL!
        //	
        public TextureBuffer(TextureBuffer tocopy) {
            if (tocopy == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Pointer to source image must not be NULL!", "Procedural::TextureBuffer::TextureBuffer(Procedural::TextureBufferPtr)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;
            mWidth = (uint)tocopy.getWidth();
            mHeight = (uint)tocopy.getHeight();

            mPixels = new byte[mWidth * mHeight * 4];
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
            memcpy(mPixels, tocopy.mPixels, mWidth * mHeight * 4 * sizeof(byte));
        }

        //    *
        //	\brief Standard constructor which creates a quadratic image buffer with the given size
        //	\param width_height Edge length in px
        //	\exception Ogre::InvalidParametersException Minimum edge size is 8!
        //	
        public TextureBuffer(uint width_height) {
            mWidth = width_height;
            mHeight = width_height;
            if (width_height < 8)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Minimum edge size is 8!", "Procedural::TextureBuffer::TextureBuffer(Procedural::TextureBufferPtr)", __FILE__, __LINE__);
                throw new Exception("Pixel location is out of bounds!");
            ;
            mPixels = new byte[mWidth * mHeight * 4];
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
            memset(mPixels, 0, mWidth * mHeight * 4 * sizeof(byte));
            for (int y = 0; y < mHeight; y++) {
                for (int x = 0; x < mWidth; x++) {
                    setAlpha(x, y, (byte)255);
                }
            }

#if OGRE_DEBUG_MODE
		Utils.log("Create texture buffer : " + StringConverter.toString(mWidth) + "x" + StringConverter.toString(mHeight));
#endif
        }
        /// <summary>
        /// 功能：由src所指内存区域复制count个字节到dest所指内存区域。
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        private void memcpy(byte[] dest, byte[] src, uint count) {
            //throw new NotImplementedException();
            int length = System.Math.Min(src.Length, (int)count);
            length = System.Math.Min(length, dest.Length);
            Array.Copy(src, dest, length);
        }
        /// <summary>
        /// 总的作用：将已开辟内存空间 array 的首 count 个字节的值设为值 value。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private void memset(byte[] array, byte value, uint count) {
            //Array.Clear(mPixels, p, (int)length);
            //throw new NotImplementedException();
            if (array == null) {
                throw new ArgumentNullException("array");
            }
            int index = 0;
            int length = System.Math.Min(array.Length, (int)count);
            //Fill the initial array
            while (index < length) {
                array[index++] = value;
            }
        }

        /// Destructor which release memory
        public void Dispose() {
            mPixels = null;
        }

        /// Get the width of the stored image in px
        //
        //ORIGINAL LINE: inline Ogre::uint getWidth() const
        public int getWidth() {
            return (int)mWidth;
        }

        /// Get the height of the stored image in px
        //
        //ORIGINAL LINE: inline Ogre::uint getHeight() const
        public int getHeight() {
            return (int)mHeight;
        }

        /// Create a new image from buffer.
        //
        //ORIGINAL LINE: Ogre::Image* getImage() const
        public unsafe Mogre.Image getImage() {
            Mogre.Image image = new Mogre.Image();
            fixed (byte* buf = mPixels) {
                image.LoadDynamicImage(buf, mWidth, mHeight, 1, PixelFormat.PF_R8G8B8A8);
            }
            return image;
        }

        //    *
        //	\brief Save the image from the buffer to a file.
        //	\param filename Name (and path) of the image file where to save the buffer.
        //	
        //
        //ORIGINAL LINE: void saveImage(Ogre::String filename) const
        public void saveImage(string filename) {
            Mogre.Image image = getImage();
            image.Save(filename);
            image.Dispose();
        }

        //    *
        //	\brief Creates an OGRE texture and add it to current TextureManager instance.
        //	\param name Name of the texture
        //	\param group Name of the resource group where to list the texture
        //	
        public TexturePtr createTexture(string name) {
            return createTexture(name, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
        }
        //
        //ORIGINAL LINE: Ogre::TexturePtr createTexture(Ogre::String name, Ogre::String group = Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME) const
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        public TexturePtr createTexture(string name, string group) {
            Mogre.TexturePtr texture = TextureManager.Singleton.CreateManual(name, group, TextureType.TEX_TYPE_2D, mWidth, mHeight, 0, PixelFormat.PF_R8G8B8A8, (int)TextureUsage.TU_DEFAULT);

            Mogre.Image image = getImage();
            texture.LoadImage(image);
            image.Dispose();

            return texture;
        }
    }

    //*
    //\brief base class for material generation classes.
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TextureProcessing
    public abstract class TextureProcessing
    {
        protected TextureBuffer mBuffer;
        protected string mName = "";
        protected bool mLog;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	\param name Filter name
        //	\exception Ogre::InvalidParametersException Texture buffer is not set!
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public TextureProcessing(TextureBuffer pBuffer, string name) {
            if (pBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Texture buffer is not set!", "Procedural::TextureProcessing::TextureProcessing(TextureBufferPtr, Ogre::String)", __FILE__, __LINE__);
                throw new Exception("texture buffer is not set!");
            ;
            mBuffer = pBuffer;
            mName = name;
#if OGRE_DEBUG_MODE
		mLog = true;
#else
            mLog = false;
#endif
        }

        //* Run processing algorithmus 
        public abstract TextureBuffer process();

        //* Get name of the filter as string. 
        public string getName() {
            return mName;
        }

        //    *
        //	Enable/Disable logging.
        //	\param enable true enables logging of filter actions
        //	
        public void setLog() {
            setLog(true);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: void setLog(bool enable = true)
        public void setLog(bool enable) {
            mLog = enable;
        }

        public virtual void Dispose() {
        }

        protected void logMsg(string msg) {
            if (mLog)
                Utils.log(msg);
        }


        /// <summary>
        /// 功能：由src所指内存区域复制count个字节到dest所指内存区域。
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        protected void memcpy<T>(T[] dest, T[] src, uint count) {
            //throw new NotImplementedException();
            int length = System.Math.Min(src.Length, (int)count);
            length = System.Math.Min(length, dest.Length);
            Array.Copy(src, dest, length);
        }
        /// <summary>
        /// 总的作用：将已开辟内存空间 array 的首 count 个字节的值设为值 value。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        protected void memset<T>(T[] array, T value, uint count) {
            //Array.Clear(mPixels, p, (int)length);
            //throw new NotImplementedException();
            if (array == null) {
                throw new ArgumentNullException("array");
            }
            int index = 0;
            int length = System.Math.Min(array.Length, (int)count);
            //Fill the initial array
            while (index < length) {
                array[index++] = value;
            }
        }
        public static int RAND_MAX = RandomNumbers.RAND_MAX;
    }
}



