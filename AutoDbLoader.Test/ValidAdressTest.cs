using AutoDbLoader.CLI.Infrastructure;
using NUnit.Framework;



namespace AutoDbLoader.Test
{
    public class ValidAdressTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        [TestCase("Екатеринбург г, Чайковского ул, д.80 к.2 стр.паркинг п/м 97-98 паркинг", "Чайковского ул, д.80 к.2")]
        [TestCase("Екатеринбург г, Куйбышева ул, д.80 к.2 кв. 1", "Куйбышева ул, д.80 к.2")]
        [TestCase("Екатеринбург г, Куйбышева ул, д.2 пом. 1", "Куйбышева ул, д.2")]
        [TestCase("Екатеринбург г, Малышева ул, д.4 к.б стр.паркинг п/м 2", "Малышева ул, д.4 к.б")]
        [TestCase("Екатеринбург г, Надеждинская ул, д.22 к.б кв. 2", "Надеждинская ул, д.22 к.б")]
        [TestCase("Екатеринбург г, Шаумяна ул, д.111 кв. 1", "Шаумяна ул, д.111")]
        [TestCase("Екатеринбург г, Малышева ул, д.4 к.б пом. 6", "Малышева ул, д.4 к.б")]
        [TestCase("Екатеринбург г, Юлиуса Фучика ул, д.5 лит.Д стр.паркинг п/м 100", "Юлиуса Фучика ул, д.5 лит.Д")]
        [TestCase("Екатеринбург г, Шефская ул, д.101 офис 1", "Шефская ул, д.101")]
        [TestCase("Екатеринбург г, Сиреневый б-р, д.8 кв. 1", "Сиреневый б-р, д.8")]
        [TestCase("Екатеринбург г, Рассветная ул, д.13 к.д кв. 1", "Рассветная ул, д.13 к.д")]
        [TestCase("Екатеринбург г, Надеждинская ул, д.22 к.б кв. 53", "Надеждинская ул, д.22 к.б")]
        [TestCase("Екатеринбург г, Чайковского ул, д.80 к.2 стр.паркинг п/м 5-6", "Чайковского ул, д.80 к.2")]
        [TestCase("Екатеринбург г, Чайковского ул, д.80 к.1 кв.82", "Чайковского ул, д.80 к.1")]
        public void CorrectAddressForChekingProsses_ShouldReturnTrue(string addr, string expectedResult)
        {
            //arrage

            var expected = true;

            //act

            var address = Validator.GetAddressForCheck(addr);
            var result = (address == expectedResult);

            //assert

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.ToString());

            Assert.AreEqual(expected, result);
        }
    }
}
