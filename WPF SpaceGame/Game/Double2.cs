// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ControlzEx.Standard;
using Microsoft.Xna.Framework;

namespace WPFSpaceGame.Game
{
    /// <summary>
    /// Describes a 2D-vector with a double.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Double2 : IEquatable<Double2>
    {
        #region Private Fields

        private static readonly Double2 zeroVector = new Double2(0f, 0f);
        private static readonly Double2 unitVector = new Double2(1f, 1f);
        private static readonly Double2 unitXVector = new Double2(1f, 0f);
        private static readonly Double2 unitYVector = new Double2(0f, 1f);
        
        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Double2"/>.
        /// </summary>
        [DataMember]
        public double X;

        /// <summary>
        /// The y coordinate of this <see cref="Double2"/>.
        /// </summary>
        [DataMember]
        public double Y;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a <see cref="Double2"/> with components 0, 0.
        /// </summary>
        public static Double2 Zero
        {
            get { return zeroVector; }
        }

        /// <summary>
        /// Returns a <see cref="Double2"/> with components 1, 1.
        /// </summary>
        public static Double2 One
        {
            get { return unitVector; }
        }

        /// <summary>
        /// Returns a <see cref="Double2"/> with components 1, 0.
        /// </summary>
        public static Double2 UnitX
        {
            get { return unitXVector; }
        }

        /// <summary>
        /// Returns a <see cref="Double2"/> with components 0, 1.
        /// </summary>
        public static Double2 UnitY
        {
            get { return unitYVector; }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Double2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Double2(double value)
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Inverts values in the specified <see cref="Double2"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Double2 operator -(Double2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Double2"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Double2 operator +(Double2 value1, Double2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Double2"/> from a <see cref="Double2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Double2"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Double2 operator -(Double2 value1, Double2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Double2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Double2 operator *(Double2 value1, Double2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Double2 operator *(Double2 value, double scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Double2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Double2 operator *(double scaleFactor, Double2 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by the components of another <see cref="Double2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Double2"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double2 operator /(Double2 value1, Double2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double2 operator /(Double2 value1, double divider)
        {
            double factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Compares whether two <see cref="Double2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Double2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Double2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Double2 value1, Double2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Compares whether two <see cref="Double2"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="Double2"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Double2"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=(Double2 value1, Double2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Double2 Add(Double2 value1, Double2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and
        /// <paramref name="value2"/>, storing the result of the
        /// addition in <paramref name="result"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Double2 Barycentric(Double2 value1, Double2 value2, Double2 value3, double amount1, double amount2)
        {
            return new Double2(
                DoubleHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                DoubleHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(ref Double2 value1, ref Double2 value2, ref Double2 value3, double amount1, double amount2, out Double2 result)
        {
            result.X = DoubleHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = DoubleHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Double2 CatmullRom(Double2 value1, Double2 value2, Double2 value3, Double2 value4, double amount)
        {
            return new Double2(
                DoubleHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                DoubleHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(ref Double2 value1, ref Double2 value2, ref Double2 value3, ref Double2 value4, double amount, out Double2 result)
        {
            result.X = DoubleHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = DoubleHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        /// <summary>
        /// Round the members of this <see cref="Double2"/> towards positive infinity.
        /// </summary>
        public void Ceiling()
        {
            X = (double)Math.Ceiling(X);
            Y = (double)Math.Ceiling(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <returns>The rounded <see cref="Double2"/>.</returns>
        public static Double2 Ceiling(Double2 value)
        {
            value.X = (double)Math.Ceiling(value.X);
            value.Y = (double)Math.Ceiling(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded towards positive infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="result">The rounded <see cref="Double2"/>.</param>
        public static void Ceiling(ref Double2 value, out Double2 result)
        {
            result.X = (double)Math.Ceiling(value.X);
            result.Y = (double)Math.Ceiling(value.Y);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Double2 Clamp(Double2 value1, Double2 min, Double2 max)
        {
            return new Double2(
                DoubleHelper.Clamp(value1.X, min.X, max.X),
                DoubleHelper.Clamp(value1.Y, min.Y, max.Y));
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(ref Double2 value1, ref Double2 min, ref Double2 max, out Double2 result)
        {
            result.X = DoubleHelper.Clamp(value1.X, min.X, max.X);
            result.Y = DoubleHelper.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static double Distance(Double2 value1, Double2 value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (double)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Double2 value1, ref Double2 value2, out double result)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (double)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static double DistanceSquared(Double2 value1, Double2 value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(ref Double2 value1, ref Double2 value2, out double result)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by the components of another <see cref="Double2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Divisor <see cref="Double2"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Double2 Divide(Double2 value1, Double2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by the components of another <see cref="Double2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Divisor <see cref="Double2"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Double2 Divide(Double2 value1, double divider)
        {
            double factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Double2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Double2 value1, double divider, out Double2 result)
        {
            double factor = 1 / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static double Dot(Double2 value1, Double2 value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Double2 value1, ref Double2 value2, out double result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Double2)
            {
                return Equals((Double2)obj);
            }

            return false;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Double2"/>.
        /// </summary>
        /// <param name="other">The <see cref="Double2"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Double2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        /// <summary>
        /// Round the members of this <see cref="Double2"/> towards negative infinity.
        /// </summary>
        public void Floor()
        {
            X = (double)Math.Floor(X);
            Y = (double)Math.Floor(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <returns>The rounded <see cref="Double2"/>.</returns>
        public static Double2 Floor(Double2 value)
        {
            value.X = (double)Math.Floor(value.X);
            value.Y = (double)Math.Floor(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded towards negative infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="result">The rounded <see cref="Double2"/>.</param>
        public static void Floor(ref Double2 value, out Double2 result)
        {
            result.X = (double)Math.Floor(value.X);
            result.Y = (double)Math.Floor(value.Y);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Double2"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Double2"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Double2 Hermite(Double2 value1, Double2 tangent1, Double2 value2, Double2 tangent2, double amount)
        {
            return new Double2(DoubleHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount), DoubleHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(ref Double2 value1, ref Double2 tangent1, ref Double2 value2, ref Double2 tangent2, double amount, out Double2 result)
        {
            result.X = DoubleHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = DoubleHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

        /// <summary>
        /// Returns the length of this <see cref="Double2"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Double2"/>.</returns>
        public double Length()
        {
            return (double)Math.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Double2"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Double2"/>.</returns>
        public double LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Double2 Lerp(Double2 value1, Double2 value2, double amount)
        {
            return new Double2(
                DoubleHelper.Lerp(value1.X, value2.X, amount),
                DoubleHelper.Lerp(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(ref Double2 value1, ref Double2 value2, double amount, out Double2 result)
        {
            result.X = DoubleHelper.Lerp(value1.X, value2.X, amount);
            result.Y = DoubleHelper.Lerp(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="DoubleHelper.LerpPrecise"/> on DoubleHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="Double2.Lerp(Double2, Double2, double)"/>.
        /// See remarks section of <see cref="DoubleHelper.LerpPrecise"/> on DoubleHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Double2 LerpPrecise(Double2 value1, Double2 value2, double amount)
        {
            return new Double2(
                DoubleHelper.LerpPrecise(value1.X, value2.X, amount),
                DoubleHelper.LerpPrecise(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains linear interpolation of the specified vectors.
        /// Uses <see cref="DoubleHelper.LerpPrecise"/> on DoubleHelper for the interpolation.
        /// Less efficient but more precise compared to <see cref="Double2.Lerp(ref Double2, ref Double2, double, out Double2)"/>.
        /// See remarks section of <see cref="DoubleHelper.LerpPrecise"/> on DoubleHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void LerpPrecise(ref Double2 value1, ref Double2 value2, double amount, out Double2 result)
        {
            result.X = DoubleHelper.LerpPrecise(value1.X, value2.X, amount);
            result.Y = DoubleHelper.LerpPrecise(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Double2"/> with maximal values from the two vectors.</returns>
        public static Double2 Max(Double2 value1, Double2 value2)
        {
            return new Double2(value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Double2"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Double2"/> with minimal values from the two vectors.</returns>
        public static Double2 Min(Double2 value1, Double2 value2)
        {
            return new Double2(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Double2"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Double2 Multiply(Double2 value1, Double2 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a multiplication of <see cref="Double2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Double2 Multiply(Double2 value1, double scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a multiplication of <see cref="Double2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Double2 value1, double scaleFactor, out Double2 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Double2 Negate(Double2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Double2 value, out Double2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Turns this <see cref="Double2"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            double val = 1.0f / (double)Math.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Double2 Normalize(Double2 value)
        {
            double val = 1.0f / (double)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Double2 value, out Double2 result)
        {
            double val = 1.0f / (double)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Double2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Double2 Reflect(Double2 vector, Double2 normal)
        {
            Double2 result;
            double val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Double2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Double2 vector, ref Double2 normal, out Double2 result)
        {
            double val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
        }


        public static void Rotate(ref Double2 value, double theta)
        {
            var x = value.X;
            var y = value.Y;

            value.X = x * Math.Cos(theta) - y * Math.Sin(theta);
            value.Y = x * Math.Sin(theta) + y * Math.Cos(theta); 
        }


        /// <summary>
        /// Round the members of this <see cref="Double2"/> to the nearest integer value.
        /// </summary>
        public void Round()
        {
            X = (double)Math.Round(X);
            Y = (double)Math.Round(Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <returns>The rounded <see cref="Double2"/>.</returns>
        public static Double2 Round(Double2 value)
        {
            value.X = (double)Math.Round(value.X);
            value.Y = (double)Math.Round(value.Y);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains members from another vector rounded to the nearest integer value.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="result">The rounded <see cref="Double2"/>.</param>
        public static void Round(ref Double2 value, out Double2 result)
        {
            result.X = (double)Math.Round(value.X);
            result.Y = (double)Math.Round(value.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Double2 SmoothStep(Double2 value1, Double2 value2, double amount)
        {
            return new Double2(
                DoubleHelper.SmoothStep(value1.X, value2.X, amount),
                DoubleHelper.SmoothStep(value1.Y, value2.Y, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(ref Double2 value1, ref Double2 value2, double amount, out Double2 result)
        {
            result.X = DoubleHelper.SmoothStep(value1.X, value2.X, amount);
            result.Y = DoubleHelper.SmoothStep(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains subtraction of on <see cref="Double2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Double2 Subtract(Double2 value1, Double2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains subtraction of on <see cref="Double2"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Double2"/>.</param>
        /// <param name="value2">Source <see cref="Double2"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Double2 value1, ref Double2 value2, out Double2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Double2"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Double2"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }


        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Double2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed <see cref="Double2"/>.</returns>
        public static Double2 Transform(Double2 position, Matrix matrix)
        {
            return new Double2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of 2d-vector by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Double2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed <see cref="Double2"/> as an output parameter.</param>
        public static void Transform(ref Double2 position, ref Matrix matrix, out Double2 result)
        {
            var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Double2"/>.</returns>
        public static Double2 Transform(Double2 value, Quaternion rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of 2d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Double2"/>.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Double2"/> as an output parameter.</param>
        public static void Transform(ref Double2 value, ref Quaternion rotation, out Double2 result)
        {
            var rot1 = new Vector3(rotation.X + rotation.X, rotation.Y + rotation.Y, rotation.Z + rotation.Z);
            var rot2 = new Vector3(rotation.X, rotation.X, rotation.W);
            var rot3 = new Vector3(1, rotation.Y, rotation.Z);
            var rot4 = rot1 * rot2;
            var rot5 = rot1 * rot3;

            var v = new Double2();
            v.X = (double)((double)value.X * (1.0 - (double)rot5.Y - (double)rot5.Z) + (double)value.Y * ((double)rot4.Y - (double)rot4.Z));
            v.Y = (double)((double)value.X * ((double)rot4.Y + (double)rot4.Z) + (double)value.Y * (1.0 - (double)rot4.X - (double)rot5.Z));
            result.X = v.X;
            result.Y = v.Y;
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Double2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Double2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Double2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            Double2[] destinationArray,
            int destinationIndex,
            int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];
                destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
                destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Double2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Double2"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform
        (
            Double2[] sourceArray,
            int sourceIndex,
            ref Quaternion rotation,
            Double2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                var position = sourceArray[sourceIndex + x];
                var destination = destinationArray[destinationIndex + x];

                Double2 v;
                Transform(ref position, ref rotation, out v);

                destination.X = v.X;
                destination.Y = v.Y;

                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Double2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Double2[] sourceArray,
            ref Matrix matrix,
            Double2[] destinationArray)
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Double2"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform
        (
            Double2[] sourceArray,
            ref Quaternion rotation,
            Double2[] destinationArray
        )
        {
            Transform(sourceArray, 0, ref rotation, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Double2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Double2 TransformNormal(Double2 normal, Matrix matrix)
        {
            return new Double2((normal.X * matrix.M11) + (normal.Y * matrix.M21), (normal.X * matrix.M12) + (normal.Y * matrix.M22));
        }

        /// <summary>
        /// Creates a new <see cref="Double2"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Double2"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref Double2 normal, ref Matrix matrix, out Double2 result)
        {
            var x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            var y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Double2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Double2"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal
        (
            Double2[] sourceArray,
            int sourceIndex,
            ref Matrix matrix,
            Double2[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int i = 0; i < length; i++)
            {
                var normal = sourceArray[sourceIndex + i];

                destinationArray[destinationIndex + i] = new Double2((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                                     (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Double2"/> by the specified <see cref="Matrix"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal
            (
            Double2[] sourceArray,
            ref Matrix matrix,
            Double2[] destinationArray
            )
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");
            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                var normal = sourceArray[i];

                destinationArray[i] = new Double2((normal.X * matrix.M11) + (normal.Y * matrix.M21),
                                                  (normal.X * matrix.M12) + (normal.Y * matrix.M22));
            }
        }

        /// <summary>
        /// Deconstruction method for <see cref="Double2"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Deconstruct(out double x, out double y)
        {
            x = X;
            y = Y;
        }

        #endregion


        #region TREVORS



        public static bool TryGetIntersection(Double2 p0, Double2 p1, Double2 p2, Double2 p3, out Double2 intersection)
        {            
            double i_x = 0;
            double i_y = 0;
            var success = TryGetIntersection(p0.X, p0.Y, p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, out i_x, out i_y);
            intersection = new Double2(i_x, i_y);
            return success;
        }

        private static bool TryGetIntersection(double p0_x, double p0_y, double p1_x, double p1_y,
            double p2_x, double p2_y, double p3_x, double p3_y, out double i_x, out double i_y)
        {
            double s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            double s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected            
                i_x = p0_x + (t * s1_x);
                i_y = p0_y + (t * s1_y);
                return true;
            }

            i_x = 0;
            i_y = 0;
            return false; // No collision
        }

        #endregion

    }
}