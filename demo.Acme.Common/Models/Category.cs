using System;
using System.Text.Json.Serialization;
using TetraPak;

namespace demo.Acme.Models
{
    public class Category : Model, IStringValue
    {
        [JsonIgnore]
        public string StringValue { get; }

        public static implicit operator string(Category category) => category.Id;

        public static implicit operator Category(string stringValue) => new(stringValue);

        // ReSharper disable once SuggestBaseTypeForParameter
        bool equals(Category other) => base.Equals(other) && StringValue == other.StringValue;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && equals((Category)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), StringValue);
        }

        public Category(string id) 
        : base(id)
        {
            StringValue = id;
        }
    }
}