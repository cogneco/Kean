using System;
using NUnit.Framework;

namespace Kean.Math.Algebra.Test
{
	public class Simplify :
	Kean.Test.Fixture<Simplify>
	{
		protected override void Run()
		{
			this.Run(
				this.AddingConstants,
				this.SubstractingConstants,
				this.MultiplyingConstants,
				this.DividingConstants,
				this.PowerOfConstants,
				this.SineConstants
			);
		}
		[Test]
		public void AddingConstants()
		{
			Expression expression = (Expression)2.3f + 1.9f;
			Verify(expression.Simplify(), Is.EqualTo((Expression)4.2f));
			expression += 2.56f;
			Verify(expression.Simplify(), Is.EqualTo((Expression)6.76f));
		}
		[Test]
		public void SubstractingConstants()
		{
			Expression before = -(Expression)1.9f - 1.2f;
			Expression after = -3.1f;
			Verify(before.Simplify(), Is.EqualTo(after));
		}
		[Test]
		public void MultiplyingConstants()
		{
			Expression before = -(Expression)9f * -3f;
			Expression after = 27f;
			Verify(before.Simplify(), Is.EqualTo(after));
		}
		[Test]
		public void DividingConstants()
		{
			Expression before = (Expression)(7f * -3f) / 2f;
			Expression after = -10.5f;
			Verify(before.Simplify(), Is.EqualTo(after));
		}
		[Test]
		public void PowerOfConstants()
		{
			Expression before = (Expression)4f ^ 2f;
			Expression after = 16f;
			Verify(before.Simplify(), Is.EqualTo(after));
		}
		[Test]
		public void SineConstants()
		{
			Expression exp0 = Variable.Create("x");
			Expression exp1 = Variable.Create("y");
			Expression befor = new Sine(exp0 + exp1 + exp0);
			Expression after = new Sine(2 * exp0 + exp1);
			Verify(befor.Simplify(), Is.EqualTo(after));
		}
	}
}