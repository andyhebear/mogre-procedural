//#define PROCEDURAL_USE_FREETYPE

using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup shapegrp
// * @{
// 
#if PROCEDURAL_USE_FREETYPE
//-----------------------------------------------------------------------
//*
// * Produces a multishape from a given text
// * \image html shape_text.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport TextShape
public class TextShape
{
	private String mText = "";
	private String mFontName = "";
	private byte mFontSize = 0; 

//    *
//	Default constructor.
//	
	public TextShape()
	{
		mText = "OgreProcedural";
		mFontName = "Arial";
		mFontSize = 12;
	}

//    *
//	Set the text content.
//	\param text New text for processing (default "OgreProcedural")
//	\exception Ogre::InvalidParametersException Empty text
//	
	public TextShape setText(string text)
	{
		if (text.empty())
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 characters in text", "Procedural::TextShape::setText(Ogre::String text)", __FILE__, __LINE__);
			;

		mText = text;
		return this;
	}

//    *
//	Set the font for the text.
//	\param fontName Filenpath of a font or name of font (only on windows desktops)
//	\param fontSize Size of font [px] (default 12)
//	\exception Ogre::InvalidParametersException Empty font name
//	\exception Ogre::InvalidParametersException Font size is below 4
//	\todo Add search for font names on non windows systems.
//	
	public TextShape setFont(string fontName, byte fontSize)
	{
		if (fontName.empty())
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 characters in font name", "Procedural::TextShape::setFont(Ogre::String fontName, Ogre::byte fontSize)", __FILE__, __LINE__);
			;
		if (fontSize < 4)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Minimum font size is 4", "Procedural::TextShape::setFont(Ogre::String fontName, Ogre::byte fontSize)", __FILE__, __LINE__);
			;

		mFontName = fontName;
		mFontSize = fontSize;
		return this;
	}


//    *
//	 * Build a MultiShape from chars (one Shape per character)
//	 * \exception Ogre::InternalErrorException Freetype error
//	 * \todo Need to split shapes of multi region chars. For example the letter \c O
//	 * has two shapes, but they are connected to one shape.
//	 
	public MultiShape realizeShapes()
	{
		MultiShape retVal = new MultiShape();
	
		FT_Library ftlib = new FT_Library();
		FT_Face face = new FT_Face();
		FT_GlyphSlot slot = new FT_GlyphSlot();
	
		FT_Error error = FT_Init_FreeType(ftlib);
		if (error == 0)
		{
			error = FT_New_Face(ftlib, getFontFileByName().c_str(), 0, face);
			if (error == FT_Err_Unknown_File_Format)
			{
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
				throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "FreeType ERROR: FT_Err_Unknown_File_Format", "Procedural::TextShape::realizeShapes()", __FILE__, __LINE__);
				;
			}
			else if (error != null)
			{
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
				throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "FreeType ERROR: FT_New_Face - " + StringConverter.toString(error), "Procedural::TextShape::realizeShapes()", __FILE__, __LINE__);
				;
			}
			else
			{
				FT_Set_Pixel_Sizes(face, 0, mFontSize);
	
				int px = 0;
				int py = 0;
				slot = face.glyph;
	
				for (int n = 0; n < mText.length(); n++)
				{
					error = FT_Load_Char(face, mText[n], FT_LOAD_NO_BITMAP);
					if (error != null)
						continue;
	
					Shape s = new Shape();
	
					int nContours = face.glyph.outline.n_contours;
					int startPos = 0;
					string tags = face.glyph.outline.tags;
					FT_Vector[] vec = face.glyph.outline.points;
	
					for (int k = 0; k < nContours; k++)
					{
						if (k > 0)
							startPos = face.glyph.outline.contours[k-1]+1;
						int endPos = face.glyph.outline.contours[k]+1;
	
						Vector2 lastPoint = Vector2.ZERO;
						for (int j = startPos; j < endPos; j++)
						{
							if (FT_CURVE_TAG(tags[j]) == FT_CURVE_TAG_ON)
							{
								lastPoint = Vector2((float)vec[j].x, (float)vec[j].y);
								s.addPoint(lastPoint / 64.0f);
							}
							else
							{
								if (FT_CURVE_TAG(tags[j]) == FT_CURVE_TAG_CUBIC)
								{
									int prevPoint = j - 1;
									if (j == 0)
										prevPoint = endPos - 1;
									int nextIndex = j + 1;
									if (nextIndex >= endPos)
										nextIndex = startPos;
									Vector2[] nextPoint = new Vector2[nextIndex]((float)vec.x, (float)vec[nextIndex].y);
									if ((FT_CURVE_TAG(tags[prevPoint]) != FT_CURVE_TAG_ON) && (FT_CURVE_TAG(tags[prevPoint]) == FT_CURVE_TAG_CUBIC))
									{
										BezierCurve2 bc = new BezierCurve2();
										bc.addPoint(Vector2((float)vec[prevPoint].x, (float)vec[prevPoint].y) / 64.0f);
										bc.addPoint(Vector2((float)vec[j].x, (float)vec[j].y) / 64.0f);
										bc.addPoint(Vector2((float)vec[nextIndex].x, (float)vec[nextIndex].y) / 64.0f);
										s.appendShape(bc.realizeShape());
									}
								}
								else
								{
									Vector2[] conicPoint = new Vector2[j]((float)vec.x, (float)vec[j].y);
									if (j == startPos)
									{
										if ((FT_CURVE_TAG(tags[endPos-1]) != FT_CURVE_TAG_ON) && (FT_CURVE_TAG(tags[endPos-1]) != FT_CURVE_TAG_CUBIC))
										{
											Vector2[] lastConnic = new Vector2[endPos - 1]((float)vec.x, (float)vec[endPos - 1].y);
											lastPoint = (conicPoint + lastConnic) / 2;
										}
									}
	
									int nextIndex = j + 1;
									if (nextIndex >= endPos)
										nextIndex = startPos;
	
									Vector2[] nextPoint = new Vector2[nextIndex]((float)vec.x, (float)vec[nextIndex].y);
	
									bool nextIsConnic = (FT_CURVE_TAG(tags[nextIndex]) != FT_CURVE_TAG_ON) && (FT_CURVE_TAG(tags[nextIndex]) != FT_CURVE_TAG_CUBIC);
									if (nextIsConnic)
										nextPoint = (conicPoint + nextPoint) / 2;
	
									int pc = s.getPointCount();
									BezierCurve2 bc = new BezierCurve2();
									if (pc == 0)
										bc.addPoint(Vector2.ZERO);
									else
										bc.addPoint(s.getPoint(pc - 1));
									bc.addPoint(lastPoint / 64.0f);
									bc.addPoint(conicPoint / 64.0f);
									bc.addPoint(nextPoint / 64.0f);
									if (pc == 0)
										s.appendShape(bc.realizeShape());
									else
									{
										List<Vector2> subShape = bc.realizeShape().getPoints();
										for (List<Vector2>.Enumerator iter = subShape.GetEnumerator(); iter.MoveNext(); iter++)
										{
											if (iter != subShape.GetEnumerator())
												s.addPoint(iter.Current);
										}
									}
	
									if (nextIsConnic)
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: lastPoint = nextPoint;
										lastPoint=(nextPoint);
								}
							}
						}
					}
	
					s.close();
					s.translate((float)px, (float)py);
					retVal.addShape(s);
	
					px += slot.advance.x >> 6;
					py += slot.advance.y >> 6;
				}
				FT_Done_Face(face);
			}
			FT_Done_FreeType(ftlib);
		}
		else
		{
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "FreeType ERROR: FT_Init_FreeTyp", "Procedural::TextShape::realizeShapes()", __FILE__, __LINE__);
			;
		}
	
		return retVal;
	}

	private String getFontFileByName()
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
			return String(windows) + "\\fonts\\" + ff;
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
		if (fontName.empty())
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
#endif



}