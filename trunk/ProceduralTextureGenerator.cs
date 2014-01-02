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
    //\brief Create a texture consisting of cells aligned in a grid, or a chessboard.
    //\details Cells can be irregular. If considered bump map, they have pyramid form.
    //
    //Examples:
    //
    //\b Default (MODE_GRID + PATTERN_BOTH)
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCellDefault(256);
    //Procedural::Cell(&bufferCellDefault).setDensity(4).process();
    //\endcode
    //\image html texture_cell_default.png
    //
    //\b MODE_CHESSBOARD + PATTERN_CONE
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCellChessCone(256);
    //Procedural::Cell(&bufferCellChessCone).setDensity(4).setMode(Procedural::Cell::MODE_CHESSBOARD).setPattern(Procedural::Cell::PATTERN_CONE).process();
    //\endcode
    //\image html texture_cell_chess.png
    //
    //\b MODE_GRID + PATTERN_CROSS
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCellGridCross(256);
    //Procedural::Cell(&bufferCellGridCross).setDensity(4).setMode(Procedural::Cell::MODE_GRID).setPattern(Procedural::Cell::PATTERN_CROSS).process();
    //\endcode
    //\image html texture_cell_grid.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Cell : public TextureProcessing
    public class Cell : TextureProcessing
    {
        //! Mode how to paint cells
        public enum CELL_MODE : int
        {
            MODE_GRID, //!< Paint cells on a grid
            MODE_CHESSBOARD //!< Paint cells on a chessboard
        }

        //! Mode how to construct cells
        public enum CELL_PATTERN : int
        {
            PATTERN_BOTH, //!< PATTERN_CROSS | PATTERN_CONE
            PATTERN_CROSS, //!< Construct cells from vertices
            PATTERN_CONE //!< Construct cells from cicles
        }

        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;
        private uint mRegularity = 0;
        private uint mDensity = 0;
        private CELL_MODE mMode;
        private CELL_PATTERN mPattern;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Cell(TextureBuffer pBuffer)
            : base(pBuffer, "Cell") {
            mColour = ColourValue.White;
            mRegularity = 128;
            mDensity = 8;
            mMode = CELL_MODE.MODE_GRID;
            mPattern = CELL_PATTERN.PATTERN_BOTH;
            mSeed = 5120;
        }

        //    *
        //	Set the colour of the cell top.
        //	\param colour New colour of the cell top (default Ogre::ColourValue::White)
        //	
        public Cell setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the cell top.
        //	\param red Red value of the cell top colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the cell top colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the cell top colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the cell top colour [0.0, 1.0] \(default 1.0)
        //	
        public Cell setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Cell& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Cell setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Cell setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Set the regularity of texture.
        //
        //	The maximum value of 255 creates identical cells. The minimum 0 creates random forms for each cells.
        //	\param regularity New value for chaotic cell forms (default 128)
        //	
        public Cell setRegularity(byte regularity) {
            mRegularity = regularity;
            return this;
        }

        //    *
        //	Set the density of cells in texture.
        //
        //	At least you have to set number of rows and columns in the grid to 1 or above.
        //	\param density New number of columns and rows (default 8)
        //	
        public Cell setDensity(uint density) {
            mDensity = density;
            if (mDensity == 0)
                mDensity = 1;
            return this;
        }

        //    *
        //	Set the cell mode of texture.
        //	\param mode New mode for cell ground (default MODE_GRID)
        //	
        public Cell setMode(CELL_MODE mode) {
            mMode = mode;
            return this;
        }

        //    *
        //	Set the cell pattern of texture.
        //	\param pattern New base of cell construction (default PATTERN_BOTH)
        //	
        public Cell setPattern(CELL_PATTERN pattern) {
            mPattern = pattern;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            bool cfc;
            float coeff = 0f;

            RandomNumbers.Seed((int)mSeed);
            float regularity = mRegularity / 255.0f;
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
            Vector3[] cellPoints = new Vector3[mDensity * mDensity]; //(Vector3)malloc(sizeof(Vector3) * mDensity * mDensity);

            for (int y = 0; y < mDensity; ++y) {
                for (int x = 0; x < mDensity; ++x) {
                    float rand1 = (float)RandomNumbers.NextNumber() / 65536.0f;
                    float rand2 = (float)RandomNumbers.NextNumber() / 65536.0f;
                    cellPoints[x + y * mDensity].x = (x + 0.5f + (rand1 - 0.5f) * (1 - regularity)) / mDensity - 1.0f / mBuffer.getWidth();
                    cellPoints[x + y * mDensity].y = (y + 0.5f + (rand2 - 0.5f) * (1 - regularity)) / mDensity - 1.0f / mBuffer.getHeight();
                    cellPoints[x + y * mDensity].z = 0;
                }
            }

            for (int y = 0; y < mBuffer.getHeight(); ++y) {
                for (int x = 0; x < mBuffer.getWidth(); ++x) {
                    Vector3 pixelPos = new Vector3();
                    pixelPos.x = (float)x / (float)mBuffer.getWidth();
                    pixelPos.y = (float)y / (float)mBuffer.getHeight();
                    pixelPos.z = 0.0f;

                    float minDist = 10;
                    float nextMinDist = minDist;
                    int xo = x * (int)mDensity / (int)mBuffer.getWidth();
                    int yo = y * (int)mDensity / (int)mBuffer.getHeight();
                    for (int v = -1; v < 2; ++v) {
                        int vo = ((yo + (int)mDensity + v) % (int)mDensity) * (int)mDensity;
                        for (int u = -1; u < 2; ++u) {
                            Vector3 cellPos = cellPoints[((xo + mDensity + u) % mDensity) + vo];
                            if (u == -1 && x * mDensity < mBuffer.getWidth())
                                cellPos.x -= 1;
                            if (v == -1 && y * mDensity < mBuffer.getHeight())
                                cellPos.y -= 1;
                            if (u == 1 && x * mDensity >= mBuffer.getWidth() * (mDensity - 1))
                                cellPos.x += 1;
                            if (v == 1 && y * mDensity >= mBuffer.getHeight() * (mDensity - 1))
                                cellPos.y += 1;

                            float norm = (pixelPos - cellPos).Length;//pixelPos.distance(cellPos);
                            if (norm < minDist) {
                                nextMinDist = minDist;
                                minDist = norm;
                            }
                            else if (norm < nextMinDist) {
                                nextMinDist = norm;
                            }
                        }
                    }

                    switch (mPattern) {
                        default:
                        //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                        case CELL_PATTERN.PATTERN_BOTH:
                            minDist = (nextMinDist - minDist) * mDensity;
                            break;

                        case CELL_PATTERN.PATTERN_CROSS:
                            minDist = 2 * nextMinDist * mDensity - 1;
                            break;

                        case CELL_PATTERN.PATTERN_CONE:
                            minDist = 1 - minDist * mDensity;
                            break;
                    }

                    if (minDist < 0)
                        minDist = 0;
                    if (minDist > 1)
                        minDist = 1;

                    switch (mMode) {
                        case CELL_MODE.MODE_CHESSBOARD:
                            cfc = ((xo & 1) ^ (yo & 1)) != 0;
                            int cfc_int = cfc ? 1 : 0;
                            coeff = (1 - 2 * cfc_int) / 2.5f;
                            mBuffer.setRed(x, y, (byte)((cfc_int + coeff * minDist) * mColour.r * 255.0f));
                            mBuffer.setGreen(x, y, (byte)((cfc_int + coeff * minDist) * mColour.g * 255.0f));
                            mBuffer.setBlue(x, y, (byte)((cfc_int + coeff * minDist) * mColour.b * 255.0f));
                            break;

                        default:
                        //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                        case CELL_MODE.MODE_GRID:
                            mBuffer.setRed(x, y, (byte)(minDist * mColour.r * 255.0f));
                            mBuffer.setGreen(x, y, (byte)(minDist * mColour.g * 255.0f));
                            mBuffer.setBlue(x, y, (byte)(minDist * mColour.b * 255.0f));
                            break;
                    }
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            logMsg("Create cell texture : " + (mDensity).ToString() + "x" + (mDensity).ToString());
            return mBuffer;
        }



        //private Vector3 malloc(long p) {
        //    throw new NotImplementedException();
        //}
    }

    //*
    //\brief Creates a cloud structured image.
    //\details Creates a cloud structure from a specified perlin noise on a coloured background.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferCloud(256);
    //Procedural::Cloud(&bufferCloud).process();
    //\endcode
    //\image html texture_cloud.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Cloud : public TextureProcessing
    public class Cloud : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Cloud(TextureBuffer pBuffer)
            : base(pBuffer, "Cloud") {
            mColour = ColourValue.White;
            mSeed = 5120;
        }

        //    *
        //	Set the colour of the background.
        //	\param colour New colour for background (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Cloud setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Sets the colour of the background
        //	\param red Red value of background colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of background colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of background colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of background colour [0.0, 1.0] \(default 1.0)
        //	
        public Cloud setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Cloud& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Cloud setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Cloud setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            RandomNumbers.Seed((int)mSeed);
            int r = RandomNumbers.NextNumber();
            PerlinNoise noise = new PerlinNoise(8, 0.5f, 1.0f / 32.0f, 1.0f);
            float filterLevel = 0.7f;
            float preserveLevel = 0.3f;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float noiseVal = System.Math.Max(0.0f, System.Math.Min(1.0f, noise.function2D(x + r, y + r) * 0.5f + 0.5f));
                    mBuffer.setRed(x, y, (byte)System.Math.Min(preserveLevel * mColour.r * 255.0f + filterLevel * mColour.r * 255.0f * noiseVal, 255.0f));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(preserveLevel * mColour.g * 255.0f + filterLevel * mColour.g * 255.0f * noiseVal, 255.0f));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(preserveLevel * mColour.b * 255.0f + filterLevel * mColour.b * 255.0f * noiseVal, 255.0f));
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            Utils.log("Create cloud texture");
            return mBuffer;
        }
    }

    //*
    //\brief Fills full image with given colour gradients.
    //\details Each corner of the image has unique color.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferGradient(256);
    //Procedural::Gradient(&bufferGradient).setColours(Ogre::ColourValue::Black, Ogre::ColourValue::Red, Ogre::ColourValue::Green, Ogre::ColourValue::Blue).process();
    //\endcode
    //\image html texture_gradient.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Gradient : public TextureProcessing
    public class Gradient : TextureProcessing
    {
        private ColourValue mColourA = new ColourValue();
        private ColourValue mColourB = new ColourValue();
        private ColourValue mColourC = new ColourValue();
        private ColourValue mColourD = new ColourValue();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Gradient(TextureBuffer pBuffer)
            : base(pBuffer, "Gradient") {
            mColourA = ColourValue.Blue;
            mColourB = ColourValue.Green;
            mColourC = ColourValue.Red;
            mColourD = new ColourValue(0.0f, 1.0f, 1.0f);
        }

        //    *
        //	Set the colour in the top left corner of the image.
        //	\param colour New colour in the top left corner for processing (default Ogre::ColourValue::Blue)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Gradient setColourA(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourA = colour;
            mColourA = (colour);
            return this;
        }

        //    *
        //	Set the colour in the top left corner of the image.
        //	\param red Red value of new colour in the top left corner [0.0, 1.0] \(default 0.0)
        //	\param green Green value of new colour in the top left corner [0.0, 1.0] \(default 0.0)
        //	\param blue Blue value of new colour in the top left corner [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of new colour in the top left corner [0.0, 1.0] \(default 1.0)
        //	
        public Gradient setColourA(float red, float green, float blue) {
            return setColourA(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Gradient& setColourA(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Gradient setColourA(float red, float green, float blue, float alpha) {
            mColourA = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the colour in the top right corner of the image.
        //	\param colour New colour in the top right corner for processing (default Ogre::ColourValue::Green)
        //	
        public Gradient setColourB(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourB = colour;
            mColourB = (colour);
            return this;
        }

        //    *
        //	Set the colour in the top right corner of the image.
        //	\param red Red value of new colour in the top right corner [0.0, 1.0] \(default 0.0)
        //	\param green Green value of new colour in the top right corner [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of new colour in the top right corner [0.0, 1.0] \(default 0.0)
        //	\param alpha %Alpha value of new colour in the top right corner [0.0, 1.0] \(default 1.0)
        //	
        public Gradient setColourB(float red, float green, float blue) {
            return setColourB(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Gradient& setColourB(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Gradient setColourB(float red, float green, float blue, float alpha) {
            mColourB = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the colour in the bottom left corner of the image.
        //	\param colour New colour in the bottom left corner for processing (default Ogre::ColourValue::Red)
        //	
        public Gradient setColourC(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourC = colour;
            mColourC = (colour);
            return this;
        }

        //    *
        //	Set the colour in the bottom left corner of the image.
        //	\param red Red value of new colour in the bottom left corner [0.0, 1.0] \(default 1.0)
        //	\param green Green value of new colour in the bottom left corner [0.0, 1.0] \(default 0.0)
        //	\param blue Blue value of new colour in the bottom left corner [0.0, 1.0] \(default 0.0)
        //	\param alpha %Alpha value of new colour in the bottom left corner [0.0, 1.0] \(default 1.0)
        //	
        public Gradient setColourC(float red, float green, float blue) {
            return setColourC(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Gradient& setColourC(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Gradient setColourC(float red, float green, float blue, float alpha) {
            mColourC = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the colour in the bottom right corner of the image.
        //	\param colour New colour in the bottom right corner for processing (default Ogre::ColourValue(0.0f, 1.0f, 1.0f))
        //	
        public Gradient setColourD(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourD = colour;
            mColourD = (colour);
            return this;
        }

        //    *
        //	Set the colour in the bottom right corner of the image.
        //	\param red Red value of new colour in the bottom right corner [0.0, 1.0] \(default 0.0)
        //	\param green Green value of new colour in the bottom right corner [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of new colour in the bottom right corner [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of new colour in the bottom right corner [0.0, 1.0] \(default 1.0)
        //	
        public Gradient setColourD(float red, float green, float blue) {
            return setColourD(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Gradient& setColourD(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Gradient setColourD(float red, float green, float blue, float alpha) {
            mColourD = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Sets the colours of the image corners.
        //	\param colourA New colour in the top left corner (default Ogre::ColourValue::Blue)
        //	\param colourB New colour in the top right corner (default Ogre::ColourValue::Green)
        //	\param colourC New colour in the bottom left corner (default Ogre::ColourValue::Red)
        //	\param colourD New colour in the bottom right corner (default Ogre::ColourValue(0.0f, 1.0f, 1.0f))
        //	
        public Gradient setColours(ColourValue colourA, ColourValue colourB, ColourValue colourC, ColourValue colourD) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourA = colourA;
            mColourA = (colourA);
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourB = colourB;
            mColourB = (colourB);
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourC = colourC;
            mColourC = (colourC);
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColourD = colourD;
            mColourD = (colourD);
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            float finv_WH = 1.0f / (float)(mBuffer.getWidth() * mBuffer.getHeight());
            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float a = (float)((mBuffer.getWidth() - x) * (mBuffer.getHeight() - y)) * finv_WH;
                    float b = (float)((x) * (mBuffer.getHeight() - y)) * finv_WH;
                    float c = (float)((mBuffer.getWidth() - x) * (y)) * finv_WH;
                    float d = (float)((x) * (y)) * finv_WH;

                    mBuffer.setRed(x, y, (byte)(((mColourA.r * a) + (mColourB.r * b) + (mColourC.r * c) + (mColourD.r * d)) * 255.0f));
                    mBuffer.setGreen(x, y, (byte)(((mColourA.g * a) + (mColourB.g * b) + (mColourC.g * c) + (mColourD.g * d)) * 255.0f));
                    mBuffer.setBlue(x, y, (byte)(((mColourA.b * a) + (mColourB.b * b) + (mColourC.b * c) + (mColourD.b * d)) * 255.0f));
                    mBuffer.setAlpha(x, y, (byte)(((mColourA.a * a) + (mColourB.a * b) + (mColourC.a * c) + (mColourD.a * d)) * 255.0f));
                }
            }

            Utils.log("Create gradient texture");
            return mBuffer;
        }
    }

    //*
    //\brief Load an image from a resource.
    //\details Try to load an image from a resource.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferImage(256);
    //Procedural::Image(&bufferImage).setFile("red_brick.jpg").process();
    //\endcode
    //\image html texture_image.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Image : public TextureProcessing
    public class Image : TextureProcessing
    {
        private string mFile = "";
        private string mGroup = "";

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Image(TextureBuffer pBuffer)
            : base(pBuffer, "Image") {
        }

        //    *
        //	Set the colour of the background.
        //	\param filename Name of an image file to load.
        //	\param groupname Name of the resource group to search for the image
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Image setFile(string filename) {
            return setFile(filename, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Image& setFile(Ogre::String filename, Ogre::String groupname = Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME)
        public Image setFile(string filename, string groupname) {
            mFile = filename;
            mGroup = groupname;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            Mogre.Image img = new Mogre.Image();
            img.Load(mFile, mGroup);
            if (img.Height < mBuffer.getHeight() || img.Width < mBuffer.getWidth())
                return mBuffer;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    mBuffer.setPixel(x, y, img.GetColourAt(x, y, 0));
                }
            }

            Utils.log("Create texture from image");
            return mBuffer;
        }
    }

    //*
    //\brief Creates a labyrinth structured image.
    //\details Creates a labyrinth structure from a specified perlin noise on a coloured background.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferLabyrinth(256);
    //Procedural::Labyrinth(&bufferLabyrinth).process();
    //\endcode
    //\image html texture_labyrinth.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Labyrinth : public TextureProcessing
    public class Labyrinth : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Labyrinth(TextureBuffer pBuffer)
            : base(pBuffer, "Labyrinth") {
            mColour = ColourValue.White;
            mSeed = 5120;
        }

        //    *
        //	Set the colour of the background.
        //	\param colour New colour for background (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Labyrinth setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Sets the colour of the background
        //	\param red Red value of background colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of background colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of background colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of background colour [0.0, 1.0] \(default 1.0)
        //	
        public Labyrinth setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Labyrinth& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Labyrinth setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Labyrinth setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            RandomNumbers.Seed((int)mSeed);
            int r = RandomNumbers.NextNumber();
            PerlinNoise noise = new PerlinNoise(1, 0.65f, 1.0f / 16.0f, 1.0f);
            float filterLevel = 0.7f;
            float preserveLevel = 0.3f;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float noiseVal = System.Math.Min(1.0f, Math.Abs(noise.function2D(x + r, y + r)));
                    mBuffer.setRed(x, y, (byte)System.Math.Min(preserveLevel * mColour.r * 255.0f + filterLevel * mColour.r * 255.0f * noiseVal, 255.0f));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(preserveLevel * mColour.g * 255.0f + filterLevel * mColour.g * 255.0f * noiseVal, 255.0f));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(preserveLevel * mColour.b * 255.0f + filterLevel * mColour.b * 255.0f * noiseVal, 255.0f));
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            Utils.log("Create labyrinth texture");
            return mBuffer;
        }
    }

    //*
    //\brief Creates a marble structured image.
    //\details Creates a marbel structure from a specified perlin noise on a coloured background.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferMarble(256);
    //Procedural::Marble(&bufferMarble).process();
    //\endcode
    //\image html texture_marble.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Marble : public TextureProcessing
    public class Marble : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Marble(TextureBuffer pBuffer)
            : base(pBuffer, "Marble") {
            mColour = ColourValue.White;
            mSeed = 5120;
        }

        //    *
        //	Set the colour of the background.
        //	\param colour New colour for marble structure (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Marble setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the background.
        //	\param red Red value of the marble structure colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the marble structure colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue valu of the marble structure coloure [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the marble structure colour [0.0, 1.0] \(default 1.0)
        //	
        public Marble setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Marble& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Marble setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Marble setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            RandomNumbers.Seed((int)mSeed);
            int r = RandomNumbers.NextNumber();
            PerlinNoise noise = new PerlinNoise(2, 0.65f, 1.0f / 32.0f, 1.0f);
            float xFact = 1.0f / 96.0f;
            float yFact = 1.0f / 48.0f;
            float filterLevel = 0.7f;
            float preserveLevel = 0.3f;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float noiseVal = System.Math.Min(1.0f, Math.Abs(Math.Sin(x * xFact + y * yFact + noise.function2D(x + r, y + r)) * Math.PI));
                    mBuffer.setRed(x, y, (byte)System.Math.Min(preserveLevel * mColour.r * 255.0f + filterLevel * mColour.r * 255.0f * noiseVal, 255.0f));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(preserveLevel * mColour.g * 255.0f + filterLevel * mColour.g * 255.0f * noiseVal, 255.0f));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(preserveLevel * mColour.b * 255.0f + filterLevel * mColour.b * 255.0f * noiseVal, 255.0f));
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            Utils.log("Create marble texture");
            return mBuffer;
        }
    }

    //*
    //\brief Fills full image with noise in a given colour.
    //\details High quality noise with various noise algorithms.
    //
    //Examples:
    //\b White noise (default)
    //\code{.cpp}
    //Procedural::TextureBuffer bufferNoiseWhite(256);
    //Procedural::Noise(&bufferNoiseWhite).setType(Procedural::Noise::NOISE_WHITE).process();
    //\endcode
    //\image html texture_noise_white.png
    //
    //\b Perlin noise
    //\code{.cpp}
    //Procedural::TextureBuffer bufferNoisePerlin(256);
    //Procedural::Noise(&bufferNoisePerlin).setType(Procedural::Noise::NOISE_PERLIN).process();
    //\endcode
    //\image html texture_noise_perlin.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Noise : public TextureProcessing
    public class Noise : TextureProcessing
    {
        //! Noise generator type
        public enum NOISE_TYPE : int
        {
            NOISE_WHITE, //!< White noise
            NOISE_PERLIN //!< Perlin noise
        }

        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;
        private NOISE_TYPE mType;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Noise(TextureBuffer pBuffer)
            : base(pBuffer, "Noise") {
            mColour = ColourValue.White;
            mSeed = 5120;
            mType = (int)NOISE_TYPE.NOISE_WHITE;
        }

        //    *
        //	Set the colour of the noise.
        //	\param colour New colour of the noise (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Noise setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the noise.
        //	\param red Red value of the noise colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of the noise colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of the noise colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of the noise colour [0.0, 1.0] \(default 1.0)
        //	
        public Noise setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Noise& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Noise setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Noise setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Set the type of noise generation.
        //	\param type Type of noise generator (default NOISE_WHITE)
        //	
        public Noise setType(NOISE_TYPE @type) {
            mType = @type;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            NoiseBase noiseGen;
            switch (mType) {
                case NOISE_TYPE.NOISE_PERLIN:
                    noiseGen = new PerlinNoise();
                    break;

                default:
                //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
                case NOISE_TYPE.NOISE_WHITE:
                    noiseGen = new WhiteNoise(mSeed);
                    break;
            }

            byte[] field = noiseGen.field2D(mBuffer.getWidth(), mBuffer.getHeight());
            for (int y = 0; y < mBuffer.getHeight(); ++y) {
                for (int x = 0; x < mBuffer.getWidth(); ++x) {
                    float noiseVal = (float)field[y * mBuffer.getWidth() + x];
                    mBuffer.setRed(x, y, (byte)(noiseVal * mColour.r));
                    mBuffer.setGreen(x, y, (byte)(noiseVal * mColour.g));
                    mBuffer.setBlue(x, y, (byte)(noiseVal * mColour.b));
                    mBuffer.setAlpha(x, y, (byte)(mColour.a * 255.0f));
                }
            }

            field = null;
            noiseGen.Dispose();
            Utils.log("Create noise texture : " + mType.ToString());//StringConverter.ToString(mType));
            return mBuffer;
        }
    }

    //*
    //\brief Fills full image with given colour.
    //\details Set all pixel to the same color.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferSolid(256);
    //Procedural::Solid(&bufferSolid).setColour(Ogre::ColourValue(0.0f, 0.5f, 1.0f, 1.0f)).process();
    //\endcode
    //\image html texture_solid.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Solid : public TextureProcessing
    public class Solid : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Solid(TextureBuffer pBuffer)
            : base(pBuffer, "Solid") {
            mColour = ColourValue.Black;
        }

        //    *
        //	Sets the fill colour of the image.
        //	\param colour New colour for processing (default Ogre::ColourValue::Black)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Solid setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Sets the fill colour of the image.
        //	\param red Red value [0.0, 1.0] \(default 0.0)
        //	\param green Green value [0.0, 1.0] \(default 0.0)
        //	\param blue Blue value [0.0, 1.0] \(default 0.0)
        //	\param alpha %Alpha value [0.0, 1.0] \(default 1.0)
        //	
        public Solid setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Solid& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Solid setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    mBuffer.setPixel(x, y, mColour);
                }
            }

            Utils.log("Create solid colour texture : " + ((int)(mColour.r * 255.0f)).ToString() + ", " + ((int)(mColour.g * 255.0f)).ToString() + ", " + ((int)(mColour.b * 255.0f)).ToString());
            return mBuffer;
        }
    }

    //*
    //\brief Creates a textile structured image.
    //\details Creates a textile structure from a specified perlin noise on a coloured background.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferTextile(256);
    //Procedural::Textile(&bufferTextile).process();
    //\endcode
    //\image html texture_textile.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Textile : public TextureProcessing
    public class Textile : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Textile(TextureBuffer pBuffer)
            : base(pBuffer, "Textile") {
            mColour = ColourValue.White;
            mSeed = 5120;
        }

        //    *
        //	Set the colour of the background.
        //	\param colour New colour for background (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Textile setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the background.
        //	\param red Red value of background colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of background colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of background colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of background colour [0.0, 1.0] \(default 1.0)
        //	
        public Textile setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Textile& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Textile setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Textile setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            RandomNumbers.Seed((int)mSeed);
            int r = RandomNumbers.NextNumber();
            PerlinNoise noise = new PerlinNoise(3, 0.65f, 1.0f / 8.0f, 1.0f);
            float filterLevel = 0.7f;
            float preserveLevel = 0.3f;

            for (int y = 0; y < mBuffer.getHeight(); y++) {
                for (int x = 0; x < mBuffer.getWidth(); x++) {
                    float noiseVal = System.Math.Max(0.0f, System.Math.Min(1.0f, (Math.Sin(x + noise.function2D(x + r, y + r)) + Math.Sin(y + noise.function2D(x + r, y + r))) * 0.25f + 0.5f));
                    mBuffer.setRed(x, y, (byte)System.Math.Min(preserveLevel * mColour.r * 255.0f + filterLevel * mColour.r * 255.0f * noiseVal, 255.0f));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(preserveLevel * mColour.g * 255.0f + filterLevel * mColour.g * 255.0f * noiseVal, 255.0f));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(preserveLevel * mColour.b * 255.0f + filterLevel * mColour.b * 255.0f * noiseVal, 255.0f));
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            Utils.log("Create textile texture");
            return mBuffer;
        }
    }

    //*
    //\brief Creates a wood slice image.
    //\details Creates a structure of annual rings from a specified perlin noise on a coloured background.
    //
    //Example:
    //\code{.cpp}
    //Procedural::TextureBuffer bufferWood(256);
    //Procedural::Wood(&bufferWood).setRings(5).process();
    //\endcode
    //\image html texture_wood.png
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Wood : public TextureProcessing
    public class Wood : TextureProcessing
    {
        private ColourValue mColour = new ColourValue();
        private uint mSeed = 0;
        private uint mRings = 0;

        //    *
        //	Default constructor.
        //	\param pBuffer Image buffer where to store the generated image.
        //	
        public Wood(TextureBuffer pBuffer)
            : base(pBuffer, "Wood") {
            mColour = ColourValue.White;
            mSeed = 5120;
            mRings = 8;
        }

        //    *
        //	Set the colour of the background.
        //	\param colour New colour for background (default Ogre::ColourValue::White)
        //	

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Wood setColour(ColourValue colour) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mColour = colour;
            mColour = (colour);
            return this;
        }

        //    *
        //	Set the colour of the background.
        //	\param red Red value of background colour [0.0, 1.0] \(default 1.0)
        //	\param green Green value of background colour [0.0, 1.0] \(default 1.0)
        //	\param blue Blue value of background colour [0.0, 1.0] \(default 1.0)
        //	\param alpha %Alpha value of background colour [0.0, 1.0] \(default 1.0)
        //	
        public Wood setColour(float red, float green, float blue) {
            return setColour(red, green, blue, 1.0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Wood& setColour(Ogre::float red, Ogre::float green, Ogre::float blue, Ogre::float alpha = 1.0f)
        public Wood setColour(float red, float green, float blue, float alpha) {
            mColour = new ColourValue(red, green, blue, alpha);
            return this;
        }

        //    *
        //	Set the seed for "random" number generator.
        //	\param seed Seed value where to set the random number generator (default 5120)
        //	
        public Wood setSeed(uint seed) {
            mSeed = seed;
            return this;
        }

        //    *
        //	Sets the number of annual rings.
        //	\param rings Number of annual rings (minimum 3, default 8)
        //	
        public Wood setRings(uint rings) {
            mRings = rings;
            if (mRings < 3)
                mRings = 3;
            return this;
        }

        //    *
        //	Run image generation
        //	\return Pointer to image buffer which has been set in the constructor.
        //	
        public override TextureBuffer process() {
            RandomNumbers.Seed((int)mSeed);
            int r = RandomNumbers.NextNumber();
            float filterLevel = 0.7f;
            float preserveLevel = 0.3f;

            PerlinNoise noise = new PerlinNoise(8, 0.5f, 1.0f / 32.0f, 0.05f);
            int w2 = mBuffer.getWidth() / 2;
            int h2 = mBuffer.getHeight() / 2;

            for (int y = 0; y < (int)mBuffer.getHeight(); y++) {
                for (int x = 0; x < (int)mBuffer.getWidth(); x++) {
                    float xv = ((float)(x - w2)) / (float)mBuffer.getWidth();
                    float yv = ((float)(y - h2)) / (float)mBuffer.getHeight();
                    float noiseVal = System.Math.Min(1.0f, Math.Abs(Math.Sin((Math.Sqrt(xv * xv + yv * yv) + noise.function2D(x + r, y + r)) * Math.PI * 2 * mRings)));
                    mBuffer.setRed(x, y, (byte)System.Math.Min(preserveLevel * mColour.r * 255.0f + filterLevel * mColour.r * 255.0f * noiseVal, 255.0f));
                    mBuffer.setGreen(x, y, (byte)System.Math.Min(preserveLevel * mColour.g * 255.0f + filterLevel * mColour.g * 255.0f * noiseVal, 255.0f));
                    mBuffer.setBlue(x, y, (byte)System.Math.Min(preserveLevel * mColour.b * 255.0f + filterLevel * mColour.b * 255.0f * noiseVal, 255.0f));
                    mBuffer.setAlpha(x, y, mColour.a);
                }
            }

            Utils.log("Create wood texture : " + StringConverter.toString(mRings));
            return mBuffer;
        }
    }







}