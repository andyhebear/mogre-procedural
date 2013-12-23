using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * Represents a curve by interpolating between a list of key/values.
// * It always refers to a "principal" curve (a path, atm), so the keys to either its point index or lineic position.
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Track
public class Track
{
	/// Defines addressing mode for the track
	/// ABSOLUTE_LINEIC : use the distance from the start of the principal curve
	/// RELATIVE_LINEIC : use the relative distance from the start of the principal curve, considering the total length of main curve is 1.
	/// POINT : right on the principal curve's key
	public enum AddressingMode: int
	{
		AM_ABSOLUTE_LINEIC,
		AM_RELATIVE_LINEIC,
		AM_POINT
	}
	/// Adressing mode of the track (see the enum definition for more details)
	protected AddressingMode mAddressingMode;

	/// Tells whether we should add new points to principal curve if a key is defined here but not on principal curve
	protected bool mInsertPoint;

	/// Key frames
	protected std.map<float, float> mKeyFrames = new std.map<float, float>();
	/// Default constructor.
	/// Point insertion default to true, and addressing to relative lineic
	public Track(AddressingMode addressingMode) : this(addressingMode, true)
	{
	}
	public Track() : this(AM_RELATIVE_LINEIC, true)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: Track(AddressingMode addressingMode=AM_RELATIVE_LINEIC, bool insertPoint=true) : mAddressingMode(addressingMode), mInsertPoint(insertPoint)
	public Track(AddressingMode addressingMode, bool insertPoint)
	{
		mAddressingMode = addressingMode;
		mInsertPoint = insertPoint;
	}

	/// Gets addressing mode of the curve
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: AddressingMode getAddressingMode() const
	public AddressingMode getAddressingMode()
	{
		return mAddressingMode;
	}

	/// Inserts a new Key/Value couple anywhere on the track (it is auto-sorted anyway)
	public Track addKeyFrame(float pos, float @value)
	{
		mKeyFrames[pos] = @value;
		return this;
	}

	/// @copydoc Track::mInsertPoint
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline bool isInsertPoint() const
	public bool isInsertPoint()
	{
		return mInsertPoint;
	}

	/// Gets the value on the current point, taking into account the addressing mode
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getValue(float absPos, float relPos, uint index) const
	public float getValue(float absPos, float relPos, uint index)
	{
		if (mAddressingMode == AM_ABSOLUTE_LINEIC)
			return getValue(absPos);
		if (mAddressingMode == AM_RELATIVE_LINEIC)
			return getValue(relPos);
		return getValue((float)index);
	}

	/// Gets the value on the current point
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float getValue(float pos) const
	public float getValue(float pos)
	{
		std.map<Real, Real>.Enumerator itAfter = _getKeyValueAfter(pos);
		std.map<Real, Real>.Enumerator itBefore = _getKeyValueBefore(pos);
	
		if (itAfter ==itBefore)
			return itBefore.second;
		if (itAfter ==mKeyFrames.begin())
			return itAfter.second;
	
		float x1 = itBefore.first;
		float y1 = itBefore.second;
		float x2 = itAfter.first;
		float y2 = itAfter.second;
		return (pos-x1)/(x2-x1)*(y2-y1)+y1;
	}

	/// Get the key value couple before current point, taking into account addressing mode.
	/// If current point is below minimum key, issues minimum key
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float absPos, float relPos, uint index) const
	public std.map<Real, Real>.Enumerator _getKeyValueBefore(float absPos, float relPos, uint index)
	{
		if (mAddressingMode == AM_ABSOLUTE_LINEIC)
			return _getKeyValueBefore(absPos);
		if (mAddressingMode == AM_RELATIVE_LINEIC)
			return _getKeyValueBefore(relPos);
		return _getKeyValueBefore((float)index);
	}

	/// Get the key value couple before current point.
	/// If current point is below minimum key, issues minimum key/value
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float pos) const
	public std.map<Real, Real>.Enumerator _getKeyValueBefore(float pos)
	{
		std.map<Real, Real>.Enumerator it = mKeyFrames.upper_bound(pos);
		if (it ==mKeyFrames.begin())
			return it;
		else
			return --it;
	}

	/// Get the key value couple after current point, taking into account addressing mode.
	/// If current point is above maximum key, issues maximum key/value
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index) const
	public std.map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index)
	{
		if (mAddressingMode == AM_ABSOLUTE_LINEIC)
			return _getKeyValueAfter(absPos);
		if (mAddressingMode == AM_RELATIVE_LINEIC)
			return _getKeyValueAfter(relPos);
		return _getKeyValueAfter((float)index);
	}

	/// Get the key value couple after current point.
	/// If current point is above maximum key, issues maximum key/value
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float pos) const
	public std.map<Real, Real>.Enumerator _getKeyValueAfter(float pos)
	{
		std.map<Real, Real>.Enumerator it = mKeyFrames.upper_bound(pos);
		if (it ==mKeyFrames.end())
			return --it;
		else
			return it;
	}

	/// Gets the first value in the track
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float getFirstValue() const
	public float getFirstValue()
	{
		return mKeyFrames.begin().second;
	}

	/// Gets the last value in the track
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float getLastValue() const
	public float getLastValue()
	{
		return (--mKeyFrames.end()).second;
	}
}
//---------------------------------------------------


}
