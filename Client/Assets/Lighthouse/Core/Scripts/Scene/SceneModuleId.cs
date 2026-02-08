using System;

namespace Lighthouse.Core.Scene
{
    public class SceneModuleId : IEquatable<SceneModuleId>
    {
        public int Id { get; }
        public string Name { get; }

        public SceneModuleId(int id, string name)
        {
            Id = id;
            Name = name ?? "";
        }

        public bool Equals(SceneModuleId other) => other is not null && Id == other.Id;
        public override bool Equals(object obj) => obj is SceneModuleId other && Equals(other);
        public override int GetHashCode() => Id;
        public static bool operator ==(SceneModuleId a, SceneModuleId b)
            => ReferenceEquals(a, b) || (a is not null && b is not null && a.Id == b.Id);
        public static bool operator !=(SceneModuleId a, SceneModuleId b) => !(a == b);
        public override string ToString() => string.IsNullOrEmpty(Name) ? Id.ToString() : $"{Name}({Id})";
    }
}