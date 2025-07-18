/* 
   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace System
{
    /// <summary>
    /// Represents a pseudo-random number generator with MersenneTwister (MT) algorithm.
    /// </summary>
    public class RandomMT : Random
    {
        #region Fields

        private readonly MersenneTwister randomGenerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instances of the <see cref="RandomMT"/> class.
        /// </summary>
        public RandomMT()
        {
            this.randomGenerator = new MersenneTwister();
        }

        /// <summary>
        /// Initializes a new instances of the <see cref="RandomMT"/> class.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        public RandomMT(uint seed)
        {
            this.randomGenerator = new MersenneTwister(seed);
        }

        /// <summary>
        /// Initializes a new instances of the <see cref="RandomMT"/> class.
        /// </summary>
        /// <param name="seeds">The random seeds for initializing a starting value.</param>
        /// <exception cref="ArgumentNullException">The <c>seeds</c> is <c>null</c>.</exception>
        public RandomMT(uint[] seeds)
        {
            this.randomGenerator = new MersenneTwister(seeds);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a non-negative random unsigned integer that is less than or equal to 0xFFFFFFFF(=4,294,967,295).
        /// </summary>
        /// <returns>A random number.</returns>
        public virtual uint NextUint32()
        {
            return this.randomGenerator.RandUint32();
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum (<c>maxValue</c> - 1).
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0, and less than <c>maxValue</c>; that is, the range of return values ordinarily includes 0 but not <c>maxValue</c>.
        /// However, if <c>maxValue</c> equals 0, <c>maxValue</c> is returned.
        /// </returns>
        public override int Next(int maxValue)
        {
            return maxValue == 0 ? 0 : this.randomGenerator.RandInt32WithRange(maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <c>maxValue</c> must be greater than or equal to <c>minValue</c>.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <c>minValue</c> and less than <c>maxValue</c>; that is, the range of return values includes <c>minValue</c> but not <c>maxValue</c>.
        /// If <c>minValue</c> equals <c>maxValue</c>, <c>minValue</c> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>minValue</c> is greater than <c>maxValue</c>.</exception>
        public override int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), minValue, $"{nameof(minValue)} is greater than {nameof(maxValue)}.");
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            return this.Next(maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">The array to be filled with random numbers.</param>
        /// <exception cref="ArgumentNullException"><c>buffer</c> is <c>null</c>.</exception>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            for (var i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)this.Next(byte.MaxValue + 1);
            }
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        public override double NextDouble()
        {
            return this.randomGenerator.Rand();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Returns a random floating-point number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than or equal to 1.0.</returns>
        protected override double Sample()
        {
            return this.randomGenerator.Rand(false);
        }

        #endregion
    }
}
