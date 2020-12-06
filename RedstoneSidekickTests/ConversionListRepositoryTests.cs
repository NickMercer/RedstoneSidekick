using NUnit.Framework;
using RedstoneSidekick.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSidekickTests
{
    public class ConversionListRepositoryTests
    {
        private ConversionDictionaryRepository _conversionListRepository;

        [SetUp]
        public void InitializeTests()
        {
            _conversionListRepository = new ConversionDictionaryRepository();
        }

        [Test]
        public void GetConversionDictionary_NoParams_ReturnsConversionDictionary()
        {
            var dictionary = _conversionListRepository.GetConversionDictionary();

            CollectionAssert.IsNotEmpty(dictionary);
        }
    }
}
