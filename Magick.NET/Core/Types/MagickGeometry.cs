﻿//=================================================================================================
// Copyright 2013-2016 Dirk Lemstra <https://magick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================

namespace ImageMagick
{
  ///<summary>
  /// Encapsulation of the ImageMagick geometry object.
  ///</summary>
  public sealed partial class MagickGeometry
  {
    private MagickGeometry(NativeMagickGeometry instance)
    {
      Initialize(instance);
    }

    private NativeMagickGeometry CreateNativeInstance()
    {
      NativeMagickGeometry instance = new NativeMagickGeometry();
      instance.Initialize(ToString());

      return instance;
    }

    private void Initialize(int x, int y, int width, int height, bool isPercentage)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
      IsPercentage = isPercentage;
    }

    private void Initialize(NativeMagickGeometry instance)
    {
      X = (int)instance.X;
      Y = (int)instance.Y;
      Width = (int)instance.Width;
      Height = (int)instance.Height;
    }

    private void Initialize(NativeMagickGeometry instance, GeometryFlags flags)
    {
      Throw.IfTrue("value", flags == GeometryFlags.NoValue, "Invalid geometry specified.");

      Initialize(instance);

      IsPercentage = EnumHelper.HasFlag(flags, GeometryFlags.PercentValue);
      IgnoreAspectRatio = EnumHelper.HasFlag(flags, GeometryFlags.IgnoreAspectRatio);
      FillArea = EnumHelper.HasFlag(flags, GeometryFlags.FillArea);
      Greater = EnumHelper.HasFlag(flags, GeometryFlags.Greater);
      Less = EnumHelper.HasFlag(flags, GeometryFlags.Less);
      LimitPixels = EnumHelper.HasFlag(flags, GeometryFlags.LimitPixels);
    }

    internal static MagickGeometry FromRectangle(MagickRectangle rectangle)
    {
      if (rectangle == null)
        return null;

      return new MagickGeometry(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    internal static MagickGeometry FromString(string value)
    {
      return value == null ? null : new MagickGeometry(value);
    }

    internal static string ToString(MagickGeometry value)
    {
      return value == null ? null : value.ToString();
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class.
    ///</summary>
    public MagickGeometry()
    {
      Initialize(0, 0, 0, 0, false);
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified width and
    /// height.
    ///</summary>
    ///<param name="widthAndHeight">The width and height.</param>
    public MagickGeometry(int widthAndHeight)
    {
      Initialize(0, 0, widthAndHeight, widthAndHeight, false);
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified width and
    /// height.
    ///</summary>
    ///<param name="width">The width.</param>
    ///<param name="height">The height.</param>
    public MagickGeometry(int width, int height)
    {
      Initialize(0, 0, width, height, false);
    }
    ///=======================================================================================
    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified offsets, width
    /// and height.
    ///</summary>
    ///<param name="x">The X offset from origin.</param>
    ///<param name="y">The Y offset from origin.</param>
    ///<param name="width">The width.</param>
    ///<param name="height">The height.</param>
    public MagickGeometry(int x, int y, int width, int height)
    {
      Initialize(x, y, width, height, false);
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified width and
    /// height.
    ///</summary>
    ///<param name="percentageWidth">The percentage of the width.</param>
    ///<param name="percentageHeight">The percentage of the  height.</param>
    public MagickGeometry(Percentage percentageWidth, Percentage percentageHeight)
    {
      Throw.IfNegative("percentageWidth", percentageWidth);
      Throw.IfNegative("percentageHeight", percentageHeight);

      Initialize(0, 0, (int)percentageWidth, (int)percentageHeight, true);
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified offsets, width
    /// and height.
    ///</summary>
    ///<param name="x">The X offset from origin.</param>
    ///<param name="y">The Y offset from origin.</param>
    ///<param name="percentageWidth">The percentage of the width.</param>
    ///<param name="percentageHeight">The percentage of the  height.</param>
    public MagickGeometry(int x, int y, Percentage percentageWidth, Percentage percentageHeight)
    {
      Throw.IfNegative("percentageWidth", percentageWidth);
      Throw.IfNegative("percentageHeight", percentageHeight);

      Initialize(x, y, (int)percentageWidth, (int)percentageHeight, true);
    }

    ///<summary>
    /// Initializes a new instance of the MagickGeometry class using the specified geometry.
    ///</summary>
    ///<param name="value">Geometry specifications in the form: &lt;width&gt;x&lt;height&gt;
    ///{+-}&lt;xoffset&gt;{+-}&lt;yoffset&gt; (where width, height, xoffset, and yoffset are numbers)</param>
    public MagickGeometry(string value)
    {
      Throw.IfNullOrEmpty("value", value);

      using (NativeMagickGeometry instance = new NativeMagickGeometry())
      {
        GeometryFlags flags = instance.Initialize(value);
        Initialize(instance, flags);
      }
    }

    /// <summary>
    /// Determines whether the specified MagickGeometry instances are considered equal.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator ==(MagickGeometry left, MagickGeometry right)
    {
      return Equals(left, right);
    }

    /// <summary>
    /// Determines whether the specified MagickGeometry instances are not considered equal.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator !=(MagickGeometry left, MagickGeometry right)
    {
      return !Equals(left, right);
    }

    /// <summary>
    /// Determines whether the first MagickGeometry is more than the second MagickGeometry.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator >(MagickGeometry left, MagickGeometry right)
    {
      if (ReferenceEquals(left, null))
        return ReferenceEquals(right, null);

      return left.CompareTo(right) == 1;
    }

    /// <summary>
    /// Determines whether the first MagickGeometry is less than the second MagickGeometry.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator <(MagickGeometry left, MagickGeometry right)
    {
      if (ReferenceEquals(left, null))
        return !ReferenceEquals(right, null);

      return left.CompareTo(right) == -1;
    }

    /// <summary>
    /// Determines whether the first MagickGeometry is more than or equal to the second MagickGeometry.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator >=(MagickGeometry left, MagickGeometry right)
    {
      if (ReferenceEquals(left, null))
        return ReferenceEquals(right, null);

      return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Determines whether the first MagickGeometry is less than or equal to the second MagickGeometry.
    /// </summary>
    /// <param name="left">The first MagickGeometry to compare.</param>
    /// <param name="right"> The second MagickGeometry to compare.</param>
    /// <returns></returns>
    public static bool operator <=(MagickGeometry left, MagickGeometry right)
    {
      if (ReferenceEquals(left, null))
        return !ReferenceEquals(right, null);

      return left.CompareTo(right) <= 0;
    }

    ///<summary>
    /// Converts the specified string to an instance of this type.
    ///</summary>
    ///<param name="value">Geometry specifications in the form: &lt;width&gt;x&lt;height&gt;
    ///{+-}&lt;xoffset&gt;{+-}&lt;yoffset&gt; (where width, height, xoffset, and yoffset are numbers)</param>
    public static explicit operator MagickGeometry(string value)
    {
      return new MagickGeometry(value);
    }

    ///<summary>
    /// Resize the image based on the smallest fitting dimension (^).
    ///</summary>
    public bool FillArea
    {
      get;
      set;
    }

    ///<summary>
    /// Resize if image is greater than size (&gt;)
    ///</summary>
    public bool Greater
    {
      get;
      set;
    }

    ///<summary>
    /// The height of the geometry.
    ///</summary>
    public int Height
    {
      get;
      set;
    }

    ///<summary>
    /// Resize without preserving aspect ratio (!)
    ///</summary>
    public bool IgnoreAspectRatio
    {
      get;
      set;
    }

    ///<summary>
    /// True if width and height are expressed as percentages.
    ///</summary>
    public bool IsPercentage
    {
      get;
      set;
    }

    ///<summary>
    /// Resize if image is less than size (&lt;)
    ///</summary>
    public bool Less
    {
      get;
      set;
    }

    ///<summary>
    /// Resize using a pixel area count limit (@).
    ///</summary>
    public bool LimitPixels
    {
      get;
      set;
    }

    ///<summary>
    /// The width of the geometry.
    ///</summary>
    public int Width
    {
      get;
      set;
    }

    ///<summary>
    /// X offset from origin
    ///</summary>
    public int X
    {
      get;
      set;
    }

    ///<summary>
    /// Y offset from origin
    ///</summary>
    public int Y
    {
      get;
      set;
    }

    ///<summary>
    /// Compares the current instance with another object of the same type.
    ///</summary>
    ///<param name="other">The object to compare this geometry with.</param>
    public int CompareTo(MagickGeometry other)
    {

      if (ReferenceEquals(other, null))
        return 1;

      int left = (this.Width * this.Height);
      int right = (other.Width * other.Height);

      if (left == right)
        return 0;

      return left < right ? -1 : 1;
    }

    ///<summary>
    /// Determines whether the specified object is equal to the current geometry.
    ///</summary>
    ///<param name="obj">The object to compare this geometry with.</param>
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
        return true;

      return Equals(obj as MagickGeometry);
    }

    ///<summary>
    /// Determines whether the specified geometry is equal to the current geometry.
    ///</summary>
    ///<param name="other">The geometry to compare this geometry with.</param>
    public bool Equals(MagickGeometry other)
    {
      if (ReferenceEquals(other, null))
        return false;

      if (ReferenceEquals(this, other))
        return true;

      return
        Width == other.Width &&
        Height == other.Height &&
        X == other.X &&
        Y == other.Y &&
        IsPercentage == other.IsPercentage &&
        IgnoreAspectRatio == other.IgnoreAspectRatio &&
        Less == other.Less &&
        Greater == other.Greater &&
        FillArea == other.FillArea &&
        LimitPixels == other.LimitPixels;
    }

    ///<summary>
    /// Serves as a hash of this type.
    ///</summary>
    public override int GetHashCode()
    {
      return
        Width.GetHashCode() ^
        Height.GetHashCode() ^
        X.GetHashCode() ^
        Y.GetHashCode() ^
        IsPercentage.GetHashCode() ^
        IgnoreAspectRatio.GetHashCode() ^
        Less.GetHashCode() ^
        Greater.GetHashCode() ^
        FillArea.GetHashCode() ^
        LimitPixels.GetHashCode();
    }

    ///<summary>
    /// Returns a string that represents the current geometry.
    ///</summary>
    public override string ToString()
    {
      string result = null;

      if (Width > 0)
        result += Width;

      if (Height > 0)
        result += "x" + Height;

      if (X != 0 || Y != 0)
      {
        if (X >= 0)
          result += '+';

        result += X;

        if (Y >= 0)
          result += '+';

        result += Y;
      }

      if (IsPercentage)
        result += '%';

      if (IgnoreAspectRatio)
        result += '!';

      if (Greater)
        result += '>';

      if (Less)
        result += '<';

      if (FillArea)
        result += '^';

      if (LimitPixels)
        result += '@';

      return result;
    }
  }
}