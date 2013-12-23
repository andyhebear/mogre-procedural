using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport NoiseBase
    public abstract class NoiseBase
    {
        public abstract float function1D(int x);
        public abstract float function2D(int x, int y);

        public virtual byte[] field1D(int wx) {
            byte[] retval = new byte[wx];
            double mi = 9999999999.9;
            double ma = -999999999.9;
            float[] field = new float[wx];
            for (int x = 0; x < wx; ++x) {
                float val = function1D(x);
                if (val < mi)
                    mi = val;
                if (val > ma)
                    ma = val;
                field[x] = val;
            }
            for (int x = 0; x < wx; ++x) {
                retval[x] = (byte)((255.0 / (ma - mi)) * (field[x] - mi));
            }
            field = null;
            return retval;
        }

        public virtual byte[] field2D(int wx, int wy) {
            byte[] retval = new byte[wx * wy];
            double mi = 9999999999.9;
            double ma = -999999999.9;
            float[] field = new float[wy * wx];
            for (int y = 0; y < wy; ++y) {
                for (int x = 0; x < wx; ++x) {
                    float val = function2D(x, y);
                    if (val < mi)
                        mi = val;
                    if (val > ma)
                        ma = val;
                    field[y * wx + x] = val;
                }
            }
            for (int y = 0; y < wy; ++y) {
                for (int x = 0; x < wx; ++x) {
                    retval[y * wx + x] = (byte)((255.0 / (ma - mi)) * (field[y * wx + x] - mi));
                }
            }
            field = null;
            return retval;
        }
    }


    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport WhiteNoise : public NoiseBase
    public class WhiteNoise : NoiseBase
    {
        public WhiteNoise()
            : this(5120) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: WhiteNoise(Ogre::uint seed = 5120)
        public WhiteNoise(uint seed) {
            RandomNumbers.Seed(seed);
        }

        public override float function1D(int x) {
            return ((float)RandomNumbers.NextNumber() / RAND_MAX);
        }
        public override float function2D(int x, int y) {
            return function1D(x * y);
        }
    }

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport PerlinNoise : public NoiseBase
    public class PerlinNoise : NoiseBase
    {
        private float mFrequency = 0f;
        private float mAmplitude = 0f;
        private float mPersistance = 0f;
        private uint mOctaves = 0;

        public PerlinNoise(uint octaves, float persistence, float frequency)
            : this(octaves, persistence, frequency, 1.0f) {
        }
        public PerlinNoise(uint octaves, float persistence)
            : this(octaves, persistence, 1.0f, 1.0f) {
        }
        public PerlinNoise(uint octaves)
            : this(octaves, 0.65f, 1.0f, 1.0f) {
        }
        public PerlinNoise()
            : this(4, 0.65f, 1.0f, 1.0f) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: PerlinNoise(Ogre::uint octaves = 4, Ogre::float persistence = 0.65f, Ogre::float frequency = 1.0f, Ogre::float amplitude = 1.0f) : mFrequency(frequency), mAmplitude(amplitude), mPersistance(persistence), mOctaves(octaves)
        public PerlinNoise(uint octaves, float persistence, float frequency, float amplitude) {
            mFrequency = frequency;
            mAmplitude = amplitude;
            mPersistance = persistence;
            mOctaves = octaves;
            if (mOctaves < 1)
                mOctaves = 1;
            if (mOctaves > 32)
                mOctaves = 32;
        }

        public void setFrequency(float frequency) {
            mFrequency = frequency;
        }
        public void setAmplitude(float amplitude) {
            mAmplitude = amplitude;
        }
        public void setPersistence(float persistence) {
            mPersistance = persistence;
        }
        public void setOctaves(uint octaves) {
            mOctaves = octaves;
            if (mOctaves < 1)
                mOctaves = 1;
            if (mOctaves > 32)
                mOctaves = 32;
        }

        public override float function1D(int x) {
            double freq = mFrequency;
            double amp = mAmplitude;
            double sum = 0.0;

            for (int i = 0; i < mOctaves; i++) {
                sum += smoothedNoise((double)x * freq) * amp;

                amp *= mPersistance;
                freq *= 2;
            }

            return (float)sum;
        }
        public override float function2D(int x, int y) {
            double freq = mFrequency;
            double amp = mAmplitude;
            double sum = 0.0;

            for (int i = 0; i < mOctaves; i++) {
                sum += smoothedNoise((double)x * freq, (double)y * freq) * amp;

                amp *= mPersistance;
                freq *= 2;
            }

            return (float)sum;
        }

        private double noise(double x) {
            int n = ((int)x << 13) ^ (int)x;
            return 1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0;
        }
        private double noise(double x, double y) {
            int n = (int)(x + y * 57.0);
            n = (n << 13) ^ n;
            return 1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0;
        }
        private double smoothedNoise(double x) {
            int XInt = (int)x;
            double XFrac = x - (double)XInt;

            return interpolate(noise(XInt), noise(XInt + 1), XFrac);
        }
        private double smoothedNoise(double x, double y) {
            int XInt = (int)x;
            int YInt = (int)y;
            double XFrac = x - XInt;
            double YFrac = y - YInt;

            double n00 = noise(XInt, YInt);
            double n10 = noise(XInt + 1, YInt);
            double n01 = noise(XInt, YInt + 1);
            double n11 = noise(XInt + 1, YInt + 1);

            double i1 = interpolate(n00, n10, XFrac);
            double i2 = interpolate(n01, n11, XFrac);

            return interpolate(i1, i2, YFrac);
        }
        private double interpolate(double x1, double x2, double a) {
            double f = (1 - Math.Cos((float)a * Math.PI)) * 0.5;
            return x1 * (1 - f) + x2 * f;
        }
    }



    //----------------------------------------------------------------------------------------
    //	Copyright ? 2006 - 2009 Tangible Software Solutions Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class provides the ability to simulate the behavior of the C/C++ functions for 
    //	generating random numbers, using the .NET Framework System.Random class.
    //	'rand' converts to the parameterless overload of NextNumber
    //	'random' converts to the single-parameter overload of NextNumber
    //	'randomize' converts to the parameterless overload of Seed
    //	'srand' converts to the single-parameter overload of Seed
    //----------------------------------------------------------------------------------------
    internal static class RandomNumbers
    {
        private static System.Random r;

        internal static int NextNumber() {
            if (r == null)
                Seed();

            return r.Next();
        }

        internal static int NextNumber(int ceiling) {
            if (r == null)
                Seed();

            return r.Next(ceiling);
        }

        internal static void Seed() {
            r = new System.Random();
        }

        internal static void Seed(int seed) {
            r = new System.Random(seed);
        }
    }
}