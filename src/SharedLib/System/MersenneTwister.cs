/* 
   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

   1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.

   3. The names of its contributors may not be used to endorse or promote 
   products derived from this software without specific prior written 
   permission.

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

   This C# porting is done by stlalv on October 8, 2010. 
   e-mail:stlalv@nifty.com
   Original C code is found at http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/MT2002/emt19937ar.html as mt19937ar.tgz
*/

namespace System
{
    /// <summary>
    /// A pseudorandom number generator with Mersenne Twister(MT) algorithm.
    /// See: http://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/mt.html
    /// </summary>
    /// <remarks>Can not use this class as cryptographically secure pseudo random number.</remarks>
    public class MersenneTwister 
    {
	    // Period parameters  
	    private const int       N          = 624;
	    private const int       M          = 397;
	    private const uint     MatrixA   = 0x9908b0df;	    // constant vector a
	    private const uint     UpperMask = 0x80000000;	    // most significant w-r bits
	    private const uint     LowerMask = 0x7fffffff;	    // least significant r bits
	    private uint[] mt         = new uint[N];	// the array for the state vector
	    private int     mti        = N + 1;         // mti==N+1 means mt[N] is not initialized

		#region Constructors

		/// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class.
        /// </summary>
        public MersenneTwister()
        {
            // set default seeds
            this.init_by_array(new uint[] { 0x123, 0x234, 0x345, 0x456 });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        public MersenneTwister(uint seed)
        {
            this.init_genrand(seed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwister"/> class.
        /// </summary>
        /// <param name="seeds">The seeds for the random array.</param>
        /// <exception cref="ArgumentNullException">The <c>seeds</c> is <c>null</c>.</exception>
        public MersenneTwister(uint[] seeds)
        {
            if (seeds is null)
            {
                throw new ArgumentNullException(nameof(seeds));
            }

            this.init_by_array(seeds);
        }

		#endregion

        #region Public methods

        /// <summary>
        /// Generates a random number on [0,0xffffffff]-interval.
        /// </summary>
        /// <returns>A random number.</returns>
        public uint RandUint32() 
        {
	        uint[] mag01 = { 0x0, MatrixA };
	        uint y;
	        // mag01[x] = x * MATRIX_A  for x=0,1
	        if (this.mti >= N) 
            {	
                // generate N words at one time
		        int kk;
    		    if (this.mti == N + 1) 
                {
                    // if init_genrand() has not been called,
                    this.init_genrand(5489);	// a default initial seed is used
    		    }

                for (kk = 0; kk < N - M; kk++) 
                {
                    y = (this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask);
                    this.mt[kk] = this.mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                for (; kk < N - 1; kk++) 
                {
                    y = (this.mt[kk] & UpperMask) | (this.mt[kk + 1] & LowerMask);
                    this.mt[kk] = this.mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                y = (this.mt[N - 1] & UpperMask) | (this.mt[0] & LowerMask);
                this.mt[N - 1] = this.mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];
                this.mti = 0;
	        }

	        y = this.mt[this.mti++];
	        // Tempering
	        y ^= (y >> 11);
	        y ^= (y <<  7) & 0x9d2c5680;
	        y ^= (y << 15) & 0xefc60000;
	        y ^= (y >> 18);

	        return y;
	    }

        /// <summary>
        /// Generates a random floating point number on [0, 1) or [0,1].
        /// [0, 1): greater than or equal to 0.0, and less than 1.0.
        /// [0, 1]: greater than or equal to 0.0, and less than or equal to 1.0.
        /// </summary>
        /// <returns>A random floating point number on [0,1), if the <c>lessThan1</c> is <c>true</c>, Otherwise; A random number floating point on [0, 1].</returns>
        public double Rand(bool lessThan1 = true) 
        {
            if (lessThan1)
            {
                return this.RandUint32() * (1.0 / 4294967296.0);	// divided by 2^32
            }

            return this.RandUint32() * (1.0 / 4294967295.0);	// divided by 2^32-1
        }

        /// <summary>
        /// Generates a random integer number from 0 to <c>maxValue</c> - 1.
        /// </summary>
        /// <param name="maxValue">The max value.</param>
        /// <returns>A random integer number from 0 to (<c>maxValue</c> - 1).</returns>
        public int RandInt32WithRange(int maxValue) 
        {
	        return (int)(this.RandUint32() * (maxValue / 4294967296.0));
	    }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the array for the state vector with a seed.
        /// </summary>
        /// <param name="s">The seed.</param>
        private void init_genrand(uint s)
        {
            this.mt[0] = s & 0xffffffff;
            for (this.mti = 1; this.mti < N; this.mti++)
            {
                this.mt[this.mti] = (1812433253 * (this.mt[this.mti - 1] ^ (this.mt[this.mti - 1] >> 30)) + (uint)this.mti);
                /* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
                /* In the previous versions, MSBs of the seed affect   */
                /* only MSBs of the array mt[].                        */
                /* 2002/01/09 modified by Makoto Matsumoto             */
                this.mt[this.mti] &= 0xffffffff;
                /* for >32 bit machines */
            }
        }

        /// <summary>
        /// Initialize by an array with array-length 
        /// </summary>
        /// <param name="initKey">The array for initializing keys</param>
        private void init_by_array(uint[] initKey)
        {
            this.init_genrand(19650218);
            int i = 1;
            int j = 0;
            int k = (N > initKey.Length ? N : initKey.Length);
            for (; k != 0; k--)
            {
                this.mt[i] = (this.mt[i] ^ ((this.mt[i - 1] ^ (this.mt[i - 1] >> 30)) * 1664525)) + initKey[j] + (uint)j; /* non linear */
                this.mt[i] &= 0xffffffff; /* for WORDSIZE > 32 machines */
                i++; j++;
                if (i >= N)
                {
                    this.mt[0] = this.mt[N - 1]; i = 1;
                }

                if (j >= initKey.Length)
                {
                    j = 0;
                }
            }
            for (k = N - 1; k != 0; k--)
            {
                this.mt[i] = (this.mt[i] ^ ((this.mt[i - 1] ^ (this.mt[i - 1] >> 30)) * 1566083941)) - (uint)i; // non linear
                this.mt[i] &= 0xffffffff; // for WORDSIZE > 32 machines
                i++;
                if (i >= N)
                {
                    this.mt[0] = this.mt[N - 1]; i = 1;
                }
            }

            this.mt[0] = 0x80000000; // MSB is 1; assuring non-zero initial array 
        }

        #endregion
    }
}
