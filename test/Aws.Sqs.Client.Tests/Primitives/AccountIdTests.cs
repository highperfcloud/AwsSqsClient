using HighPerfCloud.Aws.Sqs.Core.Primitives;
using System;
using Xunit;

namespace Aws.Sqs.Core.Tests.Primitives
{
    public class AccountIdTests
    {
        [Fact]
        public void Ctor_DoesNotThrowForValidAccountId()
        {
            _ = new AccountId("123456789012");
        }

        [Fact]
        public void Ctor_ThrowsForNullAccountId()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId(null));
        }

        [Fact]
        public void Ctor_ThrowsForEmptyAccountId()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId(""));
        }

        [Fact]
        public void Ctor_ThrowsForWhitespaceAccountId()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId("            ")); // 12 empty chars
        }

        [Fact]
        public void Ctor_ThrowsForInvalidLengthAccountId_WhenTooShort()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId("12345678901")); // 11 instead of 12
        }

        [Fact]
        public void Ctor_ThrowsForInvalidLengthAccountId_WhenTooLong()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId("1234567890123")); // 13 instead of 12
        }

        [Fact]
        public void Ctor_ThrowsForInvalidCharactersInAccountId()
        {
            _ = Assert.Throws<ArgumentException>(() => new AccountId("a23456789012"));
        }
    }
}
