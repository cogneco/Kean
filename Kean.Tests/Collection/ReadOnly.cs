using System;
using NUnit.Framework;

using Target = Kean.Collection;

namespace Kean.Collection.Test
{
    [TestFixture]
    public class ReadOnly :
        Kean.Test.Fixture<ReadOnly>
    {
        string prefix = "Kean.Collection.Test.";
        protected override void Run()
        {
            this.Run(this.Create, this.Equality, this.Count);
        }
        [Test]
        public void Create()
        {
            Target.ReadOnlyVector<int> vector = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
           Verify(vector[0], Is.EqualTo(0), this.prefix + "Create.0");
           Verify(vector[1], Is.EqualTo(1), this.prefix + "Create.1");
           Verify(vector[2], Is.EqualTo(2), this.prefix + "Create.2");
           Verify(vector[3], Is.EqualTo(3), this.prefix + "Create.3");
           Verify(vector[4], Is.EqualTo(4), this.prefix + "Create.4");
        }
        [Test]
        public void Equality()
        {
            Target.ReadOnlyVector<int> vector1 = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
            Target.ReadOnlyVector<int> vector2 = new ReadOnlyVector<int>(0, 1, 2, 3, 4);
            Target.ReadOnlyVector<int> vector3 = new ReadOnlyVector<int>(0, 1, 20, 3, 4);
           Verify(vector1, Is.EqualTo(vector2), this.prefix + "Equality.0");
           Verify(vector1, Is.Not.EqualTo(vector3), this.prefix + "Equality.1");
        }
        [Test]
        public new void Count()
        {
            Target.ReadOnlyVector<int> vector = new ReadOnlyVector<int>();
           Verify(vector.Count, Is.EqualTo(0), this.prefix + "Count.0");
            vector = new ReadOnlyVector<int>(1,2,3);
           Verify(vector.Count, Is.EqualTo(3), this.prefix + "Count.1");
        }
    }
}
