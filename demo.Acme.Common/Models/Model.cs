using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demo.Acme.Models
{
    public class Model
    {
        [Key]
        public string? Id { get; }

        bool equals(Model other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return obj?.GetType() == GetType() && equals((Model)obj);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? 0;
        }

        [JsonConstructor]
        public Model(string? id)
        {
            Id  = id;
        }
    }
}