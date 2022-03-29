using NUnit.Framework;
using System.Collections.Generic;


namespace AutoDbLoader.Test
{
    public class HashSetTest
    {
        private HashSet<string> TestHashSetData()
        {
            HashSet<string> hashset = new HashSet<string>();
            hashset.Add("AAA");
            hashset.Add("AAA");
            hashset.Add("BBB");
            hashset.Add("BBB");
            hashset.Add("CCC");
            hashset.Add("CCC");
            hashset.Add("CCC");
            return hashset;
        }

        [Test]
        public void TestHashSetToString_SouldReturnString()
        {
            // arrage

            var expectedResult = "AAA;BBB;CCC;territory_all;";

            //act

            var result = string.Join(";", TestHashSetData());
            result += ";territory_all;";

            //assert

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
