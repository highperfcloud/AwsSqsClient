using System;

namespace HighPerfCloud.Aws.Sqs.Core.Primitives
{
    public readonly struct AccountId
    {
        public AccountId(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                throw new ArgumentException(message: "Account ID cannot be null, empty or contain whitespace", nameof(accountId));

            if (accountId.Length != 12)
                throw new ArgumentException(message: "Account ID must have a length of 12 characters", nameof(accountId));

            foreach (var character in accountId)
            {
                if (!char.IsDigit(character))
                    throw new ArgumentException(message: "Account ID may only contain digits", nameof(accountId));
            }

            Value = accountId;
        }

        public string Value { get; }

        public static bool operator ==(AccountId left, AccountId right) => Equals(left, right);

        public static bool operator !=(AccountId left, AccountId right) => !Equals(left, right);

        public override bool Equals(object obj) => (obj is AccountId accountId) && Equals(accountId);

        public bool Equals(AccountId other) => (Value) == (other.Value);

        public override int GetHashCode() => HashCode.Combine(Value);

        public static implicit operator string(AccountId accountId) => accountId.Value;

        public static implicit operator AccountId(string accountId) => new AccountId(accountId);
    }
}
