using System;
using Kean;
using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Password
{
	public abstract class Generator :
	Generic.IEnumerable<string>
	{
		readonly System.Security.Cryptography.RandomNumberGenerator backend = new System.Security.Cryptography.RNGCryptoServiceProvider();
		public int Length { get; set; }
		public bool WithDigits { get; set; }
		public bool WithUppercase { get; set; }
		protected Generator()
		{
			this.Length = 8;
			this.WithDigits = true;
			this.WithUppercase = true;
		}
		protected int Generate (int count)
		{
			int length = Math.Integer.Ceiling(Math.Single.Floor(Math.Single.Logarithm(count, 2) + 1) / 8f);
			int result;
			do
			{
				result = 0;
				byte[] random = new byte[length];
				this.backend.GetBytes(random);
				foreach (var b in random)
					result = result << 8 | b;
			}
			while (result >= count || result < 0);
			return result;
		}
		public abstract string Next ();
		#region IEnumerable implementation
		Generic.IEnumerator<string> Generic.IEnumerable<string>.GetEnumerator ()
		{
			yield return this.Next();
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			yield return this.Next();
		}
		#endregion
	}
}

